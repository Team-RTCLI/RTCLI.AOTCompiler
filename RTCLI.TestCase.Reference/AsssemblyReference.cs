using System;

namespace RTCLITestCase.Reference
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
        public string Name { get; }
    }

    public struct RefStruct
    {
        public string Name;
        private string InternalName;
    }
}
