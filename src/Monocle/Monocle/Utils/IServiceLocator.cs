using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Utils
{
    public interface IServiceLocator
    {
        void RegisterService<T>(T service);
        void UnregisterService<T>();
        T GetService<T>();
    }

    public class ServiceLocator : IServiceLocator
    {
        private class Provider
        {
            public object service;

            public Provider(object service)
            {
                this.service = service;
            }
        }

        private readonly Dictionary<Type, Provider> provider = new Dictionary<Type,Provider>();

        public void RegisterService<T>(T service)
        {
            provider.Add(typeof(T), new Provider(service));
        }

        public void UnregisterService<T>()
        {
            provider.Remove(typeof(T));
        }

        public T GetService<T>()
        {
            Provider tmp;
            if (provider.TryGetValue(typeof(T), out tmp))
            {
                return (T)tmp.service;
            }
            else
            {
                throw new ArgumentException("A service of type " + typeof(T) + " is not present in the service locator");
            }
        }
    }
}
