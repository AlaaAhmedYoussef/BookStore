using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.Models.Respositories;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookStoreRepository<Book> bookRepository;
        private readonly IBookStoreRepository<Author> authorRepository;

        public BookController(IBookStoreRepository<Book> bookRepository, IBookStoreRepository<Author> authorRepository)
        {
            this.bookRepository = bookRepository;
            this.authorRepository = authorRepository;
        }

        // GET: Book
        public ActionResult Index()
        {
            var books = bookRepository.list();

            return View(books);
        }

        // GET: Book/Details/5
        public ActionResult Details(int id)
        {
            var book = bookRepository.Find(id);

            return View(book);
        }

        // GET: Book/Create
        public ActionResult Create()
        {
            var vModel = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };
            return View(vModel);
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel vModel)
        {
            try
            {
                if (vModel.AuthorId < 0)
                {
                    ViewBag.Message = "Please select an Author from the list";

                    var vm = new BookAuthorViewModel
                    {
                        Authors = FillSelectList()
                    };
                    return View(vm);
                }
                var author = authorRepository.Find(vModel.AuthorId);
                var book = new Book
                {
                    Id = vModel.BookId,
                    Title = vModel.Title,
                    Description = vModel.Description,
                    Author = author
                };
                bookRepository.Add(book);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/Edit/5
        public ActionResult Edit(int id)
        {
            var book = bookRepository.Find(id);
            var authorId = book.Author == null ? book.Author.Id = 0 : book.Author.Id;
            var vModel = new BookAuthorViewModel
            {
                BookId = book.Id,
                Title = book.Title,
                Description = book.Description,
                AuthorId = authorId,
                Authors = authorRepository.list().ToList()
            };

            return View(vModel);
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, BookAuthorViewModel vModel)
        {
            try
            {
                var author = authorRepository.Find(vModel.AuthorId);
                var book = new Book
                {
                    Title = vModel.Title,
                    Description = vModel.Description,
                    Author = author
                };
                bookRepository.Update(id, book);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Book/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        List<Author> FillSelectList()
        {
            var authors = authorRepository.list().ToList();
            authors.Insert(0, new Author { Id = -1, FullName = "--- Please select an Author ---" });

            return authors;
        }
    }
}