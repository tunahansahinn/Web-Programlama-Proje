using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BarisKuafor.Data;
using BarisKuafor.Models;
using Microsoft.AspNetCore.Authorization;

namespace BarisKuafor.Controllers
{
    
    public class BecerilersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BecerilersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Becerilers
        public async Task<IActionResult> Index()
        {
              return _context.Beceriler != null ? 
                          View(await _context.Beceriler.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Beceriler'  is null.");
        }
        [Authorize]
        // GET: Becerilers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Beceriler == null)
            {
                return NotFound();
            }

            var beceriler = await _context.Beceriler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (beceriler == null)
            {
                return NotFound();
            }

            return View(beceriler);
        }
        [Authorize(Roles = "tuna")]

        // GET: Becerilers/Create
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "tuna")]

        // POST: Becerilers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Beceri,BeceriUcreti")] Beceriler beceriler)
        {
            if (ModelState.IsValid)
            {
                _context.Add(beceriler);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(beceriler);
        }
        [Authorize(Roles = "tuna")]


        // GET: Becerilers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Beceriler == null)
            {
                return NotFound();
            }

            var beceriler = await _context.Beceriler.FindAsync(id);
            if (beceriler == null)
            {
                return NotFound();
            }
            return View(beceriler);
        }
        [Authorize(Roles = "tuna")]

        // POST: Becerilers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Beceri,BeceriUcreti")] Beceriler beceriler)
        {
            if (id != beceriler.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(beceriler);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BecerilerExists(beceriler.Id))
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
            return View(beceriler);
        }
        [Authorize(Roles = "tuna")]

        // GET: Becerilers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Beceriler == null)
            {
                return NotFound();
            }

            var beceriler = await _context.Beceriler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (beceriler == null)
            {
                return NotFound();
            }

            return View(beceriler);
        }

        // POST: Becerilers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "tuna")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Beceriler == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Beceriler'  is null.");
            }
            var beceriler = await _context.Beceriler.FindAsync(id);
            if (beceriler != null)
            {
                _context.Beceriler.Remove(beceriler);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BecerilerExists(int id)
        {
          return (_context.Beceriler?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
