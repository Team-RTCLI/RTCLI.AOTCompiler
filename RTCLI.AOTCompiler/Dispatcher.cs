using RTCLI.AOTCompiler.Translators;
using System.Collections.Generic;
using System.IO;

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

            var translateContext = new TranslateContext(assemblyPath, dispatchArgs.readSymbols);
            CXXTranslateOptions cxxOptions = new CXXTranslateOptions();
            cxxOptions.StaticAssertOnUnimplementatedILs = dispatchArgs.cxxStaticAssertOnUnimplementatedILs;
            var cxxTranslator = new CXXTranslator(translateContext, cxxOptions);

            System.Console.WriteLine(" done.");
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
        }

        public static void TranslateAll(
            CodeTextStorage storage,
            DispatchArgs dispatchArgs,
            IEnumerable<string> assemblyPaths)
        {
            foreach (var aseemblyPath in assemblyPaths)
            {
                Translate(
                    storage,
                    dispatchArgs,
                    aseemblyPath);
            }
        }

        public static void TranslateAll(
            CodeTextStorage storage,
            DispatchArgs dispatchArgs,
            params string[] assemblyPaths)
        {
            TranslateAll(
                storage,
                dispatchArgs,
                (IEnumerable<string>)assemblyPaths);
        }

        public static void TranslateAll(
            TextWriter logw,
            string outputPath,
            DispatchArgs dispatchArgs,
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
                    dispatchArgs,
                    aseemblyPath);
            }
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
