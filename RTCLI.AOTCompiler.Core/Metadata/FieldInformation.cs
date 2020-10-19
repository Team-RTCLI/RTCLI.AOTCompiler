using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Newtonsoft.Json;

namespace RTCLI.AOTCompiler.Metadata
{
    public class FieldInformation : IMemberInformation
    {
        public readonly string FullName = "None";
        public readonly string Name = "None";
        public readonly string FieldType = null;

        public FieldInformation(FieldDefinition def)
        {
            this.definition = def;

            FieldType = def.FieldType.FullName;
            FullName = def.FullName;
            Name = def.Name;
        }

        [JsonIgnore] private readonly FieldDefinition definition = null;
        public IMetadataTokenProvider Definition => definition;
    }
}