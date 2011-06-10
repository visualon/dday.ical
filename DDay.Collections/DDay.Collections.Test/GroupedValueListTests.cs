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
        IGroupedValueList<string, Property, string> _Properties;
        
        [SetUp]
        public void Setup()
        {
            _Properties = new GroupedValueList<string, Property, string>();
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
    }
}
