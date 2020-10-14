using System.Collections.Generic;
using System.IO;

namespace RTCLI.AOTCompiler
{
    public static class Dispatcher
    {
        public static void Translate(
            TextWriter logw,
            CodeTextStorage storage,
            bool readSymbols,
            bool enableBundler,
            TargetPlatforms targetPlatform,
            DebugInformationOptions debugInformationOptions,
            string assemblyPath)
        {
            logw.Write("IL2C: Preparing assembly: \"{0}\" ...", Path.GetFullPath(assemblyPath));

            var translateContext = new TranslateContext(assemblyPath, readSymbols);
            //var preparedFunctions = AssemblyPreparer.Prepare(translateContext);

            logw.WriteLine(" done.");

            using (var _ = storage.EnterScope("include"))
            {
                //AssemblyWriter.WriteHeader(
                //    storage,
                //    translateContext,
                //    preparedFunctions);
            }

            using (var _ = storage.EnterScope("src"))
            {
                //AssemblyWriter.WriteSourceCode(
                //    storage,
                //    translateContext,
                //    preparedFunctions,
                //    enableBundler,
                //    debugInformationOptions);
            }
        }

        public static void TranslateAll(
            TextWriter logw,
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
                    logw,
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
            CodeTextStorage storage,
            bool readSymbols,
            bool enableBundler,
            TargetPlatforms targetPlatform,
            DebugInformationOptions debugInformationOptions,
            params string[] assemblyPaths)
        {
            TranslateAll(
                logw,
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
                    logw,
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
