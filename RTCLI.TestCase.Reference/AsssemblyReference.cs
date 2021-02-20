using System;

namespace Reference
{
    public interface IGetName
    {
        string Name { get; }
    }

    public class RefClass : IGetName
    {
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
