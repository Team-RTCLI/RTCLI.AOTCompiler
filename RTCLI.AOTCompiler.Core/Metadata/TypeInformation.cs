using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;

namespace RTCLI.AOTCompiler.Metadata
{
    public class TypeInformation : IMemberInformation
    {
        public readonly Dictionary<MethodDefinition, MethodInformation> Methods = new Dictionary<MethodDefinition, MethodInformation>();
        public readonly Dictionary<FieldDefinition, FieldInformation> Fields = new Dictionary<FieldDefinition, FieldInformation>();
        public readonly Dictionary<PropertyDefinition, PropertyInformation> Properties = new Dictionary<PropertyDefinition, PropertyInformation>();
        public TypeInformation(TypeDefinition def)
        {
            this.definition = def;
            
            foreach(var method in def.Methods)
            {
                Methods.Add(method, new MethodInformation(method));
            }
            foreach(var prop in def.Properties)
            {
                Properties.Add(prop, new PropertyInformation(prop));
            }
            foreach(var field in def.Fields)
            {
                Fields.Add(field, new FieldInformation(field));
            }
            Console.WriteLine("");
        }

        public readonly TypeDefinition definition = null;
        public IMetadataTokenProvider Definition => definition;
    }
}