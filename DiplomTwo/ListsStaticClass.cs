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
        public static List<BookChapter> listAllBookChapter = new List<BookChapter>();
        public static List<BookGenre> listAllBookGenres = new List<BookGenre>();
        public static List<Comment> listAllComment = new List<Comment>();
        public static List<Prioritylevel> listAllPrioritylevel = new List<Prioritylevel>();
        public static List<Bookplan> listAllBookPlan = new List<Bookplan>();

        public static int currentAccount = -1;
    }
}
