using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace FriendFacadeSummit
{
    public class FriendFacade : IFriendFacade
    {
        private readonly IFriendsApi _friendsApi;
        private readonly ILogger _logger;
        private readonly MemoryCache _cache = MemoryCache.Default;
        private string _keyFriends = "keyFriend:";

        public FriendFacade(IFriendsApi friendsApi, ILogger logger)
        {
            _friendsApi = friendsApi;
            _logger = logger;
        }

        public async Task<IEnumerable<Friend>> Get(int start, int count)
        {
            VerifyValues(start, count);

            List<Friend> friends;
            var amount = start + count;

            if (ContainsKeys(start, count))
            {
                friends = new List<Friend>();
                for (int i = start; i < amount; i++)
                {
                    friends.Add(_cache.Get($"{_keyFriends}{i}") as Friend);
                }

                _logger.Log("Returns values from MemoryCache");
                return friends;
            }
            else
            {
                friends = new List<Friend>(await _friendsApi.GetFriends(start, count));
                for (int i = 0; i < count; i++)
                {
                    _cache.Set($"{_keyFriends}{i + start}", friends[i], null);
                }

                _logger.Log("Returns values in FriendsApi");
                return friends;
            }
        }

        private bool ContainsKeys(int start, int count)
        {
            var amount = start + count;
            for (int i = start; i < amount; i++)
            {
                if (!_cache.Contains($"{_keyFriends}{i}"))
                    return false;
            }
            return true;
        }

        private void VerifyValues(int start, int count)
        {
            if (start < 0)
            {
                _logger.Log("The index goes out the border of the collection.");
                throw new IndexOutOfRangeException("The index goes out the border of the collection.");
            }
            if (count < 1)
            {
                _logger.Log("Input parameter had not valid value.");
                throw new ArgumentException("Input parameter had not valid value.");
            }
        }

    }
}
