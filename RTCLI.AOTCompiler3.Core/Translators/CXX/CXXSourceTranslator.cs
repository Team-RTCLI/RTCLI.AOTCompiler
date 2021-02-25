using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Mono.Cecil;
using RTCLI.AOTCompiler3.Meta;

namespace RTCLI.AOTCompiler3.Translators
{
    public class CXXSourceTranslator : IRTCLITranslator
    {
        // ${OutputPath}/${Assembly}/src
        public void Run(CodeTextStorage Storage, AssemblyDefinition FocusedAssembly)
        {
            foreach (var Module in FocusedAssembly.Modules)
            {
                foreach (var Type in Module.Types)
                {
                    var Writer = Storage.Wirter(Path.Combine(Type.CXXNamespaceToPath(), Type.CXXShortTypeName() + ".cpp"));

                    // [S9999] Copyright
                    CXXSourceRules.Copyright(Writer);
                    // [S1000] Include Uber Headers.
                    CXXSourceRules.IncludeUberHeaders(Writer, Type);

                    // [S0001] Close Unused-Label Warning
                    using (var no_unused_lables = new ScopeNoUnusedWarning(Writer))
                    {
                        Writer.WriteLine("");
                        //WriteMethodRecursive(codeWriter, type);
                        Writer.WriteLine("");


                        Writer.WriteLine("");
                        Writer.WriteLine("// [S0000] Generate Test Point.");
                        Writer.WriteLine($"#ifdef RTCLI_TEST_POINT");
                        Writer.WriteLine("int main(void){");
                        Writer.WriteLine($"\t{Type.CXXTypeName()}::Test();");
                        Writer.WriteLine("\treturn 0;");
                        Writer.WriteLine("}");
                        Writer.WriteLine($"#endif");
                    }

                    Writer.Flush();
                }
            }
        }

        private string EnvIncludes => "#include <RTCLI/RTCLI.hpp>";
    }
}
