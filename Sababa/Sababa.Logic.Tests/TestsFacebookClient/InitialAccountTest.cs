using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FacebookClient.Models;
using FacebookClient.Services.Abstraction;
using FacebookClient.Services.Implementation;
using FacebookClient.Settings;
using NUnit.Framework;

namespace Sababa.Logic.Tests.TestsFacebookClient
{
    [TestOf(typeof(Friends))]
    public class InitialAccountTest
    {
        private CancellationTokenSource _tokenSource;
        private IFriends _iFriends;

        private string Token = "EAANTmkOIqwkBAL0GN0ope2CWr2ZCqDaGGnmTbzlpdxCDkb" +
                               "b0a3Lns0VuaKIJVDIshINofqkpcII1UkCm7IGlXi1VN8e8D" +
                               "G2EjQudGeZCSfwZC9UthvBnvakr4cRCDISR0rkvHJSUWK4W" +
                               "E7ORTNFw36eYpeFsKGN0jzkii1fZCwZDZD";

        private List<UserFriend> _collectionFriends;
        public double MusicProgress { get; set; }

        public InitialAccountTest()
        {
            _iFriends = new Friends();
            Mapper.Initialize(config => config.AddProfiles(typeof(Account)));
            _tokenSource = new CancellationTokenSource();
        }

        [Test]
        public async Task GetAccountFriends_GetListUserFriends_CheckResultIsNotNull()
        {
            _collectionFriends = await _iFriends.GetAccountFriendsAsync(Token, FbSettings.EndPoint, FbSettings.Args);

            Assert.That(_collectionFriends, Is.Not.Null);
        }

        [Test]
        public async Task GetMusicFriendsGroupByPerformer_GetListMusicGroup_CheckResultProgressLoadAndIsNotNull()
        {
            var progress = new Progress<double>(pr => MusicProgress = pr);
            _collectionFriends = await _iFriends.GetAccountFriendsAsync(Token, FbSettings.EndPoint, FbSettings.Args);
            IEnumerable<MusicGroup> musicGroups =
                await _iFriends.GetMusicFriendsGroupByPerformerAsync(progress, _tokenSource.Token);

            Assert.That(MusicProgress >= 100);
            Assert.That(musicGroups, Is.Not.Null);
        }
    }
}
