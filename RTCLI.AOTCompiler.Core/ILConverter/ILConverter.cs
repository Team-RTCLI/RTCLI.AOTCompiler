using Mono.Cecil.Cil;
using System.Dynamic;

namespace RTCLI.AOTCompiler.ILConverters
{
    public interface IILConverter
    {
        OpCode TargetOpCode();
        string Convert(Instruction instruction);
    }
}