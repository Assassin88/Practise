using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using FacebookClient.Models;
using FacebookClient.Services.Abstraction;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Popups;
using FacebookClient.Exceptions;
using FacebookClient.Settings;
using GalaSoft.MvvmLight.Views;

namespace FinalTaskFacebook.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
        private readonly ISocialNetwork _socialNetwork;
        private readonly IAccount _iAccount;
        private readonly INavigationService _navigationService;
        private Account _account;
        private UserFriend _selectedFriend;
        private ObservableCollection<MusicGroup> _musicCollection;

        public RelayCommand ClearSessionCommand { get; private set; }
        public RelayCommand LoginCommand { get; private set; }
        public RelayCommand ShowMusicFriendsCommand { get; private set; }

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
                    RaisePropertyChanged("ShowMusicFriendsCommand");
                    RaisePropertyChanged("LoginCommand");
                    RaisePropertyChanged("ClearSessionCommand");
                }
            }
        }

        public StartPageViewModel(ISocialNetwork socialNetwork, IAccount iAccount, INavigationService navigationService)
        {
            _socialNetwork = socialNetwork;
            _iAccount = iAccount;
            _navigationService = navigationService;
            if (IsInDesignMode)
                return;
            LoginAccountAsync();
            InitializeCommands();
        }

        private async Task LoginAccountAsync()
        {
            try
            {
                Account = await _socialNetwork.AuthorizeAsync(FbSettings.UserId, FbSettings.Permissions);
                Account.AccountFriends = await _iAccount.GetAccountFriendsAsync(_socialNetwork.Token, FbSettings.EndPoint, FbSettings.Args);
            }
            catch (FacebookResultException ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
            catch (HttpRequestException)
            {
                await new MessageDialog("Problems connecting to a remote server.").ShowAsync();
            }
            catch (HttpResponseException ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }

        private void InitializeCommands()
        {
            ClearSessionCommand = new RelayCommand(async () => await ClearAsync(), () => Account != null);
            LoginCommand = new RelayCommand(async () => await LoginAsync(), () => Account == null);
            ShowMusicFriendsCommand = new RelayCommand(ShowMusicFriends, () => Account != null);
        }

        private void ShowMusicFriends()
        {
            _navigationService.NavigateTo("FriendsPage");
        }

        private async Task LoginAsync()
        {
            if (Account == null)
                await LoginAccountAsync();
        }

        private async Task ClearAsync()
        {
            await _socialNetwork.LogoutAsync();
            Account = null;
            MusicCollection = null;
        }
    }
}