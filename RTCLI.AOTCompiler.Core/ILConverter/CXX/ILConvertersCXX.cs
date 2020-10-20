using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTCLI.AOTCompiler.ILConverters
{
    public class NopConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction) => "RTCLI::nop();";

        public OpCode TargetOpCode() => OpCodes.Nop;
    }
}
