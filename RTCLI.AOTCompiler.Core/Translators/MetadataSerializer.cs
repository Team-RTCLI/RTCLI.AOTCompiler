

using Newtonsoft.Json;
using System.IO;

namespace RTCLI.AOTCompiler.Translators
{
    public class MetadataSerializer
    {
        public MetadataSerializer(TranslateContext translateContext)
        {
            this.translateContext = translateContext;
        }

        public void WriteResult(CodeTextStorage storage)
        {
            var assemblyInformation 
                = translateContext.MetadataContext.Assemblies[translateContext.FocusedAssembly];
            CodeTextWriter writer = storage.Wirter(
                assemblyInformation.IdentName + ".json"
            );
            writer.WriteLine(
                JsonConvert.SerializeObject(
                    translateContext.MetadataContext.Assemblies[translateContext.FocusedAssembly],
                    Formatting.Indented)
            );
            writer.Flush();
        }

        private readonly TranslateContext translateContext = null;
    }
}