using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace DDay.iCal
{
    [DebuggerDisplay("{Source} ({Period.StartTime} - {Period.EndTime})")]
#if DATACONTRACT
    [DataContract(Name = "Occurrence", Namespace = "http://www.ddaysoftware.com/dday.ical/2009/07/")]
#endif
    [Serializable]
    public struct Occurrence :
        IComparable<Occurrence>
    {
        #region Private Fields
        
        private IPeriod m_Period;
        private IRecurrable m_Source; 

        #endregion

        #region Public Properties

#if DATACONTRACT
        [DataMember(Order = 1)]
#endif
        public IPeriod Period
        {
            get { return m_Period; }
            set { m_Period = value; }
        }

#if DATACONTRACT
        [DataMember(Order = 2)]
#endif
        public IRecurrable Source
        {
            get { return m_Source; }
            set { m_Source = value; }
        } 

        #endregion

        #region Constructors

        public Occurrence(Occurrence ao)
        {
            m_Period = ao.Period;
            m_Source = ao.Source;
        }

        public Occurrence(IRecurrable recurrable, IPeriod period)
        {
            m_Source = recurrable;
            m_Period = period;
        }

        #endregion

        #region IComparable<Occurrence> Members

        public int CompareTo(Occurrence other)
        {
            return Period.CompareTo(other.Period);
        }

        #endregion
    }
}
