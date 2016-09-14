using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Dynamic.Storage.Json
{
    public class DynamicDictionaryJsonStorage : IDynamicDictionaryStorage
    {
        public DynamicDictionaryJsonStorage(string path)
        {
            FilePath = path;
        }

        public string FilePath { get; set; }
        public Formatting JsonFormat { get; set; } = Formatting.None;

        public DynamicDictionary Load()
        {
            DynamicDictionary dictionary = new DynamicDictionary();
            var jsonString = File.ReadAllText(FilePath);

            var serializable = JsonConvert.DeserializeObject<Dictionary<string, DynamicListValueSerializable>>(jsonString);
            foreach(var v in serializable)
            {
                dictionary[v.Key] = v.Value.Items;
            }

            return dictionary;
        }

        public async Task<DynamicDictionary> LoadAsync()
        {
            return await Task.Run<DynamicDictionary>(() =>
            {
                return Load();
            });
        }

        public bool Save(DynamicDictionary dictionary, DynamicDictionarySaveMotive motive)
        {
            var serializable = new Dictionary<string, DynamicListValueSerializable>();
            foreach(var i in dictionary.ToDictionary())
            {
                serializable.Add(i.Key, new DynamicListValueSerializable(i.Value));
            }

            var jsonString = JsonConvert.SerializeObject(serializable, JsonFormat);
            File.WriteAllText(FilePath, jsonString);
            return true;
        }

        public async Task<bool> SaveAsync(DynamicDictionary dictionary, DynamicDictionarySaveMotive motive)
        {
            await Task.Run(() =>
            {
                Save(dictionary, motive);
            });
            return true;
        }
    }
}
