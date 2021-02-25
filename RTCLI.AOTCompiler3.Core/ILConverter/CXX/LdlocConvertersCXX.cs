using Mono.Cecil.Cil;
using Mono.Cecil;
using System;
using System.Linq;
using RTCLI.AOTCompiler3.Meta;
using System.Collections.Generic;

namespace RTCLI.AOTCompiler3.ILConverters
{
    public class LdlocOnvert
    {
        public static string Convert(Instruction instruction, MethodTranslateContextCXX methodContext, int index)
        {
            var type = methodContext.Method.Body.Variables[index].VariableType;
            return $"auto& {methodContext.CmptStackPushObject} = v{index}{(type.IsValueType ? "" : ".Get()")};";
        }
    }

    public class Ldloc_0ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldloc_0;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdlocOnvert.Convert(instruction, methodContext, 0);
    }

    public class Ldloc_1ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldloc_1;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdlocOnvert.Convert(instruction, methodContext, 1);
    }
    public class Ldloc_2ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldloc_2;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdlocOnvert.Convert(instruction, methodContext, 2);
    }
    public class Ldloc_3ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldloc_3;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdlocOnvert.Convert(instruction, methodContext, 3);
    }
    public class LdlocConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldloc;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdlocOnvert.Convert(instruction, methodContext, (instruction.Operand as VariableDefinition).Index);
    }
    public class Ldloc_SConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldloc_S;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdlocOnvert.Convert(instruction, methodContext, (instruction.Operand as VariableDefinition).Index);
    }
    public class LdlocaConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldloca;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => $"auto& {methodContext.CmptStackPushObject} = RTCLI_ADDRESSOF(v{(instruction.Operand as VariableDefinition).Index});";
    }
    public class Ldloca_SConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldloca_S;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => $"auto& {methodContext.CmptStackPushObject} = RTCLI_ADDRESSOF(v{(instruction.Operand as VariableDefinition).Index});";
    }
}
