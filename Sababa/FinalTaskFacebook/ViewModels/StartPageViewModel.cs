using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FinalTaskFacebook.Models;
using FinalTaskFacebook.Services.Abstraction;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Popups;

namespace FinalTaskFacebook.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
        private readonly ISocialNetwork _socialNetwork;
        private Account _account;
        public RelayCommand ClearSession { get; private set; }
        public RelayCommand Authorization { get; private set; }
        public RelayCommand MusicCommand { get; private set; }
        private const double Percent = 100;

        public double MusicProgress { get; set; }

        private UserFriend _selectedFriend;
        private ObservableCollection<MusicGroup> _musicCollection;

        public ObservableCollection<MusicGroup> MusicCollection
        {
            get => _musicCollection;
            set
            {
                if (_musicCollection != value)
                    _musicCollection = value;
            }
        }

        public UserFriend SelectedFriend
        {
            get => _selectedFriend;
            set
            {
                if (_selectedFriend != value)
                {
                    _selectedFriend = value;
                }
            }
        }

        public Account Account
        {
            get => _account;
            set
            {
                if (_account != value)
                {
                    _account = value;
                }
            }
        }

        public StartPageViewModel(ISocialNetwork socialNetwork)
        {
            _socialNetwork = socialNetwork;
            if (IsInDesignMode)
                return;
            InitializeAccount();
            InitializeCommand();
        }

        private async void InitializeAccount()
        {

            try
            {
                var resultAuthorize = await _socialNetwork.Authorize("936346953231113", "public_profile", "email", "user_friends", "user_likes");
                Account = await _socialNetwork.GetAccountAsync(resultAuthorize, "me/friends", "&fields=id,name,picture{url},music{id,name}");

            }
            catch (ArgumentException)
            {
                await new MessageDialog("To enter the application you need to log in.").ShowAsync();
            }
            catch (HttpRequestException)
            {
                await new MessageDialog("Problems connecting to a remote server.").ShowAsync();
            }
        }

        private void InitializeCommand()
        {
            ClearSession = new RelayCommand(Clear);
            Authorization = new RelayCommand(Registration);
            MusicCommand = new RelayCommand(GetFriendsMusicCollection);
        }

        private async void Registration()
        {
            if (Account == null)
                InitializeAccount();
            else
                await new MessageDialog("You are already authorized !!!").ShowAsync();
        }

        private void Clear()
        {
            _socialNetwork.ClearSession();
            Account = null;
            MusicCollection = null;
            MusicProgress = 0;
        }

        private async void GetFriendsMusicCollection()
        {
            if (Account == null)
            {
                await new MessageDialog("To enter the application you need to log in.").ShowAsync();
                return;
            }

            if (MusicCollection != null)
            {
                MusicCollection.Clear();
                MusicProgress = 0;
            }

            var steep = Percent / Account.AccountFriends.Select(x => x.MusicCollection.Count).Sum();
            List<MusicFriends> musicFriends = new List<MusicFriends>();
            foreach (var friend in Account.AccountFriends)
            {
                foreach (var item in friend.MusicCollection)
                {
                    musicFriends.Add(new MusicFriends() { Id = item.Id, Name = item.Name });
                    MusicProgress += steep;
                    await Task.Delay(2);
                }
            }

            var sortCollection = from obj in musicFriends group obj by new { obj.Name }
                                 into gr orderby gr.Count() descending
                                 select new MusicGroup() { Name = gr.Key.Name, Count = gr.Count() };
            MusicCollection = new ObservableCollection<MusicGroup>(sortCollection);
        }

    }
}