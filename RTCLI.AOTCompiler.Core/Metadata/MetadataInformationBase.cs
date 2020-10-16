using System.Reflection;
using Mono.Cecil;

namespace RTCLI.AOTCompiler.Metadata
{
    public interface IMetadataInformation
    {
        IMetadataTokenProvider Definition { get; } 
    }
}