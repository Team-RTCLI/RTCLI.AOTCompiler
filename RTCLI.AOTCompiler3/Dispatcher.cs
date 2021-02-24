using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RTCLI.AOTCompiler3
{
    public static class Dispatcher
    {
        public static void Translate(
            CodeTextStorage storage,
            DispatchArgs dispatchArgs,
            string assemblyPath)
        {
            System.Console.WriteLine("AOTCompiler: Preparing assembly: \"{0}\" ...", Path.GetFullPath(assemblyPath));

            //using (var _ = storage.EnterScope("meta"))
            //{
            //    MetadataSerializer metaSerializer = new MetadataSerializer(translateContext);
            //    metaSerializer.WriteResult(storage);
            //}

            using (var _ = storage.EnterScope("include"))
            {
                
            }
            using (var _ = storage.EnterScope("src"))
            {
                
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
