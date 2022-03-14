using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProjectMVC.Models
{
    public class BlogUser:IdentityUser
    {

        [Display(Name ="First name")]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Display(Name ="Author")]
        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }

        [DataType(DataType.Url)]
        public string GitHubUrl { get; set; }
        [DataType(DataType.Url)]
        public string TwetterUrl { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageType { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; } = new HashSet<Blog>();
        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();

    }
}
