using System;
using System.Text;
using Mono.Cecil;
using RTCLI.AOTCompiler3.Meta;
using System.Linq;

namespace RTCLI.AOTCompiler3.Translators
{
    public class CXXHeaderTranslator : IRTCLITranslator
    {
        // ${OutputPath}/${Assembly}/include
        public void Run(CodeTextStorage Storage, AssemblyDefinition FocusedAssembly)
        {
            // [H1000] UberHeader
            CXXHeaderRules.GenerateUberHeader(Storage, FocusedAssembly);

            foreach (var Module in FocusedAssembly.Modules)
            {
                foreach(var Type in Module.Types)
                {
                    var Writer = Storage.Wirter(Type.CXXHeaderPath());
                    // [H9999] Copyright
                    CXXHeaderRules.CopyWrite(Writer);
                    // [H0000] Include Protect
                    CXXHeaderRules.WriteIncludeProtect(Writer);
                    if (Type.IsPrimitive)
                        continue;

                    Writer.WriteLine(EnvIncludes);
                    // [H1001] Base Types Headers
                    CXXHeaderRules.IncludeBaseTypesHeaders(Writer, Type);
                    Writer.WriteLine();

                    // [H0001] Forward Declaration
                    CXXHeaderRules.WriteForwardDeclaration(Writer, Type);
                    Writer.WriteLine();

                    // [H2003] namespace
                    using (var ___ = new CXXNamespaceScope(Writer, Type.CXXNamespace()))
                    {
                        WriteTypeRecursively(Writer, Type);
                    }
                    Writer.Flush();
                }
            }
        }

        [H2002()]
        private void WriteTypeRecursively(CodeTextWriter codeWriter, TypeDefinition type)
        {
            // [C0004] generic
            if (type.HasGenericParameters)
                codeWriter.WriteLine(CXXHeaderRules.GenericDeclaration(type));

            // [H2000] Type Scope
            using (var typeScope = new CXXTypeScope(codeWriter, type))
            {
                codeWriter.unindent().WriteLine("public:").indent();
                foreach (var nested in type.NestedTypes)
                {
                    // [H2002] Inner Types
                    codeWriter.WriteLine($"// [H2002] Inner Types {nested.CXXShortTypeName()}");
                    WriteTypeRecursively(codeWriter, nested);
                }
                // [H2001] Method Signatures
                CXXHeaderRules.WriteMethodSignatures(codeWriter, type, type.IsValueType);

                // [H2005] Field Declaration
                CXXHeaderRules.WriteFieldDeclaration(codeWriter, type);
            }

            // [H2004] Boxed ValueType
            if (!type.IsValueType)
                return;
            CXXHeaderRules.WriteBoxedValueType(codeWriter, type);
        }

        private string EnvIncludes => "#include <RTCLI/RTCLI.hpp>";
    }
}
