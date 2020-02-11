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
    public class StoController : Controller
    {
        private readonly RestoranContext _context;

        public StoController(RestoranContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Reserved()
        {
            var stolovi = _context.Sto
                .Where(x => x.Dostupan == 0)
                .Select(x => new
                {
                    Id = x.Id,
                    Podaci = x.BrojStola.ToString() + " [" + x.BrojMjesta.ToString() + " mjesta]"
                });
            ViewData["StoId"] = new SelectList(stolovi, "Id", "Podaci");

            ViewBag.BrojZauzetihStolova = _context.Sto.Where(x => x.Dostupan == 0).Count();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reserved([Bind("Id")] Sto sto)
        {
            if (ModelState.IsValid)
            {
                var stoId = sto.Id;
                var stariSto = await  _context.Sto.FindAsync(stoId);
                stariSto.Dostupan = 1;
                _context.Sto.Update(stariSto);
                await _context.SaveChangesAsync();
                return RedirectToAction("Reserved", "Sto");
            }
            return View(sto);
        }


        public async Task<IActionResult> Index()
        {
            return View(await _context.Sto.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sto = await _context.Sto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sto == null)
            {
                return NotFound();
            }

            return View(sto);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BrojMjesta,BrojStola,Dostupan")] Sto sto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sto);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sto = await _context.Sto.FindAsync(id);
            if (sto == null)
            {
                return NotFound();
            }
            return View(sto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BrojMjesta,BrojStola,Dostupan")] Sto sto)
        {
            if (id != sto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoExists(sto.Id))
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
            return View(sto);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sto = await _context.Sto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sto == null)
            {
                return NotFound();
            }

            return View(sto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sto = await _context.Sto.FindAsync(id);
            _context.Sto.Remove(sto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoExists(int id)
        {
            return _context.Sto.Any(e => e.Id == id);
        }
    }
}
