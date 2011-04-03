using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections;

namespace DDay.iCal
{
    /// <summary>
    /// A proxy for a keyed list.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    public class KeyedListProxy<TKey, TObject> :
        IKeyedList<TKey, TObject>
        where TObject : class, IKeyedObject<TKey>
    {
        #region Private Fields

        IKeyedList<TKey, TObject> _RealObject;

        #endregion

        #region Constructors

        public KeyedListProxy(IKeyedList<TKey, TObject> realObject)
        {
            SetProxiedObject(realObject);

            _RealObject.ItemAdded += new EventHandler<ObjectEventArgs<TObject>>(_RealObject_ItemAdded);
            _RealObject.ItemRemoved += new EventHandler<ObjectEventArgs<TObject>>(_RealObject_ItemRemoved);
        }

        #endregion

        #region Event Handlers

        void _RealObject_ItemRemoved(object sender, ObjectEventArgs<TObject> e)
        {
            OnItemRemoved(e.Object);
        }

        void _RealObject_ItemAdded(object sender, ObjectEventArgs<TObject> e)
        {
            OnItemAdded(e.Object);
        }

        #endregion

        #region Public Methods

        virtual public void SetProxiedObject(IKeyedList<TKey, TObject> realObject)
        {
            _RealObject = realObject;
        }

        #endregion

        #region IKeyedList<TKey,TObject> Members

        public event EventHandler<ObjectEventArgs<TObject>> ItemAdded;
        public event EventHandler<ObjectEventArgs<TObject>> ItemRemoved;

        protected void OnItemAdded(TObject item)
        {
            if (ItemAdded != null)
                ItemAdded(this, new ObjectEventArgs<TObject>(item));
        }

        protected void OnItemRemoved(TObject item)
        {
            if (ItemRemoved != null)
                ItemRemoved(this, new ObjectEventArgs<TObject>(item));
        }

        virtual public void Add(TObject item)
        {
            _RealObject.Add(item);
        }

        virtual public void Insert(int index, TObject item)
        {
            _RealObject.Insert(index, item);
        }

        virtual public bool Remove(TObject item)
        {
            return _RealObject.Remove(item);
        }

        virtual public bool Remove(TKey key)
        {
            return _RealObject.Remove(key);
        }

        virtual public int IndexOf(TObject item)
        {
            return _RealObject.IndexOf(item);
        }

        virtual public void Clear(TKey key)
        {
            _RealObject.Clear(key);
        }

        virtual public void Clear()
        {
            _RealObject.Clear();
        }

        virtual public bool ContainsKey(TKey key)
        {
            return _RealObject.ContainsKey(key);
        }

        virtual public int CountOf(TKey key)
        {
            return _RealObject.CountOf(key);
        }

        virtual public IEnumerable<TObject> Values()
        {
            return _RealObject.Values();
        }

        virtual public IEnumerable<TObject> AllOf(TKey key)
        {
            return _RealObject.AllOf(key);
        }

        virtual public TObject this[TKey key]
        {
            get
            {
                return _RealObject[key];
            }
            set
            {
                _RealObject[key] = value;
            }
        }

        virtual public TObject[] ToArray()
        {
            return _RealObject.ToArray();
        }

        #endregion

        #region IEnumerable<TObject> Members

        virtual public IEnumerator<TObject> GetEnumerator()
        {
            return _RealObject.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _RealObject.GetEnumerator();
        }

        #endregion
    }
}
