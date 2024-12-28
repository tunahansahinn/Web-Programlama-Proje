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
    [Authorize]
    public class MesailersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MesailersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Mesailers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Mesailer.Include(m => m.berberler);
            return View(await applicationDbContext.ToListAsync());
        }
        [Authorize(Roles ="tuna")]
        // GET: Mesailers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Mesailer == null)
            {
                return NotFound();
            }

            var mesailer = await _context.Mesailer
                .Include(m => m.berberler)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mesailer == null)
            {
                return NotFound();
            }

            return View(mesailer);
        }
        [Authorize(Roles = "tuna")]

        // GET: Mesailers/Create
        public IActionResult Create()
        {
            ViewData.Clear();
            ViewData["BERBERLERID"] = new SelectList(_context.Berberler, "Id", "BerberAdi");
            return View();
        }
        [Authorize(Roles = "tuna")]


        // POST: Mesailers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BERBERLERID,mesai")] Mesailer mesailer)
        {
            ModelState.Remove("Berberler");
            if (ModelState.IsValid)
            {
                _context.Add(mesailer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BERBERLERID"] = new SelectList(_context.Berberler, "Id", "BerberAdi", mesailer.BERBERLERID);
            return View(mesailer);
        }
        [Authorize(Roles = "tuna")]

        // GET: Mesailers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Mesailer == null)
            {
                return NotFound();
            }

            var mesailer = await _context.Mesailer.FindAsync(id);
            if (mesailer == null)
            {
                return NotFound();
            }
            ViewData["BERBERLERID"] = new SelectList(_context.Berberler, "Id", "Id", mesailer.BERBERLERID);
            return View(mesailer);
        }
        [Authorize(Roles = "tuna")]

        // POST: Mesailers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BERBERLERID,mesai")] Mesailer mesailer)
        {
            if (id != mesailer.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Berberler");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mesailer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MesailerExists(mesailer.Id))
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
            ViewData["BERBERLERID"] = new SelectList(_context.Berberler, "Id", "Id", mesailer.BERBERLERID);
            return View(mesailer);
        }

        [Authorize(Roles ="tuna")]
        // GET: Mesailers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Mesailer == null)
            {
                return NotFound();
            }

            var mesailer = await _context.Mesailer
                .Include(m => m.berberler)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mesailer == null)
            {
                return NotFound();
            }

            return View(mesailer);
        }
        [Authorize(Roles = "tuna")]

        // POST: Mesailers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Mesailer == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Mesailer'  is null.");
            }
            var mesailer = await _context.Mesailer.FindAsync(id);
            if (mesailer != null)
            {
                _context.Mesailer.Remove(mesailer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MesailerExists(int id)
        {
          return (_context.Mesailer?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
