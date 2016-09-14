namespace Dynamic
{
    public enum DynamicDictionaryChangedType
    {
        /// <summary>
        /// every item removed
        /// </summary>
        Clear = 0,
        /// <summary>
        /// On added a value to a key
        /// </summary>
        AddedValue = 1,
        /// <summary>
        /// On removed a value from a key
        /// </summary>
        RemovedValue = 2,
        /// <summary>
        /// On changed a value from a key
        /// </summary>
        ChangedValue = 3,
        /// <summary>
        /// On values reorder
        /// </summary>
        OrderValue = 4
    }

    public enum SaveMotive
    {
        /// <summary>
        /// every item removed
        /// </summary>
        Clear = 0,
        /// <summary>
        /// On added a value to a key
        /// </summary>
        AddedValue = 1,
        /// <summary>
        /// On removed a value from a key
        /// </summary>
        RemovedValue = 2,
        /// <summary>
        /// On changed a value from a key
        /// </summary>
        ChangedValue = 3,
        /// <summary>
        /// On values reorder
        /// </summary>
        OrderValue = 4,
        /// <summary>
        /// The user input
        /// </summary>
        UserInput = 5
    }
}