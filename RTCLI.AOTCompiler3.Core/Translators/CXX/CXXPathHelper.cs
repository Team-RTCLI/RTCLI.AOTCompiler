using System;
using System.IO;
using System.Text;
using Mono.Cecil;
using RTCLI.AOTCompiler3.Meta;

namespace RTCLI.AOTCompiler3.Translators
{
    public static class TypeCXXPathHelper
    {
        public static string CXXHeaderPath(this TypeReference typeReference)
        {
            var T = typeReference;
            if(typeReference.IsGenericInstance)
            {
                T = typeReference.GetElementType();
            }
            return Path.Combine(T.CXXNamespaceToPath(), T.CXXShortTypeName() + ".h").Replace("\\", "/");
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
            return Path.Combine(assembly.RTCLIShortName(), $"include/{Constants.CXXUberHeaderName}").Replace("\\", "/");
        }

        public static string CXXUberHeaderPath(this AssemblyNameReference assembly)
        {
            return Path.Combine(assembly.RTCLIShortName(), $"include/{Constants.CXXUberHeaderName}").Replace("\\", "/");
        }
    }
}
