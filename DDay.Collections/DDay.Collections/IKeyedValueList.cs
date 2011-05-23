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
        void Set(TKey name, TValueType value);
        void Set(TKey name, IEnumerable<TValueType> values);
        TType Get<TType>(TKey name) where TType : TValueType;
        IList<TType> GetMany<TType>(TKey name) where TType : TValueType;
    }
}
