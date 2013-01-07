using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Stream = System.IO.Stream;
using Writer = System.IO.BinaryWriter;

namespace Content.Serialization
{
    public class BinaryWriter : IWriter
    {
        private readonly ITypeWriterFactory writerProvider;
        private readonly Writer writer;
        
        public BinaryWriter(Stream stream, ITypeWriterFactory writerProvider)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (writerProvider == null)
                throw new ArgumentNullException("writerProvider");

            this.writer = new Writer(stream);
            this.writerProvider = writerProvider;
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

        public void Write(object toWrite)
        {
            if (toWrite == null)
                throw new ArgumentNullException("toWrite");

            var typeWriter = this.writerProvider.GetTypeWriter(toWrite.GetType());
            typeWriter.WriteType(toWrite, this);
        }

        public Stream Stream
        {
            get { return this.writer.BaseStream; }
        }


        public void Write(string toWrite)
        {
            this.writer.Write(toWrite.Length);
            this.writer.Write(Encoding.UTF8.GetBytes(toWrite));
        }
    }
}
