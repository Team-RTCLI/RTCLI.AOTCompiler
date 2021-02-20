using System;
using System.Collections.Generic;
using System.Text;

namespace TestCase
{
    class TestThrow
    {
        public static void Test()
        {
            object value = null;
            try
            {
                if (value != null)
                    throw new Exception();
            }
            catch
            {

            }
        }
    }
}
