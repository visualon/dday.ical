using System;
using System.Collections.Generic;
using System.Text;

namespace DDay.Collections
{
    public interface IKeyedList<TKey, TObject> :
        ICollection<TObject>
        where TObject : class, IKeyedObject<TKey>
    {
        /// <summary>
        /// Fired after an item is added to the collection.
        /// </summary>
        event EventHandler<ObjectEventArgs<TObject, int>> ItemAdded;

        /// <summary>
        /// Fired after an item is removed from the collection.
        /// </summary>
        event EventHandler<ObjectEventArgs<TObject, int>> ItemRemoved;

        /// <summary>
        /// Removes all items with the matching key from the collection.
        /// </summary>        
        /// <returns>True if the object was removed, false otherwise.</returns>
        bool Remove(TKey key);

        /// <summary>
        /// Clears all items matching the specified key.
        /// </summary>
        void Clear(TKey key);
        
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
        /// Returns a list of objects that
        /// match the specified key.
        /// </summary>
        IEnumerable<TObject> AllOf(TKey key);

        /// <summary>
        /// Returns the index of the given item
        /// within the list, or -1 if the item
        /// is not found in the list.
        /// </summary>
        int IndexOf(TObject obj);

        /// <summary>
        /// Gets the object at the specified index.
        /// </summary>
        TObject this[int index] { get; }

        /// <summary>
        /// Sort the keys of the list.
        /// </summary>
        void SortKeys(IComparer<TKey> comparer = null);
    }
}
