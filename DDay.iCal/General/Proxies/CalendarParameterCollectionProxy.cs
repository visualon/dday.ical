using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDay.Collections;

namespace DDay.iCal
{
    public class CalendarParameterCollectionProxy :
        GroupedValueList<string, ICalendarParameter, CalendarParameter, string>
        ICalendarParameterCollectionProxy
    {
        //public CalendarParameterCollectionProxy(IGroupedList<string, ICalendarParameter> realObject) :
        //    base(realObject)
        //{
        //}

        //virtual public void SetParent(ICalendarObject parent)
        //{
        //    foreach (ICalendarParameter parameter in this)
        //    {
        //        parameter.Parent = parent;
        //    }
        //}

        //virtual public string Get(string name)
        //{
        //    var parameter = RealObject
        //        .AllOf(name)
        //        .FirstOrDefault();

        //    if (parameter != null)
        //        return parameter.Value;
        //    return null;
        //}

        //virtual public void Set(string name, string value)
        //{
        //    var parameter = RealObject
        //        .AllOf(name)
        //        .FirstOrDefault();

        //    if (parameter == null)
        //    {
        //        RealObject.Add(new CalendarParameter(name, value));
        //    }
        //    else
        //    {
        //        parameter.SetValue(value);
        //    }
        //}
        
        //virtual public void Set(string name, IEnumerable<string> values)
        //{
        //    var parameter = RealObject
        //        .AllOf(name)
        //        .FirstOrDefault();

        //    if (parameter == null)
        //    {
        //        RealObject.Add(new CalendarParameter(name, values));
        //    }
        //    else
        //    {
        //        parameter.SetValue(values);
        //    }
        //}

        //virtual public TType Get<TType>(string group)
        //{
        //    return Get(group) as TType;
        //}

        //virtual public IList<TType> GetMany<TType>(string group)
        //{
        //    return new GroupedValueListProxy<string, ICalendarParameter, CalendarParameter, string, TType>(RealObject, group);
        //}

        //virtual public new System.Collections.IEnumerator GetEnumerator()
        //{
        //    return RealObject.GetEnumerator();
        //}
        
        //virtual public int IndexOf(ICalendarParameter obj)
        //{
        //    return 
        //    throw new NotImplementedException();
        //}

        //public ICalendarParameter this[int index]
        //{
        //    get { throw new NotImplementedException(); }
        //}


        //public void Insert(int index, ICalendarParameter item)
        //{
        //    throw new NotImplementedException();
        //}

        //public void RemoveAt(int index)
        //{
        //    throw new NotImplementedException();
        //}

        //public ICalendarParameter this[int index]
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //    set
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
    }
}
