using System;

namespace RTCLITestCase
{
    //[TestExecute($"Test({12})")]
    public class TestSystem
    {
        public TestSystem(int val)
        {

        }
        public static void Test(int argInt)
        {
            System.Console.Write("Write Tested!\n");
            System.Console.WriteLine("WriteLine Tested!");
        }
    }
}
