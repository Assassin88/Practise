using System.ComponentModel;

namespace FinalTaskFacebook.Models
{
    public class UserFriend : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Id { get; set; }
        public string Name { get; set; }

        public string UriPicture { get; set; } 

        public override string ToString() => $"ID: {Id}, Name: {Name}, UriPicture: {UriPicture}.";

    }
}
