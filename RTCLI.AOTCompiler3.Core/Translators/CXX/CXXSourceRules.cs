using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using RTCLI.AOTCompiler3.Meta;
using RTCLI.AOTCompiler3.ILConverters;

namespace RTCLI.AOTCompiler3.Translators
{
    public static class CXXSourceRules
    {
        [S1000()]
        public static void IncludeUberHeaders(CodeTextWriter Writer, TypeDefinition Type)
        {
            Writer.WriteLine("// [S1000] Include Uber Headers.");
            Writer.WriteLine($"#include <{Type.CXXUberHeaderPath()}>");
            foreach (var assemblyReference in Type.Module.AssemblyReferences)
            {
                Writer.WriteLine($"#include <{assemblyReference.CXXUberHeaderPath()}>");
            }
        }
        
        [S1001()]
        public static void IncludeWeakReferences(CodeTextWriter Writer, TypeDefinition Type)
        {
            Writer.WriteLine("// [S1001] Include Uber Headers.");
            Writer.WriteLine($"#include <{Type.CXXHeaderPath()}>");
            var rs = Type.WeakReferences();
            foreach (var r in rs)
            {
                if(!r.IsImplementedByVM())
                    Writer.WriteLine($"#include <{r.CXXHeaderPath()}>");
            }
        }

        [S2000()]
        public static void WriteMethodBody(CodeTextWriter Writer, MethodDefinition Method, bool ValueType)
        {
            MethodTranslateContextCXX methodContext = new MethodTranslateContextCXX(Method);

            Writer.WriteLine("{");
            Writer.indent();
            Writer.WriteLine("// [S2000] Method Body");
            if (Method.IsConstructor && Method.IsStatic)
            {
                Writer.WriteLine("// [S2000-2] Static Constructor Body");
                Writer.WriteLine("static std::once_flag flag;");
                Writer.WriteLine("std::call_once(flag,[&]()");
                Writer.WriteLine("{");
                Writer.indent();
            }
            if(Method.Body.HasVariables)
            {
                Writer.WriteLine("// [S2000-0] Local Varaiables");
                foreach (var localVar in Method.Body.Variables)
                {
                    if (Method.Body.InitLocals)
                        Writer.WriteLine($"{localVar.CXXVarDeclaration()} v{localVar.Index} = {localVar.CXXVarInitVal()};");
                    else
                        Writer.WriteLine($"{localVar.CXXVarDeclaration()} v{localVar.Index};");
                }
            }

            Writer.WriteLine("// [S2000-1] Code Body");
            foreach (var instruction in Method.Body.Instructions)
            {
                Writer.WriteLine(NoteILInstruction(instruction, methodContext));
                Writer.WriteLine(
                    instruction.GetLabel() + ": " +
                    TranslateILInstruction(instruction, methodContext));
            }
            if (Method.IsConstructor && Method.IsStatic)
            {
                Writer.unindent();
                Writer.WriteLine("});");
                Writer.WriteLine("// [S2000-2] Static Constructor Body End");
            }
            Writer.unindent();
            Writer.WriteLine("}");
        }

        [S2000()]
        private static string NoteILInstruction(Instruction inst, MethodTranslateContextCXX methodContext)
        {
            if (Constants.CXXILConverters.TryGetValue(inst.OpCode, out ICXXILConverter targetConverter))
                return targetConverter.Note(inst, methodContext);
            return $"//{inst.ToString().HoldEscape()}";
        }

        [S2000()]
        private static string TranslateILInstruction(
            Instruction inst, MethodTranslateContextCXX methodContext,
            bool StaticAssertOnUnimplementatedILs = false)
        {
            if (Constants.CXXILConverters.TryGetValue(inst.OpCode, out ICXXILConverter targetConverter))
                return targetConverter.Convert(inst, methodContext);

            return StaticAssertOnUnimplementatedILs
                ? $"static_assert(0, \"[{inst.ToString()}] Has No Converter Implementation!\");"
                : $"RTCLI::unimplemented_il(\"{ inst.ToString()}\"); //{inst.ToString()}";
        }

        [S2001()]
        public static void WriteStaticFieldImplementation(CodeTextWriter Writer, TypeDefinition Type)
        {
            Writer.WriteLine("// [S2001] Static Field Implementation");
            foreach (var Field in Type.Fields)
            {
                if(Field.IsStatic)
                    Writer.WriteLine($"{Field.FieldType.CXXTypeName()} " +
                        $"{Type.CXXTypeName()}::{Utilities.GetCXXValidTokenString(Field.Name)};");
            }
        }

        [S9999()]
        public static void Copyright(CodeTextWriter Writer)
        {
            Writer.WriteLine("// [S9999] Copyright");
            Writer.WriteLine(Constants.CopyRight);
        }
    }

    [S0001()]
    public class ScopeNoUnusedWarning : IDisposable
    {
        public ScopeNoUnusedWarning(CodeTextWriter writer)
        {
            Writer = writer;
            Writer.WriteLine("");
            Writer.WriteLine("// [S0001] Close Unused-Label Warnings.");
            Writer.WriteLine($"#if defined(RTCLI_COMPILER_CLANG)");
            Writer.WriteLine($"#pragma clang diagnostic push");
            Writer.WriteLine($"#pragma clang diagnostic ignored \"-Wunused-label\"");
            Writer.WriteLine($"#elif defined(RTCLI_COMPILER_MSVC)");
            Writer.WriteLine($"#pragma warning(push)");
            Writer.WriteLine($"#pragma warning(disable: 4102)");
            Writer.WriteLine($"#endif");
        }

        public void Dispose()
        {
            Writer.WriteLine("");
            Writer.WriteLine("// [S0001] Pop Unused-Label Warnings.");
            Writer.WriteLine($"#if defined(RTCLI_COMPILER_CLANG)");
            Writer.WriteLine($"#pragma clang diagnostic pop");
            Writer.WriteLine($"#elif defined(RTCLI_COMPILER_MSVC)");
            Writer.WriteLine($"#pragma warning(pop)");
            Writer.WriteLine($"#endif");
        }

        CodeTextWriter Writer = null;
    }

}