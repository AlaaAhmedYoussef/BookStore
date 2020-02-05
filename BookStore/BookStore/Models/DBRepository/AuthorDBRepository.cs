using BookStore.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Respositories
{
    public class AuthorDBRepository : IBookStoreRepository<Author>
    {
        BookStoreDbContext db;

        public AuthorDBRepository(BookStoreDbContext _db)
        {
            db = _db;
        }
        public void Add(Author entity)
        {
            db.Authors.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var author = Find(id);

            db.Authors.Remove(author);
            db.SaveChanges();
        }

        public Author Find(int id)
        {
            var author = db.Authors.SingleOrDefault(a => a.Id == id);

            return author;
        }

        public bool IsNotShared(string imgUrl)
        {
            throw new NotImplementedException();
        }

        public IList<Author> list()
        {
            return db.Authors.ToList();
        }

        public List<Book> Search(string term)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, Author newAuthor)
        {            
            db.Update(newAuthor);
            db.SaveChanges();
        }

        List<Author> IBookStoreRepository<Author>.Search(string term)
        {
            throw new NotImplementedException();
        }
    }
}
