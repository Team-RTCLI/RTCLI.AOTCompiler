using System;
using System.Collections.Generic;
using System.Text;

namespace TestCase
{
    class TestGeneric
    {
        /*
        public class GenericClass<T> where T : new()
        {
            T fieldInGenericClass;
            public void MethodInGenericClass(T arg)
            {
                arg = new T();
            }
        }
        public static void GenericMethod<T>() where T : new()
        {
            var a = new GenericClass<T>();
            a.MethodInGenericClass(new T());
        }
        public void MethodUseGenericClass()
        {
            GenericMethod<PureStruct>();
            GenericMethod<float>();
            GenericMethod<TestGeneric>();
        }

        public delegate TResult GenericDelegate<TArg, out TResult>(TArg arg);
        */
    }
}
