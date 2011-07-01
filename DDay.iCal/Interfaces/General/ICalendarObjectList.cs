using System;
using System.Collections.Generic;
using System.Text;
using DDay.Collections;

namespace DDay.iCal
{
    public interface ICalendarObjectList<TType> : 
        IGroupedList<string, TType>
        where TType : class, ICalendarObject
    {
    }
}
