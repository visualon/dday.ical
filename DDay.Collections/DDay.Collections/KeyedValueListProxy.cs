using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections;

namespace DDay.Collections
{
    /// <summary>
    /// A proxy for a keyed list.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    public class KeyedValueListProxy<TKey, TOriginal, TOriginalValue, TNewValue> :
        IList<TNewValue>
        where TOriginal : class, IKeyedObject<TKey>, IValueObject<TOriginalValue>
        where TNewValue : TOriginalValue
    {
        #region Private Fields

        IKeyedValueList<TKey, TOriginal, TOriginalValue> _RealObject;

        #endregion

        #region Constructors

        public KeyedValueListProxy(IKeyedValueList<TKey, TOriginal, TOriginalValue> realObject)
        {
            _RealObject = realObject;
        }

        #endregion

        virtual public int IndexOf(TNewValue item)
        {
            throw new NotImplementedException();
        }

        virtual public void Insert(int index, TNewValue item)
        {
            throw new NotImplementedException();
        }

        virtual public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        virtual public TNewValue this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        virtual public void Add(TNewValue item)
        {
            throw new NotImplementedException();
        }

        virtual public void Clear()
        {
            throw new NotImplementedException();
        }

        virtual public bool Contains(TNewValue item)
        {
            throw new NotImplementedException();
        }

        virtual public void CopyTo(TNewValue[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        virtual public int Count
        {
            get { throw new NotImplementedException(); }
        }

        virtual public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        virtual public bool Remove(TNewValue item)
        {
            throw new NotImplementedException();
        }

        virtual public IEnumerator<TNewValue> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
