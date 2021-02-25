using System;
using System.Collections.Generic;
using System.Text;

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
}
