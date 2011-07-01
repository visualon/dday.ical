using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDay.Collections;

namespace DDay.iCal
{
    public interface IUniqueComponentCollection<TComponentType> :
        IGroupedCollection<string, TComponentType>
        where TComponentType : class, IUniqueComponent
    {
        TComponentType this[string uid] { get; set; }
    }
}
