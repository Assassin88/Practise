using System;
using FinalTaskFacebook.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Popups;

namespace FinalTaskFacebook.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
        private readonly IFacebookSocialNetwork _socialNetwork;
        private Account _account = new EmptyAccount();
        public RelayCommand ClearSession { get; private set; }
        public RelayCommand Authorization { get; private set; }
        private UserFriend _selectedFriend;

        public UserFriend SelectedFriend
        {
            get => _selectedFriend;
            set
            {
                if (value != _selectedFriend)
                {
                    _selectedFriend = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Account Account
        {
            get => _account;
            set
            {
                if (value != _account)
                {
                    _account = value;
                    RaisePropertyChanged();
                }
            }
        }

        public StartPageViewModel(IFacebookSocialNetwork socialNetwork)
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
                var resultAuthorize = await _socialNetwork.Authorize();
                Account = await _socialNetwork.GetAccountAsync(resultAuthorize);
            }
            catch (ArgumentException)
            {
                await new MessageDialog("To enter the application you need to log in.").ShowAsync();
            }
        }

        private void InitializeCommand()
        {
            ClearSession = new RelayCommand(Clear);
            Authorization = new RelayCommand(Registration);
        }

        private async void Registration()
        {
            if (Account is EmptyAccount)
                InitializeAccount();
            else
                await new MessageDialog("You are already authorized !!!").ShowAsync();
        }

        private void Clear()
        {
            _socialNetwork.ClearSession();
            Account = new EmptyAccount();
        }

    }
}
