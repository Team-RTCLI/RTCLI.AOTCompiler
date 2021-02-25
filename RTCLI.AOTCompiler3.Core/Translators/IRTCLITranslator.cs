using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace RTCLI.AOTCompiler3
{
    public interface IRTCLITranslator
    {
        public void Run(CodeTextStorage storage, AssemblyDefinition assembly);
    }
}
