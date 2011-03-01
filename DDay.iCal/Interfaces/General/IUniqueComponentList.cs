using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace DDay.iCal
{
    public interface IUniqueComponentList<T> :
        IKeyedList<string, T>
        where T : class, IUniqueComponent
    {
    }
}
