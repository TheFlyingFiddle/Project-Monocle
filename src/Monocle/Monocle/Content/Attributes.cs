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
    public class TypeWriterAttribute : Attribute
    { }

    [AttributeUsage(AttributeTargets.Class)]
    public class GenericTypeWriterAttribute : Attribute
    { }

    [AttributeUsage(AttributeTargets.Class)]
    public class GenericTypeReaderAttribute : Attribute
    { }

    [AttributeUsage(AttributeTargets.Class)]
    public class ImporterAttribute : Attribute
    {
        public bool IsDefault { get; private set; }
        public string[] FileEndings { get; private set; }

        public ImporterAttribute(bool isDefault, params string[] fileEndings)
        {
            this.FileEndings = fileEndings;
            this.IsDefault = isDefault;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ProcessorAttribute : Attribute
    {
        public bool IsDefault { get; private set; }
        public Type InputType { get; private set; }

        public ProcessorAttribute(Type inputType, bool isDefault = false) 
        {
            this.InputType = inputType;
            this.IsDefault = isDefault;
        }
    }
}
