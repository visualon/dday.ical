using System;
using System.Collections.Generic;
using System.Text;

namespace DDay.iCal
{
    public interface ICalendarParameter :
        ICalendarObject,
        IValueObject<string>
    {
        string Value { get; set; }
    }
}
