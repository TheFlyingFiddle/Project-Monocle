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
    class TypeReaderFactoryTests 
    {
        private TypeReaderFactory provider;

        [SetUp]
        public void Setup()
        {
            this.provider = new TypeReaderFactory();
        }

        [Test]
        public void CanRegisterValidTypeReader()
        {
            var mockedReader = new Mock<ITypeReader>();
            mockedReader.Setup(tr => tr.GetRedableType()).Returns(typeof(FakeType));
            this.provider.RegisterTypeReader(mockedReader.Object);
        }

        [Test]
        public void CantRegisterInvalidTypeReader()
        {
            var mockedReader = new Mock<ITypeReader>();
            mockedReader.Setup(tr => tr.GetRedableType()).Returns(typeof(int));
            Assert.Throws<ArgumentException>(
                () => this.provider.RegisterTypeReader(mockedReader.Object));    
        }

        [Test]
        public void CantRegisterDuplicateTypeReader()
        {
            var mockedReader = new Mock<ITypeReader>();
            mockedReader.Setup(tr => tr.GetRedableType()).Returns(typeof(FakeType));
            this.provider.RegisterTypeReader(mockedReader.Object);

            Assert.Throws<ArgumentException>(
                () => provider.RegisterTypeReader(mockedReader.Object));
        }

        [Test]
        public void CantRegisterMultipleTypeReadersForSameType()
        {
            var mockedReader0 = new Mock<ITypeReader>();
            mockedReader0.Setup(tr => tr.GetRedableType()).Returns(typeof(FakeType));
            var mockedReader1 = new Mock<ITypeReader>();
            mockedReader1.Setup(tr => tr.GetRedableType()).Returns(typeof(FakeType));

            this.provider.RegisterTypeReader(mockedReader0.Object);

            Assert.Throws<ArgumentException>(
                () => provider.RegisterTypeReader(mockedReader1.Object));
        }

        [Test]
        public void CanGetRegisteredTypeReader()
        {
            var mockedReader = new Mock<ITypeReader>();
            mockedReader.Setup(tr => tr.GetRedableType()).Returns(typeof(FakeType));
            this.provider.RegisterTypeReader(mockedReader.Object);

            Assert.AreSame(mockedReader.Object, this.provider.GetTypeReader<FakeType>());
            Assert.AreSame(mockedReader.Object, this.provider.GetTypeReader(typeof(FakeType)));
        }

        [Test]
        public void CanGetReadersForNonRegisteredValidTypes([ValueSource(typeof(TypeValueSource), "ReflectableTypes")] Type type)
        {
            var typeReader = this.provider.GetTypeReader(type);
            Assert.NotNull(typeReader);
        }

        [Test]
        public void CantGetReaderForNonRegisteredInvalidTypes([ValueSource(typeof(TypeValueSource),"NonReflectableTypes")] Type type)
        {
            Assert.Throws<ArgumentException>(() => this.provider.GetTypeReader(type));
        }

        [Test]
        public void CantGetReaderForMultiDimentionalArrays()
        {
            Assert.Throws<RankException>(() => this.provider.GetTypeReader(typeof(int[, ,])));
        }

        private class FakeType { }
    }

}
