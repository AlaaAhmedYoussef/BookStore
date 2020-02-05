using BookStore.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Respositories
{
    public class BookDBRepository : IBookStoreRepository<Book>
    {
        private readonly BookStoreDbContext db;

        public BookDBRepository(BookStoreDbContext _db)
        {
            db = _db;
        }

        public void Add(Book entity)
        {
            db.Books.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var book = Find(id);

            db.Books.Remove(book);
            db.SaveChanges();
        }

        public Book Find(int id)
        {
            var book = db.Books.Include(a => a.Author).SingleOrDefault(b => b.Id == id);

            return book;
        }

        public IList<Book> list()
        {
            return db.Books.Include(a => a.Author).ToList();
        }

        public void Update(int id, Book newBook)
        {
            db.Books.Update(newBook);
            db.SaveChanges();
        }

        public List<Book> Search(string term)
        {
            var result = db.Books.Include(a => a.Author).Where(b => 
            b.Title.Contains(term) ||
            b.Description.Contains(term) ||
            b.Author.FullName.Contains(term)
            );

            return result.ToList();
        }

        public bool IsNotShared(string imgUrl)
        {
            var count = db.Books.Count(b => b.ImageUrl == imgUrl);

            return (count > 1 ? false : true);
        }
    }
}
