using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Text;
using RTCLI.AOTCompiler.Translators;
using Mono.Cecil;
using RTCLI.AOTCompiler.Metadata;

namespace RTCLI.AOTCompiler.ILConverters
{
    public class Starg_ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Starg;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => $"{methodContext.MethodInfo.Parameters[(instruction.Operand as Mono.Cecil.Cil.VariableDefinition).Index].Name} = {(methodContext as CXXMethodTranslateContext).CmptStackPopObject};";
    }
    public class Starg_SConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Starg_S;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => $"{methodContext.MethodInfo.Parameters[(instruction.Operand as Mono.Cecil.Cil.VariableDefinition).Index].Name} = {(methodContext as CXXMethodTranslateContext).CmptStackPopObject};";
    }
}
