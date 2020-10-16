using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;

namespace RTCLI.AOTCompiler.Metadata
{
    public class PropertyInformation : IMemberInformation
    {

        public PropertyInformation(PropertyDefinition def)
        {
            this.definition = def;

        }

        public readonly PropertyDefinition definition = null;
        public IMetadataTokenProvider Definition => definition;
    }
}