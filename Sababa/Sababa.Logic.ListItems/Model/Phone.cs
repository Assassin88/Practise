using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sababa.Logic.ListItems
{
    public class Phone : INotifyPropertyChanged
    {
        private string title;
        private string company;
        private int price;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        public string Company
        {
            get { return company; }
            set
            {
                company = value;
                OnPropertyChanged();
            }
        }

        public int Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public override string ToString()
        {
            return $"Title: {title}, Comp: {company}, Price: {price}";
        }
    }
}
