using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Text;
using RTCLI.AOTCompiler.Translators;
using Mono.Cecil;
using RTCLI.AOTCompiler.Metadata;
using System.Reflection;

namespace RTCLI.AOTCompiler.ILConverters
{
    public class LdlocOnvert
    {
        public static string Convert(Instruction instruction, MethodTranslateContext methodContext, int index)
        {
            bool VT = methodContext.MethodInfo.LocalVariables[index].Type.IsValueType;
            return $"auto& {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = v{index}{(VT?"" : ".Get()")};";
        }
    }

    public class Ldloc_0ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldloc_0;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => LdlocOnvert.Convert(instruction, methodContext, 0);
    }

    public class Ldloc_1ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldloc_1;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => LdlocOnvert.Convert(instruction, methodContext, 1);
    }
    public class Ldloc_2ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldloc_2;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => LdlocOnvert.Convert(instruction, methodContext, 2);
    }
    public class Ldloc_3ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldloc_3;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => LdlocOnvert.Convert(instruction, methodContext, 3);
    }
    public class LdlocConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldloc;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => LdlocOnvert.Convert(instruction, methodContext, (instruction.Operand as VariableDefinition).Index);
    }
    public class Ldloc_SConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldloc_S;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => LdlocOnvert.Convert(instruction, methodContext, (instruction.Operand as VariableDefinition).Index);
    }
    public class LdlocaConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldloca;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => $"auto& {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = RTCLI_ADDRESSOF(v{(instruction.Operand as VariableDefinition).Index});";
    }
    public class Ldloca_SConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldloca_S;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => $"auto& {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = RTCLI_ADDRESSOF(v{(instruction.Operand as VariableDefinition).Index});";
    }
}
