using System;
using System.Linq;
using System.Runtime.InteropServices;
using Mono.Options;

namespace RTCLI.AOTCompiler3
{
    public class DispatchArgs
    {
        public DebugInformationOptions debugInformationOptions = DebugInformationOptions.None;
        public bool readSymbols = true;
        public bool enableBundler = false;
        public TargetPlatforms targetPlatform = TargetPlatforms.Generic;
        public bool cxxStaticAssertOnUnimplementatedILs = false;
        public bool recursivelyCompileAll = false;
    }

    class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                DispatchArgs dispatchArgs = new DispatchArgs();
                var help = false;

                var options = new OptionSet()
                {
                    { "g1|debug", "Emit debug informations (contains only comments)", v => dispatchArgs.debugInformationOptions = DebugInformationOptions.CommentOnly },
                    { "g|g2|debug-full", "Emit debug informations (contains line numbers)", v => dispatchArgs.debugInformationOptions = DebugInformationOptions.Full },
                    { "no-read-symbols", "NO read symbol files", _ => dispatchArgs.readSymbols = false },
                    { "bundler", "Produce bundler source file", _ => dispatchArgs.enableBundler = true },
                    { "target=", "Target platform [NativeCpp|InterpreterCode]", v => dispatchArgs.targetPlatform = Enum.TryParse<TargetPlatforms>(v, true, out var t) ? t : TargetPlatforms.Generic },
                    { "h|help", "Print this help", _ => help = true },
                    { "r|recursive", "Recursively Compile All Depended Assemblies", _ => dispatchArgs.recursivelyCompileAll = true },
                    { "assert-with-il-unimpl", "Gen CXX Static Assert On Unimplementated ILs", _ => dispatchArgs.cxxStaticAssertOnUnimplementatedILs = true },
                };

                var extra = options.Parse(args);
                if (help || (extra.Count < 2))
                {
                    Console.Out.WriteLine("usage: RTCLI.AOTCompiler.exe <output_path> <assembly_path> [options]");
                    options.WriteOptionDescriptions(Console.Out);
                }
                else
                {
                    var outputPath = extra[0];
                    var assemblyPaths = extra.Skip(1);
                    dispatchArgs.readSymbols = false;

                    Dispatcher.TranslateAll(
                        Console.Out,
                        outputPath,
                        dispatchArgs,
                        assemblyPaths
                    );
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
    }
}
