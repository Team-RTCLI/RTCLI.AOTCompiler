using Mono.Cecil.Cil;
using System.Dynamic;
using RTCLI.AOTCompiler3.Translators;
using RTCLI.AOTCompiler3.Meta;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace RTCLI.AOTCompiler3.ILConverters
{
    public interface ICXXILConverter : IILConverter
    {
        string Note(Instruction instruction, MethodTranslateContextCXX methodContext) 
            => "// " + instruction.ToString().HoldEscape();

        string Convert(Instruction instruction, MethodTranslateContextCXX methodContext);
        void Visit(Instruction instruction, MethodTranslateContextCXX methodContext) { }
    }
}