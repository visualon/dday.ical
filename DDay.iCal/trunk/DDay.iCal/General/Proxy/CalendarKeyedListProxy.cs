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
    public class CalendarKeyedListProxy<TType> :
        CalendarKeyedListProxy<TType, TType>
        where TType : class, ICalendarObject
    {
        public CalendarKeyedListProxy(IKeyedList<string, TType> realObject, Predicate<TType> predicate, ICalendarObject parent)
            : base(realObject, predicate, parent)
        {
        }
    }

#if !SILVERLIGHT
    [Serializable]
#endif
    public class CalendarKeyedListProxy<TOriginal, TNew> :
        KeyedListProxy<string, TOriginal, TNew>
        where TOriginal : class, ICalendarObject
        where TNew : class, TOriginal
    {
        #region Private Fields

        ICalendarObject _Parent;

        #endregion

        #region Constructors

        public CalendarKeyedListProxy(IKeyedList<string, TOriginal> realObject, Predicate<TNew> predicate, ICalendarObject parent)
            : base(realObject, predicate)
        {
            SetParent(parent);

            ItemAdded += new EventHandler<ObjectEventArgs<TNew>>(CalendarKeyedListProxy_ItemAdded);
            ItemRemoved += new EventHandler<ObjectEventArgs<TNew>>(CalendarKeyedListProxy_ItemRemoved);
        }

        #endregion

        #region Public Methods

        virtual public void SetParent(ICalendarObject parent)
        {
            _Parent = parent;
        }

        #endregion

        #region Event Handlers

        void CalendarKeyedListProxy_ItemAdded(object sender, ObjectEventArgs<TNew> e)
        {
            e.Object.Parent = _Parent;
        }

        void CalendarKeyedListProxy_ItemRemoved(object sender, ObjectEventArgs<TNew> e)
        {
            e.Object.Parent = null;
        }

        #endregion
    }
}
