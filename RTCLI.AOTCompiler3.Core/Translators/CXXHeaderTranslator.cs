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
            return Path.Combine(typeReference.CXXNamespaceToPath(), typeReference.CXXShortTypeName() + ".h").Replace("\\", "/");
        }

        public static string CXXUberHeaderPath(this TypeReference typeReference)
        {
            return typeReference.Module.Assembly.CXXUberHeaderPath();
        }
    }

    public static class AssemblyCXXPathHelper
    {
        public static string CXXUberHeaderPath(this AssemblyDefinition assembly)
        {
            return Path.Combine(assembly.RTCLIShortName(), "include/_UberHeader_.h").Replace("\\", "/");
        }

        public static string CXXUberHeaderPath(this AssemblyNameReference assembly)
        {
            return Path.Combine(assembly.RTCLIShortName(), "include/_UberHeader_.h").Replace("\\", "/");
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
            var uberHeaderWriter = Storage.Wirter("_UberHeader_.h");
            uberHeaderWriter.WriteLine("// [H1000] UberHeader"); 
            foreach (var Module in FocusedAssembly.Modules)
            {
                foreach(var Type in Module.Types)
                {
                    var codeWriter = Storage.Wirter(Type.CXXHeaderPath());
                    codeWriter.WriteLine(Utilities.CopyRight);
                    codeWriter.WriteLine("// [H0000] Include Protect");
                    codeWriter.WriteLine("#pragma once");
                    codeWriter.WriteLine(EnvIncludes);

                    codeWriter.WriteLine();
                    codeWriter.WriteLine("// [H0001] Forward Declaration");

                    codeWriter.WriteLine();


                    codeWriter.Flush();
                    uberHeaderWriter.WriteLine($"#include \"{Type.CXXHeaderPath()}\"");
                }
            }
            uberHeaderWriter.Flush();
        }

        // ${OutputPath}/${Assembly}/include
        CodeTextStorage Storage = null;
        AssemblyDefinition FocusedAssembly = null;
        private string EnvIncludes => "#include <RTCLI/RTCLI.hpp>";
    }
}
