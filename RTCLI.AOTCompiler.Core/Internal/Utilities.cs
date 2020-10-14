using System.Collections.Generic;
using System.Linq;

namespace RTCLI.AOTCompiler.Internal
{
    public static class Utilities
    {
        public static string GetCXXLanguageScopedPath(IEnumerable<string> scopeNames) =>
            string.Join("/", scopeNames.SelectMany(sn => sn.Split('.')));

        public static string GetCXXLanguageScopedPath(params string[] scopeNames) =>
            GetCXXLanguageScopedPath((IEnumerable<string>)scopeNames);
    }

}