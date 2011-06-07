using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDay.Collections
{
    public class KeyedValueList<TKey, TObject, TValueType> :
        KeyedList<TKey, TObject>,
        IKeyedValueList<TKey, TObject, TValueType>
        where TObject : class, IKeyedObject<TKey>, IValueObject<TValueType>
    {
        #region IKeyedValueList<TKey, TObject, TValueType> Members

        virtual public void Set(TKey key, TValueType value)
        {
            Set(key, new TValueType[] { value });
        }

        virtual public void Set(TKey key, IEnumerable<TValueType> values)
        {
            if (ContainsKey(key))
            {
                IEnumerable<TObject> items = AllOf(key);
                if (items != null)
                {
                    // Add a value to the first matching item in the list
                    items.FirstOrDefault().SetValue(values);
                    return;
                }
            }

            // No matching item was found, add a new item to the list
            TObject obj = Activator.CreateInstance(typeof(TObject)) as TObject;

            // Set the key for the object
            obj.Key = key;

            // Add the object to the list
            Add(obj);

            // Set the list of values for the object
            obj.SetValue(values);
        }

        virtual public TType Get<TType>(TKey key) where TType : TValueType
        {
            return AllOf(key)
                .FirstOrDefault()
                .Values
                .OfType<TType>()
                .FirstOrDefault();
        }

        virtual public IList<TType> GetMany<TType>(TKey key) where TType : TValueType
        {
            return new KeyedValueListProxy<TKey, TObject, TValueType, TType>(this);
        }

        #endregion
    }
}
