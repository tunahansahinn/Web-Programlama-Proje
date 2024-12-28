using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BarisKuafor.Data;
using BarisKuafor.Models;

namespace BarisKuafor.Controllers
{
    public class Randevus1Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public Randevus1Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Randevus1
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Randevu.Include(r => r.Beceriler).Include(r => r.Berberler);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Randevus1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Randevu == null)
            {
                return NotFound();
            }

            var randevu = await _context.Randevu
                .Include(r => r.Beceriler)
                .Include(r => r.Berberler)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (randevu == null)
            {
                return NotFound();
            }

            return View(randevu);
        }

        // GET: Randevus1/Create
        public IActionResult Create()
        {
            ViewData["BECERILERID"] = new SelectList(_context.Beceriler, "Id", "Id");
            ViewData["BERBERLERID"] = new SelectList(_context.Berberler, "Id", "Id");
            return View();
        }

        // POST: Randevus1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RandevuZamani,BERBERLERID,Adiniz,TelefonNumaraniz,BECERILERID,Okeyleme")] Randevu randevu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(randevu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BECERILERID"] = new SelectList(_context.Beceriler, "Id", "Id", randevu.BECERILERID);
            ViewData["BERBERLERID"] = new SelectList(_context.Berberler, "Id", "Id", randevu.BERBERLERID);
            return View(randevu);
        }

        // GET: Randevus1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Randevu == null)
            {
                return NotFound();
            }

            var randevu = await _context.Randevu.FindAsync(id);
            if (randevu == null)
            {
                return NotFound();
            }
            ViewData["BECERILERID"] = new SelectList(_context.Beceriler, "Id", "Id", randevu.BECERILERID);
            ViewData["BERBERLERID"] = new SelectList(_context.Berberler, "Id", "Id", randevu.BERBERLERID);
            return View(randevu);
        }

        // POST: Randevus1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RandevuZamani,BERBERLERID,Adiniz,TelefonNumaraniz,BECERILERID,Okeyleme")] Randevu randevu)
        {
            if (id != randevu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(randevu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RandevuExists(randevu.Id))
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
            ViewData["BECERILERID"] = new SelectList(_context.Beceriler, "Id", "Id", randevu.BECERILERID);
            ViewData["BERBERLERID"] = new SelectList(_context.Berberler, "Id", "Id", randevu.BERBERLERID);
            return View(randevu);
        }

        // GET: Randevus1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Randevu == null)
            {
                return NotFound();
            }

            var randevu = await _context.Randevu
                .Include(r => r.Beceriler)
                .Include(r => r.Berberler)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (randevu == null)
            {
                return NotFound();
            }

            return View(randevu);
        }

        // POST: Randevus1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Randevu == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Randevu'  is null.");
            }
            var randevu = await _context.Randevu.FindAsync(id);
            if (randevu != null)
            {
                _context.Randevu.Remove(randevu);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RandevuExists(int id)
        {
          return (_context.Randevu?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
