using System;
using System.Collections.Generic;
using System.Text;

namespace RTCLI.TestCase
{
    class TestThrow
    {
        public object Test(object value)
        {
            try
            {
                if (value != null)
                    throw new Exception();
            }
            catch
            {
                return value;
            }
            return null;
        }
    }
}
