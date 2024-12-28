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
    public class BerberBecerilerisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BerberBecerilerisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BerberBecerileris
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BerberBecerileri.Include(b => b.Beceriler).Include(b => b.Berberler);
            return View(await applicationDbContext.ToListAsync());
        }
        [Authorize(Roles = "tuna")]


        // GET: BerberBecerileris/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BerberBecerileri == null)
            {
                return NotFound();
            }

            var berberBecerileri = await _context.BerberBecerileri
                .Include(b => b.Beceriler)
                .Include(b => b.Berberler)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (berberBecerileri == null)
            {
                return NotFound();
            }

            return View(berberBecerileri);
        }
        [Authorize(Roles = "tuna")]

        // GET: BerberBecerileris/Create
        public IActionResult Create()
        {
            ViewData["BECERILERID"] = new SelectList(_context.Beceriler, "Id", "Beceri");
            ViewData["BERBERLERID"] = new SelectList(_context.Berberler, "Id", "BerberAdi");
            return View();
        }
        [Authorize(Roles = "tuna")]

        // POST: BerberBecerileris/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BERBERLERID,BECERILERID")] BerberBecerileri berberBecerileri)
        {
            ModelState.Remove("Berberler");
            ModelState.Remove("Beceriler");
            if (ModelState.IsValid)
            {
                _context.Add(berberBecerileri);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BECERILERID"] = new SelectList(_context.Beceriler, "Id", "Beceri");
            ViewData["BERBERLERID"] = new SelectList(_context.Berberler, "Id", "BerberAdi");
            return View(berberBecerileri);
        }
        [Authorize(Roles = "tuna")]

        // GET: BerberBecerileris/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BerberBecerileri == null)
            {
                return NotFound();
            }

            var berberBecerileri = await _context.BerberBecerileri.FindAsync(id);
            if (berberBecerileri == null)
            {
                return NotFound();
            }
            ViewData["BECERILERID"] = new SelectList(_context.Beceriler, "Id", "Beceri", berberBecerileri.BECERILERID);
            ViewData["BERBERLERID"] = new SelectList(_context.Berberler, "Id", "BerberAdi", berberBecerileri.BERBERLERID);
            return View(berberBecerileri);
        }
        [Authorize(Roles = "tuna")]

        // POST: BerberBecerileris/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BERBERLERID,BECERILERID")] BerberBecerileri berberBecerileri)
        {
            if (id != berberBecerileri.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(berberBecerileri);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BerberBecerileriExists(berberBecerileri.Id))
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
            ViewData["BECERILERID"] = new SelectList(_context.Beceriler, "Id", "Beceri", berberBecerileri.BECERILERID);
            ViewData["BERBERLERID"] = new SelectList(_context.Berberler, "Id", "BerberAdi", berberBecerileri.BERBERLERID);
            return View(berberBecerileri);
        }
        [Authorize(Roles = "tuna")]

        // GET: BerberBecerileris/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BerberBecerileri == null)
            {
                return NotFound();
            }

            var berberBecerileri = await _context.BerberBecerileri
                .Include(b => b.Beceriler)
                .Include(b => b.Berberler)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (berberBecerileri == null)
            {
                return NotFound();
            }

            return View(berberBecerileri);
        }
        [Authorize(Roles = "tuna")]


        // POST: BerberBecerileris/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BerberBecerileri == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BerberBecerileri'  is null.");
            }
            var berberBecerileri = await _context.BerberBecerileri.FindAsync(id);
            if (berberBecerileri != null)
            {
                _context.BerberBecerileri.Remove(berberBecerileri);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BerberBecerileriExists(int id)
        {
          return (_context.BerberBecerileri?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
