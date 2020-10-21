using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Newtonsoft.Json;

namespace RTCLI.AOTCompiler.Metadata
{
    public class PropertyInformation : IMemberInformation
    {
        public PropertyInformation(PropertyDefinition def, MetadataContext metadataContext)
        {
            this.definition = def;
            this.MetadataContext = metadataContext;
        }

        [JsonIgnore] private readonly PropertyDefinition definition = null;
        public IMetadataTokenProvider Definition => definition;
        public MetadataContext MetadataContext { get; }
    }
}