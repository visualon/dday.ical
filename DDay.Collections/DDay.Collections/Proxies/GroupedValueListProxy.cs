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
        IList<TNewValue>
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
            var items = _RealObject
                .AllOf(_Key)
                .Where(o => o.Values != null);

            foreach (TOriginal original in items)
            {
                // Clear all values from each matching object
                original.SetValue(default(TOriginalValue));
            }
        }

        virtual public bool Contains(TNewValue item)
        {
            return _RealObject
                .AllOf(_Key)
                .Where(o => o.ContainsValue(item))
                .Any();
        }

        virtual public void CopyTo(TNewValue[] array, int arrayIndex)
        {
            _RealObject
                .AllOf(_Key)
                .Where(o => o.Values != null)
                .SelectMany(o => o.Values)
                .ToArray()
                .CopyTo(array, arrayIndex);            
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
            var container = _RealObject
                .AllOf(_Key)
                .Where(o => o.ContainsValue(item))
                .FirstOrDefault();

            if (container != null)
            {
                container.RemoveValue(item);
                return true;
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
    }
}
