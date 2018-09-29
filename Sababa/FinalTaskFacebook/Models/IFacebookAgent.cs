using System.Threading.Tasks;

namespace FinalTaskFacebook.Models
{
    public interface IFacebookAgent
    {
        /// <summary>
        /// Returns the serialized object of the remote user.
        /// </summary>
        /// <typeparam name="T">Parametrized type.</typeparam>
        /// <param name="accessToken">Current user token.</param>
        /// <param name="endpoint">Segments of the current request.</param>
        /// <param name="args">The arguments of the current request.</param>
        /// <returns></returns>
        Task<T> GetRemoteClientAsync<T>(string accessToken, string endpoint, string args = null);
    }
}
