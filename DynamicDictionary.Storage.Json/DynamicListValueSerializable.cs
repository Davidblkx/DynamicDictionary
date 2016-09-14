using System;
using System.Collections.Generic;
using Dynamic;

namespace Dynamic.Storage.Json
{
    public class DynamicListValueSerializable
    {
        public DynamicListValueSerializable(DynamicListValue value)
        {
            if (!value?.HasValue ?? true) {
                BaseType = typeof(object);
                Items = new List<object>();
                return;
            }

            BaseType = value[0].GetType();
            Items = value.ToList();
        }

        public Type BaseType { get; set; }
        public List<object> Items { get; set; }
    }
}
