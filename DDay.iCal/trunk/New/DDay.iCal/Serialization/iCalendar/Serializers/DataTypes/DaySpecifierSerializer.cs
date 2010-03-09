﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DDay.iCal.Serialization.iCalendar
{
    public class DaySpecifierSerializer :
        SerializerBase
    {
        public override Type TargetType
        {
            get { return typeof(DaySpecifier); }
        }

        public override string SerializeToString(object obj)
        {
            IDaySpecifier ds = obj as IDaySpecifier;
            if (ds != null)
            {
                string value = string.Empty;
                if (ds.Num != int.MinValue)
                    value += ds.Num;
                value += Enum.GetName(typeof(DayOfWeek), ds.DayOfWeek).ToUpper().Substring(0, 2);
                return value;
            }
            return null;
        }

        public override object Deserialize(System.IO.TextReader tr)
        {
            string value = tr.ReadToEnd();

            Match match = Regex.Match(value, @"(\+|-)?(\d{1,2})?(\w{2})");
            if (match.Success)
            {
                IDaySpecifier ds = new DaySpecifier();
                if (match.Groups[2].Success)
                {
                    ds.Num = Convert.ToInt32(match.Groups[2].Value);
                    if (match.Groups[1].Success && match.Groups[1].Value.Contains("-"))
                        ds.Num *= -1;
                }
                ds.DayOfWeek = RecurrencePatternSerializer.GetDayOfWeek(match.Groups[3].Value);
                return ds;
            }
            return null;
        }
    }
}
