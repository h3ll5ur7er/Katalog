using System;
using System.Collections.Generic;

namespace Katalog
{
    public static class LinqExtentions
    {
        public static void Foreach<T>(this IEnumerable<T> c, Action<T> a)
        {
            foreach (var i in c)
                a(i);
        }
    }
}