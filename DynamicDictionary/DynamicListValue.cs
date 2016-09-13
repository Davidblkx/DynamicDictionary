using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Binder = Microsoft.CSharp.RuntimeBinder.Binder;

namespace Dynamic
{
    public class DynamicListValue : DynamicObject, IEquatable<DynamicListValue>, IEnumerable<object>, IList<object>
    {
        private readonly List<object> _list = new List<object>();

        public DynamicListValue()
        {
        }

        public DynamicListValue(object value)
        {
            if (value is IEnumerable && !(value is string))
                foreach(var obj in (IEnumerable)value)
                {
                    _list.Add(obj);
                }
            else
                _list.Add(value);
        }

        /// <summary>
        /// Gets a value indicating whether this instance has value.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has value; otherwise, <c>false</c>.
        /// </value>
        public bool HasValue
        {
            get
            {
                return _list.Count > 0;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="System.Object"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                var val = _list[index];
                _list[index] = value;
                if(val != Value)
                RaiseEvent(DynamicDictionaryChangedType.ChangedValue, value, val);
            }
        }

        /// <summary>
        /// Gets or inserts the main value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value
        {
            get
            {
                return _list.FirstOrDefault();
            }
            set
            {
                var changeType = DynamicDictionaryChangedType.AddedValue;

                _list.Insert(0, value);
                RaiseEvent(changeType, value);
            }
        }

        /// <summary>
        /// Sets the main value.
        /// </summary>
        /// <param name="index">The index.</param>
        public bool SetMainValue(int index)
        {
            if(index < _list.Count)
            {
                var val = _list[index];
                _list.RemoveAt(index);
                _list.Insert(0, val);
                RaiseEvent(DynamicDictionaryChangedType.OrderValue, val);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets the main value.
        /// </summary>
        /// <param name="index">The index.</param>
        public bool SetMainValue(object value)
        {
            if (_list.Contains(value))
            {
                _list.Remove(value);
                _list.Insert(0, value);
                RaiseEvent(DynamicDictionaryChangedType.OrderValue, value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds the specified new item.
        /// </summary>
        /// <param name="newItem">The new item.</param>
        public void Add(object newItem)
        {
            _list.Add(newItem);

            RaiseEvent(DynamicDictionaryChangedType.AddedValue, newItem);
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="values">The values.</param>
        public void AddRange(IEnumerable<object> values)
        {
            _list.AddRange(values);

            foreach(var itm in values)
            {
                RaiseEvent(DynamicDictionaryChangedType.AddedValue, itm);
            }
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="newItem">The new item.</param>
        public void Insert(int index, object newItem)
        {
            _list.Insert(index, newItem);

            RaiseEvent(DynamicDictionaryChangedType.AddedValue, newItem);
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count
        {
            get
            {
                return _list.Count;
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
        /// Casts de main Value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidCastException">Cannot converto value {typeName} to {castName}")</exception>
        public T Cast<T>(bool throwException = true, T defaultValue = default(T))
        {
            if (HasValue)
            {
                try
                {
                    return (T)Value;
                }
                catch
                {
                    if (!throwException) return defaultValue;

                    var typeName = Value.GetType().Name;
                    var castName = typeof(T).Name;

                    throw new InvalidCastException($"Cannot converto value {typeName} to {castName}");
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Casts the value at specified index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index">The index.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidCastException">Cannot converto value {typeName} to {castName}")</exception>
        /// <exception cref="System.IndexOutOfRangeException"></exception>
        public T Cast<T>(int index, bool throwException = true, T defaultValue = default(T))
        {
            if (index < Count || !throwException)
            {
                try
                {
                    return (T)_list[index];
                }
                catch
                {
                    if (!throwException) return defaultValue;

                    var typeName = _list[index].GetType().Name;
                    var castName = typeof(T).Name;

                    throw new InvalidCastException($"Cannot converto value {typeName} to {castName}");
                }
            }

            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Casts the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public List<T> CastList<T>(bool throwException = true, T defaultValue = default(T))
        {
            List<T> list = new List<T>();
            if (!HasValue) return list;

            foreach (var obj in _list)
            {
                try
                {
                    list.Add((T)obj);
                }
                catch
                {
                    if (throwException)
                    {
                        var typeName = Value.GetType().Name;
                        var castName = typeof(T).Name;

                        throw new InvalidCastException($"Cannot converto value {typeName} to {castName}");
                    }

                    list.Add(defaultValue);
                }
            }

            return list;
        }

        /// <summary>
        /// Tries the cast.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public bool TryCast<T>(out T value, T defaultValue = default(T))
        {
            value = defaultValue;
            if (!HasValue) return true;

            try
            {
                value = (T)Value;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Tries the cast.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public bool TryCast<T>(int index, out T value, T defaultValue = default(T))
        {
            value = defaultValue;
            if (index >= Count) return false;

            try
            {
                value = (T)_list[index];
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Tries the cast list. return only successfully casts, return false if any error occur
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public bool TryCastList<T>(out List<T> list, T defaultValue = default(T))
        {
            list = new List<T>();
            if (!HasValue) return true;

            var success = true;
            foreach (var obj in _list)
            {
                try
                {
                    list.Add((T)obj);
                }
                catch
                {
                    success = false;
                }
            }

            return success;
        }

        /// <summary>
        /// To the list.
        /// </summary>
        /// <returns></returns>
        public List<object> ToList()
        {
            return _list;
        }

        public override string ToString()
        {
            return HasValue ? Value.ToString() : string.Empty;
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1" />.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        /// <returns>
        /// The index of <paramref name="item" /> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(object item)
        {
            return _list.IndexOf(item);
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1" /> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            if (index < _list.Count) {
                var value = _list[index];
                _list.RemoveAt(index);
                RaiseEvent(DynamicDictionaryChangedType.RemovedValue, value);
            }
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            _list.Clear();
            RaiseEvent(DynamicDictionaryChangedType.Clear, null);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.
        /// </returns>
        public bool Contains(object item)
        {
            return _list.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public void CopyTo(object[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public bool Remove(object item)
        {
            var hasRemoved = _list.Remove(item);
            if (hasRemoved)
                RaiseEvent(DynamicDictionaryChangedType.RemovedValue, item);
            return hasRemoved;
        }

        /// <summary>
        /// Removes the range.
        /// </summary>
        /// <param name="items">The items.</param>
        public void RemoveRange(IEnumerable<object> items)
        {
            foreach (var i in items)
                Remove(i);
        }

        #region IEnumerable

        public IEnumerator<object> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion IEnumerable

        #region IEquatable

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Equals(DynamicListValue other)
        {
            if (other.GetHashCode() == this.GetHashCode()) return true;

            if (other.Count != this.Count) return false;

            for (int i = 0; i < Count; i++)
                if (other[i] != _list[i])
                    return false;

            return true;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="compareValue">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object compareValue)
        {
            if (ReferenceEquals(null, compareValue))
            {
                return false;
            }

            if (ReferenceEquals(this, compareValue))
            {
                return true;
            }

            return compareValue.GetType() == typeof(DynamicListValue) && this.Equals((DynamicListValue)compareValue);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            return _list.GetHashCode();
        }

        #endregion IEquatable

        #region DynamicObject override

        /// <summary>
        /// Provides implementation for binary operations. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations such as addition and multiplication.
        /// </summary>
        /// <returns><c>true</c> if the operation is successful; otherwise, <c>false</c>. If this method returns <c>false</c>, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)</returns>
        /// <param name="binder">Provides information about the binary operation. The binder.Operation property returns an <see cref="T:System.Linq.Expressions.ExpressionType"/> object. For example, for the sum = first + second statement, where first and second are derived from the DynamicObject class, binder.Operation returns ExpressionType.Add.</param><param name="arg">The right operand for the binary operation. For example, for the sum = first + second statement, where first and second are derived from the DynamicObject class, <paramref name="arg"/> is equal to second.</param><param name="result">The result of the binary operation.</param>
        public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
        {
            object resultOfCast;
            result = null;

            if (binder.Operation != ExpressionType.Equal)
            {
                return false;
            }

            var convert =
                Binder.Convert(CSharpBinderFlags.None, arg.GetType(), typeof(DynamicListValue));

            if (!this.TryConvert((ConvertBinder)convert, out resultOfCast))
            {
                return false;
            }

            result = (resultOfCast == null) ?
                Equals(arg, resultOfCast) :
                resultOfCast.Equals(arg);

            return true;
        }

        /// <summary>
        /// Provides implementation for type conversion operations. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations that convert an object from one type to another.
        /// </summary>
        /// <returns><c>true</c> if the operation is successful; otherwise, <c>false</c>. If this method returns <c>false</c>, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)</returns>
        /// <param name="binder">Provides information about the conversion operation. The binder.Type property provides the type to which the object must be converted. For example, for the statement (String)sampleObject in C# (CType(sampleObject, Type) in Visual Basic), where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, binder.Type returns the <see cref="T:System.String"/> type. The binder.Explicit property provides information about the kind of conversion that occurs. It returns true for explicit conversion and false for implicit conversion.</param><param name="result">The result of the type conversion operation.</param>
        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            result = null;

            if (!HasValue)
            {
                return true;
            }

            var binderType = binder.Type;
            if (binderType == typeof(string))
            {
                result = Convert.ToString(Value);
                return true;
            }

            if (binderType == typeof(Guid) || binderType == typeof(Guid?))
            {
                Guid guid;
                if (Guid.TryParse(Convert.ToString(Value), out guid))
                {
                    result = guid;
                    return true;
                }
            }
            else if (binderType == typeof(TimeSpan) || binderType == typeof(TimeSpan?))
            {
                TimeSpan timespan;
                if (TimeSpan.TryParse(Convert.ToString(Value), out timespan))
                {
                    result = timespan;
                    return true;
                }
            }
            else if (binderType.GetElementType().GetTypeInfo().IsEnum)
            {
                // handles enum to enum assignments
                if (Value.GetType().GetTypeInfo().IsEnum)
                {
                    if (binderType == Value.GetType())
                    {
                        result = Value;
                        return true;
                    }

                    return false;
                }

                // handles number to enum assignments
                if (Enum.GetUnderlyingType(binderType) == Value.GetType())
                {
                    result = Enum.ToObject(binderType, Value);
                    return true;
                }

                return false;
            }
            else
            {
                if (binderType.GetTypeInfo().IsGenericType && binderType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    binderType = binderType.GetTypeInfo().GenericTypeArguments[0];
                }

                result = Convert.ChangeType(Value, binderType);

                return true;
            }
            return base.TryConvert(binder, out result);
        }

        #endregion DynamicObject override

        #region Implicit Outbound Conversion

        public static implicit operator bool? (DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(bool?);
            }

            return (bool)dynamicValue;
        }
        public static implicit operator List<bool?>(DynamicListValue dynamicValue)
        {
            return new List<bool?>(dynamicValue.Select(x => (bool?)x));
        }

        public static implicit operator bool(DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return false;
            }

            if (dynamicValue.Value.GetType().GetTypeInfo().IsValueType)
            {
                return (Convert.ToBoolean(dynamicValue.Value));
            }

            bool result;
            if (bool.TryParse(dynamicValue.ToString(), out result))
            {
                return result;
            }

            return true;
        }
        public static implicit operator List<bool>(DynamicListValue dynamicValue)
        {
            return new List<bool>(dynamicValue.Select(x => (bool)x));
        }

        public static implicit operator string(DynamicListValue dynamicValue)
        {
            return dynamicValue.HasValue ? Convert.ToString(dynamicValue.Value) : default(string);
        }
        public static implicit operator List<string>(DynamicListValue dynamicValue)
        {
            return new List<string>(dynamicValue.Select(x => (string)x));
        }

        public static implicit operator int? (DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(int?);
            }

            return (int?)dynamicValue.Value;
        }
        public static implicit operator List<int?>(DynamicListValue dynamicValue)
        {
            return new List<int?>(dynamicValue.Select(x => (int?)x));
        }

        public static implicit operator int(DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(int);
            }

            if (dynamicValue.Value.GetType().GetTypeInfo().IsValueType)
            {
                return Convert.ToInt32(dynamicValue.Value);
            }

            return int.Parse(dynamicValue.ToString());
        }
        public static implicit operator List<int>(DynamicListValue dynamicValue)
        {
            return new List<int>(dynamicValue.Select(x => (int)x));
        }

        public static implicit operator uint? (DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(uint?);
            }

            return (uint?)dynamicValue.Value;
        }
        public static implicit operator List<uint?>(DynamicListValue dynamicValue)
        {
            return new List<uint?>(dynamicValue.Select(x => (uint?)x));
        }

        public static implicit operator uint(DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(uint);
            }

            if (dynamicValue.Value.GetType().GetTypeInfo().IsValueType)
            {
                return Convert.ToUInt32(dynamicValue.Value);
            }

            return uint.Parse(dynamicValue.ToString());
        }
        public static implicit operator List<uint>(DynamicListValue dynamicValue)
        {
            return new List<uint>(dynamicValue.Select(x => (uint)x));
        }

        public static implicit operator short? (DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(short?);
            }

            return (short?)dynamicValue.Value;
        }
        public static implicit operator List<short?>(DynamicListValue dynamicValue)
        {
            return new List<short?>(dynamicValue.Select(x => (short?)x));
        }

        public static implicit operator short(DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(int);
            }

            if (dynamicValue.Value.GetType().GetTypeInfo().IsValueType)
            {
                return Convert.ToInt16(dynamicValue.Value);
            }

            return short.Parse(dynamicValue.ToString());
        }
        public static implicit operator List<short>(DynamicListValue dynamicValue)
        {
            return new List<short>(dynamicValue.Select(x => (short)x));
        }

        public static implicit operator ushort? (DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(ushort?);
            }

            return (ushort?)dynamicValue.Value;
        }
        public static implicit operator List<ushort?>(DynamicListValue dynamicValue)
        {
            return new List<ushort?>(dynamicValue.Select(x => (ushort?)x));
        }

        public static implicit operator ushort(DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(ushort);
            }

            if (dynamicValue.Value.GetType().GetTypeInfo().IsValueType)
            {
                return Convert.ToUInt16(dynamicValue.Value);
            }

            return ushort.Parse(dynamicValue.ToString());
        }
        public static implicit operator List<ushort>(DynamicListValue dynamicValue)
        {
            return new List<ushort>(dynamicValue.Select(x => (ushort)x));
        }

        public static implicit operator Guid? (DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(Guid?);
            }

            return (Guid)dynamicValue;
        }
        public static implicit operator List<Guid?>(DynamicListValue dynamicValue)
        {
            return new List<Guid?>(dynamicValue.Select(x => (Guid?)x));
        }

        public static implicit operator Guid(DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(Guid);
            }

            if (dynamicValue.Value is Guid)
            {
                return (Guid)dynamicValue.Value;
            }

            return Guid.Parse(dynamicValue.ToString());
        }
        public static implicit operator List<Guid>(DynamicListValue dynamicValue)
        {
            return new List<Guid>(dynamicValue.Select(x => (Guid)x));
        }

        public static implicit operator DateTime? (DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(DateTime?);
            }

            return (DateTime)dynamicValue.Value;
        }
        public static implicit operator List<DateTime?>(DynamicListValue dynamicValue)
        {
            return new List<DateTime?>(dynamicValue.Select(x => (DateTime?)x));
        }

        public static implicit operator DateTime(DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(DateTime);
            }

            if (dynamicValue.Value is DateTime)
            {
                return (DateTime)dynamicValue.Value;
            }

            return DateTime.Parse(dynamicValue.ToString(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
        }
        public static implicit operator List<DateTime>(DynamicListValue dynamicValue)
        {
            return new List<DateTime>(dynamicValue.Select(x => (DateTime)x));
        }

        public static implicit operator TimeSpan? (DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(TimeSpan?);
            }

            return (TimeSpan)dynamicValue.Value;
        }
        public static implicit operator List<TimeSpan?>(DynamicListValue dynamicValue)
        {
            return new List<TimeSpan?>(dynamicValue.Select(x => (TimeSpan?)x));
        }

        public static implicit operator TimeSpan(DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(TimeSpan);
            }

            if (dynamicValue.Value is TimeSpan)
            {
                return (TimeSpan)dynamicValue.Value;
            }

            return TimeSpan.Parse(dynamicValue.Value.ToString());
        }
        public static implicit operator List<TimeSpan>(DynamicListValue dynamicValue)
        {
            return new List<TimeSpan>(dynamicValue.Select(x => (TimeSpan)x));
        }

        public static implicit operator long? (DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(long?);
            }

            return (long)dynamicValue.Value;
        }
        public static implicit operator List<long?>(DynamicListValue dynamicValue)
        {
            return new List<long?>(dynamicValue.Select(x => (long?)x));
        }

        public static implicit operator long(DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(long);
            }

            if (dynamicValue.Value.GetType().GetTypeInfo().IsValueType)
            {
                return Convert.ToInt64(dynamicValue.Value);
            }

            return long.Parse(dynamicValue.ToString());
        }
        public static implicit operator List<long>(DynamicListValue dynamicValue)
        {
            return new List<long>(dynamicValue.Select(x => (long)x));
        }

        public static implicit operator ulong? (DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(ulong?);
            }

            return (ulong?)dynamicValue.Value;
        }
        public static implicit operator List<ulong?>(DynamicListValue dynamicValue)
        {
            return new List<ulong?>(dynamicValue.Select(x => (ulong?)x));
        }

        public static implicit operator ulong(DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(ulong);
            }

            if (dynamicValue.Value.GetType().GetTypeInfo().IsValueType)
            {
                return Convert.ToUInt64(dynamicValue.Value);
            }

            return ulong.Parse(dynamicValue.ToString());
        }
        public static implicit operator List<ulong>(DynamicListValue dynamicValue)
        {
            return new List<ulong>(dynamicValue.Select(x => (ulong)x));
        }

        public static implicit operator float? (DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(float?);
            }

            return (float)dynamicValue.Value;
        }
        public static implicit operator List<float?>(DynamicListValue dynamicValue)
        {
            return new List<float?>(dynamicValue.Select(x => (float?)x));
        }

        public static implicit operator float(DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(float);
            }

            if (dynamicValue.Value.GetType().GetTypeInfo().IsValueType)
            {
                return Convert.ToSingle(dynamicValue.Value);
            }

            return float.Parse(dynamicValue.ToString());
        }
        public static implicit operator List<float>(DynamicListValue dynamicValue)
        {
            return new List<float>(dynamicValue.Select(x => (float)x));
        }

        public static implicit operator decimal? (DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(decimal?);
            }

            return (decimal)dynamicValue.Value;
        }
        public static implicit operator List<decimal?>(DynamicListValue dynamicValue)
        {
            return new List<decimal?>(dynamicValue.Select(x => (decimal?)x));
        }

        public static implicit operator decimal(DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(decimal);
            }

            if (dynamicValue.Value.GetType().GetTypeInfo().IsValueType)
            {
                return Convert.ToDecimal(dynamicValue.Value);
            }

            return decimal.Parse(dynamicValue.ToString());
        }
        public static implicit operator List<decimal>(DynamicListValue dynamicValue)
        {
            return new List<decimal>(dynamicValue.Select(x => (decimal)x));
        }

        public static implicit operator double? (DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(double?);
            }

            return (double?)dynamicValue.Value;
        }
        public static implicit operator List<double?>(DynamicListValue dynamicValue)
        {
            return new List<double?>(dynamicValue.Select(x => (double?)x));
        }

        public static implicit operator double(DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(double);
            }

            if (dynamicValue.Value.GetType().GetTypeInfo().IsValueType)
            {
                return Convert.ToDouble(dynamicValue.Value);
            }

            return double.Parse(dynamicValue.ToString());
        }
        public static implicit operator List<double>(DynamicListValue dynamicValue)
        {
            return new List<double>(dynamicValue.Select(x => (double)x));
        }

        public static implicit operator char? (DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(char?);
            }

            return (char?)dynamicValue.Value;
        }
        public static implicit operator List<char?>(DynamicListValue dynamicValue)
        {
            return new List<char?>(dynamicValue.Select(x => (char?)x));
        }

        public static implicit operator char(DynamicListValue dynamicValue)
        {
            if (!dynamicValue.HasValue)
            {
                return default(char);
            }

            if (dynamicValue.Value.GetType().GetTypeInfo().IsValueType)
            {
                return Convert.ToChar(dynamicValue.Value);
            }

            char charValue = default(char);
            char.TryParse(dynamicValue, out charValue);

            return charValue;
        }
        public static implicit operator List<char>(DynamicListValue dynamicValue)
        {
            return new List<char>(dynamicValue.Select(x => (char)x));
        }

        #endregion Implicit Outbound Conversion

        #region Implicit Inbound Conversion

        public static implicit operator DynamicListValue(bool? value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(bool value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(string value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(int? value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(int value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(uint? value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(uint value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(short? value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(short value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(ushort? value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(ushort value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(Guid? value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(Guid value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(DateTime? value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(DateTime value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(TimeSpan? value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(TimeSpan value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(long? value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(long value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(ulong? value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(ulong value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(float? value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(float value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(decimal? value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(decimal value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(double? value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(double value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(char? value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(char value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<bool?> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<bool> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<string> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<int?> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<int> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<short?> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<short> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<uint?> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<uint> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<ushort?> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<ushort> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<Guid?> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<Guid> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<DateTime?> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<DateTime> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<TimeSpan?> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<TimeSpan> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<long?> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<long> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<ulong?> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<ulong> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<float?> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<float> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<decimal?> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<decimal> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<double?> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<double> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<char?> value)
        {
            return new DynamicListValue(value);
        }

        public static implicit operator DynamicListValue(List<char> value)
        {
            return new DynamicListValue(value);
        }

        #endregion Implicit Inbound Conversion

        public static bool operator ==(DynamicListValue dynamicValue, object compareValue)
        {
            if (ReferenceEquals(null, dynamicValue))
            {
                return false;
            }

            if (dynamicValue.Value == null && compareValue == null)
            {
                return true;
            }

            return dynamicValue.Value != null && dynamicValue.Value.Equals(compareValue);
        }

        public static bool operator !=(DynamicListValue dynamicValue, object compareValue)
        {
            return !(dynamicValue == compareValue);
        }

        /// <summary>
        /// Implements the operator +. Adds items from [values] into [source]
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static DynamicListValue operator +(DynamicListValue source, DynamicListValue values)
        {
            if (source == null || values == null)
                return source;

            source.AddRange(values);
            return source;
        }

        /// <summary>
        /// Implements the operator -. Removes [values] from [source]
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="values">The values.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static DynamicListValue operator -(DynamicListValue source, DynamicListValue values)
        {
            if (source == null || values == null)
                return source;

            foreach(var obj in values)
                source.Remove(obj);

            return source;
        }

        public event DynamicListValueChanged OnDynamicListValueChanged;
        private void RaiseEvent(DynamicDictionaryChangedType type, object value, object oldValue = null)
        {
            OnDynamicListValueChanged?.Invoke(this, new DynamicListValueChangedArgs
            {
                EventType = type,
                Value = value,
                OldValue = oldValue
            });
        }
    }
}