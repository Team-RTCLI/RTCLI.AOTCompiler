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

        public string CXXTypeName => FullName;

        public readonly Dictionary<string, MethodInformation> Methods = new Dictionary<string, MethodInformation>();
        public readonly Dictionary<string, FieldInformation> Fields = new Dictionary<string, FieldInformation>();
        public readonly Dictionary<string, PropertyInformation> Properties = new Dictionary<string, PropertyInformation>();
        public TypeInformation(TypeDefinition def)
        {
            this.definition = def;
            FullName = def.FullName;
            TypeName = def.Name;
            NamespaceChain = def.Namespace.Split('.');

            foreach (var method in def.Methods)
                Methods.Add(method.FullName, new MethodInformation(method));
            foreach(var prop in def.Properties)
                Properties.Add(prop.FullName, new PropertyInformation(prop));
            foreach(var field in def.Fields)
                Fields.Add(field.FullName, new FieldInformation(field));

            TypeAttributes = def.Attributes.ToString().Split(sep, StringSplitOptions.RemoveEmptyEntries);
        }

        private char[] sep = {',', ' '};

        [JsonIgnore] private readonly TypeDefinition definition = null;
        [JsonIgnore] public IMetadataTokenProvider Definition => definition;
    }
}