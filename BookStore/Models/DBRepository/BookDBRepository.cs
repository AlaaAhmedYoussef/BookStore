using BookStore.Models.Database;
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
            var book = db.Books.SingleOrDefault(b => b.Id == id);

            return book;
        }

        public IList<Book> list()
        {
            return db.Books.ToList();
        }

        public void Update(int id, Book newBook)
        {
            db.Books.Update(newBook);
            db.SaveChanges();
        }
    }
}
