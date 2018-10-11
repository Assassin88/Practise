using System;
using System.Net.Http;
using FinalTaskFacebook.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Popups;
using FinalTaskFacebook.Services.Abstraction;

namespace FinalTaskFacebook.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
        private readonly ISocialNetwork _socialNetwork;
        private Account _account;
        public RelayCommand ClearSession { get; private set; }
        public RelayCommand Authorization { get; private set; }
        private UserFriend _selectedFriend;

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
        }

        private async void Registration()
        {
            if (Account is null)
                InitializeAccount();
            else
                await new MessageDialog("You are already authorized !!!").ShowAsync();
        }

        private void Clear()
        {
            _socialNetwork.ClearSession();
            Account = null;
        }

    }
}