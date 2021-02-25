using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using System.Linq;

namespace RTCLI.AOTCompiler3.Meta
{
    public static class TypeInformationCXX
    {
        public static string CXXNamespace(this TypeDefinition typeDef)
        {
            return "RTCLI::" + string.Join("::", typeDef.Namespace.Split('.'));
        }
        public static string CXXTypeName(this TypeReference typeReference)
        {
            if (typeReference.IsArray)
                return $"RTCLI::System::ElementArray<{typeReference.GetElementType().CXXTypeName()}>";
            if (typeReference.IsGenericInstance)
                return "UNIMPLEMENTED_CXX_TYPE_NAME";
            if (typeReference.IsGenericParameter)
                return typeReference.FullName;
            if (typeReference.IsPointer)
                return "UNIMPLEMENTED_CXX_TYPE_NAME";
            else return "RTCLI::" + string.Join("::", typeReference.FullName.Split('.', '/')).Replace("<>", "__").Replace('`', '_').Replace("<", "_").Replace(">", "_");
        }
        public static string CXXShortTypeName(this TypeReference typeReference)
        {
            if (typeReference.IsArray)
                return $"RTCLI::System::ElementArray<{typeReference.GetElementType().CXXTypeName()}>";
            if (typeReference.IsGenericInstance)
                return "UNIMPLEMENTED_CXX_SHORT_TYPE_NAME";
            if (typeReference.IsGenericParameter)
                return typeReference.FullName;
            if (typeReference.IsPointer)
                return "UNIMPLEMENTED_CXX_SHORT_TYPE_NAME";
            else return typeReference.Name.Replace("<>", "__").Replace('`', '_').Replace("<", "_").Replace(">", "_");
        }
        public static string CXXTemplateParam(this TypeReference typeReference)
        {
            var gTs  = typeReference.GenericParameters;
            return gTs != null ? string.Join(',', gTs.Select(a => $"class {a.CXXTypeName()}")) : "";
        }


        public static string NamespaceSequence(this TypeReference typeRef)
        {
            // Try Resolve
            var typeDef = typeRef.Resolve();
            if (typeDef != null)
            {
                return typeDef.NamespaceSequence();
            }
            return typeRef.Namespace;
        }
        public static string NamespaceSequence(this TypeDefinition typeDef)
        {
            return typeDef.Namespace;
        }
        public static string CXXNamespaceToPath(this TypeReference typeReference)
        {
            return typeReference.NamespaceSequence().Replace(".", "/");
        }
    }
}
