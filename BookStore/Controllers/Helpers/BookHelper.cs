using BookStore.Models;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
                file.CopyTo(new FileStream(fullPath, FileMode.Create));
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

                    // if (System.IO.File.Exists(oldPath) && IsNotShared(imageUrl)) -------later development

                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }

                }

                // if the have the same path or name dont add it
                if (oldPath != newPath)
                {
                    //Save the new file
                    file.CopyTo(new FileStream(newPath, FileMode.Create));
                }

                return file.FileName;
            }

            return imageUrl;
        }

        /* search database for imageUrl if count > 1  ? false : true 
        bool IsNotShared(string imageUrl)
        {
            // 
            if (imageUrl )
            return true;
        }
        */
    }
}
