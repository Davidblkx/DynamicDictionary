using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamic
{
    public class DynamicDictionaryChangedArgs
    {
        public DynamicDictionaryChangedType EventType { get; set; }
        public string Key { get; set; }
        public object OldValue { get; set; }
        public object Value { get; set; }
    }

    public enum DynamicDictionaryChangedType
    {
        /// <summary>
        /// every item removed
        /// </summary>
        Clear,
        /// <summary>
        /// On added a value to a key
        /// </summary>
        AddedValue,
        /// <summary>
        /// On removed a value from a key
        /// </summary>
        RemovedValue,
        /// <summary>
        /// On changed a value from a key
        /// </summary>
        ChangedValue,
        /// <summary>
        /// On values reorder
        /// </summary>
        OrderValue
    }
}
