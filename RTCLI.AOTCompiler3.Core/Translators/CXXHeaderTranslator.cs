using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Mono.Cecil;
using RTCLI.AOTCompiler3.Meta;
using System.Linq;

namespace RTCLI.AOTCompiler3.Translators
{
    public static class TypeCXXPathHelper
    {
        public static string CXXHeaderPath(this TypeReference typeReference)
        {
            return Path.Combine(typeReference.CXXNamespaceToPath(), typeReference.CXXShortTypeName() + ".h").Replace("\\", "/");
        }

        public static string CXXUberHeaderPath(this TypeReference typeReference)
        {
            return typeReference.Module.Assembly.CXXUberHeaderPath();
        }
    }

    public static class AssemblyCXXPathHelper
    {
        public static string CXXUberHeaderPath(this AssemblyDefinition assembly)
        {
            return Path.Combine(assembly.RTCLIShortName(), "include/_UberHeader_.h").Replace("\\", "/");
        }

        public static string CXXUberHeaderPath(this AssemblyNameReference assembly)
        {
            return Path.Combine(assembly.RTCLIShortName(), "include/_UberHeader_.h").Replace("\\", "/");
        }
    }

    public class CXXHeaderTranslator : IRTCLITranslator
    {
        // ${OutputPath}/${Assembly}/include
        public void Run(CodeTextStorage Storage, AssemblyDefinition FocusedAssembly)
        {
            var uberHeaderWriter = Storage.Wirter("_UberHeader_.h");
            uberHeaderWriter.WriteLine("// [H1000] UberHeader"); 
            foreach (var Module in FocusedAssembly.Modules)
            {
                foreach(var Type in Module.Types)
                {
                    var codeWriter = Storage.Wirter(Type.CXXHeaderPath());
                    codeWriter.WriteLine(Constants.CopyRight);
                    codeWriter.WriteLine("// [H0000] Include Protect");
                    codeWriter.WriteLine("#pragma once");
                    codeWriter.WriteLine(EnvIncludes);

                    codeWriter.WriteLine();
                    codeWriter.WriteLine("// [H0001] Forward Declaration");

                    codeWriter.WriteLine();
                    WriteTypeRecursively(codeWriter, Type);

                    codeWriter.Flush();
                    uberHeaderWriter.WriteLine($"#include \"{Type.CXXHeaderPath()}\"");
                }
            }
            uberHeaderWriter.Flush();
        }

        public void WriteTypeRecursively(CodeTextWriter codeWriter, TypeDefinition type)
        {
            string Interfaces = string.Join(',', type.Interfaces.Select(a => $"public {a.InterfaceType.CXXTypeName()}"));
            if (type.Interfaces.Count > 0)
                Interfaces = "," + Interfaces;
            string BaseType = type.BaseType != null ? type.BaseType.CXXTypeName() : "RTCLI::System::Object";

            string TemplateDecl = $"template<{type.CXXTemplateParam()}> ";
            string TypeDecl = type.IsValueType ?
                                 $"struct {type.CXXShortTypeName()}"
                               : $"class {type.CXXShortTypeName()} : public {BaseType}{Interfaces}";
            string ScopeString = TemplateDecl + TypeDecl;
            using (var classScope = new CXXScopeDisposer(codeWriter, ScopeString, true, $"// [H2000] TypeScope {type.CXXTypeName()} ", $"// [H2000] Exit TypeScope {type.CXXTypeName()}"))
            {
                codeWriter.unindent().WriteLine("public:").indent();
                foreach (var nested in type.NestedTypes)
                {
                    WriteTypeRecursively(codeWriter, nested);
                }
                codeWriter.WriteLine("// [H2001] Method Signatures");
                foreach (var method in type.Methods)
                {
                    if (method.HasGenericParameters)
                        codeWriter.WriteLine($"template<{method.CXXTemplateParam()}>");
                    codeWriter.WriteLine($"{(method.IsNewSlot ? "virtual " : "")}{method.CXXMethodSignature(true)};");
                }
                codeWriter.WriteLine("// [H2001] Method Signatures End");
                foreach (var field in type.Fields)
                {
                    //codeWriter.WriteLine(field.CXXFieldDeclaration());
                }
            }

            if (!type.IsValueType)
                return;

            //if (type.HasGenericParameters)
            //    codeWriter.WriteLine($"template<{type.CXXTemplateParam()}>");
            string classDef = $"class {type.CXXShortTypeName()}_V : public RTCLI::System::ValueType{Interfaces}";
            using (var classScope = new CXXScopeDisposer(codeWriter, classDef, true, $"// [H2000] TypeScope {type.CXXTypeName()}_V ", $"// [H2000] Exit TypeScope {type.CXXTypeName()}_V"))
            {
                codeWriter.unindent().WriteLine("public:").indent();
                codeWriter.WriteLine($"using ValueType = {type.CXXShortTypeName()};");
                //codeWriter.WriteLine($"using ValueType = struct {type.CXXTypeNameShort};");
                codeWriter.WriteLine($"{type.CXXShortTypeName()} value;");
                foreach (var method in type.Methods)
                {
                    //if (method.HasGenericParameters)
                    //    codeWriter.WriteLine($"template<{method.CXXTemplateParam()}>");
                    //codeWriter.WriteLine($"RTCLI_FORCEINLINE {method.CXXMethodSignature(true)} {{ value.{method.CXXMethodNameShort}{method.CXXArgSequence}; }}");
                }
            }

        }

        private sealed class CXXScopeDisposer : IDisposable
        {
            private CodeTextWriter parent;
            private bool EndWithSemicolon = false;
            private string onExit = null;
            public CXXScopeDisposer(CodeTextWriter parent, string Scope, bool bEndWithSemicolon = false, string onEnter = null, string onExit = null)
            {
                this.parent = parent;
                this.EndWithSemicolon = bEndWithSemicolon;
                this.onExit = onExit;
                if (onEnter != null) parent.WriteLine(onEnter);
                parent.WriteLine(Scope);
                parent.WriteLine("{");
                parent.indent();
            }

            public void Dispose()
            {
                if (parent != null)
                {
                    parent.unindent();
                    parent.WriteLine("}" + (EndWithSemicolon?";":""));
                    if(onExit != null) parent.WriteLine(onExit);
                    parent.WriteLine();
                    parent = null;
                }
            }
        }

        private string EnvIncludes => "#include <RTCLI/RTCLI.hpp>";
    }
}
