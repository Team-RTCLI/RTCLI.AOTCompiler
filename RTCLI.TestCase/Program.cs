using System;
using System.Collections.Generic;
using System.Text;

namespace TestCase
{
    public partial struct Value
    {
        int v;
    }

    public partial struct Value
    {
        int vv;
        public int Test(Value o)
        {
            return o.vv;
        }
    }

    public class Program
    {
        public static int Main(string[] args)
        {
            TestCase.TestSystem.Test();
            
            Value v = new Value();
            var t = v.Test(v);

            Console.WriteLine(v.ToString());
            return 0;
        }
    }
}
