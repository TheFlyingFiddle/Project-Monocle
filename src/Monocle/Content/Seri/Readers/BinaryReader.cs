using System;

using Reader = System.IO.BinaryReader;
using Stream = System.IO.Stream;
using System.Text;

namespace Content.Serialization
{
    public class BinaryReader : IReader
    {
        private readonly ITypeReaderFactory readerProvider;
        private readonly Reader reader;
           
        public BinaryReader(Stream stream, ITypeReaderFactory readerProvider)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (readerProvider == null)
                throw new ArgumentNullException("readerProvider");

            this.reader = new Reader(stream);
            this.readerProvider = readerProvider;
        }

        public byte ReadByte()
        {
            return this.reader.ReadByte();
        }

        public sbyte ReadSByte()
        {
            return this.reader.ReadSByte();
        }

        public short ReadInt16()
        {
            return this.reader.ReadInt16();
        }

        public ushort ReadUInt16()
        {
            return this.reader.ReadUInt16();
        }

        public int ReadInt32()
        {
            return this.reader.ReadInt32();
        }

        public uint ReadUInt32()
        {
            return this.reader.ReadUInt32();
        }

        public long ReadInt64()
        {
            return this.reader.ReadInt64();
        }

        public ulong ReadUInt64()
        {
            return this.reader.ReadUInt64();
        }

        public char ReadChar()
        {
            return this.reader.ReadChar();
        }

        public bool ReadBool()
        {
            return this.reader.ReadBoolean();
        }

        public float ReadFloat()
        {
            return this.reader.ReadSingle();
        }

        public double ReadDouble()
        {
            return this.reader.ReadDouble();
        }

        public decimal ReadDecimal()
        {
            return this.reader.ReadDecimal();
        }

        public object Read(Type type)
        {
            var typeReader = this.readerProvider.GetTypeReader(type);
            return typeReader.ReadType(this);
        }

        public T Read<T>()
        {
            return (T)this.Read(typeof(T));
        }


        public string ReadString()
        {
            int length = this.reader.ReadInt32();
            return Encoding.UTF8.GetString(this.reader.ReadBytes(length));

        }
    }
}
