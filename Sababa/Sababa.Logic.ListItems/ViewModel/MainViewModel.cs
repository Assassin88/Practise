using System.Collections.ObjectModel;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Sababa.Logic.ListItems.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        private Phone selectedPhone;
        public Phone SelectedPhone
        {
            get
            {
                if (selectedPhone == null)
                    selectedPhone = new Phone();
                return selectedPhone;
            }
            set
            {
                selectedPhone = value;
                RaisePropertyChanged(nameof(SelectedPhone));
            }
        }

        public MainViewModel()
        {
            Phones = new ObservableCollection<Phone>();
            AddCommand = new RelayCommand(AddItem);
        }

        public ObservableCollection<Phone> Phones { get; set; }

        public RelayCommand AddCommand { get; private set; }

        private void AddItem()
        {
            if (Phones.Count >= 5) Phones.Clear();

            Phone phone = new Phone() { Title = SelectedPhone.Title, Company = SelectedPhone.Company, Price = SelectedPhone.Price };

            Phones.Add(phone);
            SelectedPhone = phone;
        }


    }
}