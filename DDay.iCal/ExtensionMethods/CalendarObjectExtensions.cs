using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDay.iCal
{
    public static class CalendarObjectExtensions
    {
        static public void AddChild<T>(this ICalendarObject obj, T child) where T : ICalendarObject
        {
            obj.Children.Add(child);
        }

        static public void RemoveChild<T>(this ICalendarObject obj, T child) where T : ICalendarObject
        {
            obj.Children.Remove(child);
        }
    }
}
