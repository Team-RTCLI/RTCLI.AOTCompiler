using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTCLI.AOTCompiler3
{
    public static class Utilities
    {
        public static string GetCXXLanguageScopedPath(IEnumerable<string> scopeNames) =>
            string.Join("/", scopeNames.SelectMany(sn => sn.Split('.')));

        public static string GetCXXLanguageScopedPath(params string[] scopeNames) =>
            GetCXXLanguageScopedPath((IEnumerable<string>)scopeNames);

        public static string GetCXXValidTokenString(string raw_token) =>
            raw_token.Replace('<', '_').Replace('>', '_');

        public static string GetLabel(this Instruction instruction)
        {
            return instruction.ToString().Split(":")[0];
        }

        public static string HoldEscape(this string source)
        {
            return source.Replace("\\", "\\\\").Replace("\n", "\\n")
                    .Replace("\'", "\\'").Replace("\"", "\\\"")
                    .Replace("\0", "\\0").Replace("\a", "\\a")
                    .Replace("\b", "\\b").Replace("\f", "\\f")
                    .Replace("\r", "\\r").Replace("\t", "\\t")
                    .Replace("\v", "\\v");
        }

        public static string CopyRight => "// COPYRIGHT STRING";
    }
}