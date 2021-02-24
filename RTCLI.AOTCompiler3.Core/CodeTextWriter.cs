using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RTCLI.AOTCompiler3
{
    public class CodeTextWriter
    {
        public CodeTextWriter(string relatedPath)
        {
            sw = new StreamWriter(relatedPath, false, Encoding.UTF8);
        }

        public CodeTextWriter WriteLine(string toWrite)
        {
            sw.WriteLine(indentString + toWrite);
            return this;
        }

        public CodeTextWriter indent()
        {
            indentString += "\t";
            return this;
        }
        public CodeTextWriter unindent()
        {
            if(indentString.Length > 0)
                indentString = indentString.Remove(indentString.Length - 1);
            return this;
        }

        public void Flush()
        {
            sw.Flush();
        }

        private String indentString = "";
        private StreamWriter sw = null;
    }
}
