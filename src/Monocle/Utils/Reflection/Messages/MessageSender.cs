using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;

using MessageCollection = System.Collections.Generic.Dictionary<string, System.Action<object,object>>;


namespace Utils
{
    /// <summary>
    /// This specifies if a receiver of a message 
    /// is required to provide a message method or not.
    /// </summary>
    public enum MessageOptions
    {
        RequireReceiver,
        DontRequireReceiver
    }

    /// <summary>
    /// A utility class that enables the sending of messages.
    /// </summary>
    public static class MessageSender
    {
        //Thread safe lock.
        private static readonly object _lock = new object();

        //Stores delegates from types to messages.
        private static readonly Dictionary<Type, MessageCollection> messages;

        static MessageSender()
        {
            messages = new Dictionary<Type, MessageCollection>();
        }

        /// <summary>
        /// Sends a message. A message is a runtime checked method call.
        /// </summary>
        /// <param name="toSendTo">The object to send the message to.</param>
        /// <param name="methodName">The name of the method accepting the message.</param>
        /// <param name="parameter">Data sent with the message.</param>
        /// <param name="opt">Specifies if the receiving object required to have a method capable of handling the message.</param>
        public static void SendMessage(object toSendTo, string methodName, object parameter, MessageOptions opt)
        {
            MessageCollection delegateDict;
            if(messages.TryGetValue(toSendTo.GetType(), out delegateDict)) 
            {
                Action<object,object> _del;
                if (delegateDict.TryGetValue(methodName, out _del))
                    _del.Invoke(toSendTo, parameter);
                else if (opt == MessageOptions.DontRequireReceiver)
                    return;
                else        
                  throw new MessageException("The message {0} does not exist in the type {1}", methodName, toSendTo.GetType());
            } else 
               throw new MessageException("The type {0} has not been registered with the MessageSender.");
        }

        /// <summary>
        /// Caches the types able to receive messages in the assemblies provided.
        /// </summary>
        /// <param name="assembly">Assemblies.</param>
        public static void CacheAssemblieMessages(params Assembly[] assembly)
        {
            CacheAssemblieMessages((IEnumerable<Assembly>)assembly);
        }

        /// <summary>
        /// Caches the types able to receive messages in the assemblies provided.
        /// </summary>
        /// <param name="assembly">Assemblies.</param>
        public static void CacheAssemblieMessages(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    //Check if proper receiving type.
                    if ((typeof(IReceiver)).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract || type.IsValueType)
                    {
                        CacheMessagMethods(type);
                    }
                }
            }
        }

        private static void CacheMessagMethods(Type toCache)
        {
            lock (_lock)
            {
                if (messages.ContainsKey(toCache))
                    return;

                messages.Add(toCache, new MessageCollection());

                var methods = toCache.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var method in methods)
                {
                    var attribs = method.GetCustomAttributes(typeof(MessageAttribute), true);
                    if (attribs.Length > 0)
                    {
                        var delegatedMethod = CreateDelegatedMethod(method);
                        messages[toCache].Add(method.Name, delegatedMethod);
                    }
                }
            }
        }

        private static Action<object, object> CreateDelegatedMethod(MethodInfo method)
        {
            var parameters = method.GetParameters();
            if (parameters.Length == 1)
                return CreateSingleParameterDelegate(method, parameters[0].ParameterType);
            else if (parameters.Length == 0)
                return CreateParameterlessDelegate(method);
            else
                throw new MessageException("Only parameterless methods or methods with one argument can be Message methods. "
                                        + "\nThe method {0} located in type {1} does not folow this rule.");        
        }

        private static Action<object, object> CreateParameterlessDelegate(MethodInfo info)
        {
            var target = Expression.Parameter(typeof(object));
            var param = Expression.Parameter(typeof(object));
            var body = Expression.Call(Expression.Convert(target, info.DeclaringType), info);

            return Expression.Lambda<Action<object, object>>(body, target, param).Compile();
        }

        private static Action<object, object> CreateSingleParameterDelegate(MethodInfo info, Type parameterType)
        {

            var target = Expression.Parameter(typeof(object));
            var param = Expression.Parameter(typeof(object));
            var body = Expression.Call(Expression.Convert(target, info.DeclaringType), info,
                                       Expression.Convert(param, parameterType));

            return Expression.Lambda<Action<object, object>>(body, target, param).Compile();
        }
    }
}
