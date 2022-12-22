using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LotSystem;

public static class Extensions
{
    public static T CreateInstance<T>(this Type type, Dictionary<Type, object> injectableTypes)
        => (T)CreateInstance(type, injectableTypes);

    public static object CreateInstance(this Type type, Dictionary<Type, object> injectableTypes)
    {
        List<object> args = new();

        var ctr = type.GetConstructors().FirstOrDefault(x => x.GetParameters().Length > 0);
        if (ctr == null)
            return Activator.CreateInstance(type, args.ToArray());

        foreach (var parameterInfo in ctr.GetParameters())
        {
            if (injectableTypes.TryGetValue(parameterInfo.ParameterType, out var value))
                args.Add(value);
        }

        return Activator.CreateInstance(type, args.ToArray());
    }

    public static bool TryCreateInstance<T>(this Type type, Dictionary<Type, object> injectableTypes, out T @object)
    {
        List<object> args = new();
        @object = default;
        var ctr = type.GetConstructors().FirstOrDefault(x => x.GetParameters().Length > 0);
        if (ctr != null)
        {
            foreach (var parameterInfo in ctr.GetParameters())
            {
                if (injectableTypes.TryGetValue(parameterInfo.ParameterType, out var value))
                    args.Add(value);
            }
        }
        else
            ctr = type.GetConstructor(Array.Empty<Type>())!;

        if (ctr.GetParameters().Length > args.Count)
            return false;

        @object = (T)Activator.CreateInstance(type, args.ToArray());
        return true;
    }

    public static T WaitAndReturn<T>(this Task<T> task) => task.GetAwaiter().GetResult();
}
