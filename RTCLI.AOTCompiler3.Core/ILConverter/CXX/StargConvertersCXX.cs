using Mono.Cecil.Cil;
using Mono.Cecil;
using System;
using System.Linq;
using RTCLI.AOTCompiler3.Meta;
using System.Collections.Generic;

namespace RTCLI.AOTCompiler3.ILConverters
{
    public class Starg_ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Starg;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => $"{methodContext.Method.Parameters[(instruction.Operand as ParameterDefinition).Index].Name} = {methodContext.CmptStackPopObject};";
    }
    public class Starg_SConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Starg_S;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => $"{methodContext.Method.Parameters[(instruction.Operand as ParameterDefinition).Index].Name} = {methodContext.CmptStackPopObject};";
    }
}
