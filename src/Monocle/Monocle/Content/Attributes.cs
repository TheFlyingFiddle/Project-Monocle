using System;

namespace Monocle.Content.Serialization
{
    /// <summary>
    /// Attribute used to identify methods that can serialize 
    /// a specific c# type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SerializerAttribute : Attribute
    { }

    /// <summary>
    /// Attribute used to identiy methods that can deserialize 
    /// a specific type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class DeserializerAttribute : Attribute
    { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class IgnoreSerializeAttribute : Attribute
    { }


    [AttributeUsage(AttributeTargets.Constructor)]
    public class SerializeConstructorAttribute : Attribute
    { }

    [AttributeUsage(AttributeTargets.Class)]
    public class TypeReaderAttribute : Attribute
    { }

    [AttributeUsage(AttributeTargets.Class)]
    public class ContentWriterAttribute : Attribute
    { }

    [AttributeUsage(AttributeTargets.Class)]
    public class GenericTypeWriterAttribute : Attribute
    { }

    [AttributeUsage(AttributeTargets.Class)]
    public class GenericTypeReaderAttribute : Attribute
    { }
}
