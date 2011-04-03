using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Runtime.Serialization;

namespace DDay.iCal
{
#if !SILVERLIGHT
    [Serializable]
#endif
    public class CalendarParameterListProxy :
        CalendarKeyedValueListProxy<ICalendarParameter, string>,
        ICalendarParameterList
    {
        #region Constructors

        public CalendarParameterListProxy(ICalendarParameterList realObject, ICalendarObject parent)
            : base(realObject, parent)
        {
        }

        #endregion
    }
}
