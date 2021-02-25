using Mono.Cecil.Cil;
using Mono.Cecil;
using System;
using System.Linq;
using RTCLI.AOTCompiler3.Meta;

namespace RTCLI.AOTCompiler3.ILConverters
{
    public class NopConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Nop;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext) 
            => "RTCLI::nop();";
    }
    public class NewobjConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Newobj;
        private string ParseParams(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            MethodReference calledMethod = instruction.Operand as MethodReference;
            var Type = calledMethod.DeclaringType;
            string paramSequenceCXX = string.Join(',', 
                calledMethod.Parameters.Select(_ => methodContext.CmptStackPopObject).Reverse()
            );
            return $"{Type.CXXTypeName()}& {methodContext.CmptStackPushObject} = \n\t\tRTCLI::new_object<{Type.CXXTypeName()}>({paramSequenceCXX});";
        }
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext) => ParseParams(instruction, methodContext);
    }
    public class InitobjConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Initobj;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var Type = instruction.Operand as TypeReference;
            return $"RTCLI::initialize_object<{Type.CXXTypeName()}>({methodContext.CmptStackPopObject});";
        }
    }
    public class CpobjConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Cpobj;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var Type = instruction.Operand as TypeReference;
            var op1 = methodContext.CmptStackPopObject;
            var op2 = methodContext.CmptStackPopObject;
            return $"RTCLI::cpobj<{Type.CXXTypeName()}>({op1}, {op2});";
        }
    }
    public class DupConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Dup;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op1 = methodContext.CmptStackPeek;
            return $"auto& {methodContext.CmptStackPushObject} = RTCLI::Dup({op1});";
        }
    }
    public class InitblkConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Initblk;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op3 = methodContext.CmptStackPopObject;
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"RTCLI::initblk({op1}, {op2}, {op3});";
        }
    }
    public class CpblkConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Cpblk;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op3 = methodContext.CmptStackPopObject;
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"RTCLI::cpblk({op1}, {op2}, {op3});";
        }
    }
    public class NewarrConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Newarr;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var Type = instruction.Operand as TypeReference;
            var len = methodContext.CmptStackPopObject;

            return $"RTCLI::System::ElementArray<{Type.CXXTypeName()}>& {methodContext.CmptStackPushObject} = \n" +
                $"\t\tRTCLI::new_array<{Type.CXXTypeName()}>({len});";
        }
    }
    public class LdlenConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldlen;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var arr = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject} = RTCLI::ArrayLen({arr});";
        }
    }
    public class IsinstConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Isinst;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var Type = instruction.Operand as TypeReference;
            return $"auto {methodContext.CmptStackPushObject} = \n" +
                $"\t\tRTCLI::Isinst<{Type.CXXTypeName()}>({methodContext.CmptStackPopObject});";
        }
    }
    public class CastclassConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Castclass;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var Type = instruction.Operand as TypeReference;
            return $"auto {methodContext.CmptStackPushObject} = \n" +
                $"\t\tRTCLI::Castclass<{Type.CXXTypeName()}>({methodContext.CmptStackPopObject});";
        }
    }

    public class LdstrConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldstr;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => $"RTCLI::System::String {methodContext.CmptStackPushObject} = " +
            $"RTCLI_NATIVE_STRING(\"{instruction.Operand.ToString().HoldEscape()}\");";
    }

    public class RetConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ret;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => $"return {methodContext.CmptStackPopAll};";
    }

    public class AddConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Add;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Add({op1}, {op2});";
        }
    }
    public class Add_OvfConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Add_Ovf;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Add_Ovf({op1}, {op2});";
        }
    }
    public class Add_Ovf_UnConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Add_Ovf_Un;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Add_Ovf({op1}, {op2});";
        }
    }

    public class NegConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Neg;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Neg({op1});";
        }
    }

    public class XorConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Xor;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Xor({op1}, {op2});";
        }
    }

    public class ShlConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Shl;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Shl({op1}, {op2});";
        }
    }

    public class ShrConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Shr;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Shr({op1}, {op2});";
        }
    }

    public class Shr_UnConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Shr_Un;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Shr_Un({op1}, {op2});";
        }
    }

    public class Mul_ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Mul;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Mul({op1}, {op2});";
        }
    }

    public class Mul_OvfConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Mul_Ovf;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Mul_Ovf({op1}, {op2});";
        }
    }

    public class Mul_Ovf_UnConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Mul_Ovf_Un;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Mul_Ovf_Un({op1}, {op2});";
        }
    }

    public class Div_ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Div;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Div({op1}, {op2});";
        }
    }

    public class Div_UnConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Div_Un;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Div_Un({op1}, {op2});";
        }
    }

    public class Rem_ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Rem;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Rem({op1}, {op2});";
        }
    }

    public class Rem_UnConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Rem_Un;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Rem_Un({op1}, {op2});";
        }
    }

    public class SubConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Sub;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Sub({op1}, {op2});";
        }
    }
    public class Sub_OvfConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Sub_Ovf;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Sub_Ovf({op1}, {op2});";
        }
    }
    public class Sub_Ovf_UnConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Sub_Ovf_Un;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Sub_Ovf({op1}, {op2});";
        }
    }

    public class AndConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.And;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::And({op1}, {op2});";
        }
    }

    public class OrConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Or;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Or({op1}, {op2});";
        }
    }

    public class NotConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Not;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Not({op1});";
        }
    }

    public class CkfiniteConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ckfinite;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Ckfinite({op1});";
        }
    }

    public class PopConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Pop;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            return $"RTCLI::Pop({methodContext.CmptStackPopObject});//pop operation";
        }
    }

    public class StfldConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stfld;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var val = methodContext.CmptStackPopObject;
            var obj = methodContext.CmptStackPopObject;
            var op = instruction.Operand;

            if (op is PropertyReference prop)
            {
                return "RTCLI::unimplemented_il(\"stfld prop\");";
            }
            else if (op is FieldReference fld)
            {

                return $"{obj}.{Utilities.GetCXXValidTokenString(fld.Name)} = {val};";
            }
            return "";
        }
    }

    public class LdfldConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldfld;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var obj = methodContext.CmptStackPopObject;
            var op = instruction.Operand;

            if (op is PropertyReference prop)
            {
                return "RTCLI::unimplemented_il(\"ldfld prop\");";
            }
            else if (op is FieldReference fld)
            {
                var name = methodContext.CmptStackPushObject;
                return $"auto& {name} = {obj}.{Utilities.GetCXXValidTokenString(fld.Name)}{(fld.FieldType.IsValueType ? "" : ".Get()")};";
            }
            return "";
        }
    }

    public class LdsfldConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldsfld;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var fld = instruction.Operand as FieldReference;
            var Type = fld.DeclaringType.Resolve();

            return Type.CallStaticConstructor(methodContext) +
                $"auto& {methodContext.CmptStackPushObject} = {Type.CXXTypeName()}::{Utilities.GetCXXValidTokenString(fld.Name)};";
        }
    }

    public class LdsfldaConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldsflda;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var fld = instruction.Operand as FieldReference;
            var Type = fld.DeclaringType.Resolve();

            return Type.CallStaticConstructor(methodContext) +
                $"auto& {methodContext.CmptStackPushObject} = RTCLI_ADDRESSOF({Type.CXXTypeName()}::{Utilities.GetCXXValidTokenString(fld.Name)});";
        }
    }

    public class StsfldConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stsfld;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var fld = instruction.Operand as FieldReference;
            var Type = fld.DeclaringType.Resolve();
            return Type.CallStaticConstructor(methodContext) +
                $"{Type.CXXTypeName()}::{Utilities.GetCXXValidTokenString(fld.Name)} = {methodContext.CmptStackPopObject};";
        }
    }

    public class SizeofConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Sizeof;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op = instruction.Operand;
            if (op is TypeDefinition Type)
            {
                return "RTCLI::unimplemented_il(\"sizeof typedef\");";
            }
            else if (op is TypeReference TypeR)
            {
                return $"auto {methodContext.CmptStackPushObject} = RTCLI_SIZEOF({TypeR.CXXTypeName()});";
            }
            else
                return "";
        }
    }

    public class LdobjConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldobj;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var Type = instruction.Operand as TypeReference;
            var op1 = methodContext.CmptStackPopObject;

            return $"auto& {methodContext.CmptStackPushObject} =" +
                $" RTCLI::ldobj<{Type.CXXTypeName()}>({op1});";
        }
    }
    public class StobjConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stobj;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var Type = instruction.Operand as TypeReference;
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"RTCLI::stobj<{Type.CXXTypeName()}>({op1}, {op2});";
        }
    }

    public class LdftnConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldftn;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var method = instruction.Operand as MethodReference;
            var methodDef = method.Resolve();
            var Type = method.DeclaringType.Resolve();

            return $"auto {methodContext.CmptStackPushObject} = &{methodDef.CXXMethodCallName(Type)};";
        }
    }

    public class BoxConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Box;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var type = instruction.Operand as TypeReference;
            var arg = methodContext.CmptStackPopObject;
            return $"auto& {methodContext.CmptStackPushObject} = RTCLI::Box<{type.CXXTypeName()}_V>({arg});";
        }
    }

    public class UnboxConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Unbox;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var type = instruction.Operand as TypeReference;
            var arg = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject} = RTCLI::UnBox<{type.CXXTypeName()}_V>({arg});";
        }
    }

    public class Unbox_AnyConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Unbox_Any;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var type = instruction.Operand as TypeReference;
            var arg = methodContext.CmptStackPopObject;
            if (type.IsValueType)
                return $"auto {methodContext.CmptStackPushObject} = RTCLI::UnBox<{type.CXXTypeName()}_V>({arg});";
            else
                return $"auto& {methodContext.CmptStackPushObject} = RTCLI::Castclass<{type.CXXTypeName()}>({arg});";
        }
    }
}