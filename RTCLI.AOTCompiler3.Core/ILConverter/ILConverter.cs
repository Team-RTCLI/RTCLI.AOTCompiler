using Mono.Cecil.Cil;
using System.Dynamic;
using RTCLI.AOTCompiler3.Translators;

namespace RTCLI.AOTCompiler3.ILConverters
{
    public interface IILConverter
    {
        OpCode TargetOpCode();
    }
}