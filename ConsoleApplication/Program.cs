using System;
using ConsoleTables;

namespace ConsoleApplication
{
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
        public string[][] allbooks = {
                new string[]{"C# 8.0 and .NET Core 3.0", "Mark J. Price", "978-1788478120", "2019" },
                new string[]{"C# in Depth", "Jon Skeet", "978-1617294532", "2020"},
                new string[]{"A Tour of C++", "Stroustrup Bjarne", "978-0136816485", "2021"}
            };

        public Program()
        {
            // load books from a local array or from a database
            loadBooks();

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
            int index = 1;
            foreach (string[] books in allbooks)
            {
                cTable.AddRow(index, books[0], books[1], books[2], books[3]);
                index++;
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
            Console.Write("Title: ");
            string title = Console.ReadLine();
            Console.Write("Author: ");
            string author = Console.ReadLine();
            Console.Write("ISBN: ");
            string ISBN = Console.ReadLine();
            Console.Write("Published Year: ");
            string year = Console.ReadLine();

            cTable.AddRow(title, author, ISBN, year);
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
            cTable.Write();
            currentMenu = 1;
            choosenMenu = 1;
            Console.WriteLine();
        }
    }
}
