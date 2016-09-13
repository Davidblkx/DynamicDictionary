using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamic
{
    public delegate void DynamicDictionaryChanged(object sender, DynamicDictionaryChangedArgs e);
    public delegate void DynamicListValueChanged(object sender, DynamicListValueChangedArgs e);
}
