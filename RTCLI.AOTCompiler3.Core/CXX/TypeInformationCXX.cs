using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using System.Linq;
using System.Collections.Concurrent;

namespace RTCLI.AOTCompiler3.Meta
{
    public static class TypeInformationCXX
    {
        private static ConcurrentDictionary<string, List<TypeReference>> weakReferences = new ConcurrentDictionary<string, List<TypeReference>>();
        private static ConcurrentDictionary<string, List<TypeReference>> strongRefernces = new ConcurrentDictionary<string, List<TypeReference>>();
        private static ConcurrentDictionary<string, List<TypeReference>> nestedTypes = new ConcurrentDictionary<string, List<TypeReference>>();

        private static void solveTypeReferences(TypeDefinition Type)
        {
            List<TypeReference> weak = new List<TypeReference>(); 
            List<TypeReference> strong = new List<TypeReference>();
            List<TypeReference> All = new List<TypeReference>();
            var TNameCXX = Type.CXXTypeName();

            if(Type.BaseType != null)
                strong.Add(Type.BaseType);
            strong = strong.Union(Type.Interfaces.Select(i => i.InterfaceType)).ToList();

            All = All.Union(Type.Fields.Select(f => f.FieldType)).ToList();
            All = All.Union(Type.Properties.Select(p => p.PropertyType)).ToList();
            All = All.Union(Type.Methods.Select(m => m.ReturnType)).ToList();
            foreach (var Method in Type.Methods)
            {
                All = All.Union(Method.Parameters.Select(p => p.ParameterType)).ToList();
            }

            // remove self and nested types
            All = All.Except(Type.RecursedNestedTypes()).ToList();
            if(All.Contains(Type)) All.Remove(Type);

            All = All.Where((t, i) => All.FindIndex(rm => rm.FullName == t.FullName) == i).ToList();

            // value_type => strong
            // class => weak
            foreach (var T in All)
            {
                if (T.IsValueType) strong.Add(T);
                else weak.Add(T);
            }
            weakReferences.TryAdd(TNameCXX, weak);
            strongRefernces.TryAdd(TNameCXX, strong);
        }

        private static void solveNestedTypes(TypeDefinition Type)
        {
            List<TypeReference> allNested = new List<TypeReference>();
            var TNameCXX = Type.CXXTypeName();
            foreach (var Nested in Type.NestedTypes)
            {
                solveNestedTypes(Nested);
                allNested.Concat(nestedTypes[Nested.CXXTypeName()]);
                allNested.Add(Nested);
            }
            nestedTypes.TryAdd(TNameCXX, allNested);
        }

        public static List<TypeReference> RecursedNestedTypes(this TypeDefinition Type)
        {
            string TCXXTypeName = Type.CXXTypeName();
            if (nestedTypes.ContainsKey(TCXXTypeName))
            {
                return nestedTypes[TCXXTypeName];
            }
            solveNestedTypes(Type);
            return nestedTypes[TCXXTypeName];
        }

        public static List<TypeReference> WeakReferences(this TypeDefinition Type)
        {
            string TCXXTypeName = Type.CXXTypeName();
            if(weakReferences.ContainsKey(TCXXTypeName))
            {
                return weakReferences[TCXXTypeName];
            }
            solveTypeReferences(Type);
            return weakReferences[TCXXTypeName];
        }
        
        public static List<TypeReference> StrongRefernces(this TypeDefinition Type)
        {
            string TCXXTypeName = Type.CXXTypeName();
            if(strongRefernces.ContainsKey(TCXXTypeName))
            {
                return strongRefernces[TCXXTypeName];
            }
            solveTypeReferences(Type);
            return strongRefernces[TCXXTypeName];
        }

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
                return $"{elemT.CXXTypeName()}*";
            }
            else return "RTCLI::" + string.Join("::", typeReference.FullName.Split('.', '/')).Replace("<>", "__").Replace('`', '_').Replace("<", "_").Replace(">", "_");
        }
        public static string CXXShortTypeName(this TypeReference typeReference)
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
                return $"{elemT.CXXShortTypeName()}*";
            }
            else return typeReference.Name.Replace("<>", "__").Replace('`', '_').Replace("<", "_").Replace(">", "_");
        }

        public static string CallStaticConstructor(this TypeDefinition typeDefinition,
            MethodTranslateContextCXX methodContext)
        {
            MethodDefinition StaticConstructor = null;
            foreach (var method in typeDefinition.Methods)
            {
                if (method.IsStatic && method.IsConstructor)
                    StaticConstructor = method;
            }

            if (methodContext.Method == StaticConstructor)
                return "";

            if (!methodContext.StaticReference.Contains(typeDefinition))
            {
                methodContext.StaticReference.Add(typeDefinition);
                if (StaticConstructor != null)
                    return $"{StaticConstructor.CXXMethodCallName(typeDefinition)}();";
            }
            return "";
        }

        public static string CXXTemplateParam(this TypeReference typeReference)
        {
            var gTs  = typeReference.GenericParameters;
            return gTs != null ? string.Join(',', gTs.Select(a => $"class {a.CXXTypeName()}")) : "";
        }
        public static string CXXTemplateArg(this TypeReference typeReference)
        {
            var gTs  = typeReference.GenericParameters;
            return gTs != null ? string.Join(',', gTs.Select(a => a.CXXTypeName())) : "";
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
            return typeRef.Namespace;
        }

        public static string CXXNamespaceToPath(this TypeReference typeReference)
        {
            return typeReference.NamespaceSequence().Replace(".", "/");
        }
    }
}
