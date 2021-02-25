using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using RTCLI.AOTCompiler3.Meta;
using System.Linq;

namespace RTCLI.AOTCompiler3.Translators
{
    public static class CXXHeaderRules
    {
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
            var uberHeaderWriter = Storage.Wirter(Constants.CXXUberHeaderName);
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

        [C0001()]
        public static string StructDeclaration(TypeDefinition type)
        {
            var solved = type.InterfacesSolved();
            string Interfaces = string.Join(',', solved.Select(a => $"public {a.InterfaceType.CXXTypeName()}"));

            return $"/*[C0001]*/struct {type.CXXShortTypeName()}";
        }
        
        [C0002()]
        public static string InterfaceDeclaration(TypeDefinition type)
        {
            var solved = type.InterfacesSolved();
            string Interfaces = string.Join(',', solved.Select(a => $"public {a.InterfaceType.CXXTypeName()}"));

            return $"/*[C0002]*/interface {type.CXXShortTypeName()} {(type.Interfaces.Count > 0 ? ": " + Interfaces : "")}";
        }
        
        [C0003()]
        public static string ClassDeclaration(TypeDefinition type)
        {
            var solved = type.InterfacesSolved();
            string Interfaces = string.Join(',', solved.Select(a => $"public {a.InterfaceType.CXXTypeName()}"));
            string BaseType = type.BaseType != null ? type.BaseType.CXXTypeName() : "RTCLI::System::Object";

            return $"/*[C0003]*/class {type.CXXShortTypeName()} : public {BaseType}{(type.Interfaces.Count > 0 ? "," + Interfaces : "")}";
        }
        
        [C0003()]
        public static string GenericDeclaration(TypeDefinition type)
        {
            return $"/*[C0004]*/template<{type.CXXTemplateParam()}>";
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
