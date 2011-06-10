using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDay.iCal
{
    public static class CalendarParameterListExtensions
    {
        static public string Get(this ICalendarParameterCollection parameterList, string name)
        {
            return parameterList.Get<string>(name);
        }

        static public IList<string> GetMany(this ICalendarParameterCollection parameterList, string name)
        {
            return parameterList.GetMany<string>(name);
        }
    }
}
