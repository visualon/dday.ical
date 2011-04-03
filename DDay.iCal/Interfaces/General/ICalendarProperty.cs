using System;
using System.Collections.Generic;
using System.Text;

namespace DDay.iCal
{
    public interface ICalendarProperty :        
        ICalendarParameterListContainer,
        ICalendarObject,
        IValueObject<object>
    {        
    }    
}
