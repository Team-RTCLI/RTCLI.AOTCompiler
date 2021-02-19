using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Text;
using RTCLI.AOTCompiler.Translators;
using Mono.Cecil;
using RTCLI.AOTCompiler.Metadata;

namespace RTCLI.AOTCompiler.ILConverters
{
    public class BrConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            Instruction to = instruction.Operand as Instruction;
            return $"goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Br;
    }
    public class Br_SConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            Instruction to = instruction.Operand as Instruction;
            return $"goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Br_S;
    }

    public class BrtrueConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            Instruction to = instruction.Operand as Instruction;
            var op = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"if(RTCLI::Cond({op}))" +
                $" goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Brtrue;
    }
    public class Brtrue_SConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            Instruction to = instruction.Operand as Instruction;
            var op = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"if(RTCLI::Cond({op})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Brtrue_S;
    }

    public class BrfalseConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            Instruction to = instruction.Operand as Instruction;
            var op = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"if(!RTCLI::Cond({op})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Brfalse;
    }
    public class Brfalse_SConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            Instruction to = instruction.Operand as Instruction;
            var op = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"if(!RTCLI::Cond({op})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Brfalse_S;
    }


    public class LeaveConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            Instruction to = instruction.Operand as Instruction;
            return $"RTCLI_LEAVE {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Leave;
    }
    public class Leave_SConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            Instruction to = instruction.Operand as Instruction;
            return $"RTCLI_LEAVE {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Leave_S;
    }

    public class BeqConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(RTCLI::Ceq({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Beq;
    }
    public class Beq_SConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(RTCLI::Ceq({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Beq_S;
    }

    public class BgeConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(!RTCLI::Clt({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Bge;
    }
    public class Bge_SConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(!RTCLI::Clt({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Bge_S;
    }
    public class Bge_UnConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(!RTCLI::Clt_Un({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Bge_Un;
    }
    public class Bge_Un_SConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(!RTCLI::Clt_Un({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Bge_Un_S;
    }

    public class BgtConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(RTCLI::Cgt({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Bgt;
    }
    public class Bgt_SConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(RTCLI::Cgt({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Bgt_S;
    }
    public class Bgt_UnConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(RTCLI::Cgt_Un({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Bgt_Un;
    }
    public class Bgt_Un_SConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(RTCLI::Cgt_Un({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Bgt_Un_S;
    }

    public class BleConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(!RTCLI::Cgt({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Ble;
    }
    public class Ble_SConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(!RTCLI::Cgt({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Ble_S;
    }
    public class Ble_UnConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(!RTCLI::Cgt_Un({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Ble_Un;
    }
    public class Ble_Un_SConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(!RTCLI::Cgt_Un({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Ble_Un_S;
    }


    public class BltConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(RTCLI::Clt({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Blt;
    }
    public class Blt_SConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(RTCLI::Clt({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Blt_S;
    }
    public class Blt_UnConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(RTCLI::Clt_Un({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Blt_Un;
    }
    public class Blt_Un_SConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(RTCLI::Clt_Un({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Blt_Un_S;
    }
    public class Bne_UnConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(!RTCLI::Ceq({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Bne_Un;
    }
    public class Bne_Un_SConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction; 
            return $"if(!RTCLI::Ceq({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Bne_Un_S;
    }
}