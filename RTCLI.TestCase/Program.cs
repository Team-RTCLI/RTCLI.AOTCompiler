using System;
using System.Collections.Generic;
using System.Text;

namespace TestCase
{
    public class Program
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

        public static int Main(string[] args)
        {
            TestCase.TestSystem.Test();
            Object o = null;
            bool res = o.Equals(o);
            Value v = new Value();
            var t = v.Test(v);

            Console.WriteLine(v.ToString());
            return 0;
        }
    }
}
