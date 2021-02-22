using System;
using System.Collections.Generic;
using System.Text;

namespace TestCase
{
    class TestGeneric
    {
        public class GenericClass<T> where T : new()
        {
            T fieldInGenericClass;
            public void MethodInGenericClass(T arg)
            {
                arg = new T();
            }
            public static void StaticMethodInGenericClass(T arg)
            {
                arg = new T();
            }
            public void GenericMethodInGenericClass<Y>(Y arg) where Y : new()
            {
                arg = new Y();
            }
        }
        public static void GenericMethod<Y>() where Y : new()
        {
            var a = new GenericClass<Y>();
            a.MethodInGenericClass(new Y());
            GenericClass<Y>.StaticMethodInGenericClass(new Y());
        }
        public void MethodUseGenericClass()
        {
            GenericMethod<TestGeneric>();
        }

        public delegate TResult GenericDelegate<TArg, out TResult>(TArg arg);
    }
}
