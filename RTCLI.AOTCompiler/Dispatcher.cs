using RTCLI.AOTCompiler.Translators;
using System.Collections.Generic;
using System.IO;

namespace RTCLI.AOTCompiler
{
    public static class Dispatcher
    {
        public static void Translate(
            CodeTextStorage storage,
            bool readSymbols,
            bool enableBundler,
            TargetPlatforms targetPlatform,
            DebugInformationOptions debugInformationOptions,
            string assemblyPath)
        {
            System.Console.WriteLine("IL2C: Preparing assembly: \"{0}\" ...", Path.GetFullPath(assemblyPath));

            var translateContext = new TranslateContext(assemblyPath, readSymbols);

            System.Console.WriteLine(" done.");
            using (var _ = storage.EnterScope("meta"))
            {
                MetadataSerializer metaSerializer = new MetadataSerializer(translateContext);
                metaSerializer.WriteResult(storage);
            }

            using (var _ = storage.EnterScope("include"))
            {
                
            }

            using (var _ = storage.EnterScope("src"))
            {
                
            }
        }

        public static void TranslateAll(
            CodeTextStorage storage,
            bool readSymbols,
            bool enableBundler,
            TargetPlatforms targetPlatform,
            DebugInformationOptions debugInformationOptions,
            IEnumerable<string> assemblyPaths)
        {
            foreach (var aseemblyPath in assemblyPaths)
            {
                Translate(
                    storage,
                    readSymbols,
                    enableBundler,
                    targetPlatform,
                    debugInformationOptions,
                    aseemblyPath);
            }
        }

        public static void TranslateAll(
            CodeTextStorage storage,
            bool readSymbols,
            bool enableBundler,
            TargetPlatforms targetPlatform,
            DebugInformationOptions debugInformationOptions,
            params string[] assemblyPaths)
        {
            TranslateAll(
                storage,
                readSymbols,
                enableBundler,
                targetPlatform,
                debugInformationOptions,
                (IEnumerable<string>)assemblyPaths);
        }

        public static void TranslateAll(
            TextWriter logw,
            string outputPath,
            bool readSymbols,
            bool enableBundler,
            TargetPlatforms targetPlatform,
            DebugInformationOptions debugInformationOptions,
            IEnumerable<string> assemblyPaths)
        {
            var storage = new CodeTextStorage(
                logw,
                outputPath,
                "    ");

            foreach (var aseemblyPath in assemblyPaths)
            {
                Translate(
                    storage,
                    readSymbols,
                    enableBundler,
                    targetPlatform,
                    debugInformationOptions,
                    aseemblyPath);
            }
        }

        public static void TranslateAll(
            TextWriter logw,
            string outputPath,
            bool readSymbols,
            bool enableBundler,
            TargetPlatforms targetPlatform,
            DebugInformationOptions debugInformationOptions,
            params string[] assemblyPaths)
        {
            TranslateAll(
                logw,
                outputPath,
                readSymbols,
                enableBundler,
                targetPlatform,
                debugInformationOptions,
                (IEnumerable<string>)assemblyPaths);
        }
    }
}
