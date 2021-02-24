using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Mono.Cecil;
using RTCLI.AOTCompiler3.Meta;


namespace RTCLI.AOTCompiler3.Translators
{
    public static class TypeCXXPathHelper
    {
        public static string CXXHeaderPath(this TypeReference typeReference)
        {
            return Path.Combine(typeReference.CXXNamespaceToPath(), typeReference.CXXShortTypeName() + ".h");
        }
    }

    public class CXXHeaderTranslator
    {
        public CXXHeaderTranslator(CodeTextStorage storage, AssemblyDefinition assembly)
        {
            Storage = storage;
            FocusedAssembly = assembly;
        }

        public void Run()
        {
            foreach(var Module in FocusedAssembly.Modules)
            {
                foreach(var Type in Module.Types)
                {
                    var typeWriter = Storage.Wirter(Path.Combine(Type.CXXNamespaceToPath(), Type.CXXShortTypeName() + ".h"));
                    typeWriter.WriteLine(Utilities.CopyRight);

                    typeWriter.Flush();
                }
            }
        }

        // ${OutputPath}/${Assembly}/include
        CodeTextStorage Storage = null;
        AssemblyDefinition FocusedAssembly = null;
    }
}
