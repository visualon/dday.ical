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
            throw new NotImplementedException();
        }

        virtual public void Set(TKey key, IEnumerable<TValueType> values)
        {
            throw new NotImplementedException();
        }

        virtual public TType Get<TType>(TKey key) where TType : TValueType
        {
            throw new NotImplementedException();
        }

        virtual public IList<TType> GetMany<TType>(TKey key) where TType : TValueType
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
