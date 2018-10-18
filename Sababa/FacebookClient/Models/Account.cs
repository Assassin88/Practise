using System.Collections.Generic;
using System.ComponentModel;

namespace FacebookClient.Models
{
    public class Account : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Id { get; set; }
        public string Name { get; set; }
        public string UriPicture { get; set; }

        public List<UserFriend> AccountFriends { get; set; }

        public override string ToString() => $"Id: {Id}, Name: {Name}, UriPicture: {UriPicture}.";
    }
}
