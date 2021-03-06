﻿using System;
using System.Collections.Generic;
using System.Text;
using TestCase.Reference;

namespace TestCase
{
    class TestStruct
    {
        public struct PureStruct
        {
            public String name;
            public float val;
        }
        public static void Test(PureStruct arg = new PureStruct(), TestStruct arg2 = null)
        {
            PureStruct local = new PureStruct();
            local.name = "asd";
            local = new PureStruct();
            local = new PureStruct { val = 10f };
            local = arg;
        }
    }
}
