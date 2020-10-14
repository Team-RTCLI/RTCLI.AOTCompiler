using System;

namespace RTCLI.TestCase.Reference
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
}
