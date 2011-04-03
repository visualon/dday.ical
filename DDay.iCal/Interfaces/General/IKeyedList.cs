using System;
using System.Collections.Generic;
using System.Text;

namespace DDay.iCal
{
    public interface IKeyedList<TKey, TObject> :
        IEnumerable<TObject>
        where TObject : class, IKeyedObject<TKey>
    {
        /// <summary>
        /// Fired after an item is added to the collection.
        /// </summary>
        event EventHandler<ObjectEventArgs<TObject>> ItemAdded;

        /// <summary>
        /// Fired after an item is removed from the collection.
        /// </summary>
        event EventHandler<ObjectEventArgs<TObject>> ItemRemoved;

        /// <summary>
        /// Adds a keyed item to the collection.
        /// </summary>
        void Add(TObject item);

        /// <summary>
        /// Inserts a keyed item into the collection at the given index.
        /// </summary>
        void Insert(int index, TObject item);

        /// <summary>
        /// Removes a keyed item from the collection.
        /// <returns>True if the object was removed, false otherwise.</returns>
        /// </summary>
        bool Remove(TObject item);

        /// <summary>
        /// Removes all items with the matching key from the collection.
        /// </summary>        
        /// <returns>True if the object was removed, false otherwise.</returns>
        bool Remove(TKey key);

        /// <summary>
        /// Returns the index of the given object within the list
        /// matching the object's key, or -1 if no match could be found.
        /// </summary>
        int IndexOf(TObject item);

        /// <summary>
        /// Clears all items matching the specified key.
        /// </summary>
        void Clear(TKey key);

        /// <summary>
        /// Clears the entire list.
        /// </summary>
        void Clear();
        
        /// <summary>
        /// Returns true if the list contains at least one 
        /// object with a matching key, false otherwise.
        /// </summary>
        bool ContainsKey(TKey key);

        /// <summary>
        /// Returns the number of objects in the list
        /// with a matching key.
        /// </summary>
        int CountOf(TKey key);

        /// <summary>
        /// Returns all values contained in the list.
        /// </summary>
        IEnumerable<TObject> Values();

        /// <summary>
        /// Returns a list of objects that
        /// match the specified key.
        /// </summary>
        IEnumerable<TObject> AllOf(TKey key);

        /// <summary>
        /// Gets/sets an object with the matching key to
        /// the provided value.  When setting the value,
        /// if another object with a matching key exists,
        /// it will be overwritten.  If overwriting is
        /// not desired, use the Add() method instead.
        /// </summary>
        TObject this[TKey key] { get; set; }

        /// <summary>
        /// Converts the list to an array of the values contained therein.
        /// </summary>
        TObject[] ToArray();        
    }    
}
