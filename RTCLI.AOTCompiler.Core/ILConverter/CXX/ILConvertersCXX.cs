using Mono.Cecil.Cil;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using RTCLI.AOTCompiler.Translators;
using Mono.Cecil;
using RTCLI.AOTCompiler.Metadata;
using System.Text.RegularExpressions;

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
            string paramSequenceCXX = string.Join(',', calledMethod.Parameters.Select(_ => (methodContext as CXXMethodTranslateContext).CmptStackPopObject).Reverse()); 
            return $"{typeInformation.CXXTypeName}& {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = \n\t\t*RTCLI::new_object<{typeInformation.CXXTypeName}>({paramSequenceCXX});";
        }
        public string Convert(Instruction instruction, MethodTranslateContext methodContext) => ParseParams(instruction, methodContext);
    }
    public class InitobjConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Initobj;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var typeReference = instruction.Operand as TypeReference;
            TypeInformation typeInformation = methodContext.TranslateContext.MetadataContext.GetTypeInformation(typeReference);
            return $"RTCLI::initialize_object<{typeInformation.CXXTypeName}>({(methodContext as CXXMethodTranslateContext).CmptStackPopObject});";
        }
    }
    public class CpobjConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Cpobj;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var typeReference = instruction.Operand as TypeReference;
            var typeDefinition = instruction.Operand as TypeDefinition;

            TypeInformation typeInformation = methodContext.TranslateContext.MetadataContext.GetTypeInformation(typeReference);
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"RTCLI::cpobj<{typeInformation.CXXTypeName}>({op1}, {op2});";
        }
    }
    public class DupConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Dup;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPeek;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = RTCLI::dup({op1});";
        }
    }
    public class InitblkConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Initblk;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op3 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"RTCLI::initblk({op1}, {op2}, {op3});";
        }
    }
    public class CpblkConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Cpblk;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op3 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"RTCLI::cpblk({op1}, {op2}, {op3});";
        }
    }
    public class NewarrConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Newarr;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var typeReference = instruction.Operand as TypeReference;
            var len = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            TypeInformation typeInformation = methodContext.TranslateContext.MetadataContext.GetTypeInformation(typeReference);
            return $"RTCLI::ElementArray<{typeInformation.CXXTypeName}>& {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = \n" +
                $"\t\t*RTCLI::newarr<{typeInformation.CXXTypeName}>({len});";
        }
    }
    public class LdlenConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldlen;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var arr = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = RTCLI::ArrayLen({arr});";
        }
    }
    public class IsinstConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Isinst;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var typeReference = instruction.Operand as TypeReference;
            TypeInformation typeInformation = methodContext.TranslateContext.MetadataContext.GetTypeInformation(typeReference);
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = \n" +
                $"\t\tRTCLI::Isinst<{typeInformation.CXXTypeName}>({(methodContext as CXXMethodTranslateContext).CmptStackPopObject});";
        }
    }
    public class CastclassConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Castclass;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var typeReference = instruction.Operand as TypeReference;
            TypeInformation typeInformation = methodContext.TranslateContext.MetadataContext.GetTypeInformation(typeReference);
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = \n" +
                $"\t\tRTCLI::Castclass<{typeInformation.CXXTypeName}>({(methodContext as CXXMethodTranslateContext).CmptStackPopObject});";
        }
    }

    public class LdstrConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldstr;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext) 
            => $"const RTCLI::System::String {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = " +
            $"RTCLI_NATIVE_STRING(\"{instruction.Operand.ToString().HoldEscape()}\");";
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
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Add({op1}, {op2});";
        }
    }
    public class Add_OvfConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Add_Ovf;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Add_Ovf({op1}, {op2});";
        }
    }
    public class Add_Ovf_UnConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Add_Ovf_Un;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Add_Ovf({op1}, {op2});";
        }
    }

    public class NegConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Neg;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Neg({op1});";
        }
    }

    public class XorConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Xor;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Xor({op1}, {op2});";
        }
    }

    public class ShlConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Shl;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Shl({op1}, {op2});";
        }
    }

    public class ShrConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Shr;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Shr({op1}, {op2});";
        }
    }

    public class Shr_UnConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Shr_Un;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Shr_Un({op1}, {op2});";
        }
    }

    public class Mul_ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Mul;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Mul({op1}, {op2});";
        }
    }

    public class Mul_OvfConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Mul_Ovf;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Mul_Ovf({op1}, {op2});";
        }
    }

    public class Mul_Ovf_UnConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Mul_Ovf_Un;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Mul_Ovf_Un({op1}, {op2});";
        }
    }

    public class Div_ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Div;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Div({op1}, {op2});";
        }
    }

    public class Div_UnConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Div_Un;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Div_Un({op1}, {op2});";
        }
    }

    public class Rem_ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Rem;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Rem({op1}, {op2});";
        }
    }

    public class Rem_UnConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Rem_Un;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Rem_Un({op1}, {op2});";
        }
    }

    public class SubConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Sub;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Sub({op1}, {op2});";
        }
    }
    public class Sub_OvfConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Sub_Ovf;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Sub_Ovf({op1}, {op2});";
        }
    }
    public class Sub_Ovf_UnConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Sub_Ovf_Un;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Sub_Ovf({op1}, {op2});";
        }
    }

    public class AndConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.And;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::And({op1}, {op2});";
        }
    }

    public class OrConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Or;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Or({op1}, {op2});";
        }
    }

    public class NotConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Not;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Not({op1});";
        }
    }

    public class CkfiniteConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ckfinite;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Ckfinite({op1});";
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

    public class LdfldConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldfld;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var obj = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op = instruction.Operand;

            if (op is PropertyReference prop)
            {
                return "RTCLI::unimplemented_il(\"ldfld prop\");";
            }
            else if (op is FieldReference fld)
            {

                return $"auto& {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = {obj}.{Utilities.GetCXXValidTokenString(fld.Name)};";
            }
            return "";
        }
    }

    public class LdsfldConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldsfld;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var fld = instruction.Operand as FieldReference;
            TypeInformation typeInformation = methodContext.TranslateContext.MetadataContext.GetTypeInformation(fld.DeclaringType);
            return typeInformation.CallStaticConstructor(methodContext) +
                $"auto& {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = {typeInformation.CXXTypeName}::{Utilities.GetCXXValidTokenString(fld.Name)};";
        }
    }

    public class LdsfldaConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldsflda;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var fld = instruction.Operand as FieldReference;
            TypeInformation typeInformation = methodContext.TranslateContext.MetadataContext.GetTypeInformation(fld.DeclaringType);
            return typeInformation.CallStaticConstructor(methodContext) + 
                $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = RTCLI_ADDRESSOF({typeInformation.CXXTypeName}::{Utilities.GetCXXValidTokenString(fld.Name)});";
        }
    }

    public class StsfldConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stsfld;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var fld = instruction.Operand as FieldReference;
            TypeInformation typeInformation = methodContext.TranslateContext.MetadataContext.GetTypeInformation(fld.DeclaringType);
            return typeInformation.CallStaticConstructor(methodContext) +
                $"{typeInformation.CXXTypeName}::{Utilities.GetCXXValidTokenString(fld.Name)} = {(methodContext as CXXMethodTranslateContext).CmptStackPopObject};";
        }
    }

    public class SizeofConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Sizeof;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op = instruction.Operand;
            if (op is TypeDefinition typeDefinition)
            {
                return "RTCLI::unimplemented_il(\"sizeof typedef\");";
            }
            else if (op is TypeReference typeReference)
            {
                TypeInformation typeInformation = methodContext.TranslateContext.MetadataContext.GetTypeInformation(typeReference);
                return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = RTCLI_SIZEOF({typeInformation.CXXTypeName});";
            }
            else
                return "";
        }
    }

    public class LdobjConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldobj;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var typeReference = instruction.Operand as TypeReference;
            TypeInformation typeInformation = methodContext.TranslateContext.MetadataContext.GetTypeInformation(typeReference);
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto& {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} =" +
                $" RTCLI::ldobj<{typeInformation.CXXTypeName}>({op1});";
        }
    }
    public class StobjConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stobj;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var typeReference = instruction.Operand as TypeReference;
            TypeInformation typeInformation = methodContext.TranslateContext.MetadataContext.GetTypeInformation(typeReference);
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"RTCLI::stobj<{typeInformation.CXXTypeName}>({op1}, {op2});";
        }
    }

    public class LdftnConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldftn;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var method = instruction.Operand as MethodReference;
            TypeInformation typeInformation = methodContext.TranslateContext.MetadataContext.GetTypeInformation(method.DeclaringType);
            var methodInformation = typeInformation.Methods.Find(m => m.Definition == method);
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = &{methodInformation.CXXMethodCallName(typeInformation)};";
        }
    }

    public class BoxConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Box;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var type = instruction.Operand as TypeReference;
            TypeInformation typeInformation = methodContext.TranslateContext.MetadataContext.GetTypeInformation(type);
            var arg = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto& {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = RTCLI::Box<{typeInformation.CXXTypeName}_V>({arg});";
        }
    }

    public class UnboxConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Unbox;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var type = instruction.Operand as TypeReference;
            TypeInformation typeInformation = methodContext.TranslateContext.MetadataContext.GetTypeInformation(type);
            var arg = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = RTCLI::UnBox<{typeInformation.CXXTypeName}_V>({arg});";
        }
    }

    public class Unbox_AnyConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Unbox_Any;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var type = instruction.Operand as TypeReference;
            TypeInformation typeInformation = methodContext.TranslateContext.MetadataContext.GetTypeInformation(type);
            var arg = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            if(typeInformation.IsStruct)
                return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = RTCLI::UnBox<{typeInformation.CXXTypeName}_V>({arg});";
            else
                return $"auto& {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = RTCLI::Castclass<{typeInformation.CXXTypeName}>({arg});";
        }
    }
}
