using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Content_Test.Serialization
{
    class SimplePrimitiveClass
    {
        public double SomeVar0;
        private bool SomeVar1;
        public int SomeVar2;
        public int SomeVar3;

        public SimplePrimitiveClass() { }

        public override bool Equals(object obj)
        {
            if (obj is SimplePrimitiveClass)
            {
                var other = obj as SimplePrimitiveClass;
                return this.SomeVar0 == other.SomeVar0 &&
                       this.SomeVar1 == other.SomeVar1 &&
                       this.SomeVar2 == other.SomeVar2 &&
                       this.SomeVar3 == other.SomeVar3;
            }
            return false;
        }
    }

    class SimpleGenericClass<T>
    {
        private readonly T value;

        public SimpleGenericClass() { }        
        public SimpleGenericClass(T value)
        {
            this.value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is SimpleGenericClass<T>)
            {
                var other = obj as SimpleGenericClass<T>;
                return this.value.Equals(other.value);
            }
            return false;
        }
    }

    class SimpleObjectClass
    {
        private List<int> SomeVar0;
        internal int[] SomeVar1;
        public string SomeVar2;

        public event Action Delegate;
        public Action Delegate2;

        public SimpleObjectClass() { }

        public SimpleObjectClass(List<int> someList, int[] someArray, string someString)
        {
            this.SomeVar0 = someList;
            this.SomeVar1 = someArray;
            this.SomeVar2 = someString;
        }


        public override bool Equals(object obj)
        {
            if (obj is SimpleObjectClass)
            {
                var other = obj as SimpleObjectClass;
                return this.SomeVar0.Equals(other.SomeVar0) &&
                       this.SomeVar2.Equals(other.SomeVar2) &&
                       this.SomeVar2.Equals(other.SomeVar2);
            }
            return false;
        }
    }
}
