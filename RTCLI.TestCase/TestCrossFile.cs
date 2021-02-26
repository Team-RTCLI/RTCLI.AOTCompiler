using System;
using System.Collections.Generic;
using System.Text;

namespace TestCase
{
    public class TestCrossFile
    {
        public static void Test()
        {
            System.Console.WriteLine(TestSystem.val);
            System.Console.WriteLine(TestSystem.valP);
            System.Console.WriteLine(system_test.valval);
            TestSystem.Test();
        }

        static TestSystem system_test = new TestSystem();
    }
}
