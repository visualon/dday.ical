using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DDay.iCal
{
    /// <summary>
    /// A collection of iCalendar components.  This class is used by the 
    /// <see cref="iCalendar"/> class to maintain a collection of events,
    /// to-do items, journal entries, and free/busy times.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    public class UniqueComponentList<T> :
        FilteredCalendarObjectList<IUniqueComponent>,
        IUniqueComponentList<T>
        where T : class, IUniqueComponent
    {
        #region Private Fields

        private UIDFactory m_UIDFactory = new UIDFactory();

        #endregion

        #region Constructors

        public UniqueComponentList(ICalendarObject attached) : base(attached)
        {            
            ResolveUIDs();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Automatically assigns a UID to items that do not already have one.
        /// </summary>
        public void ResolveUIDs()
        {
            foreach (T item in this)
            {
                if (string.IsNullOrEmpty(item.UID))
                    item.UID = m_UIDFactory.Build();
            }
        }
                        
        #endregion
    }
}
