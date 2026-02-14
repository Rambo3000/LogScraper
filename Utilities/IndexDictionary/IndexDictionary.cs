using System;
using System.Collections.Generic;

namespace LogScraper.Utilities.IndexDictionary
{
    /// <summary>
    /// Interface for objects that have an integer index. This is used to ensure that keys in the IndexDictionary can provide a unique index for fast access.
    /// </summary>
    public static class IndexDictionary
    {
        /// <summary>
        /// Assigns sequential indexes to a list of objects that implement the IHasIndex interface. This is useful for preparing a list of keys before using them in an IndexDictionary, ensuring that each key has a unique index corresponding to its position in the list.
        /// </summary>
        /// <typeparam name="T">Type implementing IHasIndex.</typeparam>
        public static void AssignIndexes<T>(this IList<T> list) where T : IHasIndex
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Index = i;
            }
        }
    }

    /// <summary>
    /// A high-performance, array-backed dictionary-like structure for keys that can be uniquely mapped to an integer index.
    /// </summary>
    /// <typeparam name="TKey">The key type, which must implement <see cref="IHasIndex"/> to provide a unique integer index.</typeparam>
    /// <typeparam name="TValue">The value type to store.</typeparam>
    /// <remarks>
    /// <para>
    /// This structure is optimized for fast <c>O(1)</c> access, low memory overhead, and minimal GC pressure. 
    /// It is ideal for scenarios where the key space is known, bounded, and each key has a stable integer index (e.g., enums, ID-based access).
    /// </para>
    /// <para>
    /// Internally, it uses a fixed-size array for values and a parallel <c>bool[]</c> to track which slots are in use. 
    /// This makes lookups and inserts significantly faster than standard dictionaries in tight loops or high-volume data processing.
    /// </para>
    /// <para>
    /// Keys must be stable and unique per index. If two different keys return the same index, data corruption may occur.
    /// </para>
    /// </remarks>
    public class IndexDictionary<TKey, TValue>(int capacity) where TKey : IHasIndex
    {
        private readonly TValue[] _values = new TValue[capacity];
        private readonly bool[] _used = new bool[capacity];

        public void Set(TKey key, TValue value)
        {
            int index = key.Index;
            _values[index] = value;
            if (value != null) _used[index] = true;
        }

        /// <summary>
        /// Tries to get the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key to access the value.</param>
        /// <param name="value">The value associated with the specified key, if found.</param>
        /// <returns>True if the key exists in the map; otherwise, false.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            int index = key.Index;
            if ((uint)index >= _values.Length || !_used[index])
            {
                value = default!;
                return false;
            }

            value = _values[index];
            return true;
        }
        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key to access the value.</param>
        /// <returns>The value associated with the specified key.</returns>
        public TValue this[TKey key]
        {
            get => _values[key.Index];
            set
            {
                int index = key.Index;
                _values[index] = value;
                _used[index] = true;
            }
        }
        /// <summary>
        /// Gets or sets the value at the specified index.
        /// </summary>
        /// <param name="index">The index to access the value.</param>
        /// <returns>The value at the specified index.</returns>
        public TValue this[int index]
        {
            get => _values[index];
            set
            {
                _values[index] = value;
                _used[index] = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">The key of the value to remove.</param>
        public void Remove(TKey key)
        {
            int index = key.Index;
            _values[index] = default!;
            _used[index] = false;
        }
        /// <summary>
        /// Removes the value at the specified index from the dictionary.
        /// </summary>
        /// <param name="index">The index of the value to remove.</param>
        public void Remove(int index)
        {
            _values[index] = default!;
            _used[index] = false;
        }
        /// <summary>
        /// Checks if the dictionary contains a key.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the key exists in the dictionary; otherwise, false.</returns>
        public bool ContainsKey(TKey key)
        {
            int index = key.Index;
            return (uint)index < _used.Length && _used[index];
        }

        /// <summary>
        /// Clears the dictionary, setting all entries to their default values and marking them as unused.
        /// </summary>
        public void Clear()
        {
            Array.Clear(_values, 0, _values.Length);
            Array.Clear(_used, 0, _used.Length);
        }

        /// <summary>
        /// Returns the keys of the dictionary using the provided key factory function.
        /// </summary>
        /// <param name="keyFactory">A function that takes an index and returns the corresponding key.</param>
        /// <returns>An enumerable collection of keys.</returns>
        public IEnumerable<TKey> Keys(Func<int, TKey> keyFactory)
        {
            for (int i = 0; i < _used.Length; i++)
            {
                if (_used[i])
                    yield return keyFactory(i);
            }
        }

        /// <summary>
        /// Returns the values of the dictionary.
        /// </summary>
        public IEnumerable<TValue> Values
        {
            get
            {
                for (int i = 0; i < _values.Length; i++)
                {
                    if (_used[i])
                        yield return _values[i];
                }
            }
        }

        /// <summary>
        /// Returns the entries of the dictionary as tuples of (key, value) using the provided key factory function.
        /// </summary>
        /// <param name="keyFactory">A function that takes an index and returns the corresponding key.</param>
        /// <returns>An enumerable collection of tuples containing the key and value.</returns>
        public IEnumerable<(TKey Key, TValue Value)> Entries(Func<int, TKey> keyFactory)
        {
            for (int i = 0; i < _values.Length; i++)
            {
                if (_used[i])
                    yield return (keyFactory(i), _values[i]);
            }
        }

        /// <summary>
        /// Gets the number of used entries in the dictionary.
        /// </summary>
        public int Count
        {
            get
            {
                int count = 0;
                for (int i = 0; i < _used.Length; i++)
                    if (_used[i]) count++;
                return count;
            }
        }
    }

}
