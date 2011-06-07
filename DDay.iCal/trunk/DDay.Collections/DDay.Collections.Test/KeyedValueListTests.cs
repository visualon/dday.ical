using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace DDay.Collections.Test
{
    [TestFixture]
    public class KeyedValueListTests
    {
        IKeyedValueList<string, Property, string> _Properties;
        
        [SetUp]
        public void Setup()
        {
            _Properties = new KeyedValueList<string, Property, string>();
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
            Assert.AreEqual(2, categories.Count);
        }

        [Test]
        public void ItemAdded3()
        {
            int itemsAdded = 0;
            _Properties.ItemAdded += (s, e) => itemsAdded++;

            IList<string> categories = _Properties.GetMany<string>("CATEGORIES");
            Assert.AreEqual(0, itemsAdded);
            categories.Add("Work");
            Assert.AreEqual(1, itemsAdded);
            categories.Add("Personal");
            Assert.AreEqual(2, itemsAdded);
        }
    }
}
