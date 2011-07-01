using System;
using System.Collections.Generic;
using System.Text;
using DDay.Collections;

namespace DDay.iCal
{
    public interface ICalendarParameterCollection :
        IGroupedValueList<string, ICalendarParameter, CalendarParameter, string>
    {
    }
}
