using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Content_Test.Serialization
{
    class TypeValueSource
    {
        public IEnumerable<Type> ReflectableTypes()
        {
            return new List<Type>
            {
                typeof(SimplePrimitiveClass),
                typeof(SimpleGenericClass<int>),
                typeof(SimpleGenericClass<SimplePrimitiveClass>),
                typeof(SimpleGenericClass<SimpleGenericClass<SimpleObjectClass>>),
                typeof(SimpleObjectClass),
                typeof(TimeSpan),
                typeof(AttributeTargets),
                typeof(Base64FormattingOptions),
                typeof(HasSerializableCtor)
            };
        }

        public IEnumerable<Type> NonReflectableTypes()
        {
            return new List<Type>
            {
                typeof(SimpleGenericClass<>),
                typeof(Action),
                typeof(HasNonSerializableMembers),
                typeof(LacksDefaultCtor),
                typeof(float*)
            };
        }
    }
}
