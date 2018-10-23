using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FacebookClient.Models;

namespace FacebookClient.Services.Abstraction
{
    public interface IAccount
    {
        /// <summary>
        /// Initializes the current account.
        /// </summary>
        /// <param name="endpoint">Received user data.</param>
        /// <param name="args">Advanced arguments about friends.</param>
        /// <returns></returns>
        //Task<List<UserFriend>> GetAccountFriendsAsync(string endpoint, string args = null);
        Task<List<UserFriend>> GetAccountFriendsAsync(string accessToken, string endpoint, string args = null);

        /// <summary>
        /// return user friends music group by performer.
        /// </summary>
        /// <param name="musicProgress">Returns the percentage of music downloaded.</param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IEnumerable<MusicGroup>> GetMusicFriendsGroupByPerformerAsync(IProgress<double> musicProgress, CancellationToken token);
    }
}