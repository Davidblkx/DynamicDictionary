using System.Threading.Tasks;
using SQLite;
using System.Linq;

namespace Dynamic.Storage.SQLite
{
    public class DynamicDictionarySQLiteStorage : IDynamicDictionaryStorage
    {
        public DynamicDictionarySQLiteStorage(string dataBasePath)
        {
            DataBasePath = dataBasePath;
        }

        /// <summary>
        /// Gets or sets the data base path.
        /// </summary>
        /// <value>
        /// The data base path.
        /// </value>
        public string DataBasePath { get; set; }

        public DynamicDictionary Load()
        {
            var connection = new SQLiteConnection(DataBasePath);

            connection.CreateTable<DynamicDictionaryStorageModel>();
            var table = connection.Table<DynamicDictionaryStorageModel>().ToList();

            DynamicDictionary dictionary = new DynamicDictionary();

            foreach(var entry in table)
            {
                dictionary.Add(entry.Key, DynamicDictionaryStorageModel.GetDynamicListValue(entry.Value));
            }

            return dictionary;
        }
        public async Task<DynamicDictionary> LoadAsync()
        {
            var connection = new SQLiteAsyncConnection(DataBasePath);

            await connection.CreateTableAsync<DynamicDictionaryStorageModel>();
            var table = await connection.Table<DynamicDictionaryStorageModel>().ToListAsync();

            DynamicDictionary dictionary = new DynamicDictionary();

            foreach (var entry in table)
            {
                dictionary.Add(entry.Key, DynamicDictionaryStorageModel.GetDynamicListValue(entry.Value));
            }

            return dictionary;
        }

        public bool Save(DynamicDictionary dictionary, DynamicDictionarySaveMotive motive)
        {
            var connection = new SQLiteConnection(DataBasePath);
            connection.CreateTable<DynamicDictionaryStorageModel>();

            foreach(var pair in dictionary.ToDictionary())
            {
                var model = DynamicDictionaryStorageModel.Generate(pair.Key, pair.Value);

                if (connection.Table<DynamicDictionaryStorageModel>().Count(z=>z.Key == pair.Key) > 0)
                    connection.Update(model);
                else
                    connection.Insert(model);
            }

            return true;
        }
        public async Task<bool> SaveAsync(DynamicDictionary dictionary, DynamicDictionarySaveMotive motive)
        {
            var connection = new SQLiteAsyncConnection(DataBasePath);
            await connection.CreateTableAsync<DynamicDictionaryStorageModel>();

            foreach (var pair in dictionary.ToDictionary())
            {
                var model = DynamicDictionaryStorageModel.Generate(pair.Key, pair.Value);

                if (await connection.Table<DynamicDictionaryStorageModel>().Where(z=>z.Key == pair.Key).CountAsync() > 0)
                    await connection.UpdateAsync(model);
                else
                    await connection.InsertAsync(model);
            }

            return true;
        }
    }
}
