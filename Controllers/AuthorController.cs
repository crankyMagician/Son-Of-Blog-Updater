using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using SonOfBlogUpdater.Data;
using SonOfBlogUpdater.Models;
using SonOfBlogUpdater.Utils;

namespace SonOfBlogUpdater.Controllers
{
    public class AuthorController : Controller
    {

        private readonly BlogDBContext _context;
        private readonly ILogger<AuthorController> _logger;
        public AuthorController(BlogDBContext context, ILogger<AuthorController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Author
        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 10;
            var authors = _context.Author;
            return View(await PaginatedList<Author>.CreateAsync(authors, page, pageSize));
        }

  

        // GET: Author/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Author/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name, string bio)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Error occured in Author Create method, ModelState is not valid");
                return View();
            }

            var author = new Author { Name = name, Bio = bio};

            _context.Add(author);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Author created successfully.");
            return RedirectToAction(nameof(Index));
        }
        // GET: Author/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Author == null)
            {
                return NotFound();
            }

            var author = await _context.Author.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Author/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Bio,ProfilePicture")] Author author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.Id))
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
            return View(author);
        }

        // GET: Author/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Author == null)
            {
                return NotFound();
            }

            var author = await _context.Author
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Author/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Author == null)
            {
                return Problem("Entity set 'BlogDBContext.Author'  is null.");
            }
            var author = await _context.Author.FindAsync(id);
            if (author != null)
            {
                _context.Author.Remove(author);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
          return (_context.Author?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
