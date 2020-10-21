using System.Reflection;
using Mono.Cecil;
using Newtonsoft.Json;

namespace RTCLI.AOTCompiler.Metadata
{
    public interface IMetadataInformation
    {
        [JsonIgnore] IMetadataTokenProvider Definition { get; } 
        [JsonIgnore] MetadataContext MetadataContext { get; }
    }
}