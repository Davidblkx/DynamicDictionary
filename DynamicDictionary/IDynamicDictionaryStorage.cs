using Dynamic;
using System.Threading.Tasks;

namespace Dynamic.Storage
{
    public interface IDynamicDictionaryStorage
    {
        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        string TableName { get; set; }

        /// <summary>
        /// Saves the specified dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns>false, if an error occurs</returns>
        bool Save(DynamicDictionary dictionary, SaveMotive motive);

        /// <summary>
        /// Saves the specified dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns>false, if an error occurs</returns>
        Task<bool> SaveAsync(DynamicDictionary dictionary, SaveMotive motive);

        /// <summary>
        /// Loads a dictionary.
        /// </summary>
        /// <returns></returns>
        DynamicDictionary Load();

        /// <summary>
        /// Loads the dictionary.
        /// </summary>
        /// <returns></returns>
        Task<DynamicDictionary> LoadAsync();
    }
}
