using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.Serialization;
using DDay.Collections;

namespace DDay.iCal
{
    /// <summary>
    /// A collection of calendar objects.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    public class CalendarObjectCollection :
        GroupedCollection<string, ICalendarObject>,
        ICalendarObjectCollection<ICalendarObject>
    {
        ICalendarObject _Parent;

        public CalendarObjectCollection(ICalendarObject parent)
        {
            _Parent = parent;
        }
    }
}
