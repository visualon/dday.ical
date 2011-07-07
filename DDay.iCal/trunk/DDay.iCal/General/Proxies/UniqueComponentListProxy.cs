using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDay.Collections;

namespace DDay.iCal
{
    public class UniqueComponentListProxy<TComponentType> :
        CalendarObjectListProxy<TComponentType>,
        IUniqueComponentList<TComponentType>
        where TComponentType : class, IUniqueComponent
    {
        Dictionary<string, TComponentType> _Lookup;

        #region Constructors

        public UniqueComponentListProxy(IGroupedCollection<string, ICalendarObject> children) : base(children)
        {
            _Lookup = new Dictionary<string, TComponentType>();

            children.ItemAdded += new EventHandler<ObjectEventArgs<ICalendarObject, int>>(children_ItemAdded);
            children.ItemRemoved += new EventHandler<ObjectEventArgs<ICalendarObject,int>>(children_ItemRemoved);
        }
        
        #endregion

        #region Private Methods

        TComponentType Search(string uid)
        {
            if (_Lookup.ContainsKey(uid))
            {
                return _Lookup[uid];
            }

            var item = this
                .OfType<TComponentType>()
                .Where(c => string.Equals(c.UID, uid))
                .FirstOrDefault();

            if (item != null)
            {
                _Lookup[uid] = item;
                return item;
            }
            return default(TComponentType);
        }

        #endregion

        #region UniqueComponentListProxy Members

        virtual public TComponentType this[string uid]
        {
            get
            {
                return Search(uid);
            }
            set
            {
                // Find the item matching the UID
                var item = Search(uid);
                
                if (item != null)
                {
                    if (_Lookup.ContainsKey(uid))
                        _Lookup.Remove(uid);

                    // Remove it if found                    
                    Remove(item);
                }
                                
                if (value != null)
                {
                    // If we're setting another value, then
                    // we need to assign the UID to the where
                    // we just placed this component.
                    value.UID = uid;
                    Add(value);

                    _Lookup[uid] = value;
                }
            }
        }

        #endregion

        #region Event Handlers

        void children_ItemAdded(object sender, ObjectEventArgs<ICalendarObject, int> e)
        {
            if (e.First is TComponentType)
            {
                TComponentType component = (TComponentType)e.First;
                if (!string.IsNullOrEmpty(component.UID))
                    _Lookup[component.UID] = component;
            }
        }

        void children_ItemRemoved(object sender, ObjectEventArgs<ICalendarObject, int> e)
        {
            if (e.First is TComponentType)
            {
                TComponentType component = (TComponentType)e.First;
                if (!string.IsNullOrEmpty(component.UID) &&
                    _Lookup.ContainsKey(component.UID))
                {
                    _Lookup.Remove(component.UID);
                }
            }   
        }

        #endregion
    }
}
