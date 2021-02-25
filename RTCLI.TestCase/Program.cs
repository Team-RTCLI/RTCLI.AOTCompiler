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
            var Test = new TestInterface.A();
            var I1 = (TestInterface.Interface1)Test;
            var I2 = (TestInterface.Interface2)Test;
            var I3 = (TestInterface.Interface3)Test;
            var I4 = (TestInterface.Interface4)Test;
            Console.WriteLine(Test.a());
            Console.WriteLine(I1.a());
            Console.WriteLine(I2.a());
            Console.WriteLine(I3.a());
            Console.WriteLine(I4.a());
            return 0;
        }
    }
}
