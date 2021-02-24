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
                    var typeWriter = Storage.Wirter(Path.Combine(Type.CXXNamespaceToPath(), Type.CXXShortTypeName() + ".cpp"));
                    typeWriter.WriteLine(Utilities.CopyRight);

                    typeWriter.Flush();
                }
            }
        }

        // ${OutputPath}/${Assembly}/src
        CodeTextStorage Storage = null;
        AssemblyDefinition FocusedAssembly = null;
    }
}
