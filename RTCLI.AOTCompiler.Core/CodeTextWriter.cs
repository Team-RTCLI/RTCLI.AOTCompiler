using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RTCLI.AOTCompiler
{
    public class CodeTextWriter
    {
        public CodeTextWriter(string relatedPath)
        {
            sw = new StreamWriter(relatedPath, false, Encoding.UTF8);
        }

        public void WriteLine(string toWrite)
        {
            sw.WriteLine(indentString + toWrite);
        }

        public void indent()
        {
            indentString += "\t";
        }
        public void unindent()
        {
            if(indentString.Length > 0)
                indentString = indentString.Remove(indentString.Length - 1);
        }

        public void Flush()
        {
            sw.Flush();
        }

        private String indentString = "";
        private StreamWriter sw = null;
    }
}
