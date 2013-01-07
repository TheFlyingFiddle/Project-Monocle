using System;
using System.Text;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;

using MemoryStream = System.IO.MemoryStream;
using Monocle.Content.Serialization;

namespace Content_Test.Serialization
{
    [TestFixture]
    public class BinaryPrimivitesSerializationTests
    {
        protected BinaryWriter writer;
        protected BinaryReader reader;
        private Mock<ITypeReaderFactory> readerProviderMock;
        private Mock<ITypeWriterFactory> writerProviderMock;
        
        [SetUp]
        public void Setup()
        {
            this.readerProviderMock = new Mock<ITypeReaderFactory>();
            this.writerProviderMock = new Mock<ITypeWriterFactory>();

            MemoryStream stream = new MemoryStream();

            reader = new BinaryReader(stream, readerProviderMock.Object);
            writer = new BinaryWriter(stream, writerProviderMock.Object);
        }

                
        [Test]
        public void CanSerializeBytes([Random(0, byte.MaxValue, 5)] int val)
        {
            this.writer.Write((byte)val);
            this.writer.Stream.Position = 0;
            var result = this.reader.ReadByte();

            Assert.AreEqual((byte)val, result);
        }

        [Test]
        public void CanSerializeSignedBytes([Random(sbyte.MinValue, sbyte.MaxValue, 5)] int val)
        {
            this.writer.Write((sbyte)val);
            this.writer.Stream.Position = 0;
            var result = this.reader.ReadSByte();

            Assert.AreEqual((sbyte)val, result);
        }

        [Test]
        public void CanSerializeSignedShorts([Random(short.MinValue, short.MaxValue, 5)] int val)
        {
            this.writer.Write((short)val);
            this.writer.Stream.Position = 0;
            var result = this.reader.ReadInt16();

            Assert.AreEqual((short)val, result);
        }

        [Test]
        public void CanSerializeUnsignedShorts([Random(ushort.MinValue, ushort.MaxValue, 5)] int val)
        {
            this.writer.Write((ushort)val);
            this.writer.Stream.Position = 0;
            var result = this.reader.ReadUInt16();

            Assert.AreEqual((ushort)val, result);
        }

        [Test]
        public void CanSerializeSignedIntegers([Random(int.MinValue, int.MaxValue, 5)] int val)
        {
            this.writer.Write(val);
            this.writer.Stream.Position = 0;
            var result = this.reader.ReadInt32();

            Assert.AreEqual(val, result);
        }

        [Test]
        public void CanSerializeUnsignedSignedIntegers([Random(uint.MinValue, uint.MaxValue, 5)] double val)
        {
            this.writer.Write((uint)val);
            this.writer.Stream.Position = 0;
            var result = this.reader.ReadUInt32();

            Assert.AreEqual((uint)val, result);
        }

        [Test]
        public void CanSerializeSignedLongs([Random(long.MinValue, long.MaxValue, 5)] double val)
        {
            this.writer.Write((long)val);
            this.writer.Stream.Position = 0;
            var result = this.reader.ReadInt64();

            Assert.AreEqual((long)val, result);
        }

        [Test]
        public void CanSerializeUnsignedLongs([Random(ulong.MinValue, ulong.MaxValue, 5)] double val)
        {
            this.writer.Write((ulong)val);
            this.writer.Stream.Position = 0;
            var result = this.reader.ReadUInt64();

            Assert.AreEqual((ulong)val, result);
        }

        [Test]
        public void CanSerializeCharacters([Random(char.MinValue, char.MaxValue, 5)] int val)
        {
            this.writer.Write((char)val);
            this.writer.Stream.Position = 0;
            var result = this.reader.ReadChar();

            Assert.AreEqual((char)val, result);
        }

        [Test]
        public void CanSerializeBooleans()
        {
            this.writer.Write(true);
            this.writer.Stream.Position = 0;
            bool result = this.reader.ReadBool();

            Assert.IsTrue(result);

            this.writer.Stream.Position = 0;

            this.writer.Write(false);
            this.writer.Stream.Position = 0;
            result = this.reader.ReadBool();

            Assert.IsTrue(!result);
        }

        [Test]
        public void CanSerializeFloats([Random(float.MinValue, float.MaxValue, 5)] double val)
        {
            this.writer.Write((float)val);
            this.writer.Stream.Position = 0;
            float result = this.reader.ReadFloat();

            Assert.AreEqual((float)val, result);
        }

        [Test]
        public void CanSerializeDoubles([Random(double.MinValue, double.MaxValue, 5)] double val)
        {
            this.writer.Write(val);
            this.writer.Stream.Position = 0;
            var result = this.reader.ReadDouble();

            Assert.AreEqual(val, result);
        }

        [Test]
        public void ExceptionOnNullWriteValue()
        {
            Assert.Throws<ArgumentNullException>(() => this.writer.Write((object)null));
        }

        [Test]
        public void TypeWriterProviderUsedInWrite()
        {
            writerProviderMock.Setup(tp => tp.GetTypeWriter(typeof(TimeSpan))).Returns(new Mock<ITypeWriter>().Object);
            this.writer.Write(TimeSpan.Zero);
            writerProviderMock.Verify(tp => tp.GetTypeWriter(typeof(TimeSpan)));
        }

        [Test]
        public void TypeReaderProvidedUsedInRead()
        {
            var readerMock = new Mock<ITypeReader>();

            readerProviderMock.Setup(tp => tp.GetTypeReader(typeof(TimeSpan))).Returns(readerMock.Object);
            readerMock.Setup(rm => rm.ReadType(It.IsAny<IReader>())).Returns(default(TimeSpan));
            this.reader.Read<TimeSpan>();
            readerProviderMock.Verify(tp => tp.GetTypeReader(typeof(TimeSpan)));
        }

        [Test]
        public void ExceptionOnNullStreamInWriterCtor()
        {
            Assert.Throws<ArgumentNullException>(() => new BinaryWriter(null, new Mock<ITypeWriterFactory>().Object));
        }

        [Test]
        public void ExceptionOnNullTypeWriterProviderInWriterCtor()
        {
            Assert.Throws<ArgumentNullException>(() => new BinaryWriter(new MemoryStream(), null));
        }

        [Test]
        public void ExceptionOnNullStreamInReaderCtor()
        {
            Assert.Throws<ArgumentNullException>(() => new BinaryReader(null, new Mock<ITypeReaderFactory>().Object));
        }

        [Test]
        public void ExceptionOnNullTypeReaderProviderInReaderCtor()
        {
            Assert.Throws<ArgumentNullException>(() => new BinaryReader(new MemoryStream(), null));
        }

        [Test]
        public void ExceptionWhenUnableToWriteASpecificType()
        {
            writerProviderMock.Setup(tp => tp.GetTypeWriter(typeof(TimeSpan))).Throws<NotSupportedException>();
            Assert.Throws<NotSupportedException>(() => this.writer.Write(TimeSpan.FromDays(123)));
        }

        [Test]
        public void ExceptionWhenUnableToReadASpecificType()
        {
            readerProviderMock.Setup(tp => tp.GetTypeReader(typeof(TimeSpan))).Throws<NotSupportedException>();
            Assert.Throws<NotSupportedException>(() => this.reader.Read<TimeSpan>());
        }
    }
}