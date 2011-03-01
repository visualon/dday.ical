using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDay.iCal
{
    public interface IValueObject<T>
    {
        event EventHandler<ValueChangedEventArgs<T>> ValueChanged;
        IEnumerable<T> Values { get; }
        
        void SetValue(T value);
        void SetValue(IEnumerable<T> values);
        void AddValue(T value);
        void RemoveValue(T value);        
    }
}
