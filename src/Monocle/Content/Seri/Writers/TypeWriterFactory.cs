using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Content.Serialization
{
    class TypeWriterFactory : ITypeWriterFactory
    {
        private readonly IDictionary<Type, ITypeWriter> typeWriters;
        private readonly IDictionary<Type, Type> genericWriters;

        public TypeWriterFactory()
        {
            this.typeWriters = new Dictionary<Type, ITypeWriter>();
            this.genericWriters = new Dictionary<Type, Type>();
            this.RegisterPrimitiveWriters();
        }

        private void RegisterPrimitiveWriters()
        {
            this.RegisterTypeWriter(new UnsignedByteWriter());
            this.RegisterTypeWriter(new SignedByteWriter());
            this.RegisterTypeWriter(new Int16Writer());
            this.RegisterTypeWriter(new UInt16Writer());
            this.RegisterTypeWriter(new Int32Writer());
            this.RegisterTypeWriter(new UInt32Writer());
            this.RegisterTypeWriter(new Int64Writer());
            this.RegisterTypeWriter(new UInt64Writer());
            this.RegisterTypeWriter(new FloatWriter());
            this.RegisterTypeWriter(new DoubleWriter());
            this.RegisterTypeWriter(new DecimalWriter());
            this.RegisterTypeWriter(new CharacterWriter());
            this.RegisterTypeWriter(new BooleanWriter());
        }


        public ITypeWriter GetTypeWriter<T>()
        {
            return this.GetTypeWriter(typeof(T));
        }

        public ITypeWriter GetTypeWriter(Type type)
        {
            ITypeWriter typeWriter;
            if (this.typeWriters.TryGetValue(type, out typeWriter))
                return typeWriter;

            typeWriter = this.CreateTypeWriter(type);
            this.typeWriters.Add(type, typeWriter);
            return typeWriter;
        }

        private ITypeWriter CreateTypeWriter(Type type)
        {
            if (type.IsEnum)
                return new EnumTypeWriter(type);
            else if (type.IsArray)
                return CreateArrayWriter(type);
            else if (type.IsGenericType)
            {
                ITypeWriter writer;
                if (TryCreateGenericWriter(type, out writer))
                    return writer;
            }
    
            return new ReflectionTypeWriter(type);
        }

        private ITypeWriter CreateArrayWriter(Type arrayType)
        {
            if (arrayType.GetArrayRank() > 1)
                throw new RankException("Cannot serialize multidimentional arrays!");

            Type type = typeof(ArrayWriter<>).MakeGenericType(new Type[] { arrayType.GetElementType() });
            return (ITypeWriter)Activator.CreateInstance(type);     
        }

        private bool TryCreateGenericWriter(Type type, out ITypeWriter writer)
        {
            Type genericWriter;
            if (this.genericWriters.TryGetValue(type.GetGenericTypeDefinition(), out genericWriter))
            {
                genericWriter = genericWriter.MakeGenericType(type.GetGenericArguments());
                writer = (ITypeWriter)Activator.CreateInstance(genericWriter);
                return true;
            }

            writer = null;
            return false;
        }

        public void RegisterTypeWriter(ITypeWriter writer)
        {
            Type type = writer.GetWritableType();
            if (this.typeWriters.ContainsKey(type))
                throw new ArgumentException(string.Format("A type writer is already registered for the type {0}", type));

            this.typeWriters.Add(type, writer);
        }

        public void RegisterGenericTypeWriter(Type genericTypeWriterType)
        {
            if (!typeof(ITypeWriter).IsAssignableFrom(genericTypeWriterType))
                throw new ArgumentException("Not a type writer!");
            if (!genericTypeWriterType.IsGenericType)
                throw new ArgumentException("Not a generic type writer!");

            Type typeDefinition = genericTypeWriterType.BaseType.GetGenericArguments()[0];
            this.genericWriters.Add(typeDefinition.GetGenericTypeDefinition(), genericTypeWriterType);
        }
    }
}
