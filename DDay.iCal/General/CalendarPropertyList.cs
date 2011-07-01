using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Runtime.Serialization;
using DDay.Collections;

namespace DDay.iCal
{
#if !SILVERLIGHT
    [Serializable]
#endif
    public class CalendarPropertyList :
        GroupedValueList<string, ICalendarProperty, CalendarProperty, object>,
        ICalendarPropertyList
    {
        #region Private Fields

        ICalendarObject m_Parent;
        bool m_CaseInsensitive;

        #endregion

        #region Constructors

        public CalendarPropertyList()
        {
        }

        public CalendarPropertyList(ICalendarObject parent, bool caseInsensitive)
        {
            m_Parent = parent;
            m_CaseInsensitive = caseInsensitive;

            ItemAdded += new EventHandler<ObjectEventArgs<ICalendarProperty, int>>(CalendarPropertyList_ItemAdded);
            ItemRemoved += new EventHandler<ObjectEventArgs<ICalendarProperty, int>>(CalendarPropertyList_ItemRemoved);
        }

        #endregion

        #region Overrides

        protected override string GroupModifier(string group)
        {
            if (m_CaseInsensitive)
                return group.ToUpper();
            return group;
        }

        #endregion

        #region Event Handlers

        void CalendarPropertyList_ItemRemoved(object sender, ObjectEventArgs<ICalendarProperty, int> e)
        {
            e.First.Parent = null;
        }

        void CalendarPropertyList_ItemAdded(object sender, ObjectEventArgs<ICalendarProperty, int> e)
        {
            e.First.Parent = m_Parent;
        }

        #endregion
    }
}
