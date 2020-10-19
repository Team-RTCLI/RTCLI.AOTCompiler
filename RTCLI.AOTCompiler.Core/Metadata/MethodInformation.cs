using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Newtonsoft.Json;

namespace RTCLI.AOTCompiler.Metadata
{
    public class MethodInformation : IMemberInformation
    {
        public MethodInformation(MethodDefinition def)
        {
            this.definition = def;

            Console.WriteLine("");
            Console.WriteLine(def.Name);
            foreach (var Inst in def.Body.Instructions)
            {
                Console.WriteLine(Inst.ToString());
                Instructions.Add(new InstructionInformation(Inst));
            }
        }

        [JsonIgnore] private readonly MethodDefinition definition = null;
        public readonly List<InstructionInformation> Instructions = new List<InstructionInformation>();
        public IMetadataTokenProvider Definition => definition;
    }
}