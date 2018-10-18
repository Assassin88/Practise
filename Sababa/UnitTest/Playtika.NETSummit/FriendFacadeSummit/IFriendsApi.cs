using System.Collections.Generic;
using System.Threading.Tasks;

namespace FriendFacadeSummit
{
    public interface IFriendsApi
    {
        /// <summary>
        /// Provide specified count of friends from index (paging)
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Friend>> GetFriends(int start, int count);

        /// <summary>
        /// Returns the number of items in the friends collection.
        /// </summary>
        /// <returns></returns>
        Task<int> Count();
    }
}