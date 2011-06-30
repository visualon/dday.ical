using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDay.Collections
{
    public interface IGroupedValueList<TGroup, TItem, TValueType> :
        IGroupedList<TGroup, TItem>
        where TItem : class, IGroupedObject<TGroup>, IValueObject<TValueType>        
    {        
        void Set(TGroup group, TValueType value);
        void Set(TGroup group, IEnumerable<TValueType> values);
        TType Get<TType>(TGroup group) where TType : TValueType;
        IList<TType> GetMany<TType>(TGroup group) where TType : TValueType;
    }
}
