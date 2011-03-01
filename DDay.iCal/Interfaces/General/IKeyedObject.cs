using System;
using System.Collections.Generic;
using System.Text;

namespace DDay.iCal
{
    public interface IKeyedObject<T>
    {
        event EventHandler<ObjectEventArgs<T, T>> KeyChanged;
        T Key { get; }
    }
}
