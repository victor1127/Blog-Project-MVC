using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using BlogProjectMVC.Data;

namespace BlogProjectMVC.Services
{
    public class SlugService : ISlugService
    {
        private readonly ApplicationDbContext _dbContext;
        public SlugService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public string RemoveAccents(string tittle)
        {
            if (string.IsNullOrWhiteSpace(tittle)) return tittle;

            return new string(tittle.Normalize(NormalizationForm.FormD)
                                    .ToCharArray()
                                    .Where(c => CharUnicodeInfo.GetUnicodeCategory(c)!= UnicodeCategory.NonSpacingMark).ToArray())
                                    .Normalize(NormalizationForm.FormC);
        }

        public string GenerateSlug(string tittle)
        {
            string output = RemoveAccents(tittle);
            // Remove all special characters from the string.  
            output = Regex.Replace(output, @"[^A-Za-z0-9\s-]", "");
            // Remove all additional spaces in favour of just one.  
            output = Regex.Replace(output, @"\s+", " ").Trim();
            // Replace all spaces with the hyphen.  
            output = Regex.Replace(output, @"\s", "-");

            return output.ToLower(); 
        }

        public bool IsUniqueSlug(string slug, int? postId)
        {
            return !_dbContext.Posts.Any(p => p.Slug == slug && p.Id!=postId);
        }

        public string ConfirmSlug(string slug, int? postId)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return "This tittle resulted in an empty slug. Be creative and think of a good one.";
            }

            if (!IsUniqueSlug(slug, postId))
            {
                return "This tittle already exist, so a unique SLUG could not be created.";
            }

            return "";

        }

    }

}
