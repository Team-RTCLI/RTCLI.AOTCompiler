using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Mono.Cecil;
using RTCLI.AOTCompiler3.Meta;

namespace RTCLI.AOTCompiler3.Translators
{
    public class CXXSourceTranslator
    {
        public CXXSourceTranslator(CodeTextStorage storage, AssemblyDefinition assembly)
        {
            Storage = storage;
            FocusedAssembly = assembly;
        }

        public void Run()
        {
            foreach (var Module in FocusedAssembly.Modules)
            {
                foreach (var Type in Module.Types)
                {
                    var codeWriter = Storage.Wirter(Path.Combine(Type.CXXNamespaceToPath(), Type.CXXShortTypeName() + ".cpp"));
                    codeWriter.WriteLine(Utilities.CopyRight);

                    codeWriter.WriteLine(EnvIncludes);

                    codeWriter.WriteLine();
                    codeWriter.WriteLine("// [S1000] Include Uber Headers.");
                    codeWriter.WriteLine($"#include <{Type.CXXUberHeaderPath()}>");
                    foreach(var assemblyReference in Module.AssemblyReferences)
                    {
                        codeWriter.WriteLine($"#include <{assemblyReference.CXXUberHeaderPath()}>");
                    }

                    codeWriter.WriteLine("");
                    codeWriter.WriteLine("// [S0001] Close Unused-Label Warnings.");
                    codeWriter.WriteLine($"#if defined(RTCLI_COMPILER_CLANG)");
                    codeWriter.WriteLine($"#pragma clang diagnostic push");
                    codeWriter.WriteLine($"#pragma clang diagnostic ignored \"-Wunused-label\"");
                    codeWriter.WriteLine($"#elif defined(RTCLI_COMPILER_MSVC)");
                    codeWriter.WriteLine($"#pragma warning(push)");
                    codeWriter.WriteLine($"#pragma warning(disable: 4102)");
                    codeWriter.WriteLine($"#endif");
                    
                    codeWriter.WriteLine("");
                    //WriteMethodRecursive(codeWriter, type);
                    codeWriter.WriteLine("");

                    codeWriter.WriteLine("");
                    codeWriter.WriteLine("// [S0001] Pop Unused-Label Warnings.");
                    codeWriter.WriteLine($"#if defined(RTCLI_COMPILER_CLANG)");
                    codeWriter.WriteLine($"#pragma clang diagnostic pop");
                    codeWriter.WriteLine($"#elif defined(RTCLI_COMPILER_MSVC)");
                    codeWriter.WriteLine($"#pragma warning(pop)");
                    codeWriter.WriteLine($"#endif");

                    codeWriter.WriteLine("");
                    codeWriter.WriteLine("// [S0000] Generate Test Point.");
                    codeWriter.WriteLine($"#ifdef RTCLI_TEST_POINT");
                    codeWriter.WriteLine("int main(void){");
                    codeWriter.WriteLine($"\t{Type.CXXTypeName()}::Test();");
                    codeWriter.WriteLine("\treturn 0;");
                    codeWriter.WriteLine("}");
                    codeWriter.WriteLine($"#endif");

                    codeWriter.Flush();
                }
            }
        }

        // ${OutputPath}/${Assembly}/src
        CodeTextStorage Storage = null;
        AssemblyDefinition FocusedAssembly = null;
        private string EnvIncludes => "#include <RTCLI/RTCLI.hpp>";
    }
}
