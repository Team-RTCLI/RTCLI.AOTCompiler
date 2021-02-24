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
        public string FullName => IsReference ? reference.FullName : definition.FullName;
        public string Namespace => IsReference ? reference.FullName : definition.Namespace;
        public string TypeName => IsReference ? reference.FullName : definition.Name;
        public readonly string[] NamespaceChain = null;
        public readonly string[] TypeAttributes = null;

        public bool IsValueType => definition == null ? false : definition.IsValueType;
        public bool IsReference => reference != null;
        public bool IsGenericParameter => reference == null ? false : reference.IsGenericParameter;
        public bool IsArray => reference == null ? false : reference.IsArray;
        public bool IsStruct => definition!=null ? definition.IsValueType : false;
        public bool IsGenericInstance => reference == null ? false : reference.IsGenericInstance;
        public bool HasGenericParameters => definition == null ? false : definition.HasGenericParameters;

        public readonly List<MethodInformation> Methods = new List<MethodInformation>();
        public readonly List<FieldInformation> Fields = new List<FieldInformation>();
        public readonly List<PropertyInformation> Properties = new List<PropertyInformation>();
        public readonly List<TypeInformation> Nested = new List<TypeInformation>();
        public readonly List<TypeInformation> Interfaces = new List<TypeInformation>();
        public TypeInformation BaseType = null;
        public readonly MethodInformation StaticConstructor = null;
        public TypeInformation GetElementType() => elementType;

        public void SetupBaseAndInterface(MetadataContext metadataContext)
        {
            if (definition == null || IsGenericInstance)
                return;
            foreach (var Interface in definition.Interfaces)
                Interfaces.Add(MetadataContext.GetTypeInformation(Interface.InterfaceType));
            if(definition.BaseType!=null)
                BaseType = MetadataContext.GetTypeInformation(definition.BaseType);
        }

        public TypeInformation(TypeDefinition def, MetadataContext metadataContext)
        {
            this.definition = def;
            this.MetadataContext = metadataContext;
            NamespaceChain = def.Namespace.Split('.');

            foreach (var method in def.Methods)
            {
                var mtd = new MethodInformation(method, metadataContext);
                if (mtd.IsStaticConstructor)
                    StaticConstructor = mtd;
                Methods.Add(mtd);
            }
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
            reference = def;
            this.definitionArray = def;
            this.MetadataContext = metadataContext;
            var dd = definitionArray.ElementType;

            this.elementType = IsArray ? MetadataContext.GetTypeInformation(dd) : null;
            
        }

        public TypeInformation(GenericParameter def, MetadataContext metadataContext)
        {
            reference = def;
            this.definitionGP = def;
            this.MetadataContext = metadataContext;
        }

        public TypeInformation(GenericInstanceType def, MetadataContext metadataContext)
        {
            reference = def;
            this.definitionGI = def;
            this.MetadataContext = metadataContext;
            var ElementType = metadataContext.GetTypeInformation(def.ElementType);
            this.genericElementType = ElementType;
            this.genericArgumentTypes = def.GenericArguments.Select(a => MetadataContext.GetTypeInformation(a)).ToArray();
            this.definition = ElementType.definition;
            Methods = ElementType.Methods;
            StaticConstructor = ElementType.StaticConstructor;
            Properties = ElementType.Properties;
            Fields = ElementType.Fields;
            Nested = ElementType.Nested;
            TypeAttributes = ElementType.TypeAttributes;
        }

        private char[] sep = {',', ' '};

        [JsonIgnore] private readonly TypeReference reference = null;
        
        [JsonIgnore] private readonly ArrayType definitionArray = null;
        [JsonIgnore] private readonly TypeInformation elementType = null;
        [JsonIgnore] private readonly GenericInstanceType definitionGI = null;
        [JsonIgnore] private readonly GenericParameter definitionGP = null;
        
        [JsonIgnore] private readonly TypeInformation genericElementType = null;
        [JsonIgnore] private readonly TypeInformation[] genericArgumentTypes = null;
        [JsonIgnore] private readonly TypeInformation[] genericParameterTypes = null;

        [JsonIgnore] private readonly TypeDefinition definition = null;
        [JsonIgnore] public IMetadataTokenProvider Definition => definition;
        public MetadataContext MetadataContext { get; }
    }
}