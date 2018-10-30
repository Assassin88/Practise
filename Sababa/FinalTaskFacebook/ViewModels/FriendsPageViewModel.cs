using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
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
        private readonly IFriends _iFriends;
        private ObservableCollection<MusicGroup> _musicCollection;
        private CancellationTokenSource _tokenSource;
        public double MusicProgress { get; private set; }
        public string ProgressPercent => string.Format("{0:#.#} %", MusicProgress);

        public RelayCommand GoBackToStartPageCommand { get; private set; }
        public RelayCommand UpdateMusicCommand { get; private set; }
        public RelayCommand CancelTokenCommand { get; private set; }
        public ObservableCollection<MusicGroup> MusicCollection
        {
            get => _musicCollection;
            set
            {
                if (_musicCollection != value)
                    _musicCollection = value;
                RaisePropertyChanged("UpdateMusicCommand");
                RaisePropertyChanged("CancelTokenCommand");
            }
        }

        public FriendsPageViewModel(IFriends iFriends, INavigationService navigationService)
        {
            _iFriends = iFriends;
            _navigationService = navigationService;
            InitCommand();

            if (IsInDesignMode)
                return;

            ShowMusicFriendsAsync();
        }

        private void InitCommand()
        {
            GoBackToStartPageCommand = new RelayCommand(GoBack);
            UpdateMusicCommand = new RelayCommand(async () => await ShowMusicFriendsAsync(), () => MusicCollection != null);
            CancelTokenCommand = new RelayCommand(CancelToken, () => MusicCollection == null);
        }

        private void GoBack()
        {
            _navigationService.GoBack();
            CancelToken();
        }

        private void CancelToken()
        {
            _tokenSource?.Cancel();
        }

        private async Task ShowMusicFriendsAsync()
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                MusicCollection = null;

                var progress = new Progress<double>(pr => MusicProgress = pr);
                var musicGroups = await _iFriends.GetMusicFriendsGroupByPerformerAsync(progress, _tokenSource.Token);

                if (musicGroups.Count() == 0)
                {
                    await new MessageDialog("Your friends have not music performers.").ShowAsync();
                    return;
                }
                MusicCollection = new ObservableCollection<MusicGroup>(musicGroups);
            }
            finally
            {
                _tokenSource.Dispose();
                _tokenSource = null;
            }
        }

    }
}