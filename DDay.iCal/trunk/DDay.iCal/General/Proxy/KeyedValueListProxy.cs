using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections;

namespace DDay.iCal
{
    /// <summary>
    /// A proxy for a keyed list.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    public class KeyedValueListProxy<TKey, TObject, TValueType> :
        KeyedListProxy<TKey, TObject>,
        IKeyedValueList<TKey, TObject, TValueType>
        where TObject : class, IKeyedObject<TKey>, IValueObject<TValueType>
    {
        #region Private Fields

        IKeyedValueList<TKey, TObject, TValueType> _RealObject;

        #endregion

        #region Constructors

        public KeyedValueListProxy(IKeyedValueList<TKey, TObject, TValueType> realObject) :
            base(realObject)
        {
            _RealObject = realObject;
        }

        #endregion

        #region Overrides

        public override void SetProxiedObject(IKeyedList<TKey, TObject> realObject)
        {
            base.SetProxiedObject(realObject);

            if (realObject is IKeyedValueList<TKey, TObject, TValueType>)
                _RealObject = (IKeyedValueList<TKey, TObject, TValueType>)realObject;
        }

        #endregion

        #region IKeyedValueList<TKey,TObject,TValueType> Members

        virtual public void Set(TKey name, TValueType value)
        {
            _RealObject.Set(name, value);
        }

        virtual public void Set(TKey name, IEnumerable<TValueType> values)
        {
            _RealObject.Set(name, values);
        }

        virtual public TType Get<TType>(TKey name) where TType : TValueType
        {
            return _RealObject.Get<TType>(name);
        }

        virtual public IList<TType> GetMany<TType>(TKey name) where TType : TValueType
        {
            return _RealObject.GetMany<TType>(name);
        }

        #endregion
    }
}
