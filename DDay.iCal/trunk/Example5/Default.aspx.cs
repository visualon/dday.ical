using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

// Required DDay.iCal namespaces
using DDay.iCal;
using DDay.iCal.Components;

public partial class _Default : System.Web.UI.Page
{
    #region Protected Fields

    protected string _CalendarAbsPath;
    protected List<iCalendar> _Calendars = null;

    #endregion

    #region Event Sorting Class

    class EventDateSorter : IComparer<Event>
    {
        #region IComparer<Event> Members

        public int Compare(Event x, Event y)
        {
            int i = x.Start.CompareTo(y.Start);
            return i;
        }

        #endregion
    }

    #endregion

    #region Event Handlers

    protected void Page_Load(object sender, EventArgs e)
    {
        _CalendarAbsPath = MapPath("~/Calendars");

        if (!IsPostBack)
        {
            // Load our list of available calendars
            CalendarList.DataSource = LoadCalendarList();
            CalendarList.DataBind();

            // Select all calendars in the list by default
            foreach (ListItem li in CalendarList.Items)
                li.Selected = true;            
        }

        // Create an object that will sort our events by date
        EventDateSorter eventDateSorter = new EventDateSorter();

        // Build a list of today's events, sort that list by date
        // and bind it to the repeater that will display the events.
        List<Event> todaysEvents = new List<Event>(GetTodaysEvents());
        todaysEvents.Sort(eventDateSorter);
        TodaysEvents.DataSource = todaysEvents;
        TodaysEvents.DataBind();

        // Build a list of upcoming events, sort that list by date
        // and bind it to the repeater that will display the events.
        List<Event> upcomingEvents = new List<Event>(GetUpcomingEvents());
        upcomingEvents.Sort(eventDateSorter);
        UpcomingEvents.DataSource = upcomingEvents;
        UpcomingEvents.DataBind();
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Gets a list of iCalendars that are in the "Calendars" directory.
    /// </summary>
    /// <returns>
    /// A list of filenames, without extensions, of the iCalendars 
    /// in the "Calendars" directory
    /// </returns>
    protected IEnumerable<string> LoadCalendarList()
    {
        foreach (string file in Directory.GetFiles(_CalendarAbsPath, "*.ics"))
            yield return Path.GetFileNameWithoutExtension(file);
    }

    /// <summary>
    /// Loads and parses the selected calendars.
    /// </summary>
    protected void LoadSelectedCalendars()
    {
        _Calendars = new List<iCalendar>();
        foreach (ListItem li in CalendarList.Items)
        {
            // Make sure the item is selected
            if (li.Selected)
            {
                // Load the calendar from the file system
                iCalendar iCal = iCalendar.LoadFromFile(Path.Combine(_CalendarAbsPath, li.Text + @".ics"));
                if (iCal != null)
                    _Calendars.Add(iCal);
            }
        }
    }

    /// <summary>
    /// Gets a list of events that occur today.
    /// </summary>
    /// <returns>A list of events that occur today</returns>
    protected IEnumerable<Event> GetTodaysEvents()
    {
        // Load selected calendars, if we haven't already
        if (_Calendars == null)
            LoadSelectedCalendars();
                
        // Iterate through each loaded calendar
        foreach (iCalendar iCal in _Calendars)
        {
            // Evaluate today's date to see if any events occur on it
            iCal.Evaluate(DateTime.Today, DateTime.Today.AddDays(1));

            // Iterate through each event in the calendar
            foreach(Event evt in iCal.Events)
            {
                // Get all event recurrences for today
                foreach(Event e in evt.FlattenRecurrencesOn(DateTime.Today))                
                    yield return e;
            }
        }
    }

    protected IEnumerable<Event> GetUpcomingEvents()
    {
        // Load selected calendars, if we haven't already
        if (_Calendars == null)
            LoadSelectedCalendars();

        // Determine the range of events we're interested in (1 week)
        DateTime startDate = DateTime.Today.AddDays(1);
        DateTime endDate = startDate.AddDays(7);
        
        // Iterate through each loaded calendar
        foreach (iCalendar iCal in _Calendars)
        {
            // Evaluate events within our selected period to see if any events occur
            iCal.Evaluate(startDate, endDate);

            foreach (Event evt in iCal.Events)
            {
                // Return flattened occurences of the event,
                // so the start time is equal to the time it
                // recurred (not the original start time of the event)
                foreach (Event e in evt.FlattenRecurrences())
                {
                    // Make sure the flattened events occur within our designated period
                    if (e.Start.Date >= startDate &&
                        e.Start.Date <= endDate)
                        yield return e;
                }
            }                    
        }
    }

    protected string GetTimeDisplay(object obj)
    {
        if (obj is Event)
        {
            Event evt = (Event)obj;
            if (evt.IsAllDay)
                return "All Day";
            else return evt.Start.Local.ToString("h:mm tt");
        }
        return string.Empty;
    }

    #endregion
}
