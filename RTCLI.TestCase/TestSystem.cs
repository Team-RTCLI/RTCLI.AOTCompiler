using System;

namespace TestCase
{
    public class TestSystem
    {
        public TestSystem(int val)
        {

        }
        public static void Test()
        {
            System.Console.Write("Write Tested!\n");
            System.Console.WriteLine("WriteLine Tested!");

            System.Console.WriteLine($"WriteLine with Integer {12}");
        }
    }
}
