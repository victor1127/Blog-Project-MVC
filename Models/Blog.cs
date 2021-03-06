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
    public class Blog
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }

        [StringLength(100,ErrorMessage ="The {0} must be at least {2} and at most {1}", MinimumLength =2)]
        
        [Required]
        public string Title { get; set; }
        
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} and at most {1}", MinimumLength = 2)]
        [Required]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name ="Created")]
        public DateTime Created { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Updated")]
        public DateTime? Updated { get; set; }

        [Display(Name = "Image")]
        public byte[] ImageData { get; set; }
        public string ImageType { get; set; }

        [Display(Name = "Image")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        //Nav properties
        public virtual BlogUser Author { get; set; }
        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();



    }

}
