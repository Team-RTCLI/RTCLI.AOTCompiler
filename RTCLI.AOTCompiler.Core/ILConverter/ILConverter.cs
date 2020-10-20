using Mono.Cecil.Cil;
using System.Dynamic;
using RTCLI.AOTCompiler.Translators;

namespace RTCLI.AOTCompiler.ILConverters
{
    public interface IILConverter
    {
        OpCode TargetOpCode();
        string Convert(Instruction instruction, MethodTranslateContext methodContext);
    }
}