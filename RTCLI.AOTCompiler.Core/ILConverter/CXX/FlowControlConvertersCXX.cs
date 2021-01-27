using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Text;
using RTCLI.AOTCompiler.Translators;
using Mono.Cecil;
using RTCLI.AOTCompiler.Metadata;

namespace RTCLI.AOTCompiler.ILConverters
{
    public class Brfalse_SConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"if (!{op}) goto {instruction.GetLabel()};";
        }

        public OpCode TargetOpCode() => OpCodes.Brfalse_S;
    }
}