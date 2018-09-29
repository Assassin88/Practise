using System.Collections.Generic;
using System.ComponentModel;

namespace FinalTaskFacebook.Models
{
    public class Account : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Id { get; set; }
        public string Name { get; set; }
        public string UriPicture { get; set; }

        public List<UserFriend> AccountFriends { get; set; } = new List<UserFriend>();

        public override string ToString() => $"Id: {Id}, Name: {Name}, UriPicture: {UriPicture}.";

    }
}