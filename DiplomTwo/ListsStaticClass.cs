using DiplomTwo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomTwo
{
    public static class ListsStaticClass
    {
        public static List<Book> listAllBooks = new List<Book>();
        public static List<Genre> listAllGenres = new List<Genre>();
        public static List<User> listAllUsers = new List<User>();
        public static List<Reader> listAllReaders = new List<Reader>();
        public static List<Personallibrary> listAllPersonallLibrary = new List<Personallibrary>();
        public static List<Bookauthor> listAllBookAuthors = new List<Bookauthor>();
        public static List<Appauthor> listAllAppAuthors = new List<Appauthor>(); 
        public static List<Author> listAllAuthors = new List<Author>();
        public static List<Series> listAllSeries = new List<Series>();

        public static int currentAccount = -1;
    }
}
