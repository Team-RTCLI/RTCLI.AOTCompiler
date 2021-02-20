using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Mono.Cecil;
using System.Linq;
using Newtonsoft.Json;

namespace RTCLI.AOTCompiler.Metadata
{
    public partial class TypeInformation : IMemberInformation
    {
        public string FullName => IsArray ? definitionArray.FullName : IsGenericInstance ? definitionGI.FullName : IsGenericParameter ? definitionGP.FullName : definition.FullName;
        public string Namespace => IsArray ? definitionArray.Namespace : IsGenericInstance ? definitionGI.Namespace : IsGenericParameter ? definitionGP.Namespace : definition.Namespace;
        public string TypeName => IsArray ? definitionArray.Name : IsGenericInstance ? definitionGI.Name : IsGenericParameter ? definitionGP.Name : definition.Name;
        public readonly string[] NamespaceChain = null;
        public readonly string[] TypeAttributes = null;

        public bool IsGenericParameter => definitionGP != null;
        public bool IsArray => definitionArray != null;
        public bool IsStruct => definition!=null ? definition.IsValueType : false;
        public bool IsGenericInstance => definitionGI != null;
        public bool HasGenericParameters => genericParameterTypes != null ? genericParameterTypes.Length > 0 : false;

        public readonly List<MethodInformation> Methods = new List<MethodInformation>();
        public readonly List<FieldInformation> Fields = new List<FieldInformation>();
        public readonly List<PropertyInformation> Properties = new List<PropertyInformation>();
        public readonly List<TypeInformation> Nested = new List<TypeInformation>();
        public TypeInformation GetElementType() => elementType;

        public TypeInformation(TypeDefinition def, MetadataContext metadataContext)
        {
            this.definition = def;
            this.MetadataContext = metadataContext;

            NamespaceChain = def.Namespace.Split('.');

            foreach (var method in def.Methods)
                Methods.Add(new MethodInformation(method, metadataContext));
            foreach(var prop in def.Properties)
                Properties.Add(new PropertyInformation(prop, metadataContext));
            foreach(var field in def.Fields)
                Fields.Add(new FieldInformation(field, metadataContext));
            foreach(var nested in def.NestedTypes)
                Nested.Add(new TypeInformation(nested, metadataContext));

            this.genericParameterTypes = def.GenericParameters.Select(a => MetadataContext.GetTypeInformation(a)).ToArray();
            TypeAttributes = def.Attributes.ToString().Split(sep, StringSplitOptions.RemoveEmptyEntries);
        }

        public TypeInformation(ArrayType def, MetadataContext metadataContext)
        {
            this.definitionArray = def;
            this.MetadataContext = metadataContext;
            var dd = definitionArray.ElementType;

            this.elementType = IsArray ? MetadataContext.GetTypeInformation(dd) : null;
            
        }


        public TypeInformation(GenericParameter def, MetadataContext metadataContext)
        {
            this.definitionGP = def;
            this.MetadataContext = metadataContext;
        }

        public TypeInformation(GenericInstanceType def, MetadataContext metadataContext)
        {
            this.definitionGI = def;
            this.definition = metadataContext.GetTypeInformation(def.ElementType).definition;
            this.MetadataContext = metadataContext;
            var dd = def.ElementType;
            this.genericDeclaringType = IsGenericInstance ? MetadataContext.GetTypeInformation(dd) : null;
            this.genericArgumentTypes = def.GenericArguments.Select(a => MetadataContext.GetTypeInformation(a)).ToArray();


            foreach (var method in definition.Methods)
                Methods.Add(new MethodInformation(method, metadataContext));
            foreach (var prop in definition.Properties)
                Properties.Add(new PropertyInformation(prop, metadataContext));
            foreach (var field in definition.Fields)
                Fields.Add(new FieldInformation(field, metadataContext));
            foreach (var nested in definition.NestedTypes)
                Nested.Add(new TypeInformation(nested, metadataContext));

            TypeAttributes = definition.Attributes.ToString().Split(sep, StringSplitOptions.RemoveEmptyEntries);
        }

        private char[] sep = {',', ' '};

        [JsonIgnore] private readonly ArrayType definitionArray = null;
        [JsonIgnore] private readonly TypeInformation elementType = null;
        [JsonIgnore] private readonly GenericInstanceType definitionGI = null;
        [JsonIgnore] private readonly GenericParameter definitionGP = null;
        
        [JsonIgnore] private readonly TypeInformation genericDeclaringType = null;
        [JsonIgnore] private readonly TypeInformation[] genericArgumentTypes = null;
        [JsonIgnore] private readonly TypeInformation[] genericParameterTypes = null;

        [JsonIgnore] private readonly TypeDefinition definition = null;
        [JsonIgnore] public IMetadataTokenProvider Definition => definition;
        public MetadataContext MetadataContext { get; }
    }
}