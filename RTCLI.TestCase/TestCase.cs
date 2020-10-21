using System;

using RTCLI.TestCase.Reference;

namespace RTCLI.TestCase
{
    public class TestCaseClass
    {
        void Method()
        {
            var reff = new RefClass("TestCase");
            refed = new RefClass("TestCase");

            string[] TestArray = { "", "", "" };
            Array arr = TestArray;
            var test = (arr as string[])[1];
            string[] arr_typed = (string[])arr;
        }
        RefClass refed;
    }
}
