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
    public class KeyedValueListProxy<TKey, TOriginal, TNew, TValueType> :
        KeyedListProxy<TKey, TOriginal, TNew>,
        IKeyedValueList<TKey, TNew, TValueType>
        where TOriginal : class, IKeyedObject<TKey>, IValueObject<TValueType>
        where TNew : class, TOriginal
    {
        #region Private Fields

        IKeyedValueList<TKey, TOriginal, TValueType> _RealObject;

        #endregion

        #region Constructors

        public KeyedValueListProxy(IKeyedValueList<TKey, TOriginal, TValueType> realObject, Predicate<TNew> predicate) :
            base(realObject, predicate)
        {
            _RealObject = realObject;
        }

        #endregion

        #region Overrides

        public override void SetProxiedObject(IKeyedList<TKey, TOriginal> realObject)
        {
            base.SetProxiedObject(realObject);

            if (realObject is IKeyedValueList<TKey, TOriginal, TValueType>)
                _RealObject = (IKeyedValueList<TKey, TOriginal, TValueType>)realObject;
        }

        #endregion

        #region IKeyedValueList<TKey,TObject,TNew,TValueType> Members

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
