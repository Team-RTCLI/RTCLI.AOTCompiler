using Mono.Cecil.Cil;
using System.Dynamic;
using RTCLI.AOTCompiler3.Translators;
using RTCLI.AOTCompiler3.Meta;

namespace RTCLI.AOTCompiler3.ILConverters
{
    public interface ICXXILConverter : IILConverter
    {
        string Note(Instruction instruction, MethodTranslateContextCXX methodContext) 
            => "// " + instruction.ToString().HoldEscape();
    }
}