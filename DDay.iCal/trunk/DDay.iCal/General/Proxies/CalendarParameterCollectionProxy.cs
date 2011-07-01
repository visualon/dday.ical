using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDay.Collections;

namespace DDay.iCal
{
    public class CalendarParameterCollectionProxy :
        GroupedCollectionProxy<string, ICalendarParameter, ICalendarParameter>,
        ICalendarParameterCollectionProxy
    {
        public CalendarParameterCollectionProxy(IGroupedList<string, ICalendarParameter> realObject) :
            base(realObject)
        {
        }

        virtual public void SetParent(ICalendarObject parent)
        {
            foreach (ICalendarParameter parameter in this)
            {
                parameter.Parent = parent;
            }
        }

        virtual public string Get(string name)
        {
            var parameter = RealObject
                .AllOf(name)
                .FirstOrDefault();

            if (parameter != null)
                return parameter.Value;
            return default(string);
        }

        virtual public void Set(string name, string value)
        {
            var parameter = RealObject
                .AllOf(name)
                .FirstOrDefault();

            if (parameter == null)
            {
                RealObject.Add(new CalendarParameter(name, value));
            }
            else
            {
                parameter.SetValue(value);
            }
        }

        virtual public void Set(string name, IEnumerable<string> values)
        {
            var parameter = RealObject
                .AllOf(name)
                .FirstOrDefault();

            if (parameter == null)
            {
                RealObject.Add(new CalendarParameter(name, values));
            }
            else
            {
                parameter.SetValue(values);
            }
        }

        virtual public TType Get<TType>(string group)
        {
            var value = Get(group);
            if (value is TType)
                return (TType)(object)Get(group);
            return default(TType);
        }

        virtual public IList<TType> GetMany<TType>(string group)
        {
            throw new NotSupportedException();
        }

        virtual public new System.Collections.IEnumerator GetEnumerator()
        {
            return RealObject.GetEnumerator();
        }

        virtual public int IndexOf(ICalendarParameter obj)
        {
            throw new NotImplementedException();
        }

        virtual public void Insert(int index, ICalendarParameter item)
        {
            throw new NotImplementedException();
        }

        virtual public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        virtual public ICalendarParameter this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
