using System;
using System.Diagnostics;
using System.Data;
using System.Configuration;
using DDay.iCal;
using DDay.iCal;
using DDay.iCal.Serialization;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DDay.iCal
{
    /// <summary>
    /// A class that represents an RFC 5545 VJOURNAL component.
    /// </summary>
    [DebuggerDisplay("{Summary}: {(Description.ToString().Length < 32) ? Description.ToString() : Description.ToString().Substring(0, 32)}")]
#if DATACONTRACT
    [DataContract(Name = "Journal", Namespace = "http://www.ddaysoftware.com/dday.ical/2009/07/")]
#endif
    [Serializable]
    public class Journal : RecurringComponent
    {
        #region Public Properties
        
        public JournalStatus Status
        {
            get { return Properties.Get<JournalStatus>("STATUS"); }
            set { Properties.Set("STATUS", value); }
        } 

        #endregion

        #region Static Public Methods

        static public Journal Create(iCalendar iCal)
        {
            return iCal.Create<Journal>();
        }

        #endregion

        #region Constructors

        public Journal()
        {
            this.Name = ComponentFactory.JOURNAL;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Returns a typed copy of the Journal object.
        /// </summary>
        /// <returns>A typed copy of the Journal object.</returns>
        public new Journal Copy()
        {
            return base.Copy<Journal>();
        }

        internal override List<Period> Evaluate(iCalDateTime FromDate, iCalDateTime ToDate)
        {
            if (Start != null)
            {
                Period p = new Period(Start);
                if (!Periods.Contains(p))
                    Periods.Add(p);

                return base.Evaluate(FromDate, ToDate);
            }
            return new System.Collections.Generic.List<Period>();
        }

        #endregion
    }
}