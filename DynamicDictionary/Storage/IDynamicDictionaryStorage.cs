using Dynamic;
using System.Threading.Tasks;

namespace Dynamic.Storage
{
    public interface IDynamicDictionaryStorage
    {

        /// <summary>
        /// Saves the specified dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns>false, if an error occurs</returns>
        bool Save(DynamicDictionary dictionary, DynamicDictionarySaveMotive motive);

        /// <summary>
        /// Saves the specified dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns>false, if an error occurs</returns>
        Task<bool> SaveAsync(DynamicDictionary dictionary, DynamicDictionarySaveMotive motive);

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
