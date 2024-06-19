using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable Unity.RedundantHideInInspectorAttribute
// ReSharper disable Unity.RedundantSerializeFieldAttribute

namespace Utilities
{
	[Serializable]
	public class GenericDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>,
		ISerializationCallbackReceiver
	{
		[SerializeField] private List<KeyValuePair> list = new List<KeyValuePair>();

		[SerializeField, HideInInspector] private readonly Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
		[SerializeField] private readonly Dictionary<TKey, int> indexByKey = new Dictionary<TKey, int>();

		public GenericDictionary()
		{
		}

		public GenericDictionary(IReadOnlyDictionary<TKey, TValue> other)
		{
			foreach (var pair in other)
			{
				Add(pair);
			}
		}

		// IDictionary
		public TValue this[TKey key]
		{
			get => dict[key];
			set
			{
				dict[key] = value;
				if (indexByKey.ContainsKey(key))
				{
					var index = indexByKey[key];
					list[index] = new KeyValuePair(key, value);
				}
				else
				{
					list.Add(new KeyValuePair(key, value));
					indexByKey.Add(key, list.Count - 1);
				}
			}
		}

		public ICollection<TKey> Keys => dict.Keys;
		public ICollection<TValue> Values => dict.Values;

		public void Add(TKey key, TValue value)
		{
			dict.Add(key, value);
			list.Add(new KeyValuePair(key, value));
			indexByKey.Add(key, list.Count - 1);
		}

		public bool ContainsKey(TKey key)
		{
			return dict.ContainsKey(key);
		}

		public bool Remove(TKey key)
		{
			if (dict.Remove(key))
			{
				var index = indexByKey[key];
				list.RemoveAt(index);
				indexByKey.Remove(key);
				return true;
			}

			return false;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return dict.TryGetValue(key, out value);
		}

		// ICollection
		public int Count => dict.Count;
		public bool IsReadOnly { get; set; }

		public void Add(KeyValuePair<TKey, TValue> pair)
		{
			Add(pair.Key, pair.Value);
		}

		public void Clear()
		{
			dict.Clear();
			list.Clear();
			indexByKey.Clear();
		}

		public bool Contains(KeyValuePair<TKey, TValue> pair)
		{
			return dict.TryGetValue(pair.Key, out var value) &&
			       EqualityComparer<TValue>.Default.Equals(value, pair.Value);
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (array == null)
				throw new ArgumentException("The array cannot be null.");
			if (arrayIndex < 0)
				throw new ArgumentOutOfRangeException();
			if (array.Length - arrayIndex < dict.Count)
				throw new ArgumentException("The destination array has fewer elements than the collection.");
			foreach (var pair in dict)
			{
				array[arrayIndex] = pair;
				arrayIndex++;
			}
		}

		public bool Remove(KeyValuePair<TKey, TValue> pair)
		{
			if (dict.TryGetValue(pair.Key, out var value))
			{
				var valueMatch = EqualityComparer<TValue>.Default.Equals(value, pair.Value);
				if (valueMatch) return Remove(pair.Key);
			}

			return false;
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return dict.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => dict.Keys;
		IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => dict.Values;

		// Since lists can be serialized natively by Unity no custom implementation is needed
		public void OnBeforeSerialize()
		{
		}

		// Fill dictionary with list pairs and flag key-collisions.
		public void OnAfterDeserialize()
		{
			dict.Clear();
			indexByKey.Clear();

			for (var i = 0; i < list.Count; i++)
			{
				var key = list[i].key;
				if (key != null && !ContainsKey(key))
				{
					dict.Add(key, list[i].value);
					indexByKey.Add(key, i);
				}
			}
		}

		[Serializable]
		private struct KeyValuePair
		{
			public TKey key;
			public TValue value;

			public KeyValuePair(TKey key, TValue value)
			{
				this.key = key;
				this.value = value;
			}
		}
	}
}