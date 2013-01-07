using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Monocle.Content.Serialization
{
    public class ReflectionTypeReader : ITypeReader
    {
        private readonly Type type;
        private readonly ConstructorInfo ctor;

        
        internal ReflectionTypeReader(Type type)
        {
            this.type = type;
            if (!HasSerializableCtor(out ctor))
                ValidateType();
        }

        private bool HasSerializableCtor(out ConstructorInfo ctor)
        {
            var ctors = type.GetConstructors();
            foreach (var constructor in ctors)
            {
                if (constructor.IsDefined(typeof(SerializeConstructorAttribute), false))
                {
                    ctor = constructor;
                    return true;
                }
            }

            if (!type.HasDefaultConstructor())
                throw new ArgumentException("Type lacks a serializable ctor and no default ctor was found for the type.");

            ctor = null;
            return false; 
        }

        private void ValidateType()
        {
            this.ValidateType(type);
            foreach (var field in type.GetAllFields())
            {
                if (!field.IsDefined(typeof(IgnoreSerializeAttribute), false))
                    this.ValidateType(field.FieldType);
            }

            foreach (var property in type.GetProperties().Where(e => e.CanWrite && e.CanRead))
            {
                if (!property.IsDefined(typeof(IgnoreSerializeAttribute), false))
                    this.ValidateType(property.PropertyType);
            }
        }

        private void ValidateType(Type type)
        {
            if (type.IsPointer)
                throw new ArgumentException("Pointers cannot be deserialized!");
            else if (type.IsArray && type.GetArrayRank() > 1)
                throw new ArgumentException("Cannot deserialize multi dimentional arrayss");
            else if (type.IsGenericTypeDefinition)
                throw new ArgumentException("Cannot deserialize generic type definition types");
        }

        private void ValidateMembers(Type member)
        {
            if (type.IsPointer)
                throw new ArgumentException("Pointers cannot be deserialized!");
            else if (type.IsArray && type.GetArrayRank() > 1)
                throw new ArgumentException("Cannot deserialize multi dimentional arrayss");
            else if (type.IsGenericTypeDefinition)
                throw new ArgumentException("Cannot deserialize generic type definition types");
            else if (type.IsSubclassOf(typeof(Delegate)))
                throw new ArgumentException("Delegates cannot be serialized!");
        }
 
        public object ReadType(IReader reader)
        {
            if (ctor == null)
                return ReadTypeUsingType(reader);
            else
                return ReadTypeUsingCtor(reader);

        }

        private object ReadTypeUsingCtor(IReader reader)
        {
            List<object> parameters = new List<object>();
            foreach (var parameter in ctor.GetParameters())
            {
                var paramValue = reader.Read(parameter.ParameterType);
                parameters.Add(paramValue);
            }

            return Activator.CreateInstance(this.type, parameters.ToArray());
        }

        private object ReadTypeUsingType(IReader reader)
        {
            object obj = Activator.CreateInstance(type);

            foreach (var field in type.GetAllFields())
            {
                if (!CanReadField(field))
                    continue;

                object fieldValue = reader.Read(field.FieldType);
                field.SetValue(obj, fieldValue);
            }

            return obj;
        }

        private bool CanReadField(FieldInfo field)
        {
            if (field.IsDefined(typeof(CompilerGeneratedAttribute), false))
            {
                var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
                PropertyInfo info = this.type.GetProperty(field.Name.Substring(1, field.Name.IndexOf('>') - 1), flags);
                return !info.IsDefined(typeof(IgnoreSerializeAttribute), false) &&
                       !info.PropertyType.IsSubclassOf(typeof(Delegate));
            }


            return !field.IsDefined(typeof(IgnoreSerializeAttribute), false) &&
                   !field.FieldType.IsSubclassOf(typeof(Delegate));
        }

        public Type GetRedableType()
        {
            return this.type;
        }
    }
}