using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Mono.Cecil;
using Newtonsoft.Json;
using RTCLI.AOTCompiler.Internal;

namespace RTCLI.AOTCompiler.Metadata
{
    public class TypeInformation : IMemberInformation
    {
        public string FullName => IsArray ? definitionArray.FullName : definition.FullName;
        public string TypeName => IsArray ? definitionArray.Name : definition.Name;
        public readonly string[] NamespaceChain = null;
        public readonly string[] TypeAttributes = null;

        public bool IsArray => definitionArray != null;

        public string CXXTypeName => "RTCLI::" + string.Join("::", FullName.Split('.'));

        public readonly List<MethodInformation> Methods = new List<MethodInformation>();
        public readonly List<FieldInformation> Fields = new List<FieldInformation>();
        public readonly List<PropertyInformation> Properties = new List<PropertyInformation>();
        public TypeInformation GetElementType() => IsArray ? MetadataContext.GetTypeInformation(definitionArray.GetElementType()) : null;

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

            TypeAttributes = def.Attributes.ToString().Split(sep, StringSplitOptions.RemoveEmptyEntries);
        }

        public TypeInformation(ArrayType def, MetadataContext metadataContext)
        {
            definitionArray = def;
            this.MetadataContext = metadataContext;

        }

        private char[] sep = {',', ' '};

        [JsonIgnore] private readonly ArrayType definitionArray = null;
        [JsonIgnore] private readonly TypeDefinition definition = null;
        [JsonIgnore] public IMetadataTokenProvider Definition => definition;
        public MetadataContext MetadataContext { get; }
    }
}