using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDay.Collections
{
    public class GroupedValueCollection<TGroup, TItem, TValueType> :
        GroupedCollection<TGroup, TItem>,
        IGroupedValueCollection<TGroup, TItem, TValueType>
        where TItem : class, IGroupedObject<TGroup>, IValueObject<TValueType>, new()
    {
        #region IKeyedValueList<TGroup, TItem, TValueType> Members

        virtual public void Set(TGroup group, TValueType value)
        {
            Set(group, new TValueType[] { value });
        }

        virtual public void Set(TGroup group, IEnumerable<TValueType> values)
        {
            if (ContainsKey(group))
            {
                IEnumerable<TItem> items = AllOf(group);
                if (items != null)
                {
                    // Add a value to the first matching item in the list
                    items.FirstOrDefault().SetValue(values);
                    return;
                }
            }

            // No matching item was found, add a new item to the list
            TItem obj = Activator.CreateInstance(typeof(TItem)) as TItem;

            // Set the group for the object
            obj.Group = group;

            // Add the object to the list
            Add(obj);

            // Set the list of values for the object
            obj.SetValue(values);
        }

        virtual public TType Get<TType>(TGroup group) where TType : TValueType
        {
            var firstItem = AllOf(group).FirstOrDefault();
            if (firstItem != null &&
                firstItem.Values != null)
            {
                return firstItem
                    .Values
                    .OfType<TType>()
                    .FirstOrDefault();
            }
            return default(TType);
        }

        virtual public ICollection<TType> GetMany<TType>(TGroup group) where TType : TValueType
        {
            return new GroupedValueCollectionProxy<TGroup, TItem, TValueType, TType>(this, group);
        }

        #endregion
    }
}
