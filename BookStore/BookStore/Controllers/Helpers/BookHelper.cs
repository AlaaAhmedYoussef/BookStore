using BookStore.Models;
using BookStore.Models.Respositories;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BookStore.Controllers
{
    public partial class BookController
    {
        List<Author> FillSelectList()
        {
            var authors = authorRepository.list().ToList();
            authors.Insert(0, new Author { Id = -1, FullName = "--- Please select an Author ---" });

            return authors;
        }

        BookAuthorViewModel GetAllAuthors()
        {
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
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                return file.FileName;
            }
            return null;
        }

        string UploadFile(IFormFile file, string imageUrl)
        {
            if (file != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, "uploads");

                string newPath = Path.Combine(uploads, file.FileName);

                string oldPath = string.Empty;

                if (imageUrl != null)
                {
                    //Get the old path
                    oldPath = Path.Combine(uploads, imageUrl);
                    //Delete the old file

                    if (System.IO.File.Exists(oldPath) && bookRepository.IsNotShared(imageUrl))
                    {
                        System.IO.File.Delete(oldPath);
                    }

                }

                // if the have the same path or name dont add it
                if (oldPath != newPath)
                {
                    //Save the new file
                    using (var fileStream = new FileStream(newPath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    
                }

                return file.FileName;
            }

            return imageUrl;
        }
     
    }
}
