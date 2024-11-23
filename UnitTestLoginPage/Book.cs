using System;

namespace BookStoreLIB
{
    public class Book
    {
        public string ISBN { get; set; }
        public int CategoryID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public int? SupplierId { get; set; }
        public string Year { get; set; }
        public string Edition { get; set; }
        public string Publisher { get; set; }
        public int Stock { get; set; }  
        public DateTime? RestockDate { get; set; }

        private DALBook dalBook; 

        public Book()
        {
            dalBook = new DALBook();
        }

        public bool AddNewBook()
        {
            // Validate data
            if (string.IsNullOrEmpty(ISBN) || ISBN.Length != 10)
            {
                throw new ArgumentException("ISBN must be 10 characters.");
            }
            if (CategoryID <= 0)
            {
                throw new ArgumentException("CategoryID is required and must be positive.");
            }
            if (string.IsNullOrEmpty(Title))
            {
                throw new ArgumentException("Title cannot be empty.");
            }
            if (Price < 0)
            {
                throw new ArgumentException("Price cannot be negative.");
            }
            if (Stock < 0)
            {
                throw new ArgumentException("InStock cannot be negative.");
            }

            return dalBook.AddBook(this);
        }

    }
}
