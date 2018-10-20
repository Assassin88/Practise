using System.Threading.Tasks;
using FacebookClient.Models;

namespace FacebookClient.Services.Abstraction
{
    public interface ISocialNetwork
    {
        /// <summary>
        /// Authorizes the current user.
        /// </summary>
        /// <returns></returns>
        Task<Account> AuthorizeAsync(string userId, params string[] permissions);

        /// <summary>
        /// Deletes the current user session.
        /// </summary>
        /// <returns></returns>
        Task LogoutAsync();

        /// <summary>
        /// Get access token current session.
        /// </summary>
        /// <returns></returns>
        string Token { get; }
    }
}