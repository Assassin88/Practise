using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FacebookClient.Converters;
using FacebookClient.DTOModels;
using FacebookClient.Models;
using FacebookClient.Services.Abstraction;

namespace FacebookClient.Services.Implementation
{
    public class InitialAccount : IAccount, IDisposable
    {
        private readonly HttpClient _httpClient;
        private List<UserFriend> AccountFriends { get; set; }
        private const double Percent = 100;

        public InitialAccount()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://graph.facebook.com/v3.1/") };
        }

        public async Task<List<UserFriend>> GetAccountFriendsAsync(string accessToken, string endpoint, string args = null)
        {
            var accountFriends = await GetRemoteClientAsync<UserFriendsFb>(accessToken, endpoint, args);
            AccountFriends = accountFriends.Friends.Select(fb => Mapper.Map<UserFriend>(fb)).ToList();
            return AccountFriends;
        }

        private async Task<T> GetRemoteClientAsync<T>(string accessToken, string endpoint, string args = null)
        {
            var response = await _httpClient.GetAsync($"{endpoint}?access_token={accessToken}{args}");
            var v = response.StatusCode;
            response.EnsureSuccessStatusCode();
            return await Serializer.DeserializeObject<T>(response);
        }

        public IEnumerable<MusicGroup> GetMusicFriendsGroupByPerformer(IProgress<double> musicProgress)
        {
            var steep = Percent / AccountFriends.Select(x => x.MusicCollection.Count).Sum();
            var counter = 0.0;
            var musicFriends = new List<MusicFriends>();
            foreach (var friend in AccountFriends)
            {
                foreach (var item in friend.MusicCollection)
                {
                    musicFriends.Add(new MusicFriends() { Id = item.Id, Name = item.Name });
                    musicProgress.Report(counter += steep);
                    Thread.Sleep(2);
                }
            }

            return from obj in musicFriends group obj by new { obj.Name }
                   into gr orderby gr.Count() descending
                   select new MusicGroup(){ Name = gr.Key.Name, Count = gr.Count() };
        }

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _httpClient.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
