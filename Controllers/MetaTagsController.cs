using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SonOfBlogUpdater.Data;
using SonOfBlogUpdater.Models;
using SonOfBlogUpdater.Utils;

namespace SonOfBlogUpdater.Controllers
{
    public class MetaTagsController : Controller
    {
        private readonly BlogDBContext _context;

        private readonly ILogger _logger;
        public MetaTagsController(BlogDBContext context, ILogger<MetaTagsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: MetaTags
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var metaTags = _context.MetaTag.OrderBy(m => m.Id);
            int pageSize = 10;
            return View(await PaginatedList<MetaTag>.CreateAsync(metaTags.AsNoTracking(), pageNumber ?? 1, pageSize));
        }



        
        // GET: MetaTags/Create
        public IActionResult Create()
        {
            ViewBag.MetaTags = _context.MetaTag;
            return View();
        }


        // POST: MetaTags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string tagName)
        {
            _logger.LogInformation($"Create method called. TagName: {tagName}");

            if (string.IsNullOrEmpty(tagName))
            {
                _logger.LogError("Error occured in MetaTag Create method, TagName is null or empty");
                return View();
            }

            var metaTag = new MetaTag { TagName = tagName };

            if (ModelState.IsValid)
            {
                _logger.LogInformation($"MetaTag created, Model State created . ID {metaTag.Id}. TagName {metaTag.TagName}");
                try
                {
                    _context.Add(metaTag);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("MetaTag created successfully.");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occured in MetaTag Create method while saving changes to the database");
                    return View();
                }
            }
            return View();
        }


        // GET: MetaTags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MetaTag == null)
            {
                return NotFound();
            }

            var metaTag = await _context.MetaTag.FindAsync(id);
            if (metaTag == null)
            {
                return NotFound();
            }
            return View(metaTag);
        }

        // POST: MetaTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TagName")] MetaTag metaTag)
        {
            if (id != metaTag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(metaTag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MetaTagExists(metaTag.Id))
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
            return View(metaTag);
        }

        // GET: MetaTags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MetaTag == null)
            {
                return NotFound();
            }

            var metaTag = await _context.MetaTag
                .FirstOrDefaultAsync(m => m.Id == id);
            if (metaTag == null)
            {
                return NotFound();
            }

            return View(metaTag);
        }

        // POST: MetaTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MetaTag == null)
            {
                return Problem("Entity set 'BlogDBContext.MetaTag'  is null.");
            }
            var metaTag = await _context.MetaTag.FindAsync(id);
            if (metaTag != null)
            {
                _context.MetaTag.Remove(metaTag);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MetaTagExists(int id)
        {
          return (_context.MetaTag?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
