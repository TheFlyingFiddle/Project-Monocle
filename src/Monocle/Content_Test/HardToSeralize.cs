using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Content.Serialization;

namespace Content_Test.Serialization
{
    class LacksDefaultCtor
    {
        private int value;
        public LacksDefaultCtor(int value) { }

        public override bool Equals(object obj)
        {
            if (obj is LacksDefaultCtor)
                return value == (obj as LacksDefaultCtor).value;
            return base.Equals(obj);
        }
    }

    class HasSerializableCtor
    {
        private int value;

        [SerializeConstructor]
        public HasSerializableCtor(int value)
        { this.value = value; }

        public override bool Equals(object obj)
        {
            if (obj is HasSerializableCtor)
                return value == (obj as HasSerializableCtor).value;
            return base.Equals(obj);
        }
    }

    class HasIgnoredField
    {
        private int value1;

        [IgnoreSerialize]
        private int value2;

        public HasIgnoredField() { }

        public HasIgnoredField(int value1, int value2) 
        {
            this.value1 = value1;
            this.value2 = value2;
        }


        public override bool Equals(object obj)
        {
            if (obj is HasIgnoredField)
                return value1 == (obj as HasIgnoredField).value1
                    && value2 != (obj as HasIgnoredField).value2;
            return base.Equals(obj);
        }
    }

    class HasPropertyWithBackingField
    {
        //This value should only be serialized ONCE!
        private int backing;

        public int Backing
        {
            get { return this.backing; }
            set { this.backing = value; }
        }

        public HasPropertyWithBackingField() { }
        public HasPropertyWithBackingField(int value)
        {
            this.backing = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is HasPropertyWithBackingField)
                return backing == (obj as HasPropertyWithBackingField).backing;
            return base.Equals(obj);
        }
    }

    class HasAutoProperty
    {
        public int NonBackingProperty
        {
            get;
            set;
        }

        public HasAutoProperty() { }

        public HasAutoProperty(int p)
        {
            this.NonBackingProperty = p;
        }

        public override bool Equals(object obj)
        {
            if (obj is HasAutoProperty)
                return NonBackingProperty == (obj as HasAutoProperty).NonBackingProperty;
            return base.Equals(obj);
        }
    }


    class HasIgnoredAutoProperty
    {
        [IgnoreSerialize]
        public int Bam
        {
            get;
            set;
        }

        public HasIgnoredAutoProperty() { }

        public HasIgnoredAutoProperty(int p)
        {
            this.Bam = p;
        }

        public override bool Equals(object obj)
        {
            if (obj is HasIgnoredAutoProperty)
                return Bam != (obj as HasIgnoredAutoProperty).Bam;
            return base.Equals(obj);
        }
    }

    class HasHardSerializableCtor
    {
        private int value1;
        private int value2;

        public int Value3
        {
            get;
            set;
        }

        public int Value1
        {
            get { return this.value1; }
            set { this.value1 = value; } 
        }

        [SerializeConstructor]
        public HasHardSerializableCtor(int value1, int value2, int value3)
        {
        }


        public override bool Equals(object obj)
        {
            if (obj is HasHardSerializableCtor)
                return value1 == (obj as HasHardSerializableCtor).value1
                    && value2 == (obj as HasHardSerializableCtor).value2
                    && Value3 == (obj as HasHardSerializableCtor).Value3;
            return base.Equals(obj);
        }
    }
}
