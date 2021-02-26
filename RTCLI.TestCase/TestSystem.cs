using System;

namespace TestCase
{
    public struct TestSystem
    {
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

        public static int val = 0;
        public int valval;
    }
}
