using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace DDay.Collections.Test
{
    [TestFixture]
    public class KeyedListTests
    {
        IKeyedList<long, Person> _People;
        IKeyedList<long, Doctor> _Doctors;
        Person _JonSchmidt;
        Person _BobRoss;
        Person _ForrestGump;
        Person _MichaelJackson;
        Person _DoogieHowser;

        [SetUp]
        public void Setup()
        {
            _JonSchmidt = new Person() { Key = 1, Name = "Jon Schmidt" };
            _BobRoss = new Person() { Key = 2, Name = "Bob Ross" };
            _ForrestGump = new Doctor() { Key = 3, Name = "Forrest Gump", ProviderNumber = "123456" };
            _MichaelJackson = new Person() { Key = 4, Name = "Michael Jackson" };
            _DoogieHowser = new Doctor() { Key = 5, Name = "Doogie Howser", ProviderNumber = "234567" };

            _People = new KeyedList<long, Person>();

            _People.Add(_ForrestGump);
            _People.Add(_JonSchmidt);
            _People.Add(_BobRoss);
            _People.Add(_DoogieHowser);
            _People.Add(_MichaelJackson);

            _Doctors = new KeyedListProxy<long, Person, Doctor>(_People);
        }

        /// <summary>
        /// Ensures that the Add() correctly adds items when
        /// called from the original list.
        /// </summary>
        [Test]
        public void Add1()
        {
            var newDoctor = new Doctor() { Key = 5, Name = "New Doctor", ProviderNumber = "23456" };
            Assert.AreEqual(5, _People.Count);
            Assert.AreEqual(2, _Doctors.Count);
            _People.Add(newDoctor);
            Assert.AreEqual(6, _People.Count);
            Assert.AreEqual(3, _Doctors.Count);
        }

        /// <summary>
        /// Tests the basic operation of the AllOf() method.
        /// </summary>
        [Test]
        public void AllOf1()
        {
            Assert.AreEqual(1, _People.AllOf(1).Count());
            Assert.AreEqual(1, _People.AllOf(2).Count());
            Assert.AreEqual(1, _People.AllOf(3).Count());
            Assert.AreEqual(1, _People.AllOf(4).Count());
            Assert.AreEqual(1, _People.AllOf(5).Count());
        }

        /// <summary>
        /// Tests the AllOf() method after one of the 
        /// object's keys has changed.
        /// </summary>
        [Test]
        public void AllOf2()
        {
            Assert.AreEqual(1, _People.AllOf(4).Count());
            Assert.AreEqual(1, _People.AllOf(5).Count());
            _MichaelJackson.Key = 5;
            Assert.AreEqual(2, _People.AllOf(5).Count());
            Assert.AreEqual(0, _People.AllOf(4).Count());
        }

        /// <summary>
        /// Tests the basic function of the Count property.
        /// </summary>
        [Test]
        public void Count1()
        {
            Assert.AreEqual(5, _People.Count);
        }

        /// <summary>
        /// Ensures the Count property works as expected with proxied lists.
        /// </summary>
        [Test]
        public void CountProxy1()
        {
            Assert.AreEqual(2, _Doctors.Count);
        }

        /// <summary>
        /// Ensure that the KeyedList enumerator properly enumerates the items in the list.
        /// </summary>
        [Test]
        public void Enumeration1()
        {
            var people = new Person[] { _ForrestGump, _JonSchmidt, _BobRoss, _DoogieHowser, _MichaelJackson };
            int i = 0;
            foreach (var person in _People)
            {
                Assert.AreSame(people[i++], person);
            }
        }

        /// <summary>
        /// Ensures that the indexer properly retrieves the items at the specified index.
        /// </summary>
        [Test]
        public void Indexer1()
        {
            Assert.AreSame(_ForrestGump, _People[0]);
            Assert.AreSame(_JonSchmidt, _People[1]);
            Assert.AreSame(_BobRoss, _People[2]);
            Assert.AreSame(_DoogieHowser, _People[3]);
            Assert.AreSame(_MichaelJackson, _People[4]);
        }

        /// <summary>
        /// Ensures that the indexer works properly with proxies.
        /// </summary>
        [Test]
        public void IndexerProxy1()
        {
            Assert.AreSame(_ForrestGump, _Doctors[0]);
            Assert.AreSame(_DoogieHowser, _Doctors[1]);
        }

        /// <summary>
        /// Ensures the IndexOf() method works as expected.
        /// </summary>
        [Test]
        public void IndexOf1()
        {
            Assert.AreEqual(0, _People.IndexOf(_ForrestGump));
            Assert.AreEqual(1, _People.IndexOf(_JonSchmidt));
            Assert.AreEqual(2, _People.IndexOf(_BobRoss));
            Assert.AreEqual(3, _People.IndexOf(_DoogieHowser));
            Assert.AreEqual(4, _People.IndexOf(_MichaelJackson));
        }

        /// <summary>
        /// Ensures the IndexOf() method works as expected when using proxied lists.
        /// </summary>
        [Test]
        public void IndexOfProxy1()
        {
            Assert.AreEqual(0, _Doctors.IndexOf((Doctor)_ForrestGump));
            Assert.AreEqual(1, _Doctors.IndexOf((Doctor)_DoogieHowser));
        }

        /// <summary>
        /// Ensures items are properly removed 
        /// when calling Remove() from the original list.
        /// </summary>
        [Test]
        public void Remove1()
        {
            Assert.AreEqual(5, _People.Count);
            Assert.AreEqual(2, _Doctors.Count);
            _People.Remove(_DoogieHowser);
            Assert.AreEqual(4, _People.Count);
            Assert.AreEqual(1, _Doctors.Count);
        }

        /// <summary>
        /// Ensures items are properly removed 
        /// when calling Remove() from the proxied list.
        /// </summary>
        [Test]
        public void RemoveProxy1()
        {
            Assert.AreEqual(2, _Doctors.Count);
            Assert.AreEqual(5, _People.Count);
            _People.Remove(_DoogieHowser);
            Assert.AreEqual(1, _Doctors.Count);
            Assert.AreEqual(4, _People.Count);
        }

        /// <summary>
        /// Ensure items are presented in ascending order (by key)
        /// </summary>
        [Test]
        public void SortKeys1()
        {
            _People.SortKeys();
            
            long key = -1;
            foreach (var person in _People)
            {
                Assert.LessOrEqual(key, person.Key);
                key = person.Key;
            }
        }
    }
}
