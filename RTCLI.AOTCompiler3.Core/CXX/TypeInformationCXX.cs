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
            if(typeDef.Namespace == null || typeDef.Namespace.Length == 0)
            {
                return "RTCLI";
            }
            return "RTCLI::" + string.Join("::", typeDef.Namespace.Split('.'));
        }
        private static string GenericInstanceString(this TypeReference genericInstance)
        {
            var elemT = genericInstance.GetElementType();
            GenericInstanceType def = genericInstance as GenericInstanceType;

            var genericArgumentTypes = def.GenericArguments;
            return $"{elemT.CXXTypeName()}<{string.Join(',', genericArgumentTypes.Select(a => a.CXXTypeName()))}>";
        }
        public static string CXXTypeName(this TypeReference typeReference)
        {
            if (typeReference.IsArray)
            {
                var elemT = typeReference.GetElementType();
                return $"RTCLI::System::ElementArray<{elemT.CXXTypeName()}>";
            }
            if (typeReference.IsGenericInstance)
                return typeReference.GenericInstanceString();
            if (typeReference.IsGenericParameter)
                return typeReference.FullName;

            if (typeReference.IsPointer)
            {
                var elemT = typeReference.GetElementType();
                return $"{elemT}*";
            }
            else return "RTCLI::" + string.Join("::", typeReference.FullName.Split('.', '/')).Replace("<>", "__").Replace('`', '_').Replace("<", "_").Replace(">", "_");
        }
        public static string CXXShortTypeName(this TypeReference typeReference)
        {
            var elemT = typeReference.GetElementType();
            if (typeReference.IsArray)
                return $"RTCLI::System::ElementArray<{typeReference.GetElementType().CXXTypeName()}>";
            if (typeReference.IsGenericInstance)
                return typeReference.GenericInstanceString();
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
        public static List<InterfaceImplementation> InterfacesSolved(this TypeDefinition type)
        {
            Mono.Collections.Generic.Collection<InterfaceImplementation> toRemove = new Mono.Collections.Generic.Collection<InterfaceImplementation>();
            foreach (var i in type.Interfaces)
            {
                var InterfaceDef = i.InterfaceType.Resolve();
                var Interfaces = InterfaceDef.Interfaces.Select(a => a.InterfaceType);
                foreach (var ii in type.Interfaces)
                {
                    if (Interfaces.Contains(ii.InterfaceType))
                    {
                        toRemove.Add(ii);
                    }
                }
            }
            return type.Interfaces.Except(toRemove).ToList();
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
