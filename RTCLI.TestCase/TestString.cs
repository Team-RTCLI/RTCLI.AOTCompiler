using System;
using System.Collections.Generic;
using System.Text;

namespace TestCase
{
    class TestString
    {
        public static void Test()
        {
            String nullStr = null;
            String str = "Static String";

            System.Console.WriteLine(str);
            System.Console.WriteLine(str.Length);
            String lowerStr = str.ToLower();
            System.Console.Write("ToLower: ");
            System.Console.WriteLine(lowerStr);
        }
    }
}
