using System;
using System.Collections.Generic;
using System.Text;

namespace DDay.iCal
{
    public interface ICalendarObjectList<TType> : 
        IKeyedList<string, TType>
        where TType : class, ICalendarObject
    {
    }
}
