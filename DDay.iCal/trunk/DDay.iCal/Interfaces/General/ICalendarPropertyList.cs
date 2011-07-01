using System;
using System.Collections.Generic;
using System.Text;
using DDay.Collections;

namespace DDay.iCal
{
    public interface ICalendarPropertyList : 
        IGroupedValueList<string, ICalendarProperty, object>
    {
    }
}
