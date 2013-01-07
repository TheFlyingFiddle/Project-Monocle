using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Monocle.Content.Serialization
{
    public class AssetReader : IReader
    {
        private static byte VERSION = 1;

        private readonly BinaryReader reader;
        private readonly ITypeReader[] typeReaders;


        private AssetReader(Stream stream, ITypeReader[] typeReaders)
        {
            this.reader = new BinaryReader(stream);
            this.typeReaders = typeReaders;
        }
        
        public static T ReadAsset<T>(Stream stream, ITypeReaderFactory readerProvider)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (readerProvider == null)
                throw new ArgumentNullException("readerProvider");

            BinaryReader reader = new BinaryReader(stream);

            var version = reader.ReadByte();
            if (version != VERSION)
                throw new ArgumentException("The stream does not contain a valid file!");


            var typeReaders = ReadTypesUsed(readerProvider, reader);
            var assetReader = new AssetReader(stream, typeReaders);
            
            return assetReader.Read<T>();
        }

        private static ITypeReader[] ReadTypesUsed(ITypeReaderFactory readerProvider, BinaryReader reader)
        {

            int count = (int)reader.ReadUInt32();
            ITypeReader[] typeReaders = new ITypeReader[count];
            for (int i = 0; i < count; i++)
            {
                int length = reader.ReadInt32();
                string typeName = Encoding.UTF8.GetString(reader.ReadBytes(length));

                Type type = Type.GetType(typeName);
                typeReaders[i] = readerProvider.GetTypeReader(type);
            }
            return typeReaders;
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
            int readerIndex = this.reader.ReadInt32();
            if (readerIndex == -1)
                return null;

            var typeReader = typeReaders[readerIndex];

            if(type.IsAssignableFrom(typeReader.GetRedableType()))
                return typeReader.ReadType(this);

            throw new ArgumentException("Stream is corrupted!");
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