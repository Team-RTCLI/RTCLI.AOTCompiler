using System;
using System.Linq;
using System.Runtime.InteropServices;

using Mono.Options;

namespace RTCLI.AOTCompiler
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                var debugInformationOptions = DebugInformationOptions.None;
                var readSymbols = true;
                var enableBundler = false;
                var targetPlatform = TargetPlatforms.Generic;
                var help = false;

                var options = new OptionSet()
                {
                    { "g1|debug", "Emit debug informations (contains only comments)", v => debugInformationOptions = DebugInformationOptions.CommentOnly },
                    { "g|g2|debug-full", "Emit debug informations (contains line numbers)", v => debugInformationOptions = DebugInformationOptions.Full },
                    { "no-read-symbols", "NO read symbol files", _ => readSymbols = false },
                    { "bundler", "Produce bundler source file", _ => enableBundler = true },
                    { "target=", "Target platform [generic|ue4]", v => targetPlatform = Enum.TryParse<TargetPlatforms>(v, true, out var t) ? t : TargetPlatforms.Generic },
                    { "h|help", "Print this help", _ => help = true },
                };

                var extra = options.Parse(args);
                if (help || (extra.Count < 2))
                {
                    Console.Out.WriteLine("usage: il2c.exe [options] <output_path> <assembly_path>");
                    options.WriteOptionDescriptions(Console.Out);
                }
                else
                {
                    var outputPath = extra[0];
                    var assemblyPaths = extra.Skip(1);

                    Dispatcher.TranslateAll(
                        Console.Out,
                        outputPath,
                        readSymbols,
                        enableBundler,
                        targetPlatform,
                        debugInformationOptions,
                        assemblyPaths);
                }

                return 0;
            }
            catch (OptionException ex)
            {
                Console.Error.WriteLine(ex.Message);
                return Marshal.GetHRForException(ex);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return Marshal.GetHRForException(ex);
            }
        }

        public TranslateContext translateContext = null;
    }
}
