using SQLite;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;

namespace Dynamic.Storage.SQLite
{
    public class DynamicDictionaryStorageModel
    {
        [PrimaryKey, MaxLength(int.MaxValue)]
        public string Key { get; set; }

        [MaxLength(int.MaxValue)]
        public string Value { get; set; }

        public static DynamicDictionaryStorageModel Generate(string key, DynamicListValue value)
        {
            var storageObject = new DynamicDictionaryStorageModel();
            storageObject.Key = key;
            storageObject.Value = JsonConvert.SerializeObject(value.ToList());
            return storageObject;
        }
        public static DynamicListValue GetDynamicListValue(string value)
        {
            List<object> obj = JsonConvert.DeserializeObject<List<object>>(value);
            return new DynamicListValue(obj);
        }
    }
}
