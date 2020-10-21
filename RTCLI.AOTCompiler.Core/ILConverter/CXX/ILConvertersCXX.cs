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
            string paramSequenceCXX = $"{typeInformation.CXXTypeName}";
            foreach(var param in calledMethod.Parameters)
            {
                paramSequenceCXX = paramSequenceCXX + ", " + (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            }
            return $"RTCLI::newobj({paramSequenceCXX})";
        }
        public string Convert(Instruction instruction, MethodTranslateContext methodContext) => ParseParams(instruction, methodContext);
    }

    public class Ldarg_0ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldarg_0;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext) 
            => $"RTCLI::object_ref {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = *static_cast<RTCLI::object*>(this);";
    }

    public class LdstrConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldstr;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext) 
            => $"const char* {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = \"{instruction.Operand}\";";
    }
}
