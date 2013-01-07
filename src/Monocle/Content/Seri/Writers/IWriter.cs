using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Content.Serialization
{
    /// <summary>
    ///  Writer that can serialize C# objects.
    /// </summary>
    public interface IWriter
    {
        /// <summary>
        /// Writes an object to serialized data.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="toWrite">The object to write.</param>
        void Write(object toWrite);

        /// <summary>
        /// Serializes a byte.
        /// </summary>
        /// <param name="toWrite">Byte to serialize.</param>
        void Write(byte toWrite);

        /// <summary>
        /// Serializes a signed byte.
        /// </summary>
        /// <param name="toWrite">Signed byte to serialize.</param>
        void Write(sbyte toWrite);

        /// <summary>
        /// Serializes a 16 bit signed integer.
        /// </summary>
        /// <param name="toWrite">16 bit signed integer to serialize.</param>
        void Write(short toWrite);

        /// <summary>
        /// Serializes a 16 bit unsigned integer.
        /// </summary>
        /// <param name="toWrite">16 bit unsigned integer to serialize.</param>
        void Write(ushort toWrite);

        /// <summary>
        /// Serializes a 32 bit signed integer.
        /// </summary>
        /// <param name="toWrite">32 bit signed integer to serialize.</param>
        void Write(int toWrite);

        /// <summary>
        /// Serializes a 32 bit unsigned integer.
        /// </summary>
        /// <param name="toWrite">32 bit signed integer to serialize.</param>
        void Write(uint toWrite);

        /// <summary>
        /// Serializes a 64 bit signed integer.
        /// </summary>
        /// <param name="toWrite">64 bit signed integer to serialize.</param>
        void Write(long toWrite);

        /// <summary>
        /// Serializes a 64 bit unsigned integer.
        /// </summary>
        /// <param name="toWrite">64 bit unsigned integer to serialize.</param>
        void Write(ulong toWrite);

        /// <summary>
        /// Serializes a 32 bit floting-point-number.
        /// </summary>
        /// <param name="toWrite">32 bit floting-point-number to serialize.</param>
        void Write(float toWrite);

        /// <summary>
        /// Serializes a 64 bit floting-point-number.
        /// </summary>
        /// <param name="toWrite">64 bit floting-point-number to serialize.</param>
        void Write(double toWrite);

        /// <summary>
        /// Serializes a 128 bit high-precision-floting-point-number.
        /// </summary>
        /// <param name="toWrite">128 bit high-precision-floting-point-number to serialize.</param>
        void Write(decimal toWrite);

        /// <summary>
        /// Serializes a byte.
        /// </summary>
        /// <param name="toWrite">Byte to serialize.</param>
        void Write(char toWrite);

        /// <summary>
        /// Serializes a byte.
        /// </summary>
        /// <param name="toWrite">Byte to serialize.</param>
        void Write(bool toWrite);

        /// <summary>
        /// Serializes a string.
        /// </summary>
        /// <param name="toWrite">String to serialize.</param>
        void Write(string toWrite);

    }
}
