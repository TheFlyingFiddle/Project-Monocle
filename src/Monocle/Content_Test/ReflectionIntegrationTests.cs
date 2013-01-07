using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Monocle.Content.Serialization;

namespace Content_Test.Serialization
{
    [TestFixture]
    class ReflectionIntegrationTests
    {
        [Test]
        public void CanSerializeValidTypes([ValueSource("Values")] object value)
        {
            var stream = new System.IO.MemoryStream();

            AssetWriter.WriteAsset(stream, value, new TypeWriterFactory());
            stream.Position = 0;
            object result = AssetReader.ReadAsset<object>(stream, new TypeReaderFactory());

            Assert.AreEqual(value, result);
        }


        public IEnumerable<object> Values()
        {
            return new List<object>()
            {
                new SimplePrimitiveClass(),
                new SimpleGenericClass<int>(),
                new SimpleGenericClass<SimpleGenericClass<SimplePrimitiveClass>>(new SimpleGenericClass<SimplePrimitiveClass>(new SimplePrimitiveClass())),
                new TimeSpan(123),
                new HasSerializableCtor(10),
                new HasIgnoredField(132,1325),
                new HasPropertyWithBackingField(123),
                new HasAutoProperty(123),
                new HasIgnoredAutoProperty(123),
                new HasHardSerializableCtor(1,2,3)
            };
        }

    }
}
