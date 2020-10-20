using Mono.Cecil.Cil;
using System.Dynamic;
using RTCLI.AOTCompiler.Translators;

namespace RTCLI.AOTCompiler.ILConverters
{
    public interface ICXXILConverter : IILConverter
    {
        string Note(Instruction instruction, MethodTranslateContext methodContext) => "// " + instruction.ToString();
    }
}