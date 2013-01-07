using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Monocle.Content.Serialization
{
    public class AssetWriter : IWriter
    {
        private static byte VERSION = 1;

        private readonly MemoryStream contentStream;
        private readonly Dictionary<Type, int> typeMap;
        private readonly List<ITypeWriter> typeWriters;
        private ITypeWriterFactory typeWriterFactory;
        private BinaryWriter writer;
        

        public AssetWriter(ITypeWriterFactory typeWriterFactory)
        {
            this.typeWriterFactory = typeWriterFactory;
            this.typeMap = new Dictionary<Type, int>();
            this.typeWriters = new List<ITypeWriter>();
            this.contentStream = new MemoryStream();
            this.writer = new BinaryWriter(contentStream);
        }

        public static void WriteAsset(Stream stream, object asset, ITypeWriterFactory typeWriterFactory)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            else if (typeWriterFactory == null)
                throw new ArgumentNullException("typeWriterFactory");


            var writer = new AssetWriter(typeWriterFactory);
            writer.Write(asset);

            var contentStream = writer.writer.BaseStream;

            writer.writer = new BinaryWriter(stream);
            writer.Write(VERSION);
            writer.WriteTypesUsed();

            contentStream.Position = 0;
            contentStream.CopyTo(stream);

            contentStream.Dispose();
        }

        private void WriteTypesUsed()
        {
            this.writer.Write(this.typeWriters.Count);
            foreach (var item in this.typeWriters)
            {
                this.Write(item.GetWritableType().AssemblyQualifiedName);
            }
        }
        

        public void Write(object toWrite)
        {
            Type t = toWrite.GetType();
            if (this.typeMap.ContainsKey(t))
            {
                WriteType(t, toWrite);
            }
            else if (t == null)
            {
                this.writer.Write(-1);
            }
            else
            {
                this.AddType(t);
                WriteType(t, toWrite);
            }

        }

        private void AddType(Type t)
        {
            this.typeMap.Add(t, this.typeWriters.Count);
            this.typeWriters.Add(this.typeWriterFactory.GetTypeWriter(t));
        }

        private void WriteType(Type type, object toWrite)
        {
            int index = typeMap[type];
            this.writer.Write(index);
            this.typeWriters[index].WriteType(toWrite, this);
        }

        public void Write(byte toWrite)
        {
            this.writer.Write(toWrite);
        }

        public void Write(sbyte toWrite)
        {
            this.writer.Write(toWrite);
        }

        public void Write(short toWrite)
        {
            this.writer.Write(toWrite);
        }

        public void Write(ushort toWrite)
        {
            this.writer.Write(toWrite);
        }

        public void Write(int toWrite)
        {
            this.writer.Write(toWrite);
        }

        public void Write(uint toWrite)
        {
            this.writer.Write(toWrite);
        }

        public void Write(long toWrite)
        {
            this.writer.Write(toWrite);
        }

        public void Write(ulong toWrite)
        {
            this.writer.Write(toWrite);
        }

        public void Write(float toWrite)
        {
            this.writer.Write(toWrite);
        }

        public void Write(double toWrite)
        {
            this.writer.Write(toWrite);
        }

        public void Write(decimal toWrite)
        {
            this.writer.Write(toWrite);
        }

        public void Write(char toWrite)
        {
            this.writer.Write(toWrite);
        }

        public void Write(bool toWrite)
        {
            this.writer.Write(toWrite);
        }

        public void Write(string toWrite)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(toWrite);
            this.writer.Write(bytes.Length);
            this.writer.Write(bytes);
        }
    }
}
