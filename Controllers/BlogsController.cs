using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogProjectMVC.Data;
using BlogProjectMVC.Models;
using BlogProjectMVC.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using X.PagedList;
using Microsoft.Extensions.Options;
using BlogProjectMVC.ViewModels;

namespace BlogProjectMVC.Controllers
{
    public class BlogsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly UserManager<BlogUser> _userManager;
        private readonly PageListSettings _pageListSettings;
        public BlogsController(ApplicationDbContext context,
                               IImageService imageService, 
                               UserManager<BlogUser> userManager,
                               IOptions<PageListSettings> pageListSettings)
        {
            _context = context;
            _imageService = imageService;
            _userManager = userManager;
            _pageListSettings = pageListSettings.Value;
        }

        // GET: Blogs
        public async Task<IActionResult> Index(int? page)
        {
            int pageNumber = page ?? _pageListSettings.PageNumber;
            int pageSize = _pageListSettings.PageSize;

            var blogs = _context.Blogs
                        .Where(b=>b.Posts.Any(p=>p.Status==Enums.PostStates.Ready))
                        .Include(b => b.Author)
                        .OrderByDescending(b=>b.Created)
                        .ToPagedListAsync(pageNumber, pageSize);
                                               
            return View(await blogs);
        }

        // GET: Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Blogs/Create
        [Authorize]
        public IActionResult Create()
        {
            //ViewData["AuthorId"] = new SelectList(_context.BlogUsers, "Id", "Id");
            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,ImageFile")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                blog.Created = DateTime.Now;
                blog.AuthorId = _userManager.GetUserId(User);
                blog.ImageData = await _imageService.ConvertFileToByteArray(blog.ImageFile);
                blog.ImageType = blog.ImageFile?.ContentType;

                _context.Add(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["AuthorId"] = new SelectList(_context.BlogUsers, "Id", "Id", blog.AuthorId);
            return View(blog);
        }

        // GET: Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            //ViewData["AuthorId"] = new SelectList(_context.BlogUsers, "Id", "Id", blog.AuthorId);
            return View(blog);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,ImageFile")] Blog blog)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var newBlog = await _context.Blogs.FindAsync(blog.Id);
                    newBlog.Title = blog.Title;
                    newBlog.Description = blog.Description;
                    newBlog.Updated = DateTime.Now;

                    if (blog.ImageFile != null)
                    {
                        newBlog.ImageData = await _imageService.ConvertFileToByteArray(blog.ImageFile);
                        newBlog.ImageType = blog.ImageFile.ContentType;
                    }

                    _context.Update(newBlog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.BlogUsers, "Id", "Id", blog.AuthorId);
            return View(blog);
        }

        public async Task<IActionResult> SearchIndex(int? page, string searchTerm)
        {
            var pageNumber = page ?? _pageListSettings.PageNumber;
            var pageSize = _pageListSettings.PageSize;

            var blogs = _context.Blogs
                                   .Where(b => b.Posts.Any(p => p.Status == Enums.PostStates.Ready))
                                   .Include(b => b.Author)
                                   .OrderByDescending(b=>b.Created)
                                   .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.ToLower();
                ViewData["SearchIndex"] = term;

                blogs = blogs.Where(b => b.Title.ToLower().Contains(term) ||
                                    b.Description.ToLower().Contains(term) ||
                                    b.Author.FirstName.ToLower().Contains(term) ||
                                    b.Author.LastName.ToLower().Contains(term)
                                    );                              
            }
   
            return View(await blogs.ToPagedListAsync(pageNumber, pageSize));
        }
        // GET: Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.Id == id);
        }
    }
}
