using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using Monocle.Content.Serialization;

namespace Content_Test.Serialization
{
    [TestFixture]
    class ReflectionTypeWriterTests
    {
        private Mock<IWriter> mockedWriter;

        [SetUp]
        public void Setup()
        {
            mockedWriter = new Mock<IWriter>();
        }

        [Test]
        public void CanCreateReflectionWriterForReflectableType([ValueSource(typeof(TypeValueSource), "ReflectableTypes")] Type type)
        {
            new ReflectionTypeWriter(type);
        }

        [Test]
        public void CantCreatereflectionWriterForNonReflectableTypes([ValueSource(typeof(TypeValueSource), "NonReflectableTypes")] Type type)
        {
            Assert.Throws<ArgumentException>(() => new ReflectionTypeWriter(type));
        }

        [Test]
        public void IgnoresFieldThatUseIgnoreSerializeAttribute()
        {
            var toWrite = new HasIgnoredField();
            var reflectionWriter = new ReflectionTypeWriter(toWrite.GetType());
            reflectionWriter.WriteType(toWrite, this.mockedWriter.Object);

            this.mockedWriter.Verify(mr => mr.Write(It.IsAny<object>()), Times.Once());
        }

        [Test]
        public void NonAutoPropertiesAreNotDeSerialized()
        {
            var toWrite = new HasPropertyWithBackingField();
            var reflectionWriter = new ReflectionTypeWriter(toWrite.GetType());
            reflectionWriter.WriteType(toWrite, this.mockedWriter.Object);

            this.mockedWriter.Verify(mr => mr.Write(It.IsAny<object>()), Times.Once());
        }

        [Test]
        public void AutoPropertiesAreDeserialized()
        {
            var toWrite = new HasAutoProperty();
            var reflectionWriter = new ReflectionTypeWriter(toWrite.GetType());
            reflectionWriter.WriteType(toWrite ,this.mockedWriter.Object);

            this.mockedWriter.Verify(mr => mr.Write(It.IsAny<object>()), Times.Once());
        }

        [Test]
        public void IgnoredAutoPropertiesAreNotDeserialized()
        {
            var toWrite = new HasIgnoredAutoProperty(123);
            var reflectionWriter = new ReflectionTypeWriter(toWrite.GetType());
            reflectionWriter.WriteType(toWrite, this.mockedWriter.Object);

            this.mockedWriter.Verify(mr => mr.Write(It.IsAny<Type>()), Times.Never());
        }

        [Test]
        public void CanWriteHardSerializeableCtor()
        {
            var toWrite = new HasHardSerializableCtor(1, 2, 3);
            var reflectionWriter = new ReflectionTypeWriter(toWrite.GetType());
            reflectionWriter.WriteType(toWrite, this.mockedWriter.Object);

            this.mockedWriter.Verify(mw => mw.Write(It.IsAny<object>()), Times.Exactly(3));
        }

        [Test]
        public void CantWriteWrongType()
        {
            var toWrite = "Hello";
            var reflectionWriter = new ReflectionTypeWriter(typeof(SimpleObjectClass));
            
            Assert.Throws<ArgumentException>(() => reflectionWriter.WriteType(toWrite, this.mockedWriter.Object));
        }
    }
}
