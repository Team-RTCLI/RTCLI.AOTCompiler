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
        public readonly string FullName = "None";
        public readonly string TypeName = "None";
        public readonly string[] NamespaceChain = null;
        public readonly string[] TypeAttributes = null;

        public readonly Dictionary<MethodDefinition, MethodInformation> Methods = new Dictionary<MethodDefinition, MethodInformation>();
        public readonly Dictionary<FieldDefinition, FieldInformation> Fields = new Dictionary<FieldDefinition, FieldInformation>();
        public readonly Dictionary<PropertyDefinition, PropertyInformation> Properties = new Dictionary<PropertyDefinition, PropertyInformation>();
        public TypeInformation(TypeDefinition def)
        {
            this.definition = def;
            FullName = def.FullName;
            TypeName = def.Name;
            NamespaceChain = def.Namespace.Split('.');

            foreach (var method in def.Methods)
                Methods.Add(method, new MethodInformation(method));
            foreach(var prop in def.Properties)
                Properties.Add(prop, new PropertyInformation(prop));
            foreach(var field in def.Fields)
                Fields.Add(field, new FieldInformation(field));

            TypeAttributes = def.Attributes.ToString().Split(sep, StringSplitOptions.RemoveEmptyEntries);
        }

        private char[] sep = {',', ' '};

        [JsonIgnore] private readonly TypeDefinition definition = null;
        [JsonIgnore] public IMetadataTokenProvider Definition => definition;
    }
}