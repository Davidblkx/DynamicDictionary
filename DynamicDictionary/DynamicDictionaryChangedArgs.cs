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
}
