using System;

namespace TestProgram
{
    public struct TestStaticVal
    {
        public static int val = 1;
    }

    public interface Interface0
    {
        void Slot()
        {
            Console.WriteLine("0");
        }
    }
    public interface Interface1 : Interface0
    {
        new void Slot()
        {
            Console.WriteLine("1");
        }
    }
    public class Class : Interface1
    {
        void Interface1.Slot()
        {
            Console.WriteLine("Override Interface1");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Class Fuck = new Class();
            (Fuck as Interface0).Slot();
            (Fuck as Interface1).Slot();

            Console.WriteLine(TestStaticVal.val);
        }
    }
}
