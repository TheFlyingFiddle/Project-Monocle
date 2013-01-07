using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Content.Serialization
{
    public class TypeReaderFactory : ITypeReaderFactory
    {
        private readonly IDictionary<Type, ITypeReader> typeReaders;
        private readonly IDictionary<Type, Type> genericReaders;

        public TypeReaderFactory()
        {
            this.typeReaders = new Dictionary<Type, ITypeReader>();
            this.genericReaders = new Dictionary<Type, Type>();      
            this.AddPrimitiveReaders();
        }

        private void AddPrimitiveReaders()
        {
            this.RegisterTypeReader(new UnsignedByteReader());
            this.RegisterTypeReader(new SignedByteReader());
            this.RegisterTypeReader(new Int16Reader());
            this.RegisterTypeReader(new UInt16Reader());
            this.RegisterTypeReader(new Int32Reader());
            this.RegisterTypeReader(new UInt32Reader());
            this.RegisterTypeReader(new Int64Reader());
            this.RegisterTypeReader(new UInt64Reader());
            this.RegisterTypeReader(new FloatReader());
            this.RegisterTypeReader(new DoubleReader());
            this.RegisterTypeReader(new DecimalReader());
            this.RegisterTypeReader(new CharacterReader());
            this.RegisterTypeReader(new BooleanReader());
        }

        public ITypeReader GetTypeReader<T>()
        {
            return this.GetTypeReader(typeof(T));
        }

        public ITypeReader GetTypeReader(Type type)
        {
            ITypeReader typeReader;
            if (this.typeReaders.TryGetValue(type, out typeReader))
                return typeReader;

            typeReader = this.CreateTypeReader(type);
            this.typeReaders.Add(type, typeReader);
            return typeReader;
        }

        private ITypeReader CreateTypeReader(Type type)
        {
            if (type.IsEnum)
                return new EnumTypeReader(type);
            else if (type.IsArray)
                return CreateArrayReader(type);
            else if (type.IsGenericType)
            {
                ITypeReader reader;
                if (TryCreateGenericReader(type, out reader))
                    return reader;
            }
  

            return new ReflectionTypeReader(type);
        }

        private bool TryCreateGenericReader(Type type, out ITypeReader reader)
        {
            Type genericReader;
            if (this.genericReaders.TryGetValue(type.GetGenericTypeDefinition(), out genericReader))
            {
                genericReader = genericReader.MakeGenericType(type.GetGenericArguments());
                reader = (ITypeReader)Activator.CreateInstance(genericReader);
                return true;
            }

            reader = null;
            return false;
        }

        private ITypeReader CreateArrayReader(Type arrayType)
        {
            if (arrayType.GetArrayRank() > 1)
                throw new RankException("Cannot serialize multidimentional arrays!");

            Type type = typeof(ArrayReader<>).MakeGenericType(new Type[] { arrayType.GetElementType() });
            return (ITypeReader)Activator.CreateInstance(type);      
        }

        public void RegisterTypeReader(ITypeReader typeReader)
        {
            Type type = typeReader.GetRedableType();
            if (this.typeReaders.ContainsKey(type))
                throw new ArgumentException(string.Format("A type reader is already registered for the type {0}", type));

            this.typeReaders.Add(type, typeReader);
        }

        public void RegisterGenericTypeReader(Type genericTypeReader)
        {
            if (!typeof(ITypeReader).IsAssignableFrom(genericTypeReader))
                throw new ArgumentException("Not a type reader!");
            if (!genericTypeReader.IsGenericType)
                throw new ArgumentException("Not a generic type reader!");

            Type typeDefinition = genericTypeReader.BaseType.GetGenericArguments()[0];
            this.genericReaders.Add(typeDefinition.GetGenericTypeDefinition(), genericTypeReader);
        }
    }
}
