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
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Add({op0}, {op1});";
        }
    }
    public class Add_OvfConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Add_Ovf;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Add_Ovf({op0}, {op1});";
        }
    }
    public class Add_Ovf_UnConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Add_Ovf_Un;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Add_Ovf({op0}, {op1});";
        }
    }

    public class NegConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Neg;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Neg({op0});";
        }
    }

    public class XorConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Xor;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Xor({op0}, {op1});";
        }
    }

    public class ShlConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Shl;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Shl({op0}, {op1});";
        }
    }

    public class ShrConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Shr;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Shr({op0}, {op1});";
        }
    }

    public class Shr_UnConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Shr_Un;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Shr_Un({op0}, {op1});";
        }
    }

    public class Mul_ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Mul;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Mul({op0}, {op1});";
        }
    }

    public class Mul_OvfConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Mul_Ovf;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Mul_Ovf({op0}, {op1});";
        }
    }

    public class Mul_Ovf_UnConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Mul_Ovf_Un;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Mul_Ovf_Un({op0}, {op1});";
        }
    }

    public class Div_ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Div;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Div({op0}, {op1});";
        }
    }

    public class Div_UnConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Div_Un;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Div_Un({op0}, {op1});";
        }
    }

    public class Rem_ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Rem;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Rem({op0}, {op1});";
        }
    }

    public class Rem_UnConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Rem_Un;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Rem_Un({op0}, {op1});";
        }
    }

    public class SubConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Sub;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Sub({op0}, {op1});";
        }
    }
    public class Sub_OvfConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Sub_Ovf;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Sub_Ovf({op0}, {op1});";
        }
    }
    public class Sub_Ovf_UnConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Sub_Ovf_Un;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Sub_Ovf({op0}, {op1});";
        }
    }

    public class AndConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.And;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::And({op0}, {op1});";
        }
    }

    public class OrConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Or;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Or({op0}, {op1});";
        }
    }

    public class NotConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Not;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Not({op0});";
        }
    }

    public class CkfiniteConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ckfinite;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Ckfinite({op0});";
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
            var op = instruction.Operand;

            if(op is PropertyReference prop)
            {
                return "RTCLI::unimplemented_il(\"stfld prop\");";
            }
            else if(op is FieldReference fld)
            {

                return $"{obj}.{Utilities.GetCXXValidTokenString(fld.Name)} = {val};";
            }
            return "";
        }
    }
}
