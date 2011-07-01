using System;
using System.Collections.Generic;
using System.Text;
using DDay.Collections;

namespace DDay.iCal
{
    public interface ICalendarParameterList :
        IGroupedValueList<string, ICalendarParameter, string>
    {        
    }
}
