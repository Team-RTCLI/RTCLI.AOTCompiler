using Mono.Cecil;
using Mono.Cecil.Cil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTCLI.AOTCompiler.Metadata
{
    public partial class VariableInformation
    {
        public VariableInformation(VariableDefinition def, MetadataContext metadataContext)
        {
            Definition = def;
            MetadataContext = metadataContext;
        }

        public override string ToString() => Definition.ToString();
        public int Index => Definition.Index;
        public bool IsPinned => Definition.IsPinned;

        [JsonIgnore] public TypeInformation Type => MetadataContext.GetTypeInformation(Definition.VariableType);
        [JsonIgnore] public readonly VariableDefinition Definition = null;
        [JsonIgnore] public MetadataContext MetadataContext { get; }
    }
}
