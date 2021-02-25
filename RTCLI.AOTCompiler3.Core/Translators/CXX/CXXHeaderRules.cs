using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using RTCLI.AOTCompiler3.Meta;

namespace RTCLI.AOTCompiler3.Translators
{
    public static class CXXHeaderRules
    {
        [H2001()]
        public static void WriteMethodSignatures(CodeTextWriter codeWriter, TypeDefinition type)
        {
            if (type.Methods != null & type.Methods.Count != 0)
            {
                codeWriter.WriteLine("// [H2001] Method Signatures");
                foreach (var method in type.Methods)
                {
                    if (method.HasGenericParameters)
                        codeWriter.WriteLine($"template<{method.CXXTemplateParam()}>");
                    codeWriter.WriteLine($"{(method.IsNewSlot ? "virtual " : "")}{method.CXXMethodSignature(true)};");
                }
                codeWriter.WriteLine();
            }
        }

        [H0000()]
        public static void WriteIncludeProtect(CodeTextWriter codeWriter, TypeDefinition type)
        {
            codeWriter.WriteLine("// [H0000] Include Protect");
            codeWriter.WriteLine("#pragma once");
        }
        
        [H0001()]
        public static void WriteForwardDeclaration(CodeTextWriter codeWriter, TypeDefinition type)
        {
            codeWriter.WriteLine("// [H0001] Forward Declaration");
            codeWriter.WriteLine("// TODO");
        }

        [H1000()]
        public static void GenerateUberHeader(CodeTextStorage Storage, AssemblyDefinition FocusedAssembly)
        {
            var uberHeaderWriter = Storage.Wirter("_UberHeader_.h");
            uberHeaderWriter.WriteLine("// [H1000] UberHeader");
            foreach (var Module in FocusedAssembly.Modules)
            {
                foreach (var Type in Module.Types)
                {
                    uberHeaderWriter.WriteLine($"#include \"{Type.CXXHeaderPath()}\"");
                }
            }
            uberHeaderWriter.Flush();
        }
    }

    [H2003()]
    public class CXXNamespaceScope : CXXScopeDisposer
    {
        private CodeTextWriter parent;
        private bool EndWithSemicolon = false;
        private string onExit = null;
        public CXXNamespaceScope(CodeTextWriter parent, string CXXNamespace)
            :base(parent, "namespace " + CXXNamespace,
                 false, "// [H2003] namespace", "// [H2003] Exit namespace")
        {

        }
    }
}
