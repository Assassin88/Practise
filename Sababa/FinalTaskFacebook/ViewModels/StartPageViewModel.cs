using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using FacebookClient.Models;
using FacebookClient.Services.Abstraction;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Popups;

namespace FinalTaskFacebook.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
        private readonly ISocialNetwork _socialNetwork;
        private readonly IAccount _iAccount;
        private Account _account;
        public double MusicProgress { get; set; }
        private UserFriend _selectedFriend;
        private ObservableCollection<MusicGroup> _musicCollection;
        private readonly string _userId = "936346953231113";
        private readonly string _endPoint = "me/friends";
        private readonly string[] _permitions = { "public_profile", "email", "user_friends", "user_likes" };
        private readonly string _args = "&fields=id,name,picture{url},music{id,name}";

        public RelayCommand ClearSession { get; private set; }
        public RelayCommand Authorization { get; private set; }
        public RelayCommand MusicCommand { get; private set; }

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

        public StartPageViewModel(ISocialNetwork socialNetwork, IAccount iAccount)
        {
            _socialNetwork = socialNetwork;
            _iAccount = iAccount;
            if (IsInDesignMode)
                return;
            InitializeAccount();
            InitializeCommand();
        }

        private async Task InitializeAccount()
        {
            try
            {
                Account = await _socialNetwork.AuthorizeAsync(_userId, _permitions);
                Account.AccountFriends = await _iAccount.GetAccountFriendsAsync(_socialNetwork.GetToken(), _endPoint, _args);
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
                await InitializeAccount();
            else
                await new MessageDialog("You are already authorized !!!").ShowAsync();
        }

        private void Clear()
        {
            _socialNetwork.LogoutAsync();
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

            Account.AccountFriends = await _iAccount.GetAccountFriendsAsync(_socialNetwork.GetToken(), _endPoint, _args);
            var progress = new Progress<double>(pr => MusicProgress = pr);
            var musicGroups = await Task.Run(() => _iAccount.GetMusicFriendsGroupByPerformer(progress));
            MusicCollection = new ObservableCollection<MusicGroup>(musicGroups);
        }
    }
}