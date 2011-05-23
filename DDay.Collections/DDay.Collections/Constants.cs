using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDay.Collections
{
    #region EventArgs

    public class ObjectEventArgs<T> :
        EventArgs
    {
        public T Object { get; set; }

        public ObjectEventArgs(T obj)
        {
            Object = obj;
        }
    }

    public class ObjectEventArgs<T, U> :
        EventArgs
    {
        public T First { get; set; }
        public U Second { get; set; }

        public ObjectEventArgs(T first, U second)
        {
            First = first;
            Second = second;
        }
    }

    public class ValueChangedEventArgs<T> :
        EventArgs
    {
        public IList<T> RemovedValues { get; protected set; }
        public IList<T> AddedValues { get; protected set; }

        public ValueChangedEventArgs(IEnumerable<T> removedValues, IEnumerable<T> addedValues)
        {
            RemovedValues = removedValues.ToList().AsReadOnly();
            AddedValues = addedValues.ToList().AsReadOnly();
        }
    }
    
    #endregion
}
