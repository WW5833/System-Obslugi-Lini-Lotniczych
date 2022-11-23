using System;
using System.Collections.Generic;
using System.Linq;

namespace LotSystem;

public static class Extensions
{
    public static T CreateInstance<T>(this Type type, Dictionary<Type, object> injectableTypes)
        => (T)CreateInstance(type, injectableTypes);

    public static object CreateInstance(this Type type, Dictionary<Type, object> injectableTypes)
    {
        List<object> args = new List<object>();

        var ctr = type.GetConstructors().FirstOrDefault(x => x.GetParameters().Length > 0);
        if (ctr != null)
        {
            foreach (var parameterInfo in ctr.GetParameters())
            {
                if (injectableTypes.TryGetValue(parameterInfo.ParameterType, out var value))
                    args.Add(value);
            }
        }

        return Activator.CreateInstance(type, args.ToArray());
    }
}
