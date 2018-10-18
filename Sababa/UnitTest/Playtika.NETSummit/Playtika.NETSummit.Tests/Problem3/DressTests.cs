using System.Collections.Generic;
using NUnit.Framework;
using Playtika.NETSummit.Problem3;

namespace Playtika.NETSummit.Tests.Problem3
{
    // Problem 3.
    // Tests are correct. CUT class should be fixed.
    [TestOf(typeof(Dress))]
    public class DressTests
    {
        [Test]
        public void Ctor_WhenDressIsAddedToHashSet_HashSetContainsDress()
        {
            var hashSet = new HashSet<Dress>();
            var dress = new Dress(48, 180, 20);
            hashSet.Add(dress);

            Assert.That(hashSet.Contains(dress), Is.True);
        }

        [Test]
        public void Ctor_WhenDressIsAddedToHashSet_HashSetContainsNewDressWithSameParameters()
        {
            var hashSet = new HashSet<Dress>();
            var dress = new Dress(48, 180, 20);
            hashSet.Add(dress);

            Assert.That(hashSet.Contains(new Dress(48, 180, 20)), Is.True);
        }
    }
}