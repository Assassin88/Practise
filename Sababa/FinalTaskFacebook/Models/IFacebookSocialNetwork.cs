using System.Threading.Tasks;
using winsdkfb;

namespace FinalTaskFacebook.Models
{
    public interface IFacebookSocialNetwork
    {
        /// <summary>
        /// Authorizes the current user.
        /// </summary>
        /// <returns></returns>
        Task<FBResult> Authorize();

        /// <summary>
        /// Deletes the current user session.
        /// </summary>
        /// <returns></returns>
        Task ClearSession();

        /// <summary>
        /// Initializes the current account.
        /// </summary>
        /// <param name="fbResult">Received server response about the user.</param>
        /// <returns></returns>
        Task<Account> GetAccountAsync(FBResult fbResult);
    }
}
