using System;
using System.Collections.Generic;

namespace Dynamic.Serialization
{
    public class DynamicListValueSerializable
    {
        public DynamicListValueSerializable(DynamicListValue value)
        {
            //If null or empty initialize an empty class
            if (!value?.HasValue ?? true)
            {
                BaseType = typeof(object);
                Items = new List<object>();
                return;
            }

            BaseType = value[0].GetType();
            Items = value.ToList();
        }

        public Type BaseType { get; set; }
        public List<object> Items { get; set; }

        public DynamicListValue Get()
        {
            return new DynamicListValue(Items);
        }
    }
}
