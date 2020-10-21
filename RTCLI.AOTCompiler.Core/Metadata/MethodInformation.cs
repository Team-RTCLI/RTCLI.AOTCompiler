﻿using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Newtonsoft.Json;

namespace RTCLI.AOTCompiler.Metadata
{
    public class MethodInformation : IMemberInformation
    {
        public MethodInformation(MethodDefinition def, MetadataContext metadataContext)
        {
            this.definition = def;
            this.MetadataContext = metadataContext;

            if (def.HasBody)
            foreach (var Inst in def.Body.Instructions)
            {
                Instructions.Add(new InstructionInformation(Inst));
            }
        }

        [JsonIgnore] public MethodBody Body => definition.Body;
        [JsonIgnore] private readonly MethodDefinition definition = null;
        
        
        public string CXXMethodName => "CXXMethod";
        public string CXXRetType => definition.ReturnType.FullName;

        public readonly List<InstructionInformation> Instructions = new List<InstructionInformation>();
        public IMetadataTokenProvider Definition => definition;
        public MetadataContext MetadataContext { get; }
    }
}