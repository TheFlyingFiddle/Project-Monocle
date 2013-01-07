using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Monocle.Content.Serialization;

namespace Content_Test.Serialization
{
    [TestFixture]
    class CollectionSerializationIntegrationTests
    {
        [Test]
        public void CanSerializeValidTypes([ValueSource("Values")] object value)
        {
            var stream = new System.IO.MemoryStream();

            AssetWriter.WriteAsset(stream, value, CreateTypeWriterFactory());
            stream.Position = 0;
            object result = AssetReader.ReadAsset<object>(stream, CreateTypeReaderFactory());

            Assert.AreEqual(value, result);
        }

        private ITypeWriterFactory CreateTypeWriterFactory()
        {
            var factory = new TypeWriterFactory();
            return factory;
        }

        private ITypeReaderFactory CreateTypeReaderFactory()
        {
            var factory = new TypeReaderFactory();
            return factory;
        }

        public IEnumerable<object> Values()
        {
            return new List<object> 
            {
                new int[] { 1, 2, 3, 4, 5 },
                new List<int>() { 1,2,3,4,5},
                new List<List<List<int>>>() { new List<List<int>>() { new List<int>() { 2,3,4,1,5 } } }, 
                new List<SimplePrimitiveClass> { new SimplePrimitiveClass(), new SimplePrimitiveClass() },
                new Dictionary<int,float>() { {1 ,4f}, {4,0.45f} },
                new HashSet<long>() { 123, 12412, 1245, 13931, 181, 914818 },
                new Dictionary<string, ICloneable> { { "Hello", new SimplePrimitiveClass()}, {"Man", new SimpleGenericClass<int>(1) } }  
            };
        }

    }
}
