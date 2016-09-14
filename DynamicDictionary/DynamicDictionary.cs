using System;
using System.Dynamic;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dynamic.Storage;
using System.Threading.Tasks;

namespace Dynamic
{
    public class DynamicDictionary : DynamicObject, IEquatable<DynamicDictionary>, IDictionary<string, object>
    {
        private readonly IDictionary<string, DynamicListValue> _dictionary =
            new Dictionary<string, DynamicListValue>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets or sets the storage object that handles persistent storage.
        /// </summary>
        /// <value>
        /// The storage.
        /// </value>
        public IDynamicDictionaryStorage Storage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [save on change].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [save on change]; otherwise, save only on user input.
        /// </value>
        public bool SaveOnChange { get; set; } = false;

        /// <summary>
        /// Gets or sets the <see cref="dynamic"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="dynamic"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public dynamic this[string key]
        {
            get
            {
                if (_dictionary.ContainsKey(key))
                {
                    dynamic obj = _dictionary[key];
                    return obj;
                }
                return new DynamicListValue();
            }

            set
            {
                if (_dictionary.ContainsKey(key))
                {
                    var val = _dictionary[key];
                    _dictionary[key] = new DynamicListValue(value);
                    _dictionary[key].OnDynamicListValueChanged += OnValueChanged;

                    if (val != value)
                        RaiseEvent(DynamicDictionaryChangedType.ChangedValue, key, value);
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public int Count
        {
            get
            {
                return _dictionary.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        public ICollection<string> Keys
        {
            get
            {
                return _dictionary.Keys;
            }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        public ICollection<object> Values
        {
            get
            {
                return _dictionary.Values.Select(x=> x as object).ToList();
            }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public void Add(KeyValuePair<string, object> item)
        {
            _dictionary.Add(item.Key, new DynamicListValue(item.Value));
            _dictionary[item.Key].OnDynamicListValueChanged += OnValueChanged;
            RaiseEvent(DynamicDictionaryChangedType.AddedValue, item.Key, _dictionary[item.Key]);
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Add(string key, object value)
        {
            _dictionary.Add(key, new DynamicListValue(value));
            _dictionary[key].OnDynamicListValueChanged += OnValueChanged;
            RaiseEvent(DynamicDictionaryChangedType.AddedValue, key, _dictionary[key]);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Clear()
        {
            _dictionary.Clear();
            RaiseEvent(DynamicDictionaryChangedType.Clear, null, null);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.
        /// </returns>
        public bool Contains(KeyValuePair<string, object> item)
        {
            return _dictionary.Contains(new KeyValuePair<string, DynamicListValue>(item.Key, new DynamicListValue(item.Value)));
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2" />.</param>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the key; otherwise, false.
        /// </returns>
        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            var pair = array.Select(x => new KeyValuePair<string, DynamicListValue>(x.Key, new DynamicListValue(x.Value))).ToArray();
            _dictionary.CopyTo(pair, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public bool Remove(KeyValuePair<string, object> item)
        {
            DynamicListValue value = new DynamicListValue(item.Value);
            if (item.Value is DynamicListValue)
                value = item.Value as DynamicListValue;

            var pair = new KeyValuePair<string, DynamicListValue>(item.Key, value);
            if (_dictionary.Remove(pair))
            {
                RaiseEvent(DynamicDictionaryChangedType.RemovedValue, pair.Key, pair.Value);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key" /> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </returns>
        public bool Remove(string key)
        {
            if (!_dictionary.ContainsKey(key)) return false;

            var value = _dictionary[key];
            if (_dictionary.Remove(key))
            {
                RaiseEvent(DynamicDictionaryChangedType.RemovedValue, key, value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
        /// <returns>
        /// true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key; otherwise, false.
        /// </returns>
        public bool TryGetValue(string key, out object value)
        {
            DynamicListValue defaultValue;

            bool result = _dictionary.TryGetValue(key, out defaultValue);
            value = defaultValue;

            return result;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public IDictionary<string, DynamicListValue> ToDictionary()
        {
            return _dictionary;
        }

        #region IEquatable
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(DynamicDictionary other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            return ReferenceEquals(this, other) || Equals(other._dictionary, this._dictionary);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == typeof(DynamicDictionary) && this.Equals((DynamicDictionary)obj);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return (_dictionary != null ? _dictionary.GetHashCode() : 0);
        }
        #endregion

        /// <summary>
        /// Provides the implementation for operations that set member values. Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for operations such as setting a value for a property.
        /// </summary>
        /// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member to which the value is being assigned. For example, for the statement sampleObject.SampleProperty = "Test", where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="value">The value to set to the member. For example, for sampleObject.SampleProperty = "Test", where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, the <paramref name="value" /> is "Test".</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)
        /// </returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this[binder.Name] = value;
            return true;
        }

        /// <summary>
        /// Provides the implementation for operations that get member values. Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for operations such as getting a value for a property.
        /// </summary>
        /// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member on which the dynamic operation is performed. For example, for the Console.WriteLine(sampleObject.SampleProperty) statement, where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="result">The result of the get operation. For example, if the method is called for a property, you can assign the property value to <paramref name="result" />.</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a run-time exception is thrown.)
        /// </returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this[binder.Name];
            return true;
        }

        /// <summary>
        /// Returns the enumeration of all dynamic member names.
        /// </summary>
        /// <returns>
        /// A sequence that contains dynamic member names.
        /// </returns>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _dictionary.Keys;
        }

        /// <summary>
        /// Saves this instance using Storage object.
        /// </summary>
        public bool Save()
        {
            return Storage?.Save(this, SaveMotive.UserInput) ?? false;
        }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveAsync()
        {
            if (Storage == null) return false;

            return await Storage.SaveAsync(this, SaveMotive.UserInput);
        }

        private async void SaveAsync(SaveMotive motive)
        {
            if (SaveOnChange && Storage != null)
            {
                await Storage.SaveAsync(this, motive);
            }
        }

        public event DynamicDictionaryChanged OnChange;
        private void RaiseEvent(DynamicDictionaryChangedType type, string key, DynamicListValue value, DynamicListValue oldValue = null)
        {
            OnChange?.Invoke(this, new DynamicDictionaryChangedArgs
            {
                EventType = type,
                Key = key,
                OldValue = oldValue,
                Value = value
            });

            SaveAsync((SaveMotive)type);
        }

        private void OnValueChanged(object sender, DynamicListValueChangedArgs e)
        {
            SaveAsync(SaveMotive.ChangedValue);
        }
    }
}
