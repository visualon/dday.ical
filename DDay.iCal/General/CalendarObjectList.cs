using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace DDay.iCal
{
    /// <summary>
    /// A collection of calendar objects.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    public class CalendarObjectList :
        KeyedList<string, ICalendarObject>,
        ICalendarObjectList<ICalendarObject>
    {
    }
}
