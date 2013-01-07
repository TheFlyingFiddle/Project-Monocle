using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Content.Serialization
{
   public class UnsignedByteReader : TypeReader<byte>
    {
       public override byte Read(IReader reader)
        {
            return reader.ReadByte();
        }
    }

    public class SignedByteReader : TypeReader<sbyte>
    {
        public override sbyte Read(IReader reader)
        {
            return reader.ReadSByte();
        }
    }

    public class Int16Reader : TypeReader<short>
    {
        public override short Read(IReader reader)
        {
            return reader.ReadInt16();
        }
    }

    public class UInt16Reader : TypeReader<ushort>
    {
        public override ushort Read(IReader reader)
        {
            return reader.ReadUInt16(); 
        }
    }

    public class Int32Reader : TypeReader<int>
    {
        public override int Read(IReader reader)
        {
            return reader.ReadInt32();
        }
    }

    public class UInt32Reader : TypeReader<uint>
    {
        public override uint Read(IReader reader)
        {
            return reader.ReadUInt32(); 
        }
    }

    public class Int64Reader : TypeReader<long>
    {
        public override long Read(IReader reader)
        {
            return reader.ReadInt64();
        }
    }

    public class UInt64Reader : TypeReader<ulong>
    {
        public override ulong Read(IReader reader)
        {
            return reader.ReadUInt64(); 
        }
    }

    public class CharacterReader : TypeReader<char>
    {
        public override char Read(IReader reader)
        {
            return reader.ReadChar();
        }
    }

    public class BooleanReader : TypeReader<bool>
    {
        public override bool Read(IReader reader)
        {
            return reader.ReadBool();
        }
    }

    public class FloatReader : TypeReader<float>
    {
        public override float Read(IReader reader)
        {
            return reader.ReadFloat();
        }
    }

    public class DoubleReader : TypeReader<double>
    {
        public override double Read(IReader reader)
        {
            return reader.ReadDouble();
        }
    }

    public class DecimalReader : TypeReader<decimal>
    {
        public override decimal Read(IReader reader)
        {
            return reader.ReadDecimal();
        }
    }

    public class StringReader : TypeReader<string>
    {
        public override string Read(IReader reader)
        {
            return reader.ReadString();
        }
    }
}
