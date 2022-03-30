using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogProjectMVC.Data;
using BlogProjectMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BlogProjectMVC.Services;
using BlogProjectMVC.ViewModels;
using Microsoft.Extensions.Options;
using X.PagedList;

namespace BlogProjectMVC.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BlogUser> _userManager;
        private readonly ISlugService _slugService;
        private readonly IImageService _imageService;
        private readonly PageListSettings _pageListSettings;
        public PostsController(ApplicationDbContext context,
                               UserManager<BlogUser> userManager,
                               ISlugService slugservice,
                               IImageService imageService,
                               IOptions<PageListSettings> pageListSettings)
        {
            _context = context;
            _userManager = userManager;
            _slugService = slugservice;
            _imageService = imageService;
            _pageListSettings = pageListSettings.Value;
        }

        // GET: Posts
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page ?? _pageListSettings.PageNumber;
            var pageSize = _pageListSettings.PageSize;

            var posts = _context.Posts.Include(p => p.Author)
                                      .OrderByDescending(p => p.Created)
                                      .ToPagedListAsync(pageNumber, pageSize);

            return View(await posts);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Blog)
                .Include(p=>p.Tags)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        public async Task<IActionResult> PostsBlog(int? id, int? page)
        {
            ViewData["BlogPostsId"] = id;
            if (id == null) return NotFound();

            var pageNumber = page ?? _pageListSettings.PageNumber;

            var posts = _context.Posts.Where(p => p.BlogId == id)
                                      .Include(p => p.Author)
                                      .OrderByDescending(p=>p.Created)
                                      .ToPagedListAsync(pageNumber, _pageListSettings.PageSize);

            if (posts == null) return NotFound();
            return View(await posts);
        }
        public IActionResult PostsTags(string tagTittle)
        {
            if (string.IsNullOrWhiteSpace(tagTittle)) return NotFound();

            var tags = _context.Tags.Include(t => t.Author)
                                    .Include(t => t.Post)
                                    .Where(t => t.Title == tagTittle);
                                   
            if (!tags.Any()) return NotFound();

            var posts = new List<Post>();
            foreach(var tag in tags.ToList())
            {
                posts.Add(tag.Post);
            }

            return View("Index", posts);

        }

        // GET: Posts/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BlogId,Title,Abstract,Content,State,ImageFile")] Post post, List<string> tagValues)
        {
            if (ModelState.IsValid && post.BlogId != 0)
            {
                post.Slug = _slugService.GenerateSlug(post.Title);
                string slugError = _slugService.ConfirmSlug(post.Slug,null);
                
                if (!string.IsNullOrEmpty(slugError))
                {
                    ModelState.AddModelError("Title", $"{slugError}.");
                    ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Title", post.BlogId);
                    ViewData["Tags"] = new SelectList(tagValues);
                    return View(post);
                }
                post.Created = DateTime.Now;
                post.AuthorId =  _userManager.GetUserId(User);
                post.ImageData = await _imageService.ConvertFileToByteArray(post.ImageFile);
                post.ImageType = post.ImageFile?.ContentType;

                _context.Add(post);
                foreach (var tag in tagValues)
                {
                    _context.Add(new Tag()
                    {
                        PostId = post.Id,
                        AuthorId = post.AuthorId,
                        Title = tag.ToLower()
                    });
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)) ;
            }

            ModelState.AddModelError(string.Empty, "Fill out the complete form.");
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Title", post.BlogId);
            ViewData["Tags"] = new SelectList(tagValues);
            return View(post);
        }
        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.Include(p => p.Tags).FirstOrDefaultAsync(p=>p.Id==id);
            if (post == null)
            {
                return NotFound();
            }

            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Title", post.BlogId);
            ViewData["Tags"] = new SelectList(post.Tags, "Title", "Title");

            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogId,Title,Abstract,Content,State,ImageFile")] Post post, List<string> tagValues)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid && post.BlogId != 0)
            {
                try
                {
                    var dbPost = await _context.Posts.Include(p=>p.Tags).FirstOrDefaultAsync(p=>p.Id==id);

                    if (dbPost.Title != post.Title)
                    {
                        dbPost.Slug = _slugService.GenerateSlug(post.Title);
                        string slugError = _slugService.ConfirmSlug(dbPost.Slug, dbPost.Id);

                        if (!string.IsNullOrEmpty(slugError))
                        {
                            ModelState.AddModelError("Title", $"{slugError}");
                            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Title", post.BlogId);
                            ViewData["Tags"] = new SelectList(tagValues);
                            return View(post);
                        }

                        dbPost.Title = post.Title;
                    }

                    dbPost.Updated = DateTime.Now;
                    dbPost.BlogId = post.BlogId;
                    dbPost.Abstract = post.Abstract;
                    dbPost.Content = post.Content;
                    dbPost.Status = post.Status;
                    dbPost.ImageData = await _imageService.ConvertFileToByteArray(post.ImageFile);
                    dbPost.ImageType = post.ImageFile?.ContentType;

                    _context.RemoveRange(dbPost.Tags);

                    foreach (var tag in tagValues)
                    {
                        _context.Add(new Tag()
                        {
                            PostId = dbPost.Id,
                            AuthorId = dbPost.AuthorId,
                            Title = tag.ToLower()
                        });
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }

            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Title", post.BlogId);
            ViewData["Tags"] = new SelectList(tagValues);
            return View(post);
        }
        public async Task<IActionResult> SearchIndex(int? page, string searchTerm)
        {
            int pageNumber = page ?? _pageListSettings.PageNumber;
            int pageSize = _pageListSettings.PageSize;

            var posts = _context.Posts.Include(p => p.Author)
                                      .OrderByDescending(p => p.Created)
                                      .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.ToLower();
                ViewData["SearchTerm"] = term;

                posts = posts.Where(p => p.Title.ToLower().Contains(term) ||
                                    p.Abstract.ToLower().Contains(term) ||
                                    p.Author.FirstName.ToLower().Contains(term) ||
                                    p.Author.LastName.ToLower().Contains(term)
                                   );
            }

            return View(await posts.ToPagedListAsync(pageNumber,pageSize));
        }
        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Blog)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
