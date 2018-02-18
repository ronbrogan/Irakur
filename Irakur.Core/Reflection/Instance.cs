using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Irakur.Core.Reflection
{
    public static class Instance
    {
        private delegate object ObjectActivator(params object[] args);
        private static ConcurrentDictionary<Type, ObjectActivator> ConstructorCache = new ConcurrentDictionary<Type, ObjectActivator>();

        public static T Create<T>(params object[] args)
        {
            var type = typeof(T);

            if (ConstructorCache.ContainsKey(type))
                return (T)ConstructorCache[type](args);

            return (T)Create(type, args);
        }

        public static object Create(Type type, params object[] args)
        {
            var types = args.Select(p => p.GetType());
            var constructor = type.GetConstructor(types.ToArray());

            var paraminfo = constructor.GetParameters();

            var paramex = Expression.Parameter(typeof(object[]), "args");

            var argex = new Expression[paraminfo.Length];
            for (int i = 0; i < paraminfo.Length; i++)
            {
                var index = Expression.Constant(i);
                var paramType = paraminfo[i].ParameterType;
                var accessor = Expression.ArrayIndex(paramex, index);
                var cast = Expression.Convert(accessor, paramType);
                argex[i] = cast;
            }

            var newex = Expression.New(constructor, argex);
            var lambda = Expression.Lambda(typeof(ObjectActivator), newex, paramex);
            var result = (ObjectActivator)lambda.Compile();
            ConstructorCache.TryAdd(type, result);
            return result(args);
        }


    }
}
