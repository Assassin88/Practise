using System.Collections.Generic;

namespace FacebookClient.Models
{
    public class UserFriend
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string UriPicture { get; set; }

        public List<MusicFriends> MusicCollection { get; set; }

        public string AlbumNumber => $"AlbumNumber: {MusicCollection.Count.ToString()}";

        public override string ToString() => $"ID: {Id}, Name: {Name}, UriPicture: {UriPicture}.";
    }
}