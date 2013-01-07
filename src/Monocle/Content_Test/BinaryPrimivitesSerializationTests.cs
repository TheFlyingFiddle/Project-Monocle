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
        protected MemoryStream stream;
        private Mock<ITypeReaderFactory> readerProviderMock;
        private Mock<ITypeWriterFactory> writerProviderMock;
        
        [SetUp]
        public void Setup()
        {
            this.readerProviderMock = new Mock<ITypeReaderFactory>();
            this.writerProviderMock = new Mock<ITypeWriterFactory>();

            stream = new MemoryStream();
        }

                
        [Test]
        public void CanSerializeBytes([Random(0, byte.MaxValue, 5)] int val)
        {
            this.writerProviderMock.Setup((w) => w.GetTypeWriter(typeof(byte))).Returns(new UnsignedByteWriter());
            this.readerProviderMock.Setup((w) => w.GetTypeReader(typeof(byte))).Returns(new UnsignedByteReader());


            AssetWriter.WriteAsset(this.stream, (byte)val, this.writerProviderMock.Object);
            this.stream.Position = 0;
            var result = AssetReader.ReadAsset<byte>(stream, this.readerProviderMock.Object);

            Assert.AreEqual((byte)val, result);
        }

        [Test]
        public void CanSerializeSignedBytes([Random(sbyte.MinValue, sbyte.MaxValue, 5)] int val)
        {
            this.writerProviderMock.Setup((w) => w.GetTypeWriter(typeof(sbyte))).Returns(new SignedByteWriter());
            this.readerProviderMock.Setup((w) => w.GetTypeReader(typeof(sbyte))).Returns(new SignedByteReader());

            AssetWriter.WriteAsset(this.stream, (sbyte)val, this.writerProviderMock.Object);
            this.stream.Position = 0;
            var result = AssetReader.ReadAsset<sbyte>(stream, this.readerProviderMock.Object);

            Assert.AreEqual((sbyte)val, result);
        }

        [Test]
        public void CanSerializeSignedShorts([Random(short.MinValue, short.MaxValue, 5)] int val)
        {
            this.writerProviderMock.Setup((w) => w.GetTypeWriter(typeof(short))).Returns(new Int16Writer());
            this.readerProviderMock.Setup((w) => w.GetTypeReader(typeof(short))).Returns(new Int16Reader());


            AssetWriter.WriteAsset(this.stream, (short)val, this.writerProviderMock.Object);
            this.stream.Position = 0;
            var result = AssetReader.ReadAsset<short>(stream, this.readerProviderMock.Object);

            Assert.AreEqual((short)val, result);
        }

        [Test]
        public void CanSerializeUnsignedShorts([Random(ushort.MinValue, ushort.MaxValue, 5)] int val)
        {
            this.writerProviderMock.Setup((w) => w.GetTypeWriter(typeof(ushort))).Returns(new UInt16Writer());
            this.readerProviderMock.Setup((w) => w.GetTypeReader(typeof(ushort))).Returns(new UInt16Reader());


            AssetWriter.WriteAsset(this.stream, (ushort)val, this.writerProviderMock.Object);
            this.stream.Position = 0;
            var result = AssetReader.ReadAsset<ushort>(stream, this.readerProviderMock.Object);

            Assert.AreEqual((ushort)val, result);
        }

        [Test]
        public void CanSerializeSignedIntegers([Random(int.MinValue, int.MaxValue, 5)] int val)
        {
            this.writerProviderMock.Setup((w) => w.GetTypeWriter(typeof(int))).Returns(new Int32Writer());
            this.readerProviderMock.Setup((w) => w.GetTypeReader(typeof(int))).Returns(new Int32Reader());


            AssetWriter.WriteAsset(this.stream, (int)val, this.writerProviderMock.Object);
            this.stream.Position = 0;
            var result = AssetReader.ReadAsset<int>(stream, this.readerProviderMock.Object);

            Assert.AreEqual((int)val, result);
        }

        [Test]
        public void CanSerializeUnsignedIntegers([Random(uint.MinValue, uint.MaxValue, 5)] double val)
        {
            this.writerProviderMock.Setup((w) => w.GetTypeWriter(typeof(uint))).Returns(new UInt32Writer());
            this.readerProviderMock.Setup((w) => w.GetTypeReader(typeof(uint))).Returns(new UInt32Reader());


            AssetWriter.WriteAsset(this.stream, (uint)val, this.writerProviderMock.Object);
            this.stream.Position = 0;
            var result = AssetReader.ReadAsset<uint>(stream, this.readerProviderMock.Object);

            Assert.AreEqual((uint)val, result);
        }

        [Test]
        public void CanSerializeSignedLongs([Random(long.MinValue, long.MaxValue, 5)] double val)
        {
            this.writerProviderMock.Setup((w) => w.GetTypeWriter(typeof(long))).Returns(new Int64Writer());
            this.readerProviderMock.Setup((w) => w.GetTypeReader(typeof(long))).Returns(new Int64Reader());


            AssetWriter.WriteAsset(this.stream, (long)val, this.writerProviderMock.Object);
            this.stream.Position = 0;
            var result = AssetReader.ReadAsset<long>(stream, this.readerProviderMock.Object);

            Assert.AreEqual((long)val, result);
        }

        [Test]
        public void CanSerializeUnsignedLongs([Random(ulong.MinValue, ulong.MaxValue, 5)] double val)
        {
            this.writerProviderMock.Setup((w) => w.GetTypeWriter(typeof(ulong))).Returns(new UInt64Writer());
            this.readerProviderMock.Setup((w) => w.GetTypeReader(typeof(ulong))).Returns(new UInt64Reader());


            AssetWriter.WriteAsset(this.stream, (ulong)val, this.writerProviderMock.Object);
            this.stream.Position = 0;
            var result = AssetReader.ReadAsset<ulong>(stream, this.readerProviderMock.Object);

            Assert.AreEqual((ulong)val, result);
        }

        [Test]
        public void CanSerializeCharacters([Random(char.MinValue, char.MaxValue, 5)] int val)
        {
            this.writerProviderMock.Setup((w) => w.GetTypeWriter(typeof(char))).Returns(new CharacterWriter());
            this.readerProviderMock.Setup((w) => w.GetTypeReader(typeof(char))).Returns(new CharacterReader());

            AssetWriter.WriteAsset(this.stream, (char)val, this.writerProviderMock.Object);
            this.stream.Position = 0;
            var result = AssetReader.ReadAsset<char>(stream, this.readerProviderMock.Object);

            Assert.AreEqual((char)val, result);
        }

        [Test]
        public void CanSerializeBooleans()
        {

            this.writerProviderMock.Setup((w) => w.GetTypeWriter(typeof(bool))).Returns(new BooleanWriter());
            this.readerProviderMock.Setup((w) => w.GetTypeReader(typeof(bool))).Returns(new BooleanReader());


            AssetWriter.WriteAsset(this.stream, true, this.writerProviderMock.Object);
            this.stream.Position = 0;
            var result = AssetReader.ReadAsset<bool>(stream, this.readerProviderMock.Object);

            Assert.IsTrue(result);

            this.stream.Position = 0;
            AssetWriter.WriteAsset(this.stream, false, this.writerProviderMock.Object);
            this.stream.Position = 0;
            result = AssetReader.ReadAsset<bool>(stream, this.readerProviderMock.Object);

            Assert.IsTrue(!result);
        }

        [Test]
        public void CanSerializeFloats([Random(float.MinValue, float.MaxValue, 5)] double val)
        {

            this.writerProviderMock.Setup((w) => w.GetTypeWriter(typeof(float))).Returns(new FloatWriter());
            this.readerProviderMock.Setup((w) => w.GetTypeReader(typeof(float))).Returns(new FloatReader());


            AssetWriter.WriteAsset(this.stream, (float)val, this.writerProviderMock.Object);
            this.stream.Position = 0;
            var result = AssetReader.ReadAsset<float>(stream, this.readerProviderMock.Object);

            Assert.AreEqual((float)val, result);
        }

        [Test]
        public void CanSerializeDoubles([Random(double.MinValue, double.MaxValue, 5)] double val)
        {
            this.writerProviderMock.Setup((w) => w.GetTypeWriter(typeof(double))).Returns(new DoubleWriter());
            this.readerProviderMock.Setup((w) => w.GetTypeReader(typeof(double))).Returns(new DoubleReader());


            AssetWriter.WriteAsset(this.stream, (double)val, this.writerProviderMock.Object);
            this.stream.Position = 0;
            var result = AssetReader.ReadAsset<double>(stream, this.readerProviderMock.Object);

            Assert.AreEqual((double)val, result);
        }
        
    }
}