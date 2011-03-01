using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;

namespace DDay.iCal
{
    /// <summary>
    /// A list of objects that are keyed.  Note that keys are not ordered.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    public class KeyedList<TKey, TObject> :
        IKeyedList<TKey, TObject>
        where TObject : class, IKeyedObject<TKey>
    {
        #region Private Fields

        SortedDictionary<TKey, IList<TObject>> _Items = new SortedDictionary<TKey, IList<TObject>>();

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
                    if (_Items.ContainsKey(key))
                    {
                        IList<TObject> items = _Items[key];
                        int index = items.IndexOf(obj);
                        if (index >= 0)
                        {
                            items.RemoveAt(index);
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
        public event EventHandler<ObjectEventArgs<TObject>> ItemAdded;

        [field: NonSerialized]
        public event EventHandler<ObjectEventArgs<TObject>> ItemRemoved;

        protected void OnItemAdded(TObject obj)
        {
            if (ItemAdded != null)
                ItemAdded(this, new ObjectEventArgs<TObject>(obj));
        }

        protected void OnItemRemoved(TObject obj)
        {
            if (ItemRemoved != null)
                ItemRemoved(this, new ObjectEventArgs<TObject>(obj));
        }

        virtual public void Add(TObject item)
        {
            if (item != null)
            {
                TKey key = KeyModifier(item.Key);
                if (!_Items.ContainsKey(key))
                    _Items[key] = new List<TObject>();

                _Items[key].Add(SubscribeToKeyChanges(item));
                OnItemAdded(item);
            }
        }

        virtual public void Insert(int index, TObject item)
        {
            if (index >= 0)
            {
                TKey key = KeyModifier(item.Key);
                if (!_Items.ContainsKey(key))
                {
                    if (index != 0)
                        return;
                    _Items[key] = new List<TObject>();
                }

                IList<TObject> list = _Items[key];
                if (index >= 0 && index < list.Count)
                {
                    list.Insert(index, item);
                    OnItemAdded(item);
                }
            }
        }

        virtual public int IndexOf(TObject item)
        {
            TKey key = KeyModifier(item.Key);
            if (_Items.ContainsKey(key))
                return _Items[key].IndexOf(item);
            return -1;
        }

        virtual public void Clear(TKey key)
        {
            if (_Items.ContainsKey(key))
            {
                var items = _Items[key].ToArray();
                _Items[key].Clear();
                foreach (var item in items)
                    OnItemRemoved(item);
            }
        }

        virtual public void Clear()
        {
            var items = _Items.Values.SelectMany(i => i).ToArray();
            _Items.Clear();
            foreach (var item in items)
                OnItemRemoved(item);
        }

        virtual public bool ContainsKey(TKey key)
        {
            return _Items.ContainsKey(key);
        }

        virtual public int CountOf(TKey key)
        {
            if (_Items.ContainsKey(key))
                return _Items[key].Count;
            return 0;
        }

        virtual public IEnumerable<TObject> Values()
        {
            return _Items.Values.SelectMany(i => i);
        }

        virtual public IEnumerable<TObject> AllOf(TKey key)
        {
            if (_Items.ContainsKey(key))
                return _Items[key];
            return null;
        }

        virtual public TObject this[TKey key]
        {
            get
            {
                if (_Items.ContainsKey(key) &&
                    _Items[key].Count > 0)
                    return _Items[key][0];
                return default(TObject);
            }
            set
            {
                if (!_Items.ContainsKey(key))
                    _Items[key] = new List<TObject>();
                
                if (value != null)
                {
                    if (_Items[key].Count > 0)
                    {
                        if (_Items[key][0] != null)
                            UnsubscribeFromKeyChanges(_Items[key][0]);
                        _Items[key][0] = SubscribeToKeyChanges(value);
                    }
                    else
                    {
                        _Items[key].Add(SubscribeToKeyChanges(value));
                    }

                    OnItemAdded(value);
                }
                else if (_Items[key].Count > 0)
                {
                    TObject item = UnsubscribeFromKeyChanges(_Items[key][0]);
                    _Items[key].RemoveAt(0);
                    OnItemRemoved(item);
                }
            }
        }

        virtual public bool Remove(TObject obj)
        {
            TKey key = KeyModifier(obj.Key);
            if (_Items.ContainsKey(key))
            {
                IList<TObject> items = _Items[key];
                if (items.Remove(obj))
                {
                    OnItemRemoved(UnsubscribeFromKeyChanges(obj));
                    return true;
                }
            }
            return false;
        }

        virtual public TObject[] ToArray()
        {            
            return _Items.Values.SelectMany(i => i).ToArray();
        }

        #endregion

        #region IEnumerable<U> Members

        public IEnumerator<TObject> GetEnumerator()
        {
            return new KeyedCollectionEnumerator<TKey, TObject>(this);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new KeyedCollectionEnumerator<TKey, TObject>(this);
        }

        #endregion

        #region Helper Classes

        private class KeyedCollectionEnumerator<T, U> :
            IEnumerator<U>
            where U : class, IKeyedObject<T>
        {
            KeyedList<T, U> _KeyedCollection;
            IEnumerator<T> _KeyEnumerator;
            IEnumerator<U> _ValueEnumerator;

            public KeyedCollectionEnumerator(KeyedList<T, U> collection)
            {
                _KeyedCollection = collection;
            }

            #region IEnumerator<U> Members

            public U Current
            {
                get
                {
                    if (_ValueEnumerator != null)
                        return _ValueEnumerator.Current;
                    return default(U);
                }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                _KeyEnumerator.Dispose();
                _KeyEnumerator = null;
                _ValueEnumerator.Dispose();
                _ValueEnumerator = null;
            }

            #endregion

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    if (_ValueEnumerator != null)
                        return _ValueEnumerator.Current;
                    return default(U);
                }
            }

            bool MoveNextKey()
            {
                if (_KeyEnumerator == null)
                    _KeyEnumerator = _KeyedCollection._Items.Keys.GetEnumerator();
                return _KeyEnumerator.MoveNext();                
            }

            virtual public bool MoveNext()
            {
                if (_KeyEnumerator == null)
                    MoveNextKey();

                if (_ValueEnumerator == null)
                {
                    T key = _KeyedCollection.KeyModifier(_KeyEnumerator.Current);
                    _ValueEnumerator = _KeyedCollection._Items[key].GetEnumerator();
                }

                if (_ValueEnumerator.MoveNext())
                    return true;
                else
                {
                    if (MoveNextKey())
                        return MoveNext();
                }
                return false;
            }

            virtual public void Reset()
            {
                _KeyEnumerator.Dispose();
                _KeyEnumerator = null;
                _ValueEnumerator.Dispose();
                _ValueEnumerator = null;
            }

            #endregion
        }

        #endregion
    }
}
