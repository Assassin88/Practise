using NUnit.Framework;
using Playtika.NETSummit.Problem2;

namespace Playtika.NETSummit.Tests.Problem2
{
    // Problem 2.
    // Tests are correct. CUT class should be fixed.
    [TestOf(typeof(Utils))]
    public class UtilsTests
    {
        [TestCase(5, 120)]
        [TestCase(1, 1)]
        [TestCase(12, 479001600)]
        public void Foo1_Calls_ResultIsCorrect(int n, int expectedResult)
        {
            var result = Utils.Foo1(n);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase(3, 3, 3, true)]
        [TestCase(7, 5, 1, false)]
        [TestCase(98, 50, 50, true)]
        [TestCase(2, 2, 5, false)]
        public void Foo2_Calls_ResultIsCorrect(int a, int b, int c, bool expectedResult)
        {
            var result = Utils.Foo2(a, b, c);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase(1, true)]
        [TestCase(33, false)]
        [TestCase(2017, true)]
        [TestCase(4, false)]
        public void Foo3_Calls_ResultIsCorrect(int n, bool expectedResult)
        {
            var result = Utils.Foo3(n);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase(0, 1)]
        [TestCase(1, 1)]
        [TestCase(-10_000, 5)]
        [TestCase(749, 3)]
        public void Foo4_Calls_ResultIsCorrect(int n, int expectedResult)
        {
            var result = Utils.Foo4(n);

            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}