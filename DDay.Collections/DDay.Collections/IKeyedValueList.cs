using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDay.Collections
{
    public interface IKeyedValueList<TKey, TObject, TValueType> :
        IKeyedList<TKey, TObject>
        where TObject : class, IKeyedObject<TKey>, IValueObject<TValueType>        
    {        
        void Set(TKey key, TValueType value);
        void Set(TKey key, IEnumerable<TValueType> values);
        TType Get<TType>(TKey key) where TType : TValueType;
        IList<TType> GetMany<TType>(TKey key) where TType : TValueType;
    }
}
