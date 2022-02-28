using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProjectMVC.Models
{
    public class BlogUser:IdentityUser
    {
        [Required]
        [Display(Name ="First name")]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        public string FullName
        {
            get { return $"{FirstName }{ LastName}"; }
        }

        [DataType(DataType.Url)]
        public string GitHubUrl { get; set; }
        [DataType(DataType.Url)]
        public string TwetterUrl { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageType { get; set; }
        public IFormFile ImageFile { get; set; }


        public ICollection<Blog> Blogs { get; set; } = new HashSet<Blog>();
        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();

    }
}
