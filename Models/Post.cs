using BlogProjectMVC.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogProjectMVC.Models
{
    public class Post
    {
        public int Id { get; set; }
       
        [Display(Name ="Blog Id")]
        public int BlogId { get; set; }
        public string AuthorId { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1}", MinimumLength = 2)]
        public string Title { get; set; }

        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at most {1}", MinimumLength = 2)]
        public string Abstract { get; set; }

        [Required]
        public string Content { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Created date")]
        public DateTime Created { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Updated date")]
        public DateTime? Updated { get; set; }
        public PostStates State { get; set; }
        public string Slug { get; set; }
        [Display(Name ="Cover")]
        public byte[] ImageData { get; set; }
        public string ImageType { get; set; } 

        [Display(Name = "Cover")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        //Nav properties
        public virtual Blog Blog { get; set; }
        public virtual BlogUser Author { get; set; }
        public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();



    }

}
