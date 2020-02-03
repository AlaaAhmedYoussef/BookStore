using BookStore.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class BookAuthorViewModel
    {
        public int BookId { get; set; }

        [Required]
        [StringLength(maximumLength: 25, MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [StringLength(maximumLength: 120, MinimumLength = 5)]
        public string Description { get; set; }

        public int AuthorId { get; set; }

        public List<Author> Authors { get; set; }

        //for all files here will be used for images only
        public IFormFile File { get; set; }
        public string ImageUrl { get; set; }
    }
}
