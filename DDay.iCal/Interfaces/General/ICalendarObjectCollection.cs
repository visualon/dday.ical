using System;
using System.Collections.Generic;
using System.Text;
using DDay.Collections;

namespace DDay.iCal
{
    public interface ICalendarObjectCollection<TType> : 
        IGroupedCollection<string, TType>
        where TType : class, ICalendarObject
    {
    }
}
