using System;
using System.Collections.Generic;
using System.Text;

namespace DDay.iCal
{
    public interface ICalendarPropertyList : 
        IKeyedValueList<string, ICalendarProperty, object>
    {
    }
}
