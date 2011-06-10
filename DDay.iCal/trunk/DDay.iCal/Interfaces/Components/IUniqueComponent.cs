using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDay.iCal
{
    public interface IUniqueComponent :
        ICalendarComponent
    {
        string UID { get; set; }

        ICollection<IAttendee> Attendees { get; set; }
        ICollection<string> Comments { get; set; }
        IDateTime DTStamp { get; set; }
        IOrganizer Organizer { get; set; }
        ICollection<IRequestStatus> RequestStatuses { get; set; }
        Uri Url { get; set; }
    }
}
