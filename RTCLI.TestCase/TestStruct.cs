using System;
using System.Collections.Generic;
using System.Text;
using RTCLITestCase.Reference;

namespace RTCLI.TestCase
{
    public struct PureStruct
    {
        public String name;
        public float val;
    }

    class TestStruct
    {
        public float Test(PureStruct arg)
        {
            PureStruct local = new PureStruct();
            local.name = "asd";
            arg = local;
            arg = new PureStruct();
            arg = new PureStruct { val = 10f };
            return arg.val;
        }
    }
}
