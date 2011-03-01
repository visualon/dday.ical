using System;
using System.Collections.Generic;
using System.Text;

namespace DDay.iCal
{
    public interface ICalendarObjectList<T> : 
        IKeyedList<string, T>
        where T : class, ICalendarObject
    {
    }
}
