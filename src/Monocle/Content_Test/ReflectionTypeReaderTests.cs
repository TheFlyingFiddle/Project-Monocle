using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using Content.Serialization;

namespace Content_Test.Serialization
{
    [TestFixture]
    class ReflectionTypeReaderTests 
    {
        private Mock<IReader> mockedReader;

        [SetUp]
        public void Setup()
        {
            mockedReader = new Mock<IReader>();
        }

        [Test]
        public void CanCreateReflectionReaderForReflectableType([ValueSource(typeof(TypeValueSource), "ReflectableTypes")] Type type)
        {
           new ReflectionTypeReader(type);
        }

        [Test]
        public void CantCreateReflectionReaderForNonReflectableTypes([ValueSource(typeof(TypeValueSource), "NonReflectableTypes")] Type type)
        {
            Assert.Throws<ArgumentException>(() => new ReflectionTypeReader(type));
        }

        [Test]
        public void IgnoresFieldThatUseIgnoreSerializeAttribute()
        {
            var reflectionReader = new ReflectionTypeReader(typeof(HasIgnoredField));
            reflectionReader.ReadType(this.mockedReader.Object);

            this.mockedReader.Verify(mr => mr.Read(It.IsAny<Type>()), Times.Once());            
        }

        [Test]
        public void NonAutoPropertiesAreNotDeSerialized()
        {
            var reflectionReader = new ReflectionTypeReader(typeof(HasPropertyWithBackingField));
            reflectionReader.ReadType(this.mockedReader.Object);

            this.mockedReader.Verify(mr => mr.Read(typeof(int)), Times.Once());
        }

        [Test]
        public void AutoPropertiesAreDeserialized()
        {
            var reflectionReader = new ReflectionTypeReader(typeof(HasAutoProperty));
            reflectionReader.ReadType(this.mockedReader.Object);

            this.mockedReader.Verify(mr => mr.Read(typeof(int)), Times.Once());
        }

        [Test]
        public void IgnoredAutoPropertiesAreNotDeserialized()
        {
            var reflectionReader = new ReflectionTypeReader(typeof(HasIgnoredAutoProperty));
            reflectionReader.ReadType(this.mockedReader.Object);

            this.mockedReader.Verify(mr => mr.Read(It.IsAny<Type>()), Times.Never());  
        }
    }
}
