using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace DDay.Collections.Test
{
    [TestFixture]
    public class GroupedValueListTests
    {
        GroupedValueList<string, Property, string> _Properties;

        [SetUp]
        public void Setup()
        {
            _Properties = new GroupedValueList<string, Property, string>();
        }

        private IList<string> AddCategories()
        {
            var categories = _Properties.GetMany<string>("CATEGORIES");
            categories.Add("Work");
            categories.Add("Personal");
            return categories;
        }

        [Test]
        public void ItemAdded1()
        {
            int itemsAdded = 0;
            _Properties.ItemAdded += (s, e) => itemsAdded++;

            Assert.AreEqual(0, itemsAdded);
            _Properties.Set("CATEGORIES", "Test");
            Assert.AreEqual(1, itemsAdded);
            Assert.AreEqual("Test", _Properties.Get<string>("CATEGORIES"));
        }

        [Test]
        public void ItemAdded2()
        {
            int itemsAdded = 0;
            _Properties.ItemAdded += (s, e) => itemsAdded++;
            var categories = _Properties.GetMany<string>("CATEGORIES");

            Assert.AreEqual(0, itemsAdded);
            _Properties.Set("CATEGORIES", new string[] { "Work", "Personal" });
            Assert.AreEqual(1, itemsAdded);

            var items = categories.ToArray();
            Assert.AreEqual(2, categories.Count);
        }

        [Test]
        public void ItemAdded3()
        {
            int itemsAdded = 0;
            _Properties.ItemAdded += (s, e) => itemsAdded++;

            // Get a collection value proxy
            ICollection<string> categories = _Properties.GetMany<string>("CATEGORIES");
            Assert.AreEqual(0, itemsAdded);
            
            // Add a work category
            categories.Add("Work");

            // Ensure a "CATEGORIES" item was added
            Assert.AreEqual(1, itemsAdded);

            // Ensure the "Work" value is accounted for
            Assert.AreEqual(1, categories.Count);
            Assert.AreEqual(1, _Properties.AllOf("CATEGORIES").Sum(o => o.ValueCount));

            // Add a personal category
            categories.Add("Personal");

            // Ensure only the original "CATEGORY" item was added
            Assert.AreEqual(1, itemsAdded);

            // Ensure the "Work" and "Personal" categories are accounted for
            Assert.AreEqual(2, categories.Count);
            Assert.AreEqual(2, _Properties.AllOf("CATEGORIES").Sum(o => o.ValueCount));
        }

        /// <summary>
        /// Ensures the Add() method works properly with GroupedValueListProxy.
        /// </summary>
        [Test]
        public void AddProxy1()
        {
            var proxy = _Properties.GetMany<string>("CATEGORIES");
            Assert.AreEqual(0, proxy.Count);
            proxy.Add("Work");
            Assert.AreEqual(1, proxy.Count);
            proxy.Add("Personal");
            Assert.AreEqual(2, proxy.Count);
            Assert.IsTrue(new string[] { "Work", "Personal" }.SequenceEqual(_Properties.AllOf("CATEGORIES").SelectMany(p => p.Values)));
        }

        [Test]
        public void ClearProxy1()
        {
            _Properties.Set("Test", "Test");

            // Set another property to ensure it isn't cleared when the categories are cleared
            Assert.AreEqual(1, _Properties.AllOf("Test").Count());
            Assert.AreEqual(1, _Properties.GetMany<string>("Test").Count);

            // Get a proxy for categories, and add items to it, ensuring
            // the items are added propertly afterward.
            var items = new string[] { "Work", "Personal" };
            var proxy = _Properties.GetMany<string>("CATEGORIES");

            foreach (var item in items)
                proxy.Add(item);
            Assert.IsTrue(items.SequenceEqual(proxy.ToArray()));

            proxy.Clear();
            Assert.AreEqual(0, proxy.Count);

            Assert.AreEqual(1, _Properties.AllOf("Test").Count());
            Assert.AreEqual(1, _Properties.GetMany<string>("Test").Count);
        }

        /// <summary>
        /// Ensures the Contains() method works properly with GroupedValueListProxy.
        /// </summary>
        [Test]
        public void ContainsProxy1()
        {
            var proxy = _Properties.GetMany<string>("CATEGORIES");
            Assert.IsFalse(proxy.Contains("Work"));
            proxy.Add("Work");
            Assert.IsTrue(proxy.Contains("Work"));
        }

        [Test]
        public void CopyToProxy1()
        {
            var categories = AddCategories();

            string[] values = new string[2];
            categories.CopyTo(values, 0);
            Assert.IsTrue(categories.ToArray().SequenceEqual(values));
        }

        [Test]
        public void CountProxy1()
        {
            var categories = AddCategories();
            Assert.AreEqual(2, categories.Count);
        }

        [Test]
        public void RemoveProxy1()
        {
            var categories = AddCategories();
            Assert.AreEqual(2, categories.Count);

            categories.Remove("Work");
            Assert.AreEqual(1, categories.Count);

            categories.Remove("Bla");
            Assert.AreEqual(1, categories.Count);

            categories.Remove(null);
            Assert.AreEqual(1, categories.Count);

            categories.Remove("Personal");
            Assert.AreEqual(0, categories.Count);
        }
    }
}
