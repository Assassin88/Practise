using System;

namespace Sababa.logic.HttpClient.Tests
{
    [Serializable]
    public class Book
    {
        private int _id;
        private string _name;
        private double _price { get; set; }

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public Book()
        {

        }

        public Book(int id, string name, double price)
        {
            _id = id;
            _name = name;
            _price = price;
        }

        public override string ToString()
        {
            return string.Format($"Id: {_id}; Name: {_name}; Price: {_price}");
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Book book = obj as Book;
            if (book == null)
            {
                return false;
            }

            return book.ID == this.ID && book.Name == this.Name && book.Price.Equals(this.Price);
        }
    }
}
