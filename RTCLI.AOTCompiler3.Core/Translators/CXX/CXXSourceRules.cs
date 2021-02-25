using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace RTCLI.AOTCompiler3.Translators
{
    public static class CXXSourceRules
    {
        [S1000]
        public static void IncludeUberHeaders(CodeTextWriter Writer, TypeDefinition type)
        {
            Writer.WriteLine("// [S1000] Include Uber Headers.");
            Writer.WriteLine($"#include <{type.CXXUberHeaderPath()}>");
            foreach (var assemblyReference in type.Module.AssemblyReferences)
            {
                Writer.WriteLine($"#include <{assemblyReference.CXXUberHeaderPath()}>");
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
            codeWriter = writer;
            codeWriter.WriteLine("");
            codeWriter.WriteLine("// [S0001] Close Unused-Label Warnings.");
            codeWriter.WriteLine($"#if defined(RTCLI_COMPILER_CLANG)");
            codeWriter.WriteLine($"#pragma clang diagnostic push");
            codeWriter.WriteLine($"#pragma clang diagnostic ignored \"-Wunused-label\"");
            codeWriter.WriteLine($"#elif defined(RTCLI_COMPILER_MSVC)");
            codeWriter.WriteLine($"#pragma warning(push)");
            codeWriter.WriteLine($"#pragma warning(disable: 4102)");
            codeWriter.WriteLine($"#endif");
        }

        public void Dispose()
        {
            codeWriter.WriteLine("");
            codeWriter.WriteLine("// [S0001] Pop Unused-Label Warnings.");
            codeWriter.WriteLine($"#if defined(RTCLI_COMPILER_CLANG)");
            codeWriter.WriteLine($"#pragma clang diagnostic pop");
            codeWriter.WriteLine($"#elif defined(RTCLI_COMPILER_MSVC)");
            codeWriter.WriteLine($"#pragma warning(pop)");
            codeWriter.WriteLine($"#endif");
        }

        CodeTextWriter codeWriter = null;
    }

}