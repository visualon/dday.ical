using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;

namespace DDay.Collections
{
    /// <summary>
    /// A list of objects that are keyed.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    public class KeyedList<TKey, TObject> :
        IKeyedList<TKey, TObject>
        where TObject : class, IKeyedObject<TKey>
    {
        #region Private Fields

        List<IMultiLinkedList<TObject>> _Lists = new List<IMultiLinkedList<TObject>>();
        Dictionary<TKey, IMultiLinkedList<TObject>> _Dictionary = new Dictionary<TKey, IMultiLinkedList<TObject>>();

        #endregion

        #region Private Methods

        TObject SubscribeToKeyChanges(TObject item)
        {
            if (item != null)
                item.KeyChanged += item_KeyChanged;
            return item;
        }

        TObject UnsubscribeFromKeyChanges(TObject item)
        {
            if (item != null)
                item.KeyChanged -= item_KeyChanged;
            return item;
        }

        #endregion

        #region Protected Methods

        virtual protected TKey KeyModifier(TKey key)
        {
            return key;
        }

        #endregion

        #region Private Methods

        IMultiLinkedList<TObject> EnsureList(TKey key, bool createIfNecessary)
        {
            if (!_Dictionary.ContainsKey(key))
            {
                if (createIfNecessary)
                {
                    MultiLinkedList<TObject> list = new MultiLinkedList<TObject>();
                    _Dictionary[key] = list;

                    if (_Lists.Count > 0)
                    {
                        // Attach the list to our list chain
                        var previous = _Lists[_Lists.Count - 1];
                        previous.SetNext(list);
                        list.SetPrevious(previous);
                    }

                    _Lists.Add(list);
                    return list;
                }
            }
            else
            {
                return _Dictionary[key];
            }
            return null;
        }

        #endregion

        #region Event Handlers

        void item_KeyChanged(object sender, ObjectEventArgs<TKey, TKey> e)
        {
            TKey oldValue = e.First;
            TKey newValue = e.Second;
            TObject obj = sender as TObject;

            if (obj != null)
            {
                // Remove the object from the hash table
                // based on the old key.
                if (!object.Equals(oldValue, default(TKey)))
                {
                    // Find the specific item and remove it
                    TKey key = KeyModifier(oldValue);
                    if (_Dictionary.ContainsKey(key))
                    {
                        IMultiLinkedList<TObject> items = _Dictionary[key];

                        // Find the item's index within the list
                        int index = items.IndexOf(obj);
                        if (index >= 0)
                        {
                            // Get a reference to the object
                            TObject item = items[index];

                            // Remove the object
                            items.RemoveAt(index);

                            // Notify that this item was removed, with the overall
                            // index of the item in the keyed list.
                            OnItemRemoved(UnsubscribeFromKeyChanges(item), items.StartIndex + index);
                        }
                    }
                }

                // If a new key exists, then re-add this item into the hash
                if (!object.Equals(newValue, default(TKey)))
                    Add(obj);
            }
        }

        #endregion

        #region IKeyedList<TKey, TObject> Members

        [field: NonSerialized]
        public event EventHandler<ObjectEventArgs<TObject, int>> ItemAdded;

        [field: NonSerialized]
        public event EventHandler<ObjectEventArgs<TObject, int>> ItemRemoved;

        protected void OnItemAdded(TObject obj, int index)
        {
            if (ItemAdded != null)
                ItemAdded(this, new ObjectEventArgs<TObject, int>(obj, index));
        }

        protected void OnItemRemoved(TObject obj, int index)
        {
            if (ItemRemoved != null)
                ItemRemoved(this, new ObjectEventArgs<TObject, int>(obj, index));
        }

        virtual public void Add(TObject item)
        {
            if (item != null)
            {
                // Get the "real" key for this item
                TKey key = KeyModifier(item.Key);

                // Add a new list if necessary
                var list = EnsureList(key, true);
                int index = list.Count;
                list.Add(SubscribeToKeyChanges(item));
                OnItemAdded(item, list.StartIndex + index);
            }
        }

        virtual public int IndexOf(TObject item)
        {
            // Get the "real" key
            TKey key = KeyModifier(item.Key);
            if (_Dictionary.ContainsKey(key))
            {
                // Get the list associated with this object's key
                var list = _Dictionary[key];

                // Find the object within the list.
                int index = list.IndexOf(item);

                // Return the index within the overall KeyedList
                if (index >= 0)
                    return list.StartIndex + index;
            }
            return -1;
        }

        virtual public void Clear(TKey key)
        {
            key = KeyModifier(key);

            if (_Dictionary.ContainsKey(key))
            {
                // Get the list associated with the key
                var list = _Dictionary[key].ToArray();
                
                // Save the number of items in the list
                int count = list.Length;

                // Save the starting index of the list
                int startIndex = _Dictionary[key].StartIndex;

                // Clear the list (note that this also clears the list
                // in the _Lists object).
                _Dictionary[key].Clear();

                // Notify that each of these items were removed
                for (int i = list.Length - 1; i >= 0; i--)
                    OnItemRemoved(UnsubscribeFromKeyChanges(list[i]), startIndex + i);
            }
        }

        virtual public void Clear()
        {
            // Get a list of items that are being cleared
            var items = _Lists.SelectMany(i => i).ToArray();

            // Clear our lists out
            _Dictionary.Clear();
            _Lists.Clear();

            // Notify that each item was removed
            for (int i = items.Length - 1; i >= 0; i--)
                OnItemRemoved(UnsubscribeFromKeyChanges(items[i]), i);
        }

        virtual public bool ContainsKey(TKey key)
        {
            key = KeyModifier(key);
            return _Dictionary.ContainsKey(key);
        }

        virtual public int Count
        {
            get
            {
                int count = 0;
                foreach (var list in _Lists)
                    count += list.Count;
                return count;
            }
        }

        virtual public int CountOf(TKey key)
        {
            key = KeyModifier(key);
            if (_Dictionary.ContainsKey(key))
                return _Dictionary[key].Count;
            return 0;
        }

        virtual public IEnumerable<TObject> Values()
        {
            return _Dictionary.Values.SelectMany(i => i);
        }

        virtual public IEnumerable<TObject> AllOf(TKey key)
        {
            key = KeyModifier(key);
            if (_Dictionary.ContainsKey(key))
                return _Dictionary[key];
            return null;
        }

        virtual public TObject this[int index]
        {
            get
            {
                foreach (var list in _Lists)
                {
                    var startIndex = list.StartIndex;
                    if (list.StartIndex <= index &&
                        list.ExclusiveEnd > index)
                    {
                        return list[index - list.StartIndex];
                    }
                }
                return default(TObject);
            }
        }
        
        virtual public bool Remove(TObject obj)
        {
            TKey key = KeyModifier(obj.Key);
            if (_Dictionary.ContainsKey(key))
            {
                var items = _Dictionary[key];
                int index = items.IndexOf(obj);

                if (index >= 0)
                {
                    TObject item = items[index];
                    items.RemoveAt(index);
                    OnItemRemoved(UnsubscribeFromKeyChanges(obj), index);
                    return true;
                }
            }
            return false;
        }

        virtual public bool Remove(TKey key)
        {
            key = KeyModifier(key);
            if (_Dictionary.ContainsKey(key))
            {
                var list = _Dictionary[key];

                for (int i = list.Count - 1; i >= 0; i--)
                {
                    var obj = list[i];
                    list.RemoveAt(i);
                    OnItemRemoved(UnsubscribeFromKeyChanges(obj), list.StartIndex + i);
                }
                return true;
            }
            return false;
        }

        virtual public void SortKeys(IComparer<TKey> comparer = null)
        {
            TKey[] keys = _Dictionary.Keys.ToArray();

            _Lists.Clear();

            IMultiLinkedList<TObject> previous = null;
            foreach (TKey key in _Dictionary.Keys.OrderBy(k => k, comparer))
            {
                var list = _Dictionary[key];
                if (previous == null)
                {
                    previous = list;
                    previous.SetPrevious(null);
                }
                else
                {
                    previous.SetNext(list);
                    list.SetPrevious(previous);
                    previous = list;
                }

                _Lists.Add(list);
            }
        }
        
        #endregion

        #region ICollection<TObject> Members

        virtual public bool Contains(TObject item)
        {
            var key = KeyModifier(item.Key);
            if (_Dictionary.ContainsKey(key))
                return _Dictionary[key].Contains(item);
            return false;
        }

        virtual public void CopyTo(TObject[] array, int arrayIndex)
        {
            _Dictionary.SelectMany(kvp => kvp.Value).ToArray().CopyTo(array, arrayIndex);
        }

        virtual public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion
        
        #region IEnumerable<U> Members

        public IEnumerator<TObject> GetEnumerator()
        {
            return new MultiLinkedListEnumerator<TObject>(_Lists);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new MultiLinkedListEnumerator<TObject>(_Lists);
        }

        #endregion

        #region Helper Classes

        private class MultiLinkedListEnumerator<TType> :
            IEnumerator<TType>
        {
            IList<IMultiLinkedList<TType>> _Lists;
            IEnumerator<IMultiLinkedList<TType>> _ListsEnumerator;
            IEnumerator<TType> _ListEnumerator;

            public MultiLinkedListEnumerator(IList<IMultiLinkedList<TType>> lists)
            {
                _Lists = lists;
            }
            
            virtual public TType Current
            {
                get 
                {
                    if (_ListEnumerator != null)
                        return _ListEnumerator.Current;
                    return default(TType);
                }
            }

            virtual public void Dispose()
            {
                Reset();
            }

            void DisposeListEnumerator()
            {
                if (_ListEnumerator != null)
                {
                    _ListEnumerator.Dispose();
                    _ListEnumerator = null;
                }
            }

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    if (_ListEnumerator != null)
                        return _ListEnumerator.Current;
                    return default(TType);
                }
            }

            private bool MoveNextList()
            {
                if (_ListsEnumerator == null)
                {
                    _ListsEnumerator = _Lists.GetEnumerator();
                }

                if (_ListsEnumerator != null)
                {
                    if (_ListsEnumerator.MoveNext())
                    {
                        DisposeListEnumerator();
                        if (_ListsEnumerator.Current != null)
                        {
                            _ListEnumerator = _ListsEnumerator.Current.GetEnumerator();
                            return true;
                        }
                    }
                }

                return false;
            }

            virtual public bool MoveNext()
            {
                if (_ListEnumerator != null)
                {
                    if (_ListEnumerator.MoveNext())
                    {
                        return true;
                    }
                    else
                    {
                        DisposeListEnumerator();
                        if (MoveNextList())
                            return MoveNext();
                    }
                }
                else
                {
                    if (MoveNextList())
                        return MoveNext();
                }
                return false;
            }

            virtual public void Reset()
            {
                
                if (_ListsEnumerator != null)
                {
                    _ListsEnumerator.Dispose();
                    _ListsEnumerator = null;
                }
            }
        }

        #endregion
    }    
}
