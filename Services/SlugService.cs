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

            if (IsUniqueSlug(output)) return output;

            return null;
            
        }

        public bool IsUniqueSlug(string slug)
        {
            return !_dbContext.Posts.Any(s => s.Slug == slug);
        }
    }

}
