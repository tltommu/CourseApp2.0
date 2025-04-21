using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseApp2._0.Data;
using CourseApp2._0.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace CourseApp2._0.Controllers
{
    public class SamplesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SamplesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Samples
        public async Task<IActionResult> Index()
        {
              return _context.Sample != null ? 
                          View(await _context.Sample.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Sample'  is null.");
        }
        // GET: Samples/ShowSearch
        public async Task<IActionResult> ShowSearch()
        {
            return View();
        }

        // POST:Samples/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String name)
        {
            return _context.Sample != null ?
                          View("Index", await _context.Sample.Where(s => s.name.Contains(name)).ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Sample'  is null.");
        }

        // GET: Samples/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sample == null)
            {
                return NotFound();
            }

            var sample = await _context.Sample
                .FirstOrDefaultAsync(m => m.id == id);
            if (sample == null)
            {
                return NotFound();
            }

            return View(sample);
        }

        // GET: Samples/Create
        
        public IActionResult Create()
        {
            return View();
        }

        // POST: Samples/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,description")] Sample sample)
        {
            sample.UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (ModelState.IsValid)
            {
                _context.Add(sample);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sample);
        }

        // GET: Samples/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sample == null)
            {
                return NotFound();
            }

            var sample = await _context.Sample.FindAsync(id);
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (sample == null || sample.UserId != currentUserId)
            {
                return Forbid(); // or return RedirectToAction("Index")
            }
            
            return View(sample);
        }

        // POST: Samples/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,description")] Sample sample)
        {
            if (id != sample.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sample);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SampleExists(sample.id))
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
            return View(sample);
        }

        // GET: Samples/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sample == null)
            {
                return NotFound();
            }
            
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var sample = await _context.Sample
                .FirstOrDefaultAsync(m => m.id == id);
            
            if (sample == null || sample.UserId != currentUserId)
            {
                return Forbid(); // or return RedirectToAction("Index")
            }

            return View(sample);
        }

        // POST: Samples/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sample == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Sample'  is null.");
            }
            var sample = await _context.Sample.FindAsync(id);
            if (sample != null)
            {
                _context.Sample.Remove(sample);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SampleExists(int id)
        {
          return (_context.Sample?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
