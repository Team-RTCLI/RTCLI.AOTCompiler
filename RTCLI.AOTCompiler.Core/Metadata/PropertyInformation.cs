using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Newtonsoft.Json;

namespace RTCLI.AOTCompiler.Metadata
{
    public class PropertyInformation : IMemberInformation
    {

        public PropertyInformation(PropertyDefinition def)
        {
            this.definition = def;

        }

        [JsonIgnore] private readonly PropertyDefinition definition = null;
        public IMetadataTokenProvider Definition => definition;
    }
}