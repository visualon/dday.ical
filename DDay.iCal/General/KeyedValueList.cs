using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDay.iCal
{
    public class KeyedValueList<TKey, TObject, TValueType> :
        KeyedList<TKey, TObject>,
        IKeyedValueList<TKey, TObject, TValueType>
        where TObject : class, IKeyedObject<TKey>, IValueObject<TValueType>
    {
        #region IKeyedValueList<TKey, TObject, TValueType> Members

        virtual public void Set(TKey key, TValueType value)
        {
            List<TValueType> values = new List<TValueType>();
            values.Add(value);
            this[key].SetValue(value);
        }

        virtual public void Set(TKey key, IEnumerable<TValueType> values)
        {
            this[key].SetValue(values);
        }

        virtual public TType Get<TType>(TKey key) where TType : TValueType
        {
            if (ContainsKey(key))
                return this[key].Values.OfType<TType>().FirstOrDefault();
            return default(TType);
        }

        virtual public IList<TType> GetMany<TType>(TKey key) where TType : TValueType
        {
            // FIXME: return a collection here.
            // We probably need a concrete class to accomplish this.
            // (instead of the following commented code)
            //if (ContainsKey(key))
            //    return this[key].Values.OfType<TType>();
            return null;
        }

        #endregion
    }
}
