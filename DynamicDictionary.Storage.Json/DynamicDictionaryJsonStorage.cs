using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Text;

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
        public Encoding EncondingFormat { get; set; } = Encoding.UTF8;

        public DynamicDictionary Load()
        {
            var jsonString = File.ReadAllText(FilePath, EncondingFormat);

            DynamicDictionary dictionary = Deserialize(jsonString);

            return dictionary;
        }
        public async Task<DynamicDictionary> LoadAsync()
        {
            string jsonString;

            using(Stream reader = File.OpenRead(FilePath))
            {
                byte[] bytes = new byte[reader.Length];
                await reader.ReadAsync(bytes, 0, bytes.Length);
                jsonString = EncondingFormat.GetString(bytes);
            }

            return Deserialize(jsonString);
        }

        public bool Save(DynamicDictionary dictionary, DynamicDictionarySaveMotive motive)
        {
            var jsonString = Serialize(dictionary, JsonFormat);
            File.WriteAllText(FilePath, jsonString, EncondingFormat);
            return true;
        }
        public async Task<bool> SaveAsync(DynamicDictionary dictionary, DynamicDictionarySaveMotive motive)
        {
            var jsonString = Serialize(dictionary, JsonFormat);
            using(FileStream writer = File.OpenWrite(FilePath))
            {
                byte[] bytes = EncondingFormat.GetBytes(jsonString);
                await writer.WriteAsync(bytes, 0, bytes.Length);
            }
            return true;
        }

        public static string Serialize(DynamicDictionary dictionary, Formatting jsonFormat)
        {
            var serializable = new Dictionary<string, DynamicListValueSerializable>();
            foreach (var i in dictionary.ToDictionary())
            {
                serializable.Add(i.Key, new DynamicListValueSerializable(i.Value));
            }

            return JsonConvert.SerializeObject(serializable, jsonFormat);
        }
        public static DynamicDictionary Deserialize(string jsonString)
        {
            DynamicDictionary dictionary = new DynamicDictionary();
            var serializable = JsonConvert.DeserializeObject<Dictionary<string, DynamicListValueSerializable>>(jsonString);
            foreach (var v in serializable)
            {
                dictionary[v.Key] = v.Value.Items;
            }

            return dictionary;
        }
    }
}
