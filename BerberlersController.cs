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

    public class BerberlersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BerberlersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Berberlers
        public async Task<IActionResult> Index()
        {
              return _context.Berberler != null ? 
                          View(await _context.Berberler.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Berberler'  is null.");
        }
        [Authorize(Roles = "tuna")]

        // GET: Berberlers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Berberler == null)
            {
                return NotFound();
            }

            var berberler = await _context.Berberler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (berberler == null)
            {
                return NotFound();
            }

            return View(berberler);
        }
        [Authorize(Roles = "tuna")]

        // GET: Berberlers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Berberlers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "tuna")]

        public async Task<IActionResult> Create([Bind("Id,BerberAdi,Duzeyi,TelefonNumarasi")] Berberler berberler)
        {
            
            if (ModelState.IsValid)
            {
                _context.Add(berberler);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(berberler);
        }
        [Authorize(Roles = "tuna")]


        // GET: Berberlers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Berberler == null)
            {
                return NotFound();
            }

            var berberler = await _context.Berberler.FindAsync(id);
            if (berberler == null)
            {
                return NotFound();
            }
            return View(berberler);
        }
        [Authorize(Roles = "tuna")]

        // POST: Berberlers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BerberAdi,Duzeyi,TelefonNumarasi")] Berberler berberler)
        {
            if (id != berberler.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(berberler);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BerberlerExists(berberler.Id))
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
            return View(berberler);
        }
        [Authorize(Roles = "tuna")]


        // GET: Berberlers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Berberler == null)
            {
                return NotFound();
            }

            var berberler = await _context.Berberler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (berberler == null)
            {
                return NotFound();
            }

            return View(berberler);
        }
        [Authorize(Roles = "tuna")]


        // POST: Berberlers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Berberler == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Berberler'  is null.");
            }
            var berberler = await _context.Berberler.FindAsync(id);
            if (berberler != null)
            {
                _context.Berberler.Remove(berberler);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BerberlerExists(int id)
        {
          return (_context.Berberler?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
