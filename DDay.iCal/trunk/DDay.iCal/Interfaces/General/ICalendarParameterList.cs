using System;
using System.Collections.Generic;
using System.Text;

namespace DDay.iCal
{
    public interface ICalendarParameterList :
        IKeyedValueList<string, ICalendarParameter, string>
    {        
    }
}
