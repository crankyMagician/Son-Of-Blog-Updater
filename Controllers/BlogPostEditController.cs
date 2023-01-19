using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SonOfBlogUpdater.Data;
using SonOfBlogUpdater.Models;
using SonOfBlogUpdater.Utils;
using System.Drawing.Printing;
//for using search filter easily
using MockQueryable.Moq;
using Newtonsoft.Json;
using System.Linq;

namespace SonOfBlogUpdater.Controllers
{
    public class BlogPostEditController : Controller
    {
        private readonly BlogDBContext _context;
        private readonly ILogger<BlogPostEditController> _logger;

        //a private list initialized in the constructor
        private List<BlogPost> blogPostList = new List<BlogPost>();
        private List<Author> authorList = new List<Author>();
        private List<MetaTag> metaTagList = new List<MetaTag>();

        private List<BlogPostAuthor> blogPostAuthors = new List<BlogPostAuthor>();
        private List<BlogPostMetaTag> blogPostMetaTags = new List<BlogPostMetaTag>();

        // a const for fuzzy search must score within 3 
        const int threshold = 14;

        //a set amount of blog posts to display
        private const int PageSize = 5;
        public BlogPostEditController(BlogDBContext context, ILogger<BlogPostEditController> logger)
        {
            _context = context;
            _logger = logger;

            //set lists 
            blogPostList = _context.BlogPost.ToList();
            authorList = _context.Author.ToList();
            metaTagList = _context.MetaTag.ToList();

            blogPostAuthors = _context.BlogPostAuthor.ToList();
            blogPostMetaTags = _context.BlogPostMetaTag.ToList();

        }




        public async Task<IActionResult> Index(int? page)
        {
            int currentPage = page ?? 1;


            if (currentPage < 1)
            {
                currentPage = 1;
            }

            _logger.LogInformation("Index page accessed");

            var blogPosts = _context.BlogPost
                                                .Include(p => p.BlogPostAuthors)
                                                .ThenInclude(p => p.Author)
                                                .Include(p => p.BlogPostMetaTags)
                                                .ThenInclude(p => p.MetaTag);

            var paginatedList = await PaginatedList<BlogPost>.CreateAsync(blogPosts.AsNoTracking(), currentPage, PageSize);

            return View(paginatedList);
        }


        [HttpGet]
        public IActionResult CreateNew()
        {

            ViewBag.Authors = _context.Author.ToList();

            //This line gets a list of all MetaTags from the database
            ViewBag.AllMetaTags = metaTagList;//_context.MetaTag.ToList();
            // This line maps the list of MetaTags to a list of SelectListItem,
            // with the Text property set to the TagName, and the Value property set to the Id.
            // This list is then stored in the ViewBag for use in the view.

            return View();
        }

        /*
         * The title search should not return too many results 
         * since the titles are relatively unique
         * the meta tags, author, and blog body have many more results so it gets its own page 
         */

        /*
        //the action result
        public async Task<ActionResult> Search(string metaTag, string author, string blogBody, string titleSearch, int page)
        {

            // Check if any of the search criteria strings are null or empty
            if (string.IsNullOrEmpty(metaTag))
            {
                metaTag = "";
            }
            if (string.IsNullOrEmpty(author))
            {
                author = "";
            }
            if (string.IsNullOrEmpty(blogBody))
            {
                blogBody = "";
            }
            if (string.IsNullOrEmpty(titleSearch))
            {
                titleSearch = "";
            }
            // Execute the LINQ query with the modified search criteria strings
       8
            var matchingBlogPost = blogPostList
              .SelectMany(s => s.BlogPostMetaTags.Select(mt => new { Post = s, Word = mt.MetaTag.TagName }))
              .Concat(blogPostList.Select(s => new { Post = s, Word = s.Body }))
              .Concat(blogPostList.SelectMany(s => s.BlogPostAuthors.Select(a => new { Post = s, Word = a.Author.Name })))
              .Where(x => FuzzySearch.ComputeDistance(x.Word, metaTag) < threshold || FuzzySearch.ComputeDistance(x.Word, author) < threshold || FuzzySearch.ComputeDistance(x.Word, blogBody) < threshold || FuzzySearch.ComputeDistance(x.Post.Title, titleSearch) < threshold)
              .Select(x => x.Post)
              .GroupBy(x => x.Id) // group the posts by their Id property
              .Select(g => g.First()) // select the first post of each group
              .ToList();



            if (matchingBlogPost.Count == 0)
            {
                ViewBag.Message = "No matching blog posts were found.";
                _logger.LogInformation("No matching blog posts were found.");
                return View();
            }

            var mock = matchingBlogPost.BuildMock();


            var paginatedList = await PaginatedList<BlogPost>.CreateAsync(mock, page, PageSize);
            
            return View(paginatedList);
        }
        */



        [HttpPost]
        public IActionResult CreateNew(BlogPost blogPost, string[] metaTags, int SelectedAuthor)
        {

            var selectedAuthor = _context.Author.Where(a => a.Id == SelectedAuthor).FirstOrDefault();
            blogPost.BlogPostAuthors = new List<BlogPostAuthor>() { new BlogPostAuthor() { Author = selectedAuthor } };

            if (metaTags != null)
            {
             

                var metatags = new List<BlogPostMetaTag>();
                foreach (var tag in metaTags)
                {
                    var blogPostMetaTag = new BlogPostMetaTag();
                    blogPostMetaTag.MetaTag = _context.MetaTag.Where(mt => mt.TagName == tag).FirstOrDefault();
                    metatags.Add(blogPostMetaTag);
                }
                blogPost.BlogPostMetaTags = metatags;
            }
            //Check input, if passed then save it to the db
            _context.BlogPost.Add(blogPost);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }






        [HttpGet]
        public IActionResult Edit(int id)
        {
            var blogPost = _context.BlogPost.Include(p => p.BlogPostAuthors)
                                           .ThenInclude(p => p.Author)
                                          
                                           .Include(p => p.BlogPostMetaTags)
                                           .ThenInclude(p => p.MetaTag)
                                           .FirstOrDefault(p => p.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }
            ViewBag.Authors = _context.Author.ToList();
            
            //This line gets a list of all MetaTags from the database
            ViewBag.AllMetaTags = _context.MetaTag.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.TagName }).ToList();//_context.MetaTag.ToList();
            // This line maps the list of MetaTags to a list of SelectListItem,
            // with the Text property set to the TagName, and the Value property set to the Id.
            // This list is then stored in the ViewBag for use in the view.
            var selectedMetaTags = blogPost.BlogPostMetaTags.Select(mt => mt.MetaTagId).ToList();

            ViewBag.SelectedMetaTags = selectedMetaTags;

            
            ViewBag.MetaTags = blogPost.BlogPostMetaTags.Select(bpmt => bpmt.MetaTag).ToList();
            int selectedAuthorId = blogPost.BlogPostAuthors.FirstOrDefault().AuthorId;
            var selectedAuthor = blogPost.BlogPostAuthors.FirstOrDefault(bpa => bpa.AuthorId == selectedAuthorId);

            ViewBag.SelectedAuthor = selectedAuthor;
            //

            return View(blogPost);
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var blogPost = _context.BlogPost.Find(id);
            _context.BlogPost.Remove(blogPost);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }




        [HttpPost]
        public IActionResult Edit(BlogPost model, List<int> metaTagIds)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var blogPost = _context.BlogPost.Include(p => p.BlogPostAuthors)
                                           .ThenInclude(p => p.Author)
                                          
                                           .Include(p => p.BlogPostMetaTags)
                                           .ThenInclude(p => p.MetaTag)
                                           .FirstOrDefault(p => p.Id == model.Id);

            if (blogPost == null)
            {
                return NotFound();
            }

            var currentBlogPostMetaTags = blogPost.BlogPostMetaTags.ToList();
            var newBlogPostMetaTags = new List<BlogPostMetaTag>();

            foreach (var metaTagId in metaTagIds)
            {
                var currentBlogPostMetaTag = currentBlogPostMetaTags.FirstOrDefault(p => p.MetaTagId == metaTagId);

                if (currentBlogPostMetaTag != null)
                {
                    currentBlogPostMetaTags.Remove(currentBlogPostMetaTag);
                }
                else
                {
                    newBlogPostMetaTags.Add(new BlogPostMetaTag { MetaTagId = metaTagId });
                }
            }

            _context.BlogPostMetaTag.RemoveRange(currentBlogPostMetaTags);
            _context.BlogPostMetaTag.AddRange(newBlogPostMetaTags);
     
            blogPost.Title = model.Title;
            blogPost.Body = model.Body;
            blogPost.Date = model.Date;
            _context.BlogPost.Update(blogPost);
            _context.SaveChanges();

            return RedirectToAction("Index", "BlogPost");
        }


    }
}

