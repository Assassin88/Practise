using System.Collections.Generic;
using System.Threading.Tasks;

namespace FriendFacadeSummit
{
    public interface IFriendFacade
    {
        /// <summary>
        /// Provide specified count of friends from index (paging)
        /// </summary>
        /// <param name="start">Start index</param>
        /// <param name="count">Friends count</param>
        /// <returns></returns>
        Task<IEnumerable<Friend>> Get(int start, int count);
    }
}
