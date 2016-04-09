using System.Collections.Generic;

namespace Katalog
{
    public static class Filter
    {
        public static readonly Dictionary<object, bool> filters = new Dictionary<object, bool>();
    }
}
namespace Katalog
{
using static Filter;
    public class Filter<T> : ViewModel where T:class
    { 
        public bool Toggled
        {
            get
            {
                if (!filters.ContainsKey(Value))
                    filters.Add(Value, false);
                return filters[Value];
            }
            set
            {
                if (filters.ContainsKey(Value))
                    filters[Value] = value;
                else
                    filters.Add(Value, value);
            }
        }

        public T Value { get; }

        public Filter(T value)
        {
            Value = value;
        }
    }
}