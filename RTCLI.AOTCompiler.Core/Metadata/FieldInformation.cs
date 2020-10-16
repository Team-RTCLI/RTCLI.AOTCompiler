using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;

namespace RTCLI.AOTCompiler.Metadata
{
    public class FieldInformation : IMemberInformation
    {

        public FieldInformation(FieldDefinition def)
        {
            this.definition = def;

        }

        public readonly FieldDefinition definition = null;
        public IMetadataTokenProvider Definition => definition;
    }
}