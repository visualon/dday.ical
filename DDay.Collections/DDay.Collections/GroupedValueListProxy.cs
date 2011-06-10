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
    public class GroupedValueListProxy<TGroup, TOriginal, TOriginalValue, TNewValue> :
        ICollection<TNewValue>
        where TOriginal : class, IGroupedObject<TGroup>, IValueObject<TOriginalValue>, new()
        where TNewValue : TOriginalValue
    {
        #region Private Fields

        GroupedValueList<TGroup, TOriginal, TOriginalValue> _RealObject;
        TGroup _Key;
        TOriginal _Container;

        #endregion

        #region Constructors

        public GroupedValueListProxy(GroupedValueList<TGroup, TOriginal, TOriginalValue> realObject, TGroup group)
        {
            _RealObject = realObject;
            _Key = group;
        }

        #endregion

        #region Private Methods

        TOriginal EnsureContainer()
        {
            if (_Container == null)
            {
                // Find an item that matches our group
                _Container = _RealObject.AllOf(_Key).FirstOrDefault();

                // If no item is found, create a new object and add it to the list
                if (_Container == default(TOriginal))
                {
                    _Container = new TOriginal();
                    _Container.Group = _Key;
                    _RealObject.Add(_Container);
                }
            }
            return _Container;
        }

        #endregion

        virtual public void Add(TNewValue item)
        {
            // Add the value to the object
            EnsureContainer().AddValue(item);
        }

        virtual public void Clear()
        {
            foreach (TOriginal original in _RealObject.AllOf(_Key))
            {
                if (original.Values != null)
                {
                    // Clear all values from each matching object
                    original.SetValue(default(TOriginalValue));
                }
            }
        }

        virtual public bool Contains(TNewValue item)
        {
            foreach (TOriginal original in _RealObject.AllOf(_Key))
            {
                if (original.ContainsValue(item))
                {
                    return true;
                }
            }
            return false;
        }

        virtual public void CopyTo(TNewValue[] array, int arrayIndex)
        {
            int index = arrayIndex;
            foreach (TOriginal original in _RealObject.AllOf(_Key))
            {
                if (original.Values != null)
                {
                    var valueArray = original.Values.ToArray();
                    valueArray.CopyTo(array, arrayIndex + index);
                    index += valueArray.Length;
                }
            }
        }
        
        virtual public int Count
        {
            get
            {
                return _RealObject
                    .AllOf(_Key)
                    .Sum(o => o.ValueCount);
            }
        }

        virtual public bool IsReadOnly
        {
            get { return false; }
        }

        virtual public bool Remove(TNewValue item)
        {
            foreach (TOriginal original in _RealObject.AllOf(_Key))
            {
                if (original.ContainsValue(item))
                {
                    original.RemoveValue(item);
                    return true;
                }
            }
            return false;
        }

        virtual public IEnumerator<TNewValue> GetEnumerator()
        {
            return new GroupedValueListEnumerator<TGroup, TOriginal, TOriginalValue, TNewValue>(_RealObject, _Key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new GroupedValueListEnumerator<TGroup, TOriginal, TOriginalValue, TNewValue>(_RealObject, _Key);
        }
    }
}
