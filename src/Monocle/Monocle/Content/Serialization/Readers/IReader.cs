using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Monocle.Content.Serialization
{
    public interface IReader
    {
        /// <summary>
        /// Reads an object from the serialized data.
        /// </summary>
        /// <typeparam name="T">The type of the object to be read.</typeparam>
        /// <returns>A deserialized object.</returns>
        T Read<T>();

        /// <summary>
        /// Reads an object from the serialized data.
        /// </summary>
        /// <param name="type">Type of the object to read.</param>
        /// <returns>A deserialized object.</returns>
        object Read(Type type);


        /// <summary>
        /// Reads a byte from the serialized data.
        /// </summary>
        /// <returns>A deserialized byte.</returns>
        byte ReadByte();

        /// <summary>
        /// Reads a sbyte from the serialized data.
        /// </summary>
        /// <returns>A deserialized sbyte.</returns>
        sbyte ReadSByte();

        /// <summary>
        /// Reads a short form the serialized data.
        /// </summary>
        /// <returns>A deserialized short.</returns>
        short ReadInt16();

        /// <summary>
        /// Reads a ushort form the serialized data.
        /// </summary>
        /// <returns>A deserialized ushort.</returns>
        ushort ReadUInt16();

        /// <summary>
        /// Reads an int form the serialized data.
        /// </summary>
        /// <returns>A deserialized int.</returns>
        int ReadInt32();

        /// <summary>
        /// Reads an uint form the serialized data.
        /// </summary>
        /// <returns>A deserialized uint.</returns>
        uint ReadUInt32();

        /// <summary>
        /// Reads a long from the serialized data.
        /// </summary>
        /// <returns>A deserialized long.</returns>
        long ReadInt64();

        /// <summary>
        /// Reads a ulong from the serialized data.
        /// </summary>
        /// <returns>A deserialized ulong.</returns>
        ulong ReadUInt64();

        /// <summary>
        /// Reads a float form the serialized data.
        /// </summary>
        /// <returns>A deserialized float.</returns>
        float ReadFloat();

        /// <summary>
        /// Reads a double from the serialized data.
        /// </summary>
        /// <returns>A deserialized double.</returns>
        double ReadDouble();

        /// <summary>
        /// Reads a char from the serialized data.
        /// </summary>
        /// <returns>A deserialized char.</returns>
        char ReadChar();

        /// <summary>
        /// Reads a boolean from the serialized data.
        /// </summary>
        /// <returns>A deserialized bool.</returns>
        bool ReadBool();

        /// <summary>
        /// Reads a decimal from the serialized data.
        /// </summary>
        /// <returns>A decerialized decimal.</returns>
        decimal ReadDecimal();

        /// <summary>
        /// Reads a string from the serialized data.
        /// </summary>
        /// <returns>A deserialized string.</returns>
        string ReadString();

    }
}
