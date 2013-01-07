using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Content.Serialization
{
    class ReflectionTypeWriter : ITypeWriter
    {
        private readonly Type type;
        private readonly ConstructorInfo ctor;

        internal ReflectionTypeWriter(Type type)
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
                throw new ArgumentException(string.Format("Type {0} lacks a serializable ctor and no " +
                                                           " default ctor was found for the type.", this.type));

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
                throw new ArgumentException("Pointers cannot be serialized!");
            else if (type.IsArray && type.GetArrayRank() > 1)
                throw new ArgumentException("Cannot serialize multi dimentional arrayss");
            else if (type.IsGenericTypeDefinition)
                throw new ArgumentException("Cannot serialize generic type definition types");
        }

        private void ValidateMembers(Type member)
        {
            if (type.IsPointer)
                throw new ArgumentException("Pointers cannot be deserialized!");
            else if (type.IsArray && type.GetArrayRank() > 1)
                throw new ArgumentException("Cannot serialize multi dimentional arrayss");
            else if (type.IsGenericTypeDefinition)
                throw new ArgumentException("Cannot serialize generic type definition types");
            else if (type.IsSubclassOf(typeof(Delegate)))
                throw new ArgumentException("Delegates cannot be serialized!");
        }

        public void WriteType(object toWrite, IWriter writer)
        {
            if(toWrite.GetType() != this.type)
                throw new ArgumentException(string.Format("Can only write objects of type {0} but toWrite was {1}",
                                                          this.type, toWrite.GetType()));

            if (this.ctor == null)
                this.WriteTypeUsingType(toWrite, writer);
            else
                this.WriteTypeUsingCtor(toWrite, writer);
        }

        private void WriteTypeUsingType(object toWrite, IWriter writer)
        {
            foreach (var field in type.GetAllFields())
            {
                if (!CanWriteField(field))
                    continue;

                object fieldValue = field.GetValue(toWrite);
                writer.Write(fieldValue);
            }
        }

        private void WriteTypeUsingCtor(object toWrite, IWriter writer)
        {
            foreach (var parameter in ctor.GetParameters())
            {
                FieldInfo field;
                if(TryGetField(parameter.Name, out field))
                {
                    object fieldValue = field.GetValue(toWrite);
                    writer.Write(fieldValue);
                    continue;
                }

                PropertyInfo property;
                if (TryGetProperty(parameter.Name, out property))
                {
                    object propertyValue = property.GetValue(toWrite, null);
                    writer.Write(propertyValue);
                    continue;
                }

                throw new ArgumentException(string.Format("Could not find a field or property named : {0}.", parameter.Name));
            }
        }


        private bool TryGetField(string fieldName, out FieldInfo field)
        {
            field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (field != null)
                return true;
            return false;
        }

        private bool TryGetProperty(string propertyName, out PropertyInfo property)
        {
            propertyName = propertyName.Substring(0, 1).ToUpper() + propertyName.Substring(1);
            property = type.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (property != null)
                return true;
            return false;
        }
        
        private bool CanWriteField(FieldInfo field)
        {
            if (field.IsDefined(typeof(CompilerGeneratedAttribute), false))
            {
                var flags =  BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
                PropertyInfo info = this.type.GetProperty(field.Name.Substring(1, field.Name.IndexOf('>') - 1), flags);
                return !info.IsDefined(typeof(IgnoreSerializeAttribute), false) &&
                       !info.PropertyType.IsSubclassOf(typeof(Delegate));
            }

            return !field.IsDefined(typeof(IgnoreSerializeAttribute), false) &&
                   !field.FieldType.IsSubclassOf(typeof(Delegate));
        }

        public Type GetWritableType()
        {
            return this.type;
        }
    }
}
