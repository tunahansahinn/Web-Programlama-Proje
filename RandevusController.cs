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
    public class RandevusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RandevusController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles ="tuna")]
        // GET: Randevus
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Randevu.Include(r => r.Beceriler).Include(r => r.Berberler);
            return View(await applicationDbContext.ToListAsync());
        }
        [Authorize(Roles = "tuna")]


        // GET: Randevus/Details/5
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
        public IActionResult Basarili()
        {
            return View();
        }
        // GET: Randevus/Create
        public IActionResult Create()
        {
            ViewData["BECERILERID"] = new SelectList(_context.Beceriler, "Id", "Beceri");
            ViewData["BERBERLERID"] = new SelectList(_context.Berberler, "Id", "BerberAdi");
            return View();
        }

        // POST: Randevus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RandevuZamani,BERBERLERID,Adiniz,TelefonNumaraniz,BECERILERID,Okeyleme")] Randevu randevu)
        {
            ModelState.Remove("Berberler");
            ModelState.Remove("Beceriler");

            // 1. Berberin mesaisi var mı kontrolü
            var berberMesaileri = await _context.Mesailer
                .Where(m => m.BERBERLERID == randevu.BERBERLERID && m.mesai.Date == randevu.RandevuZamani.Date)
                .ToListAsync();
            var islemUyumluMu = await _context.BerberBecerileri
    .AnyAsync(bb => bb.BERBERLERID == randevu.BERBERLERID && bb.BECERILERID == randevu.BECERILERID);

            if (!islemUyumluMu)
            {
                ModelState.AddModelError("", "Seçtiğiniz işlem, bu berberin yetenek alanında değildir.");
            }

            if (!berberMesaileri.Any())
            {
                ModelState.AddModelError("", "Seçtiğiniz berberin o gün mesaisi bulunmamaktadır.");
                return View(randevu);
            }

            // 2. Saat kontrolü (sadece 09:00 - 21:00 arası randevu kabul ediliyor)
            if (randevu.RandevuZamani.TimeOfDay < TimeSpan.FromHours(9) || randevu.RandevuZamani.TimeOfDay >= TimeSpan.FromHours(21))
            {
                ModelState.AddModelError("", "Randevular yalnızca sabah 09:00 ile akşam 21:00 saatleri arasında alınabilir.");
                return View(randevu);
            }

            // 3. Çakışan randevu kontrolü (1 saat öncesi ve sonrası)
            var cakisanRandevu = await _context.Randevu
                .Where(r => r.BERBERLERID == randevu.BERBERLERID &&
                            r.RandevuZamani.Date == randevu.RandevuZamani.Date &&
                            (r.RandevuZamani >= randevu.RandevuZamani.AddHours(-1) &&
                             r.RandevuZamani <= randevu.RandevuZamani.AddHours(1)))
                .FirstOrDefaultAsync();

            if (cakisanRandevu != null)
            {
                ModelState.AddModelError("", "Seçtiğiniz saatte veya o saate yakın bir zamanda bu berberin başka bir randevusu bulunmaktadır.");
                return View(randevu);
            }

            // Tüm kontroller geçtiyse randevuyu ekleyip kaydet
            if (ModelState.IsValid)
            {
                _context.Add(randevu);
                await _context.SaveChangesAsync();
                return RedirectToAction("Basarili");
            }
            ViewData["BECERILERID"] = new SelectList(_context.Beceriler, "Id", "Beceri");
            ViewData["BERBERLERID"] = new SelectList(_context.Berberler, "Id", "Berberadi");
            return View(randevu);
        }

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

        // POST: Randevus/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RandevuZamani,BERBERLERID,Adiniz,TelefonNumaraniz,BECERILERID,Okeyleme")] Randevu randevu)
        {
            if (id != randevu.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Berberler");
            ModelState.Remove("Beceriler");

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

        [Authorize(Roles = "tuna")]

        // GET: Randevus/Delete/5
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

        // POST: Randevus/Delete/5
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
