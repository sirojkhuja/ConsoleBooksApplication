using System;
using System.Collections;
using ConsoleTables;
using MySql.Data.MySqlClient;

namespace ConsoleApplication
{
    // Main class
    internal class Program
    {
        static void Main(string[] args)
        {
            new Program().start();
        }

        public ConsoleTable cTable = null;
        public string[][] menus = null;
        public int currentMenu = 0;
        public int choosenMenu = 0;
        public bool exit = false;
        private BookService service;
        public string[][] allbooks = {
                new string[]{"C# 8.0 and .NET Core 3.0", "Mark J. Price", "978-1788478120", "2019" },
                new string[]{"C# in Depth", "Jon Skeet", "978-1617294532", "2020"},
                new string[]{"A Tour of C++", "Stroustrup Bjarne", "978-0136816485", "2021"}
            };

        public Program()
        {
            this.service = new BookService();

            // menus with method names
            menus = new string[][]{
                new string[] {"main"},
                new string[] {"exitApp", "books", "users"},
                new string[] {"back", "bookInfo", "findBook", "allBooks", "giveBook", "changeBook"},
                new string[] {"back", "addBook", "editBook", "deleteBook"}
            };
        }

        public void loadBooks()
        {
            // set table header data
            cTable = new ConsoleTable("#","Title", "Author", "ISBN", "Published Year");
            ArrayList allbooks = this.service.getAllBooks();
            foreach (Book book in allbooks)
            {
                cTable.AddRow(book.ID, book.Title, book.Author, book.ISBN, book.Year);
            }
        }

        public void start()
        {
            while (!exit)
                showMenu(menus[currentMenu][choosenMenu]);
        }

        public void showMenu(string menuName)
        {
            try
            {
                 typeof(Program).GetMethod(menuName).Invoke(this, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void main()
        {
            currentMenu = 1;
            string menus = "1. Books.\n"
                        + "2. Users.\n"
                        + "0. Exit.\n"
                        + "Please enter section number: ";
            Console.Write(menus);
            choosenMenu = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
        }

        public void books()
        {
            currentMenu = 2;
            string menus = "1. Info about a book.\n"
                        + "2. Find a book.\n"
                        + "3. Show all books.\n"
                        + "4. Give a book.\n"
                        + "5. Change a book.\n"
                        + "0. Back.\n"
                        + "Please enter section number: ";
            Console.Write(menus);
            choosenMenu = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
        }

        public void changeBook()
        {
            currentMenu = 3;
            string menus = "1. Add a new book.\n"
                        + "2. Change a book.\n"
                        + "3. Delete a book.\n"
                        + "0. Back.\n"
                        + "Please enter section number: ";
            Console.Write(menus);
            choosenMenu = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
        }

        public void addBook()
        {
            Book newBook = new Book();
            Console.Write("Title: ");
            newBook.Title = Console.ReadLine();
            Console.Write("Author: ");
            newBook.Author = Console.ReadLine();
            Console.Write("ISBN: ");
            newBook.ISBN = Console.ReadLine();
            Console.Write("Published Year: ");
            newBook.Year = Console.ReadLine();

            service.AddNewBook(newBook);
            currentMenu--;
            choosenMenu = 5;
            Console.WriteLine();
        }

        public void editBook()
        {

        }

        public void deleteBook()
        {

        }

        public void users()
        {
            currentMenu = 1;
            string menus = "1. Books.\n"
                        + "2. Users.\n"
                        + "3. Exit."
                        + "Please enter section number: ";
            Console.Write(menus);
            choosenMenu = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
        }

        public void exitApp()
        {
            exit = true;
        }

        public void back()
        {
            if (currentMenu == 2)
                choosenMenu = 0;

            if (currentMenu == 3)
                choosenMenu = 1;
            currentMenu -= 2;
        }

        public void allBooks()
        {
            loadBooks();
            cTable.Write();
            currentMenu = 1;
            choosenMenu = 1;
            Console.WriteLine();
        }
    }

   
    // Entity (DTO)
    internal class Book
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Year { get; set; }
        
        public Book(int ID, string title, string author, string isbn, string year)
        {
            this.ID = ID;
            this.Title = title;
            this.Author = author;
            this.ISBN = isbn;
            this.Year = year;
        }

        public Book(string title, string author, string isbn, string year)
        {
            this.Title = title;
            this.Author = author;
            this.ISBN = isbn;
            this.Year = year;
        }

        public Book(int ID)
        {
            this.ID = ID;
        }

        public Book() { }
    }

    // Service class (DAO)
    internal class BookService
    {
        DBConnection conn;
        public BookService()
        {
            conn = DBConnection.getConnection();
        }

        public ArrayList getAllBooks()
        { 
            return conn.SelectAllBooks();
        }

        public void AddNewBook(Book newBook)
        {
            conn.InsertBook(newBook);
        }

        public bool EditBook(Book editedBook)
        {

            return true;
        }

        public bool DeleteBook(Book bookToDelete)
        {

            return true;
        }
    }

    // Database connection class
    internal class DBConnection
    {
        private string DB_NAME;
        private string DB_USER;
        private string DB_PASS;
        private MySqlCommand cmd;
        private DBConnection()
        {
            loadConfigs();
            connect();
        }

        private void loadConfigs()
        {
            DotNetEnv.Env.TraversePath().Load();
            DB_NAME = DotNetEnv.Env.GetString("DBNAME");
            DB_USER = DotNetEnv.Env.GetString("DBUSER");
            DB_PASS = DotNetEnv.Env.GetString("DBPASS");
        }

        private void connect()
        {
            string connString = string.Format("server=localhost;userid={1};password={2};database={0}", DB_NAME, DB_USER, DB_PASS);
            var conn = new MySqlConnection(connString);
            conn.Open();

            cmd = new MySqlCommand();
            cmd.Connection = conn;
        }

        public ArrayList SelectAllBooks(string table = "tb_books")
        {
            ArrayList allBooks = new ArrayList();
            cmd.CommandText = string.Format("SELECT * FROM {0};", table);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                allBooks.Add(new Book(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4)));
            }
            rdr.Close();
            return allBooks;
        }

        public void InsertBook(Book newBook, string table = "tb_books")
        {
            cmd.CommandText = string.Format("INSERT INTO {0}(`title`,`author`,`isbn`,`year`) VALUES('{1}','{2}','{3}','{4}')", table, newBook.Title, newBook.Author, newBook.ISBN, newBook.Year);
            cmd.ExecuteNonQuery();
        }

        public static DBConnection getConnection()
        {
            return new DBConnection();
        }
    }
}
