using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Popups;
using FacebookClient.Models;
using FacebookClient.Services.Abstraction;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace FinalTaskFacebook.ViewModels
{
    public class FriendsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IAccount _iAccount;
        public RelayCommand GoBackToStartPageCommand { get; private set; }
        public RelayCommand UpdateMusicCommand { get; private set; }
        private ObservableCollection<MusicGroup> _musicCollection;
        public double MusicProgress { get; set; }

        public ObservableCollection<MusicGroup> MusicCollection
        {
            get => _musicCollection;
            set
            {
                if (_musicCollection != value)
                    _musicCollection = value;
                RaisePropertyChanged("UpdateMusicCommand");
            }
        }
        public string ProgressPercent => string.Format("{0:#.#} %", MusicProgress);

        public FriendsPageViewModel(IAccount iAccount, INavigationService navigationService)
        {
            _iAccount = iAccount;
            _navigationService = navigationService;
            InitCommand();

            if (IsInDesignMode)
                return;

            ShowMusicFriendsAsync();
        }

        private void InitCommand()
        {
            GoBackToStartPageCommand = new RelayCommand(_navigationService.GoBack);
            UpdateMusicCommand = new RelayCommand(async () => await ShowMusicFriendsAsync(), () => MusicCollection != null);
        }

        private async Task ShowMusicFriendsAsync()
        {
            try
            {
                if (MusicCollection != null)
                    MusicCollection = null;

                var progress = new Progress<double>(pr => MusicProgress = pr);
                var musicGroups = await _iAccount.GetMusicFriendsGroupByPerformerAsync(progress);
                MusicCollection = new ObservableCollection<MusicGroup>(musicGroups);
            }
            catch (DivideByZeroException ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }

    }
}