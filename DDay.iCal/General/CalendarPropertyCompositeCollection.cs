using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace DDay.iCal
{
    /// <summary>
    /// This class takes multiple calendar properties/property values
    /// and consolidates them into a single list.
    /// 
    /// <example>
    /// Consider the following example:
    /// 
    /// BEGIN:VEVENT
    /// CATEGORIES:APPOINTMENT,EDUCATION
    /// CATEGORIES:MEETING
    /// END:EVENT
    /// </example>
    /// 
    /// When we process this event, we don't really care that there
    /// are 2 different CATEGORIES properties, no do we care that
    /// the first CATEGORIES property has 2 values, whereas the 
    /// second CATEGORIES property only has 1 value.  In the end,
    /// we want a list of 3 values: APPOINTMENT, EDUCATION, and MEETING.
    /// 
    /// This class consolidates properties of a given name into a list,
    /// and allows you to work with those values directly against the
    /// properties themselves.  This preserves the notion that our values
    /// are still stored directly within the calendar property, but gives
    /// us the flexibility to work with multiple properties through a
    /// single (composite) list.
    /// </summary>
    public class CalendarPropertyCompositeCollection<T> :
        ICalendarPropertyCompositeCollection<T>
    {
        #region Private Fields

        ICalendarPropertyCollection m_PropertyList;
        string m_PropertyName;

        #endregion

        #region Constructors

        public CalendarPropertyCompositeCollection(ICalendarPropertyCollection propertyList, string propertyName)
        {
            m_PropertyList = propertyList;
            m_PropertyName = propertyName;
        }

        #endregion
        
        #region ICollection<T> Members

        public void Add(T item)
        {
            // If there's already a property containing an IList<object>
            // as its value, then let's append our value to it
            if (m_PropertyList.ContainsKey(m_PropertyName))
            {
                ICalendarProperty property = m_PropertyList[m_PropertyName];
                if (property.Value is IList<object>)
                {
                    ((IList<object>)property.Value).Add(item);
                    return;
                }
            }

            // Create a new list to store our item
            IList<object> list = new List<object>();
            list.Add(item);

            // Create a new property to store our item
            CalendarProperty p = new CalendarProperty();
            p.Name = m_PropertyName;
            p.Value = list;

            m_PropertyList.Add(p);
        }

        virtual public void Clear()
        {
            m_PropertyList.Remove(m_PropertyName);
        }

        virtual public bool Contains(T item)
        {
            if (m_PropertyName.Contains(m_PropertyName))
            {
                foreach (ICalendarProperty p in m_PropertyList[
            }
        }

        virtual public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        virtual public int Count
        {
            get
            {
                int count = 0;
                foreach (ICalendarProperty p in m_PropertyList.AllOf(m_PropertyName))
                {
                    if (p.Value is IList<object>)
                    {
                        foreach (object obj in (IList<object>)p.Value)
                        {
                            if (obj is T)
                                count++;
                        }
                    }
                }
                return count;
            }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
