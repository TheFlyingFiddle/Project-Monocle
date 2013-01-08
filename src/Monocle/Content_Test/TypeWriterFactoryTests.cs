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
    class TypeWriterFactoryTests
    {
        private TypeWriterFactory provider;

        [SetUp]
        public void Setup()
        {
            this.provider = new TypeWriterFactory();
        }

        [Test]
        public void CanRegisterValidTypeWriter()
        {
            var mockedReader = new Mock<ITypeWriter>();
            mockedReader.Setup(tr => tr.GetInputType()).Returns(typeof(FakeType));
            this.provider.RegisterTypeWriter(mockedReader.Object);
        }

        [Test]
        public void CantRegisterInvalidTypeWriter()
        {
            var mockedReader = new Mock<ITypeWriter>();
            mockedReader.Setup(tr => tr.GetInputType()).Returns(typeof(int));
            Assert.Throws<ArgumentException>(
                () => this.provider.RegisterTypeWriter(mockedReader.Object));
        }

        [Test]
        public void CantRegisterDuplicateTypeWriter()
        {
            var mockedReader = new Mock<ITypeWriter>();
            mockedReader.Setup(tr => tr.GetInputType()).Returns(typeof(FakeType));
            this.provider.RegisterTypeWriter(mockedReader.Object);

            Assert.Throws<ArgumentException>(
                () => provider.RegisterTypeWriter(mockedReader.Object));
        }

        [Test]
        public void CantRegisterMultipleTypeWritersForSameType()
        {
            var mockedWriter0 = new Mock<ITypeWriter>();
            mockedWriter0.Setup(tr => tr.GetInputType()).Returns(typeof(FakeType));
            var mockedWriter1 = new Mock<ITypeWriter>();
            mockedWriter1.Setup(tr => tr.GetInputType()).Returns(typeof(FakeType));

            this.provider.RegisterTypeWriter(mockedWriter0.Object);

            Assert.Throws<ArgumentException>(() => provider.RegisterTypeWriter(mockedWriter1.Object));
        }

        [Test]
        public void CanGetRegisteredTypeWriter()
        {
            var mockedWriter = new Mock<ITypeWriter>();
            mockedWriter.Setup(tr => tr.GetInputType()).Returns(typeof(FakeType));
            this.provider.RegisterTypeWriter(mockedWriter.Object);

            Assert.AreSame(mockedWriter.Object, this.provider.GetTypeWriter<FakeType>());
            Assert.AreSame(mockedWriter.Object, this.provider.GetTypeWriter(typeof(FakeType)));
        }

        [Test]
        public void CanGetWriterForNonRegisteredValidTypes([ValueSource(typeof(TypeValueSource), "ReflectableTypes")] Type type)
        {
            var typeWriter = this.provider.GetTypeWriter(type);
            Assert.NotNull(typeWriter);
        }

        [Test]
        public void CantGetWriterForNonRegisteredInvalidTypes([ValueSource(typeof(TypeValueSource), "NonReflectableTypes")] Type type)
        {
            Assert.Throws<ArgumentException>(() => this.provider.GetTypeWriter(type));
        }

        [Test]
        public void CantGetWriterForMultiDimentionalArrays()
        {
            Assert.Throws<RankException>(() => this.provider.GetTypeWriter(typeof(int[, ,])));
        }

        private class FakeType { }
    }
}