using RTCLI.AOTCompiler;
using RTCLI.AOTCompiler.ILConverters;
using System;
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

    public class CXXTranslator
    {
        private static bool TypeNameFilter(Type typeObj, Object criteriaObj)
        {
            if (typeObj.ToString() == criteriaObj.ToString())
                return true;
            else
                return false;
        }

        public CXXTranslator(TranslateContext translateContext, CXXTranslateOptions options)
        {
            this.options = options;
            this.translateContext = translateContext;

            Assembly assembly = Assembly.GetAssembly(typeof(ILConverters.IILConverter));
            TypeFilter typeNameFilter = new TypeFilter(TypeNameFilter);
            foreach (var type in assembly.GetTypes())
            {
                Type[] typeInterfaces = type.FindInterfaces(typeNameFilter, typeof(ILConverters.ICXXILConverter));
                if (typeInterfaces.Length > 0)
                {
                    var newConv = System.Activator.CreateInstance(type) as ILConverters.ICXXILConverter;
                    convertersCXX.Add(newConv.TargetOpCode(), newConv);
                    System.Console.WriteLine($"Registered: {newConv.TargetOpCode()}");
                }
            }
            System.Console.WriteLine($"Total Converters: {convertersCXX.Count}/{Enum.GetValues(typeof(Code)).Length}");
        }

        private string NoteILInstruction(Instruction inst, MethodTranslateContext methodContext)
        {
            if (convertersCXX.TryGetValue(inst.OpCode, out ICXXILConverter targetConverter))
                return targetConverter.Note(inst, methodContext);
            return $"//{inst.ToString()}";
        }

        private string TranslateILInstruction(Instruction inst, MethodTranslateContext methodContext)
        {
            if (convertersCXX.TryGetValue(inst.OpCode, out ICXXILConverter targetConverter))
                return targetConverter.Convert(inst, methodContext);
            return options.StaticAssertOnUnimplementatedILs 
                ? $"static_assert(0, \"[{inst.ToString()}] Has No Converter Implementation!\");"
                : $"RTCLI::unimplemented_il(\"{ inst.ToString()}\"); //{inst.ToString()}";
        }

        private string EnvIncludes => "#include <RTCLI.h>";
        public void WriteHeader(CodeTextStorage storage)
        {
            using (var _ = storage.EnterScope(translateContext.FocusedAssemblyInformation.IdentName))
            {
                foreach (var module in translateContext.FocusedAssemblyInformation.Modules.Values)
                {
                    foreach (var type in module.Types.Values)
                    {
                        var codeWriter = storage.Wirter(type.TypeName + ".h");
                        typeHeaderWriters[type.FullName] = codeWriter;
                        codeWriter.WriteLine(EnvIncludes);
                        using (var ___ = new CXXScopeDisposer(codeWriter, "\nnamespace " + type.CXXNamespace))
                        {
                            using (var classScope = new CXXScopeDisposer(codeWriter,
                                type.IsStruct ?
                                  $"struct {type.CXXTypeNameShort}"
                                : $"RTCLI_API class {type.CXXTypeNameShort} : public RTCLI::System::Object",

                                true))
                            {
                                codeWriter.unindent().WriteLine("public:").indent();
                                foreach (var method in type.Methods)
                                {
                                    if (method.IsPublic)
                                        codeWriter.WriteLine($"{method.CXXMethodSignature};");
                                }
                                foreach(var field in type.Fields)
                                {
                                    if (field.IsPublic)
                                        codeWriter.WriteLine(field.CXXFieldDeclaration);
                                }
                                codeWriter.unindent().WriteLine("private:").indent();
                                foreach (var method in type.Methods)
                                {
                                    if (method.IsPrivate)
                                        codeWriter.WriteLine($"{method.CXXMethodSignature};");
                                }
                                foreach (var field in type.Fields)
                                {
                                    if (field.IsPrivate)
                                        codeWriter.WriteLine(field.CXXFieldDeclaration);
                                }
                                codeWriter.unindent().WriteLine("protected:").indent();
                                foreach (var method in type.Methods)
                                {
                                    if (method.IsFamily)
                                        codeWriter.WriteLine($"{method.CXXMethodSignature};");
                                }
                                foreach (var field in type.Fields)
                                {
                                    if (field.IsFamily)
                                        codeWriter.WriteLine(field.CXXFieldDeclaration);
                                }
                            }
                        }
                        typeHeaderWriters[type.FullName].Flush();
                    }
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
                        codeWriter.WriteLine($"#include <{type.TypeName}.h>");
                        foreach (var method in type.Methods)
                        {
                            CXXMethodTranslateContext methodContext = new CXXMethodTranslateContext(translateContext, method);
                            // [2-1] Stack Code
                            codeWriter.WriteLine($"\n//{method.CXXMethodName}\n//[2-1] Here Begins Stack Declaration");
                            codeWriter.WriteLine($"struct {method.CXXStackName}");
                            codeWriter.WriteLine("{");
                            codeWriter.indent();
                            foreach (var localVar in method.LocalVariables)
                            {
                                codeWriter.WriteLine($"{localVar.CXXTypeName} v{localVar.Index};");
                            }
                            codeWriter.WriteLine("template<bool InitLocals> static void Init(){};//Active with MethodBody.InitLocals Property.");
                            codeWriter.unindent();
                            codeWriter.WriteLine("};\n");

                            // [2-2-1] Method Code
                            codeWriter.WriteLine("//[2-2] Here Begins Method Body");
                            codeWriter.WriteLine(
                                method.CXXRetType + " " + method.CXXMethodName + method.CXXParamSequence);
                            codeWriter.WriteLine("{");
                            // [2-2-2] Code Body
                            codeWriter.indent();
                            //codeWriter.WriteLine($"{method.CXXStackName} stack;");
                            //codeWriter.WriteLine($"stack.Init<{method.InitLocals.ToString().ToLower()}>();");
                            foreach (var instruction in method.Body.Instructions)
                            {
                                codeWriter.WriteLine(NoteILInstruction(instruction, methodContext));
                                codeWriter.WriteLine(
                                    instruction.GetLabel() + ": "+ 
                                    TranslateILInstruction(instruction, methodContext));
                            }
                            codeWriter.unindent();
                            codeWriter.WriteLine("}");
                        }
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
        private readonly Dictionary<OpCode, ICXXILConverter> convertersCXX = new Dictionary<OpCode, ICXXILConverter>(); 
    }
}