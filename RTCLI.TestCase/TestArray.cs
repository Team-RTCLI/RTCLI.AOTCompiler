using System;
using System.Collections.Generic;
using System.Text;

namespace TestCase
{
    class TestArray
    {
        public static void Test()
        {
            int[] list = new int[12];
            list[0] = 1111111;
            list[11] = 111111;
            list[0] = 12;
        }
    }
}
