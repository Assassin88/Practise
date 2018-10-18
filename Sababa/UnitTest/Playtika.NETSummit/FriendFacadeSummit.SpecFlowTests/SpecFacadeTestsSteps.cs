using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace FriendFacadeSummit.SpecFlowTests
{
    [Binding]
    public class SpecFacadeTestsSteps
    {
        protected AutoMocker Mocker { get; private set; }
        private FriendFacade _friendFacade;
        private int _start;
        private int _count;
        private IEnumerable<Friend> _friends;
        private IEnumerable<Friend> _friendsFromCache;

        [Given(@"Setup before test")]
        public void GivenSetupBeforeTest()
        {
            Mocker = new AutoMocker();
            _friendFacade = Mocker.CreateInstance<FriendFacade>();
        }

        [Given(@"I have entered (.*) into the facade")]
        public void GivenIHaveEnteredIntoTheFacade(int start)
        {
            this._start = start;
        }

        [Given(@"I have also entered (.*) into the facade")]
        public void GivenIHaveAlsoEnteredIntoTheFacade(int count)
        {
            this._count = count;
        }

        [Then(@"I setup Api GetFriends inter ""(.*)"" and ""(.*)"" values")]
        public void ThenISetupApiGetFriendsInterAndValues(string key1, string key2)
        {
            Mocker.GetMock<IFriendsApi>()
                .Setup(x => x.GetFriends(_start, _count))
                .Returns(() => Task.FromResult((IEnumerable<Friend>)new List<Friend>()
                {
                    new Friend($"{key1}", 20),
                    new Friend($"{key2}", 25),
                }));
        }

        [Then(@"I setup Api GetFriends with three keys inter ""(.*)"" and ""(.*)"" and ""(.*)"" values")]
        public void ThenISetupApiGetFriendsWithThreeKeysInterAndAndValues(string key1, string key2, string key3)
        {
            Mocker.GetMock<IFriendsApi>()
                .Setup(x => x.GetFriends(5, 3))
                .Returns(() => Task.FromResult((IEnumerable<Friend>)new List<Friend>()
                {
                    new Friend($"{key1}", 20),
                    new Friend($"{key2}", 25),
                    new Friend($"{key3}", 27)
                }));
        }

        [When(@"I call Get")]
        public async Task WhenICallGet()
        {
            _friends = await _friendFacade.Get(_start, _count);
        }

        [Then(@"I call Get again")]
        public async  Task ThenICallGetAgain()
        {
            _friendsFromCache = await _friendFacade.Get(_start, _count);
        }

        [Then(@"the result should be Logger Log called once and collection should be equals")]
        public void ThenTheResultShouldBeLoggerLogCalledOnceAndCollectionShouldBeEquals()
        {
            Mocker.GetMock<ILogger>().Verify(x => x.Log("Returns values from MemoryCache"), Times.Once);
            Assert.That(_friends, Is.EquivalentTo(_friendsFromCache));
        }

        [Then(@"the result should be Api Get called once")]
        public void ThenTheResultShouldBeApiGetCalledOnce()
        {
            Mocker.GetMock<IFriendsApi>().Verify(x => x.GetFriends(_start, _count), Times.Once);
        }

        [Then(@"the result should be Logger Log called once")]
        public void ThenTheResultShouldBeLoggerLogCalledOnce()
        {
            Mocker.GetMock<ILogger>().Verify(x => x.Log(It.IsAny<string>()), Times.Once);
        }

        [Then(@"I entered an invalid number in parameters Get and check throw IndexOutOfRangeException")]
        public void ThenIEnteredAnInvalidNumberInParametersGetAndCheckThrowIndexOutOfRangeException()
        {
            Assert.That(async () => await _friendFacade.Get(-1, 0), Throws.InstanceOf<IndexOutOfRangeException>());
            Mocker.GetMock<ILogger>().Verify(x => x.Log(It.IsAny<string>()), Times.Once);
        }

        [Then(@"I entered an invalid number in parameters Get and check throw ArgumentException")]
        public void ThenIEnteredAnInvalidNumberInParametersGetAndCheckThrowArgumentException()
        {
            Assert.That(async () => await _friendFacade.Get(0, 0), Throws.ArgumentException);
            Mocker.GetMock<ILogger>().Verify(x => x.Log(It.IsAny<string>()), Times.Once);
        }

    }
}
