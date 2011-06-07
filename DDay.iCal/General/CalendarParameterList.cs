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
    public class CalendarParameterList :
        KeyedValueList<string, ICalendarParameter, string>,
        ICalendarParameterList
    {
        #region Private Fields

        ICalendarObject m_Parent;
        bool m_CaseInsensitive;

        #endregion

        #region Constructors

        public CalendarParameterList()
        {
        }

        public CalendarParameterList(ICalendarObject parent, bool caseInsensitive)
        {
            m_Parent = parent;
            m_CaseInsensitive = caseInsensitive;

            ItemAdded += new EventHandler<ObjectEventArgs<ICalendarParameter>>(OnParameterAdded);
            ItemRemoved += new EventHandler<ObjectEventArgs<ICalendarParameter>>(OnParameterRemoved);
        }

        #endregion

        #region Protected Methods

        protected void OnParameterRemoved(object sender, ObjectEventArgs<ICalendarParameter> e)
        {
            e.Object.Parent = null;
        }

        protected void OnParameterAdded(object sender, ObjectEventArgs<ICalendarParameter> e)
        {
            e.Object.Parent = m_Parent;
        }

        #endregion

        #region Overrides

        protected override string KeyModifier(string key)
        {
            if (m_CaseInsensitive)
                return key.ToUpper();
            return key;
        }

        #endregion
    }
}
