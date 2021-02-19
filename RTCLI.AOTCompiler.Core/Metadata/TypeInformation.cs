using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Mono.Cecil;
using Newtonsoft.Json;

namespace RTCLI.AOTCompiler.Metadata
{
    public partial class TypeInformation : IMemberInformation
    {
        public string FullName => IsArray ? definitionArray.FullName : definition.FullName;
        public string Namespace => IsArray ? definitionArray.Namespace : definition.Namespace;
        public string TypeName => IsArray ? definitionArray.Name : definition.Name;
        public readonly string[] NamespaceChain = null;
        public readonly string[] TypeAttributes = null;

        public bool IsArray => definitionArray != null;
        public bool IsStruct => definition.IsValueType;

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

            TypeAttributes = def.Attributes.ToString().Split(sep, StringSplitOptions.RemoveEmptyEntries);
        }

        public TypeInformation(ArrayType def, MetadataContext metadataContext)
        {
            this.definitionArray = def;
            this.MetadataContext = metadataContext;
            var dd = definitionArray.ElementType;

            this.elementType = IsArray ? MetadataContext.GetTypeInformation(dd) : null;
            
        }

        private char[] sep = {',', ' '};

        [JsonIgnore] private readonly ArrayType definitionArray = null;
        [JsonIgnore] private readonly TypeInformation elementType = null;

        [JsonIgnore] private readonly TypeDefinition definition = null;
        [JsonIgnore] public IMetadataTokenProvider Definition => definition;
        public MetadataContext MetadataContext { get; }
    }
}