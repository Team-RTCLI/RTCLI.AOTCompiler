using Mono.Cecil.Cil;
using System.Dynamic;
using RTCLI.AOTCompiler3.Translators;
using RTCLI.AOTCompiler3.Meta;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace RTCLI.AOTCompiler3.ILConverters
{
    public class NopConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Nop;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext) 
            => "RTCLI::nop();";
    }
}