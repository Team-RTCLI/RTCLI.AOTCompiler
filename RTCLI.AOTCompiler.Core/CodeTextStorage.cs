using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using RTCLI.AOTCompiler.Internal;

namespace RTCLI.AOTCompiler
{
    public class CodeTextStorage
    {
        private readonly TextWriter logw;
        public readonly string BasePath;
        private readonly Queue<string> scopeNames = new Queue<string>();

        public CodeTextStorage(TextWriter logw, string basePath, string indent)
        {
            this.logw = logw;
            this.BasePath = basePath;
        }

        private string getScopePath()
        {
            string fullDirName = BasePath;
            foreach (var scope in scopeNames)
            {
                fullDirName = Path.Combine(fullDirName, scope);
            }
            return fullDirName;
        }

        public IDisposable EnterScope(string scopeName, bool splitScope = true)
        {
            scopeNames.Enqueue(splitScope ? Utilities.GetCXXLanguageScopedPath(scopeName) : scopeName);
            if (!Directory.Exists(getScopePath()))
            {
                Directory.CreateDirectory(getScopePath());     
            }
            return new ScopeDisposer(this);
        }

        public CodeTextWriter Wirter(string FileName)
        {
            return new CodeTextWriter(Path.Combine(getScopePath(), FileName));
        }

        private sealed class ScopeDisposer : IDisposable
        {
            private CodeTextStorage parent;
            public ScopeDisposer(CodeTextStorage parent)
            {
                this.parent = parent;
            }

            public void Dispose()
            {
                if (parent != null)
                {
                    parent.scopeNames.Dequeue();
                    parent = null;
                }
            }
        }
    }
}
