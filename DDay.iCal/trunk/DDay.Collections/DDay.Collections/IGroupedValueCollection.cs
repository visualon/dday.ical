using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDay.Collections
{
    public interface IGroupedValueCollection<TGroup, TItem, TValueType> :
        IGroupedCollection<TGroup, TItem>
        where TItem : class, IGroupedObject<TGroup>, IValueObject<TValueType>        
    {        
        void Set(TGroup group, TValueType value);
        void Set(TGroup group, IEnumerable<TValueType> values);
        TType Get<TType>(TGroup group) where TType : TValueType;
        ICollection<TType> GetMany<TType>(TGroup group) where TType : TValueType;
    }
}
