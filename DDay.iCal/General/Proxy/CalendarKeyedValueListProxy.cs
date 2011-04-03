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
    public class CalendarKeyedValueListProxy<TObject, TValueType> :
        KeyedValueListProxy<string, TObject, TValueType>
        where TObject : class, ICalendarObject, IValueObject<TValueType>
    {
        #region Private Fields

        ICalendarObject _Parent;

        #endregion

        #region Constructors

        public CalendarKeyedValueListProxy(IKeyedValueList<string, TObject, TValueType> realObject, ICalendarObject parent)
            : base(realObject)
        {
            SetParent(parent);

            ItemAdded += new EventHandler<ObjectEventArgs<TObject>>(CalendarKeyedValueListProxy_ItemAdded);
            ItemRemoved += new EventHandler<ObjectEventArgs<TObject>>(CalendarKeyedValueListProxy_ItemRemoved);
        }

        #endregion

        #region Public Methods

        virtual public void SetParent(ICalendarObject parent)
        {
            _Parent = parent;
        }

        #endregion

        #region Event Handlers

        void CalendarKeyedValueListProxy_ItemAdded(object sender, ObjectEventArgs<TObject> e)
        {
            e.Object.Parent = _Parent;
        }

        void CalendarKeyedValueListProxy_ItemRemoved(object sender, ObjectEventArgs<TObject> e)
        {
            e.Object.Parent = null;
        }

        #endregion
    }
}
