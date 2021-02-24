using System;

namespace TestCase
{
    public class TestSystem
    {
        public TestSystem(int val, in int val2, out int val3, ref int val4, int val5 = 30)
        {
            val3 = 10;
            val4 = 20;
            val = val2;

        }
        public static void Test()
        {
            System.Console.Write("Write Tested!\n");
            System.Console.WriteLine("WriteLine Tested!");

            /*
            string notInterned = "This string is not Interned!";
            if(String.IsInterned(notInterned) == null)
            {
                System.Console.WriteLine(notInterned);
            }
            else
            {
                System.Console.WriteLine("ERROR: String:notInsterned is not Interned!");
            }
            */

            //string readed = System.Console.ReadLine();
            //String.Intern(readed);
            //System.Console.Write(readed);
        }
    }
}
