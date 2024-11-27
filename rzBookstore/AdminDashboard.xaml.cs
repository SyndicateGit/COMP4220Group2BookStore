﻿using System;
using System.Collections.Generic;
using System.Windows;
using BookStoreLIB;

namespace BookStoreGUI
{
    public partial class AdminDashboard : Window
    {
        private UserData userData;
        private List<Book> books;
        private DALCategory dalCategory = new DALCategory();
        private DALSupplier dalSupplier = new DALSupplier();

        public AdminDashboard()
        {
            InitializeComponent();
            userData = new UserData();
            books = new List<Book>();
            LoadCategories();
            LoadSuppliers();
        }

        // Load categories into ComboBox
        private void LoadCategories()
        {
            try
            {
                var categories = dalCategory.GetCategories();
                categoryComboBox.ItemsSource = categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load categories: " + ex.Message);
            }
        }

        private void LoadSuppliers()
        {
            try
            {
                var suppliers = dalSupplier.GetSuppliers();
                supplierComboBox.ItemsSource = suppliers;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load suppliers: " + ex.Message);
            }
        }

        // Search User
        private void SearchUser_Click(object sender, RoutedEventArgs e)
        {
            string query = searchUserTextBox.Text;
            
            // To be implemented
        }

        // Update User
        private void UpdateUser_Click(object sender, RoutedEventArgs e)
        {

            // To be implemented
        }

        // Ban User
        private void BanUser_Click(object sender, RoutedEventArgs e)
        {
            // To be implemented
        }

        // Search Book
        private void SearchBook_Click(object sender, RoutedEventArgs e)
        {
            string searchValue = searchBookTextBox.Text;

            if (string.IsNullOrWhiteSpace(searchValue))
            {
                MessageBox.Show("Please enter an ISBN or Title to search.");
                return;
            }

            try
            {
                var dalBook = new DALBook();
                Book foundBook = dalBook.GetBookByISBNOrTitle(searchValue);

                if (foundBook != null)
                {
                    updateISBNTextBox.Text = foundBook.ISBN;
                    updateTitleTextBox.Text = foundBook.Title;
                    updateAuthorTextBox.Text = foundBook.Author;
                    updatePriceTextBox.Text = foundBook.Price.ToString("F2");
                    updateStockTextBox.Text = foundBook.Stock.ToString();
                }
                else
                {
                    MessageBox.Show("No book found with the given ISBN or Title.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while searching for the book: " + ex.Message);
            }
        }


        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            // Initialize a new book and set properties from UI input
            var newBook = new Book
            {
                ISBN = isbnTextBox.Text,
                CategoryID = (int)categoryComboBox.SelectedValue,
                Title = titleTextBox.Text,
                Author = authorTextBox.Text,
                Price = decimal.TryParse(priceTextBox.Text, out var price) ? price : 0,
                SupplierId = (int?)supplierComboBox.SelectedValue,
                Year = yearTextBox.Text,
                Edition = editionTextBox.Text,
                Publisher = publisherTextBox.Text,
                Stock = int.TryParse(stockTextBox.Text, out var stock) ? stock : 0,
                RestockDate = null
            };

            // Attempt to add the book using the business logic in Book.cs
            try
            {
                bool isAdded = newBook.AddNewBook();
                if (isAdded)
                {
                    MessageBox.Show("Book added successfully!");
                }
                else
                {
                    MessageBox.Show("Failed to add the book.");
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message);
            }
        }


        // Update Book
        private void UpdateBook_Click(object sender, RoutedEventArgs e)
        {
            // To be implemented
        }
    }
}
