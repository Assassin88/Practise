using System.Threading.Tasks;
using FinalTaskFacebook.Models;
using winsdkfb;

namespace FinalTaskFacebook.Services.Abstraction
{
    public interface ISocialNetwork
    {
        /// <summary>
        /// Authorizes the current user.
        /// </summary>
        /// <returns></returns>
        Task<FBResult> Authorize(string userId, params string[] permissions);

        /// <summary>
        /// Deletes the current user session.
        /// </summary>
        /// <returns></returns>
        Task ClearSession();

        /// <summary>
        /// Initializes the current account.
        /// </summary>
        /// <param name="fbResult">Received server response about the user.</param>
        /// <param name="endpoint">Received user data.</param>
        /// <param name="args">Advanced arguments about friends.</param>
        /// <returns></returns>
        Task<Account> GetAccountAsync(FBResult fbResult, string endpoint, string args = null);
    }
}
