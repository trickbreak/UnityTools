using System;
// using System.Collections;
using System.Collections.Generic;
// using System.Linq;

namespace MessageManagement
{
    public class SimpleDictionary<TValue>
    {
        private KeyValuePair<Type, TValue>[] dictionaryValues;

        private int GetHashValue(Type type)
        {
            return Math.Abs(type.GetHashCode()) % dictionaryValues.Length;
        }



        public SimpleDictionary()
        {
            dictionaryValues = new KeyValuePair<Type, TValue>[15];
        }

        public void Add(Type type, TValue value)
        {
            int hash = GetHashValue(type);

            if(dictionaryValues[hash].Key == null)
            {
                dictionaryValues[hash] = new KeyValuePair<Type, TValue>(type, value);
            }
        }

        public void Remove(Type type)
        {
            int hash = GetHashValue(type);

            if (dictionaryValues[hash].Key != null)
            {
                dictionaryValues[hash] = new KeyValuePair<Type, TValue>();
            }
        }

        public bool ContainsKey(Type type)
        {
            int hash = GetHashValue(type);
            
            return dictionaryValues[hash].Key != null;
        }

        public bool TryGetValue(Type type, out TValue value)
        {
            int hash = GetHashValue(type);
            
            if (dictionaryValues[hash].Key != null)
            {
                value = dictionaryValues[hash].Value;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }
    }

    // public class SimpleDic<TKey, UValue> : IEnumerable<KeyValuePair<TKey, UValue>>
    // {
    //     private LinkedList<KeyValuePair<TKey, UValue>>[]  _values;

    //     private int capacity;

    //     private int GetHashValue(TKey key)
    //     {
    //         return (Math.Abs(key.GetHashCode())) % _values.Length;
    //     }

    //     private void ResizeCollection()
    //     {
    //         throw new NotImplementedException();
    //     }



    //     public int Count => _values.Length;

    //     public SimpleDic()
    //     {
    //         _values = new LinkedList<KeyValuePair<TKey, UValue>>[15];
    //     }

    //     public UValue this[TKey key]
    //     {
    //         get
    //         {
    //             int h = GetHashValue(key);
    //             if (_values[h] == null) throw new KeyNotFoundException("Keys not found");
    //             return _values[h].FirstOrDefault(p=>p.Key.Equals(key)).Value;
    //         }
    //         set
    //         {
    //             int h = GetHashValue(key);
    //             _values[h] = new LinkedList<KeyValuePair<TKey, UValue>>();
    //             _values[h].AddLast(new KeyValuePair<TKey, UValue>
    //                                                 (key,value));
    //         }
    //     }

    //     public void Add(TKey key,UValue val)
    //     {
    //         var hash = GetHashValue(key);
            
    //         if(_values[hash]==null)
    //         {
    //             _values[hash] = new LinkedList<KeyValuePair<TKey, UValue>>();
    //         }
            
    //         var keyPresent = _values[hash].Any(p => p.Key.Equals(key));
            
    //         if(keyPresent)
    //         {
    //             throw new Exception("Duplicate key has been found");
    //         }

    //         var newValue = new KeyValuePair<TKey, UValue>(key, val);
    //         _values[hash].AddLast(newValue);
    //         capacity++;
    //         if(Count<= capacity)
    //         {
    //             ResizeCollection();
    //         }
    //     }

    //     public bool ContainsKey(TKey key)
    //     {
    //         var hash = GetHashValue(key);
    //         return _values[hash] == null ? false : _values[hash].Any(p => p.Key.Equals(key));
    //     }

    //     public UValue GetValue(TKey key)
    //     {
    //         var hash = GetHashValue(key);
    //         return _values[hash] == null ? default(UValue) :
    //             _values[hash].First(m => m.Key.Equals(key)).Value;
    //     }

    //     public bool TryGetValue(TKey key, out UValue value)
    //     {
    //         if (ContainsKey(key))
    //         {
    //             value = GetValue(key);
    //             return true;
    //         }
    //         else
    //         {
    //             value = default;
    //             return false;
    //         }
    //     }

    //     public IEnumerator<KeyValuePair<TKey, UValue>> GetEnumerator()
    //     {
    //         return (from collections in _values
    //                 where collections != null
    //                 from item in collections
    //                 select item).GetEnumerator();
    //     }

    //     IEnumerator IEnumerable.GetEnumerator()
    //     {
    //         return this.GetEnumerator();
    //     }
    // }
}
