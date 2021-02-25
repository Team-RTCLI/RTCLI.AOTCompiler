using System;

public class RawC
{

}

namespace TestCase.Reference
{
    public interface BaseInterface
    {
        string Note() => "aa";
        string Name { get; }
    }

    public interface IGetName : BaseInterface
    {
        new string Note() => "aaa";

        public class InAnInterface
        {
            int val;
        }
    }

    public class RefClass : IGetName
    {
        string Note() => "aaaa";
        public RefClass(string name)
        {
            this.Name = name;
        }
        public void CallTest(int u)
        {
            System.Console.WriteLine("Call Test");
            return;
        }
        public void CallTestF(float u)
        {
            System.Console.WriteLine("Call Test F");
            return;
        }
        public string Name { get; }
    }

    public struct RefStruct
    {
        public string Name;
        private string InternalName;
    }
}

namespace TestCase.Reference2
{
    public class RefClass2
    {
        public int value;   
    }
}
