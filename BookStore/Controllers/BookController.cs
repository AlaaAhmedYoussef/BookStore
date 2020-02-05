using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.Models.Respositories;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public partial class BookController : Controller
    {
        private readonly IBookStoreRepository<Book> bookRepository;
        private readonly IBookStoreRepository<Author> authorRepository;
        private readonly IWebHostEnvironment hosting;

        public BookController(IBookStoreRepository<Book> _bookRepository,
            IBookStoreRepository<Author> _authorRepository,
            IWebHostEnvironment _hosting)
        {
            this.bookRepository = _bookRepository;
            this.authorRepository = _authorRepository;
            hosting = _hosting;
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
            // if all validations are true(achieved)
            if (ModelState.IsValid)
            {
                try
                {
                    string fileName = UploadFile(vModel.File) ?? string.Empty;

                    if (vModel.AuthorId < 0)
                    {
                        ViewBag.Message = "Please select an Author from the list";

                        return View(GetAllAuthors());
                    }
                    var author = authorRepository.Find(vModel.AuthorId);
                    var book = new Book
                    {
                        Id = vModel.BookId,
                        Title = vModel.Title,
                        Description = vModel.Description,
                        ImageUrl = fileName,
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
            //ModelState will passed to validation-summary
            ModelState.AddModelError("", "You have to fill all the required fields");
            return View(GetAllAuthors());
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
                ImageUrl = book.ImageUrl,
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
                string fileName = UploadFile(vModel.File, vModel.ImageUrl);

                var author = authorRepository.Find(vModel.AuthorId);
                var book = new Book
                {
                    Id = id,
                    Title = vModel.Title,
                    Description = vModel.Description,
                    ImageUrl = fileName,
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
            var book = bookRepository.Find(id);

            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            try
            {
                bookRepository.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Search(string term)
        {
            var result = bookRepository.Search(term);

            return View("Index", result);
        }
    }
}