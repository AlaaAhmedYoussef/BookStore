﻿using System;
using System.Collections.Generic;
using System.IO;
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
    public class BookController : Controller
    {
        private readonly IBookStoreRepository<Book> bookRepository;
        private readonly IBookStoreRepository<Author> authorRepository;
        private readonly IHostingEnvironment hosting;

        public BookController(IBookStoreRepository<Book> _bookRepository,
            IBookStoreRepository<Author> _authorRepository,
            IHostingEnvironment _hosting)
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
                string fileName = string.Empty;

                if (vModel.File != null)
                {
                    string uploads = Path.Combine(hosting.WebRootPath, "uploads");
                    fileName = vModel.File.FileName;
                    string fullPath = Path.Combine(uploads, fileName);

                    string oldFullPath = string.Empty;
                    if (vModel.ImageUrl != null)
                    {
                        //Get the old file
                        string oldFilename = vModel.ImageUrl;
                        oldFullPath = Path.Combine(uploads, oldFilename);
                        //Delete the old file
                        System.IO.File.Delete(oldFullPath);
                    }
                  
                    // if the have the same path or name dont add it
                    if (oldFullPath != fullPath)
                    {
                        //Save the new file
                        vModel.File.CopyTo(new FileStream(fullPath, FileMode.Create));
                    }
                }
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
            catch(Exception ex)
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

        List<Author> FillSelectList()
        {
            var authors = authorRepository.list().ToList();
            authors.Insert(0, new Author { Id = -1, FullName = "--- Please select an Author ---" });

            return authors;
        }

        BookAuthorViewModel GetAllAuthors() { 
            var vModel = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };
            return vModel;
        }

        string UploadFile(IFormFile file)
        {
            if (file != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, "uploads");
                string fullPath = Path.Combine(uploads, file.FileName);
                file.CopyTo(new FileStream(fullPath, FileMode.Create));
                return file.FileName;
            }
            return null;
        }
       
}
}