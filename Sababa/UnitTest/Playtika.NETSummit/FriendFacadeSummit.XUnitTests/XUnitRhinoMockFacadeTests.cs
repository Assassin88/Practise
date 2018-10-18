using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMock;
using Rhino.Mocks;
using Xunit;

namespace FriendFacadeSummit.XUnitTests
{
    public class XUnitRhinoMockFacadeTests
    {
        public RhinoAutoMocker<FriendFacade> Mocker { get; set; }

        public XUnitRhinoMockFacadeTests()
        {
            Mocker = new RhinoAutoMocker<FriendFacade>();
        }

        [Fact]
        public async Task CheckCallMethodGetFriends_GetInstanceFriendFacade_VerifyCalls()
        {
            Mocker.Get<IFriendsApi>()
                .Expect(x => x.GetFriends(10, 2))
                .Return(Task.FromResult((IEnumerable<Friend>)new List<Friend>()
                {
                    new Friend("keyFriend:1", 20),
                    new Friend("keyFriend:2", 25),
                }));

            var facade = Mocker.ClassUnderTest;
            var friends = await facade.Get(10, 2);

            Mocker.Get<IFriendsApi>().AssertWasCalled(x => x.GetFriends(10, 2), options => options.Repeat.Once());
        }

        [Fact]
        public async Task CheckCallMethodGetLog_GetMockIFriendsApi_VerifyCalls()
        {
            Mocker.Get<IFriendsApi>()
                .Expect(x => x.GetFriends(2, 2))
                .Return(Task.FromResult((IEnumerable<Friend>)new List<Friend>()
                {
                    new Friend("keyFriend:1", 20),
                    new Friend("keyFriend:2", 25),
                }));

            var friendFacade = Mocker.ClassUnderTest;
            var friends = await friendFacade.Get(2, 2);

            Mocker.Get<ILogger>().AssertWasCalled(x => x.Log("Returns values in FriendsApi"), options => options.Repeat.Once());
        }

        [Fact]
        public async Task SaveCollectionFriendsIntoTheCache_CallsTwiceMethodGet_CheckLogAngEqualsCollection()
        {
            Mocker.Get<IFriendsApi>()
                .Expect(x => x.GetFriends(5, 3))
                .Return(Task.FromResult((IEnumerable<Friend>)new List<Friend>()
                {
                    new Friend("keyFriend:4", 20),
                    new Friend("keyFriend:5", 25),
                    new Friend("keyFriend:6", 27),
                }));

            var friendFacade = Mocker.ClassUnderTest;
            var friends = await friendFacade.Get(5, 3);
            var friendsFromCache = await friendFacade.Get(5, 3);

            Mocker.Get<ILogger>().AssertWasCalled(x => x.Log("Returns values from MemoryCache"), options => options.Repeat.Once());
            Assert.Equal(friends, friendsFromCache);
        }

        [Fact]
        public async Task CheckThrowException_SetInvalidValues_VerifyThrowIndexOutOfRangeException()
        {
            var friendFacade = Mocker.ClassUnderTest;

            await Assert.ThrowsAsync<IndexOutOfRangeException>((async () => await friendFacade.Get(-1, 0)));
            Mocker.Get<ILogger>().AssertWasCalled(x => x.Log("The index goes out the border of the collection."), options => options.Repeat.Once());
        }

        [Fact]
        public async Task CheckThrowException_SetInvalidValues_VerifyThrowArgumentException()
        {
            var friendFacade = Mocker.ClassUnderTest;

            await Assert.ThrowsAsync<ArgumentException>(async () => await friendFacade.Get(0, 0));
            Mocker.Get<ILogger>().AssertWasCalled(x => x.Log("Input parameter had not valid value."), options => options.Repeat.Once());
        }

    }
}
