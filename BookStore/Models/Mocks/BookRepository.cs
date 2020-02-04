using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Respositories
{
    public class BookRepository : IBookStoreRepository<Book>
    {
        List<Book> books;
        public BookRepository()
        {
            books = new List<Book>()
            {
                new Book
                {
                    Id = 1, Title = "C# Programming", 
                    Description = "No Description", 
                    ImageUrl = "apricots.jpg",
                    Author = new Author{ Id = 2 }
                },
                new Book
                {
                    Id = 2, Title = "Java Programming",
                    Description = "Nothing",
                    ImageUrl = "avocado.jpg",
                    Author = new Author()
                },
                new Book
                {
                    Id = 3, Title = "Python Programming",
                    Description = "No Data",
                    ImageUrl = "vegatables.jpg",
                    Author = new Author()
                },
            };
        }

        public void Add(Book entity)
        {
            entity.Id = books.Max(b => b.Id)+ 1;
            
            books.Add(entity);
        }

        public void Delete(int id)
        {
            var book = Find(id);

            books.Remove(book);
        }

        public Book Find(int id)
        {
            var book = books.SingleOrDefault(b => b.Id == id);

            return book;
        }

        public IList<Book> list()
        {
            return books;
        }

        public List<Book> Search(string term)
        {
            var result = books.Where(b =>
            b.Title.Contains(term) ||
            b.Description.Contains(term) ||
            b.Author.FullName.Contains(term)
            );
            
            return result.ToList();
        }

        public void Update(int id, Book newBook)
        {
            var book = Find(id);

            book.Title = newBook.Title;
            book.Description = newBook.Description;
            book.Author = newBook.Author;
            book.ImageUrl = newBook.ImageUrl;
        }
    }
}
