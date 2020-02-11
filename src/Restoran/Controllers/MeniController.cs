using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restoran.Models;

namespace Restoran.Controllers
{
    public class MeniController : Controller
    {
        private readonly RestoranContext _context;

        public MeniController(RestoranContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var restoranContext = _context.Meni.Include(m => m.TipMenija);
            return View(await restoranContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meni = await _context.Meni
                .Include(m => m.TipMenija)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meni == null)
            {
                return NotFound();
            }

            return View(meni);
        }

        public IActionResult Create()
        {
            ViewData["TipMenijaId"] = new SelectList(_context.Tipmenija, "Id", "Naziv");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TipMenijaId,Naziv,Cijena,Kolicina")] Meni meni)
        {
            if (ModelState.IsValid)
            {
                _context.Add(meni);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipMenijaId"] = new SelectList(_context.Tipmenija, "Id", "Id", meni.TipMenijaId);
            return View(meni);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meni = await _context.Meni.FindAsync(id);
            if (meni == null)
            {
                return NotFound();
            }
            ViewData["TipMenijaId"] = new SelectList(_context.Tipmenija, "Id", "Id", meni.TipMenijaId);
            return View(meni);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TipMenijaId,Naziv,Cijena,Kolicina")] Meni meni)
        {
            if (id != meni.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(meni);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeniExists(meni.Id))
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
            ViewData["TipMenijaId"] = new SelectList(_context.Tipmenija, "Id", "Id", meni.TipMenijaId);
            return View(meni);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meni = await _context.Meni
                .Include(m => m.TipMenija)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meni == null)
            {
                return NotFound();
            }

            return View(meni);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var meni = await _context.Meni.FindAsync(id);
            _context.Meni.Remove(meni);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MeniExists(int id)
        {
            return _context.Meni.Any(e => e.Id == id);
        }
    }
}
