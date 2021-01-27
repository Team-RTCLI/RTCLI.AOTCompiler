using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Newtonsoft.Json;

namespace RTCLI.AOTCompiler.Metadata
{
    public partial class FieldInformation : IMemberInformation
    {
        public readonly string FullName = "None";
        public string Name => definition.Name;
        public readonly string FieldType = null;

        public FieldInformation(FieldDefinition def, MetadataContext metadataContext)
        {
            this.definition = def;
            this.MetadataContext = metadataContext;

            FieldType = def.FieldType.FullName;
            FullName = def.FullName;
        }

        public bool IsPrivate => definition.IsPrivate;
        public bool IsStatic => definition.IsStatic;
        public bool IsPublic => definition.IsPublic;
        public bool IsFamily => definition.IsFamily;

        [JsonIgnore] private readonly FieldDefinition definition = null;
        public IMetadataTokenProvider Definition => definition;
        public MetadataContext MetadataContext { get; }
    }
}