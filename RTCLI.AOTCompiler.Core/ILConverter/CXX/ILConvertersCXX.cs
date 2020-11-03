using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Text;
using RTCLI.AOTCompiler.Translators;
using Mono.Cecil;
using RTCLI.AOTCompiler.Metadata;

namespace RTCLI.AOTCompiler.ILConverters
{
    public class NopConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Nop;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext) => "RTCLI::nop();";
    }

    public class NewobjConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Newobj;
        private string ParseParams(Instruction instruction, MethodTranslateContext methodContext)
        {
            MethodReference calledMethod = instruction.Operand as MethodReference;
            var typeReference = calledMethod.DeclaringType;
            TypeInformation typeInformation = methodContext.TranslateContext.MetadataContext.GetTypeInformation(typeReference);
            string paramSequenceCXX = "stack"; 
            foreach(var param in calledMethod.Parameters)
            {
                paramSequenceCXX = paramSequenceCXX + ", " + (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            }
            return $"{typeInformation.CXXTypeName}& {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = \n\t\t*RTCLI::newobj<{typeInformation.CXXTypeName}>({paramSequenceCXX});";
        }
        public string Convert(Instruction instruction, MethodTranslateContext methodContext) => ParseParams(instruction, methodContext);
    }
    public class LdstrConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldstr;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext) 
            => $"const char* {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = \"{instruction.Operand}\";";
    }

    public class RetConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ret;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => $"return {(methodContext as CXXMethodTranslateContext).CmptStackPopAll};";
    }

    public class AddConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Add;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = RTCLI::Add({op0}, {op1});";
        }
    }

    public class SubConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Sub;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = RTCLI::Sub({op0}, {op1});";
        }
    }
    public class PopConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Pop;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            return $"RTCLI::Pop({(methodContext as CXXMethodTranslateContext).CmptStackPopObject});//pop operation";
        }
    }

    public class StfldConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stfld;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var val = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var obj = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"{obj}.{(instruction.Operand as FieldReference).Name} = {val};";
        }
    }
}
