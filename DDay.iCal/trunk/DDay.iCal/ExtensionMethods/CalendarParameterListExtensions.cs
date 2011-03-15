using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDay.iCal
{
    public static class CalendarParameterListExtensions
    {
        static public string Get(this ICalendarParameterList parameterList, string name)
        {
            return parameterList.Get<string>(name);
        }
    }
}
