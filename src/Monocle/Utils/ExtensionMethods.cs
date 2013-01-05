using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class ExtensionMethods
{
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var item in enumerable)
        {
            action(item);
        }
    }
}