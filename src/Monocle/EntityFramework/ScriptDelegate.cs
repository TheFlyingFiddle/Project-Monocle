using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace EntityFramework
{
    public class ScriptMethod
    {
        private readonly string methodName;
        private readonly Dictionary<object, Action> instances;

        public ScriptMethod(string methodName)
        {
            this.methodName = methodName;
            this.instances = new Dictionary<object, Action>();
        }

        public bool TrackInstance(object instance)
        {
            var methodInfo = instance.GetType().GetMethod(methodName);
            if (methodInfo != null)
            {
                var _del = CreateDelegate(methodInfo, instance);
                this.instances.Add(instance, _del);
                this.Method += _del;
                return true;
            }
            return false;
        }

        public bool UnTrackInstance(object instance)
        {
            Action _del;
            if (this.instances.TryGetValue(instance, out _del))
            {
                this.Method -= _del;
                this.instances.Remove(instance);
                return true;
            }
            return false;
        }

        private Action CreateDelegate(MethodInfo methodInfo, object instance)
        {
            return (Action)Delegate.CreateDelegate(typeof(Action), instance, methodInfo);
        }

        public Action Method;
    }

    public class ScriptMethod<T> 
    {
        private readonly string methodName;
        private readonly Dictionary<object, Action<T>> instances;

        public ScriptMethod(string methodName)
        {
            this.methodName = methodName;
            this.instances = new Dictionary<object, Action<T>>();
        }

        public bool TrackInstance(object instance)
        {
            var methodInfo = instance.GetType().GetMethod(methodName);
            if (methodInfo != null)
            {
                var _del = CreateDelegate(methodInfo, instance);
                this.instances.Add(instance, _del);
                Method += _del;
                return true;
            }
            return false;
        }

        public bool UnTrackInstance(object instance)
        {
            Action<T> _del;
            if (this.instances.TryGetValue(instance, out _del))
            {
                this.Method -= _del;
                this.instances.Remove(instance);
                return true;
            }
            return false;
        }

        private Action<T> CreateDelegate(MethodInfo methodInfo, object instance)
        {
            return (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), instance, methodInfo);
        }

        public Action<T> Method;
    }
}