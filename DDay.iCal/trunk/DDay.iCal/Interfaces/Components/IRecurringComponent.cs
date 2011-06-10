using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDay.iCal
{
    public interface IRecurringComponent :
        IUniqueComponent,
        IRecurrable,
        IAlarmContainer
    {
        ICollection<IAttachment> Attachments { get; set; }
        ICollection<string> Categories { get; set; }
        string Class { get; set; }
        ICollection<string> Contacts { get; set; }
        IDateTime Created { get; set; }
        string Description { get; set; }
        IDateTime LastModified { get; set; }
        int Priority { get; set; }
        ICollection<string> RelatedComponents { get; set; }
        int Sequence { get; set; }
        string Summary { get; set; }
    }
}
