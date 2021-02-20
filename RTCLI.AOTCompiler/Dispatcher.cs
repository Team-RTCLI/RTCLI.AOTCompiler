using RTCLI.AOTCompiler.Translators;
using System.Collections.Generic;
using System.IO;
using RTCLI.AOTCompiler.Metadata;
using System.Threading.Tasks;

namespace RTCLI.AOTCompiler
{
    public static class Dispatcher
    {
        public static void Translate(
            CodeTextStorage storage,
            DispatchArgs dispatchArgs,
            string assemblyPath)
        {
            System.Console.WriteLine("AOTCompiler: Preparing assembly: \"{0}\" ...", Path.GetFullPath(assemblyPath));

            var metaContext = new MetadataContext(assemblyPath, dispatchArgs.readSymbols);
            var translateContext = new TranslateContext(assemblyPath, dispatchArgs.readSymbols, metaContext);
            CXXTranslateOptions cxxOptions = new CXXTranslateOptions();
            cxxOptions.StaticAssertOnUnimplementatedILs = dispatchArgs.cxxStaticAssertOnUnimplementatedILs;
            var cxxTranslator = new CXXTranslator(translateContext, cxxOptions);

            using (var _ = storage.EnterScope("meta"))
            {
                MetadataSerializer metaSerializer = new MetadataSerializer(translateContext);
                metaSerializer.WriteResult(storage);
            }

            using (var _ = storage.EnterScope("include"))
            {
                cxxTranslator.WriteHeader(storage);
            }
            using (var _ = storage.EnterScope("src"))
            {
                cxxTranslator.WriteSource(storage);
            }

            System.Console.WriteLine(" done.");
        }

        public static void TranslateAll(
            TextWriter logw,
            string outputPath,
            DispatchArgs dispatchArgs,
            IEnumerable<string> assemblyPaths)
        {
            Parallel.ForEach(assemblyPaths, aseemblyPath => {
                var storage = new CodeTextStorage(
                logw,
                outputPath,
                "    ");

                Translate(
                    storage,
                    dispatchArgs,
                    aseemblyPath);
            });
        }

        public static void TranslateAll(
            TextWriter logw,
            string outputPath,
            DispatchArgs dispatchArgs,
            params string[] assemblyPaths)
        {
            TranslateAll(
                logw,
                outputPath,
                dispatchArgs,
                (IEnumerable<string>)assemblyPaths);
        }
    }
}
