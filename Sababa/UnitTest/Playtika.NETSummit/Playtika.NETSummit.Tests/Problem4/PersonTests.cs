using NUnit.Framework;
using Playtika.NETSummit.Problem4;

namespace Playtika.NETSummit.Tests.Problem4
{
    // Problem 4.
    // Tests are correct. CUT class should be fixed.
    [TestOf(typeof(Person))]
    public class PersonTests
    {
        [Test]
        public void Deconstruct_Calls_FieldsAreCorrect()
        {
            var person = new Person("Jeffrey", "Richter");

            var (firstName, lastName) = person;

            Assert.That(firstName, Is.EqualTo("Jeffrey"));
            Assert.That(lastName, Is.EqualTo("Richter"));
        }
    }
}