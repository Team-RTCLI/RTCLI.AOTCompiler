using RTCLI.AOTCompiler;
using RTCLI.AOTCompiler.ILConverters;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Reflection;
using Mono.Cecil.Cil;

namespace RTCLI.AOTCompiler.Translators
{
    public struct CXXTranslateOptions
    {
        public DebugInformationOptions DebugInformationOption;
        public bool StaticAssertOnUnimplementatedILs;
        public IEnumerable<string> assemblyPaths;
    }

    public class CXXConvertersCenter
    {
        private static bool TypeNameFilter(Type typeObj, Object criteriaObj)
        {
            if (typeObj.ToString() == criteriaObj.ToString())
                return true;
            else
                return false;
        }

        static CXXConvertersCenter()
        {
            System.Console.WriteLine("Preparing ConvertersCXX...");

            Assembly assembly = Assembly.GetAssembly(typeof(ILConverters.IILConverter));
            TypeFilter typeNameFilter = new TypeFilter(TypeNameFilter);
            int currentLineCursor = Console.CursorTop;
            int index = 0;
            foreach (var type in assembly.GetTypes())
            {
                Type[] typeInterfaces = type.FindInterfaces(typeNameFilter, typeof(ILConverters.ICXXILConverter));
                if (typeInterfaces.Length > 0)
                {
                    var newConv = System.Activator.CreateInstance(type) as ILConverters.ICXXILConverter;
                    ConvertersCXX.Add(newConv.TargetOpCode(), newConv);

                    System.Console.SetCursorPosition(0, currentLineCursor);
                    Console.Write(new string(' ', Console.WindowWidth));
                    System.Console.SetCursorPosition(0, currentLineCursor);
                    System.Console.WriteLine($"Registered: {newConv.TargetOpCode()}, {++index} / {Enum.GetValues(typeof(Code)).Length}");
                }
            }
            System.Console.WriteLine($"Total Converters: {ConvertersCXX.Count} / {Enum.GetValues(typeof(Code)).Length}");
        }

        public static readonly Dictionary<OpCode, ICXXILConverter> ConvertersCXX = new Dictionary<OpCode, ICXXILConverter>();
    }

    public class CXXTranslator
    { 
        public CXXTranslator(TranslateContext translateContext, CXXTranslateOptions options)
        {
            this.options = options;
            this.translateContext = translateContext;
        }

        private string NoteILInstruction(Instruction inst, MethodTranslateContext methodContext)
        {
            if (convertersCXX.TryGetValue(inst.OpCode, out ICXXILConverter targetConverter))
                return targetConverter.Note(inst, methodContext);
            return $"//{inst.ToString().HoldEscape()}";
        }

        private string TranslateILInstruction(Instruction inst, MethodTranslateContext methodContext)
        {
            if (convertersCXX.TryGetValue(inst.OpCode, out ICXXILConverter targetConverter))
                return targetConverter.Convert(inst, methodContext);
            return options.StaticAssertOnUnimplementatedILs 
                ? $"static_assert(0, \"[{inst.ToString()}] Has No Converter Implementation!\");"
                : $"RTCLI::unimplemented_il(\"{ inst.ToString()}\"); //{inst.ToString()}";
        }

        private string EnvIncludes => "#include <RTCLI/RTCLI.hpp>";
        public void WriteTypeRecursively(CodeTextWriter codeWriter, Metadata.TypeInformation type)
        {
            if(type.HasGenericParameters)
                codeWriter.WriteLine($"template<{type.CXXTemplateParam}>");
            string Interfaces = string.Join(',', type.Interfaces.Select(a => $"public {a.CXXTypeName}"));
            if (type.Interfaces.Count > 0)
                Interfaces = "," + Interfaces;
            string BaseType = type.BaseType != null ? type.BaseType.CXXTypeName : "RTCLI::System::Object";
            using (var classScope = new CXXScopeDisposer(codeWriter,
                               type.IsStruct ?
                                 $"struct {type.CXXTypeNameShort}_v"
                               : $"class {type.CXXTypeNameShort} : public {BaseType}{Interfaces}",

                               true))
             {
                codeWriter.unindent().WriteLine("public:").indent();
                foreach (var nested in type.Nested)
                {
                    WriteTypeRecursively(codeWriter, nested);
                }
                foreach (var method in type.Methods)
                {
                    if(method.HasGenericParameters)
                        codeWriter.WriteLine($"template<{method.CXXTemplateParam}>");
                    codeWriter.WriteLine($"{method.CXXMethodSignature};");
                }
                foreach (var field in type.Fields)
                {
                    codeWriter.WriteLine(field.CXXFieldDeclaration);
                }
            }

            if (!type.IsStruct)
                return;

            if (type.HasGenericParameters)
                codeWriter.WriteLine($"template<{type.CXXTemplateParam}>");
            string classDef = $"class {type.CXXTypeNameShort} : public RTCLI::System::ValueType{Interfaces}";
            using (var classScope = new CXXScopeDisposer(codeWriter, classDef, true))
            {
                codeWriter.unindent().WriteLine("public:").indent();
                codeWriter.WriteLine($"using ValueType = {type.CXXTypeNameShort}_v;");
                //codeWriter.WriteLine($"using ValueType = struct {type.CXXTypeNameShort};");
                codeWriter.WriteLine($"{type.CXXTypeNameShort}_v value;");
                foreach (var method in type.Methods)
                {
                    if (method.HasGenericParameters)
                        codeWriter.WriteLine($"template<{method.CXXTemplateParam}>");
                    codeWriter.WriteLine($"RTCLI_FORCEINLINE {method.CXXMethodSignature} {{ value.{method.CXXMethodNameShort}{method.CXXArgSequence}; }}");
                }
            }

        }

        public void WriteMethodRecursive(CodeTextWriter codeWriter, Metadata.TypeInformation type)
        {
            foreach (var nested in type.Nested)
            {
                WriteMethodRecursive(codeWriter, nested);
            }
            foreach (var method in type.Methods)
            {
                if (method.Body == null)
                    continue;
                CXXMethodTranslateContext methodContext = new CXXMethodTranslateContext(translateContext, method);
                // [2-1] Stack Code
                codeWriter.WriteLine($"\n//{method.CXXMethodDeclareName}\n//[2-1] Here Begins Stack Declaration");
                if(type.HasGenericParameters || method.HasGenericParameters)
                {
                    List<string> paramList = new List<string>();
                    if(type.HasGenericParameters)
                        paramList.Add(type.CXXTemplateParam);
                    if (method.HasGenericParameters)
                        paramList.Add(method.CXXTemplateParam);
                    codeWriter.WriteLine($"template<{string.Join(',', paramList)}>");
                }
                codeWriter.WriteLine($"struct {method.CXXStackName}");
                codeWriter.WriteLine("{");
                codeWriter.indent();
                foreach (var localVar in method.LocalVariables)
                {
                    codeWriter.WriteLine($"{localVar.CXXVarDeclaration} v{localVar.Index} = {localVar.CXXVarInitVal};");
                }
                codeWriter.WriteLine("template<bool InitLocals> static void Init(){};//Active with MethodBody.InitLocals Property.");
                codeWriter.unindent();
                codeWriter.WriteLine("};\n");

                // [2-2-1] Method Code
                codeWriter.WriteLine("//[2-2] Here Begins Method Body");
                if(type.HasGenericParameters)
                    codeWriter.WriteLine($"template<{type.CXXTemplateParam}>");
                if (method.HasGenericParameters)
                    codeWriter.WriteLine($"template<{method.CXXTemplateParam}>");

                codeWriter.WriteLine(
                    method.CXXRetType + " " + method.CXXMethodDeclareName + method.CXXParamSequence);

                codeWriter.WriteLine("{");
                // [2-2-2] Code Body
                codeWriter.indent();
                if (method.IsConstructor && method.IsStatic)
                {
                    codeWriter.WriteLine("static std::once_flag flag;");
                    codeWriter.WriteLine("std::call_once(flag,[&]()");
                    codeWriter.WriteLine("{");
                    codeWriter.indent();
                }
                if (type.HasGenericParameters || method.HasGenericParameters)
                {
                    List<string> argList = new List<string>();
                    if (type.HasGenericParameters)
                        argList.Add(type.CXXTemplateArg);
                    if (method.HasGenericParameters)
                        argList.Add(method.CXXTemplateArg);
                    codeWriter.WriteLine($"{method.CXXStackName}<{string.Join(',', argList)}> stack;");
                }
                else
                    codeWriter.WriteLine($"{method.CXXStackName} stack;");
                codeWriter.WriteLine($"stack.Init<{method.InitLocals.ToString().ToLower()}>();");
                foreach (var instruction in method.Body.Instructions)
                {
                    codeWriter.WriteLine(NoteILInstruction(instruction, methodContext));
                    codeWriter.WriteLine(
                        instruction.GetLabel() + ": " +
                        TranslateILInstruction(instruction, methodContext));
                }
                if (method.IsConstructor && method.IsStatic)
                {
                    codeWriter.unindent();
                    codeWriter.WriteLine("});");
                }
                codeWriter.unindent();
                codeWriter.WriteLine("}");
            }
        }

        public void WriteHeader(CodeTextStorage storage)
        {
            using (var _ = storage.EnterScope(translateContext.FocusedAssemblyInformation.IdentName))
            {
                foreach (var module in translateContext.FocusedAssemblyInformation.Modules.Values)
                {
                    var totalHeaderWriter = storage.Wirter(module.CXXHeaderName + ".h");
                    totalHeaderWriter.WriteLine("#pragma once");
                    totalHeaderWriter.WriteLine(EnvIncludes);
                    foreach (var type in module.Types.Values)
                    {
                        var codeWriter = storage.Wirter(type.TypeName + ".h");
                        typeHeaderWriters[type.FullName] = codeWriter;
                        codeWriter.WriteLine("#pragma once");
                        codeWriter.WriteLine(EnvIncludes);
                        using (var ___ = new CXXScopeDisposer(codeWriter, "\nnamespace " + type.CXXNamespace))
                        {
                            WriteTypeRecursively(codeWriter, type);
                        }
                        typeHeaderWriters[type.FullName].Flush();

                        totalHeaderWriter.WriteLine($"#include \"{type.TypeName}.h\"");
                    }

                    totalHeaderWriter.Flush();
                }
            }//End Dispose EnterScope
        }

        public void WriteSource(CodeTextStorage storage)
        {
            using (var _ = storage.EnterScope(translateContext.FocusedAssemblyInformation.IdentName))
            {
                foreach (var module in translateContext.FocusedAssemblyInformation.Modules.Values)
                {
                    foreach (var type in module.Types.Values)
                    {
                        var codeWriter = storage.Wirter(type.TypeName + ".cpp");
                        typeSourceWriters[type.FullName] = codeWriter;
                        codeWriter.WriteLine(EnvIncludes);
                        codeWriter.WriteLine($"#include <{translateContext.FocusedAssemblyInformation.IdentName}/{type.TypeName}.h>");
                        codeWriter.WriteLine($"#if defined(RTCLI_COMPILER_CLANG)");
                        codeWriter.WriteLine($"#pragma clang diagnostic push");
                        codeWriter.WriteLine($"#pragma clang diagnostic ignored \"-Wunused-label\"");
                        codeWriter.WriteLine($"#elif defined(RTCLI_COMPILER_MSVC)");
                        codeWriter.WriteLine($"#pragma warning(push)");
                        codeWriter.WriteLine($"#pragma warning(disable: 4102)");
                        codeWriter.WriteLine($"#endif");
                        WriteMethodRecursive(codeWriter, type);
                        codeWriter.WriteLine($"#if defined(RTCLI_COMPILER_CLANG)");
                        codeWriter.WriteLine($"#pragma clang diagnostic pop");
                        codeWriter.WriteLine($"#elif defined(RTCLI_COMPILER_MSVC)");
                        codeWriter.WriteLine($"#pragma warning(pop)");
                        codeWriter.WriteLine($"#endif");

                        codeWriter.WriteLine($"#ifdef RTCLI_TEST_POINT");
                        codeWriter.WriteLine("int main(void){");
                        codeWriter.WriteLine($"\t{type.CXXTypeName}::Test();");
                        codeWriter.WriteLine("\treturn 0;");
                        codeWriter.WriteLine("}");
                        codeWriter.WriteLine($"#endif");
                        
                        typeSourceWriters[type.FullName].Flush();
                    }
                }
            }//End Dispose EnterScope
        }

        private sealed class CXXScopeDisposer : IDisposable
        {
            private CodeTextWriter parent;
            private bool EndWithSemicolon = false;
            public CXXScopeDisposer(CodeTextWriter parent, string Scope, bool bEndWithSemicolon = false)
            {
                this.parent = parent;
                this.EndWithSemicolon = bEndWithSemicolon;
                parent.WriteLine(Scope);
                parent.WriteLine("{");
                parent.indent();
            }

            public void Dispose()
            {
                if (parent != null)
                {
                    parent.unindent();
                    parent.WriteLine("}" + (EndWithSemicolon?";":""));
                    parent = null;
                }
            }
        }
        public readonly CXXTranslateOptions options;
        private readonly TranslateContext translateContext = null;
        private readonly Dictionary<string, CodeTextWriter> typeSourceWriters = new Dictionary<string, CodeTextWriter>();
        private readonly Dictionary<string, CodeTextWriter> typeHeaderWriters = new Dictionary<string, CodeTextWriter>();
        private Dictionary<OpCode, ICXXILConverter> convertersCXX => CXXConvertersCenter.ConvertersCXX;
    }
}