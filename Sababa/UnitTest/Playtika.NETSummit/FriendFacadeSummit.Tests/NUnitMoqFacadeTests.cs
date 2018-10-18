using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;

namespace FriendFacadeSummit.NUnitTests
{
    [TestOf(typeof(FriendFacade))]
    public class NUnitMoqFacadeTests
    {
        protected AutoMocker Mocker { get; private set; }

        [SetUp]
        public void SetUp()
        {
            Mocker = new AutoMocker();
        }

        [Test]
        public async Task CheckCallMethodGetFriends_GetInstanceFriendFacade_VerifyCalls()
        {
            Mocker.GetMock<IFriendsApi>()
                .Setup(x => x.GetFriends(10, 2))
                .Returns(() => Task.FromResult((IEnumerable<Friend>)new List<Friend>()
                {
                    new Friend("keyFriend:9", 20),
                    new Friend("keyFriend:10", 25),
                }));

            var friendFacade = Mocker.CreateInstance<FriendFacade>();

            var friends = await friendFacade.Get(10, 2);

            Mocker.GetMock<IFriendsApi>().Verify(x => x.GetFriends(10, 2), Times.Once);
        }

        [Test]
        public async Task CheckCallMethodGetLog_GetMockIFriendsApi_VerifyCalls()
        {
            Mocker.GetMock<IFriendsApi>()
                .Setup(x => x.GetFriends(2, 2))
                .Returns(() => Task.FromResult((IEnumerable<Friend>) new List<Friend>()
                {
                    new Friend("keyFriend:1", 20),
                    new Friend("keyFriend:2", 25),
                }));

            var friendFacade = Mocker.CreateInstance<FriendFacade>();
            var friends = await friendFacade.Get(2, 2);

            Mocker.GetMock<ILogger>().Verify(x => x.Log(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task SaveCollectionFriendsIntoTheCache_CallsTwiceMethodGet_CheckLogAngEqualsCollection()
        {
            Mocker.GetMock<IFriendsApi>()
                .Setup(x => x.GetFriends(5, 3))
                .Returns(() => Task.FromResult((IEnumerable<Friend>)new List<Friend>()
                {
                    new Friend("keyFriend:4", 20),
                    new Friend("keyFriend:5", 25),
                    new Friend("keyFriend:6", 27)
                }));

            var friendFacade = Mocker.CreateInstance<FriendFacade>();
            var friends = await friendFacade.Get(5, 3);
            var friendsFromCache = await friendFacade.Get(5, 3);

            Mocker.GetMock<ILogger>().Verify(x => x.Log("Returns values from MemoryCache"), Times.Once);
            Assert.That(friends,Is.EquivalentTo(friendsFromCache));
        }

        [Test]
        public void CheckThrowException_SetInvalidValues_VerifyThrowIndexOutOfRangeException()
        {
            var friendFacade = Mocker.CreateInstance<FriendFacade>();

            Assert.That(async () => await friendFacade.Get(-1, 0), Throws.InstanceOf<IndexOutOfRangeException>());
            Mocker.GetMock<ILogger>().Verify(x => x.Log(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void CheckThrowException_SetInvalidValues_VerifyThrowArgumentException()
        {
            var friendFacade = Mocker.CreateInstance<FriendFacade>();

            Assert.That(async () => await friendFacade.Get(0, 0), Throws.ArgumentException);
            Mocker.GetMock<ILogger>().Verify(x => x.Log(It.IsAny<string>()), Times.Once);
        }

    }
}
