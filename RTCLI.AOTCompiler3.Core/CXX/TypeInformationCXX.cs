using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace RTCLI.AOTCompiler3.Meta
{
    public static class TypeInformationCXX
    {
        public static string CXXNamespaceSequence(this TypeReference typeRef)
        {
            // Try Resolve
            var typeDef = typeRef.Resolve();
            if(typeDef != null)
            {
                return typeDef.CXXNamespaceSequence();
            }
            return typeRef.Namespace;
        }

        public static string CXXNamespaceSequence(this TypeDefinition typeDef)
        {
            return typeDef.Namespace;
        }
        public static string CXXTypeName(this TypeReference typeReference)
        {
            return "UNIMPLEMENTED_CXX_TYPE_NAME";
        }
        public static string CXXShortTypeName(this TypeReference typeReference)
        {
            if (typeReference.IsArray)
                return $"RTCLI::System::ElementArray<{typeReference.GetElementType().CXXTypeName()}>";
            if (typeReference.IsGenericInstance || typeReference.IsGenericParameter)
                    return "UNIMPLEMENTED_CXX_SHORT_NAME";
            if (typeReference.IsPointer)
                return "UNIMPLEMENTED_CXX_SHORT_NAME";
            else return typeReference.Name.Replace("<>", "__").Replace('`', '_').Replace("<", "_").Replace(">", "_");
        }
        public static string CXXNamespaceToPath(this TypeReference typeReference)
        {
            return typeReference.CXXNamespaceSequence().Replace(".", "/");
        }
    }
}
