using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Content.Serialization
{
    public class UnsignedByteWriter : TypeWriter<byte>
    {
        public override void WriteType(byte toWrite, IWriter writer)
        {
            writer.Write(toWrite);
        }
    }

    public class SignedByteWriter : TypeWriter<sbyte>
    {
        public override void WriteType(sbyte toWrite, IWriter writer)
        {
            writer.Write(toWrite);
        }
    }

    public class Int16Writer : TypeWriter<short>
    {
        public override void WriteType(short toWrite, IWriter writer)
        {
            writer.Write(toWrite);
        }
    }

    public class UInt16Writer : TypeWriter<ushort>
    {
        public override void WriteType(ushort toWrite, IWriter writer)
        {
            writer.Write(toWrite);
        }
    }

    public class Int32Writer : TypeWriter<int>
    {
        public override void WriteType(int toWrite, IWriter writer)
        {
            writer.Write(toWrite);
        }
    }

    public class UInt32Writer : TypeWriter<uint>
    {
        public override void WriteType(uint toWrite, IWriter writer)
        {
            writer.Write(toWrite);
        }
    }

    public class Int64Writer : TypeWriter<long>
    {
        public override void WriteType(long toWrite, IWriter writer)
        {
            writer.Write(toWrite);
        }
    }

    public class UInt64Writer : TypeWriter<ulong>
    {
        public override void WriteType(ulong toWrite, IWriter writer)
        {
            writer.Write(toWrite);
        }
    }

    public class CharacterWriter : TypeWriter<char>
    {
        public override void WriteType(char toWrite, IWriter writer)
        {
            writer.Write(toWrite);
        }
    }

    public class BooleanWriter : TypeWriter<bool>
    {
        public override void WriteType(bool toWrite, IWriter writer)
        {
            writer.Write(toWrite);
        }
    }

    class FloatWriter : TypeWriter<float>
    {
        public override void WriteType(float toWrite, IWriter writer)
        {
            writer.Write(toWrite);
        }
    }

    public class DoubleWriter : TypeWriter<double>
    {
        public override void WriteType(double toWrite, IWriter writer)
        {
            writer.Write(toWrite);
        }
    }

    public class DecimalWriter : TypeWriter<decimal>
    {
        public override void WriteType(decimal toWrite, IWriter writer)
        {
            writer.Write(toWrite);
        }
    }

    public class StringWriter : TypeWriter<string>
    {
        public override void WriteType(string toWrite, IWriter writer)
        {
            writer.Write(toWrite);
        }
    }


}
