using System;
using System.Collections.Generic;
using System.Text;

namespace RTCLI.TestCase
{
    class TestArray
    {
        public void Test(int argInt)
        {
            int[] list = new int[argInt];
            list[0] = 1111111;
            list[argInt] = 111111;
            list[0] = argInt;
            argInt = list[0];
            argInt = list[argInt];
        }
    }
}
