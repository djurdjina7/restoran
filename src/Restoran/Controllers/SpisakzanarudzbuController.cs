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
    public class SpisakzanarudzbuController : Controller
    {
        private readonly RestoranContext _context;

        public SpisakzanarudzbuController(RestoranContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? narudzbaId)
        {
            if(narudzbaId == 0)
            {
                return NotFound();
            }

            var narudzba = await _context.Narudzba.FindAsync(narudzbaId);
            if (narudzba == null)
            {
                return NotFound();
            }
            ViewBag.Cijena = narudzba.Cijena;
            ViewBag.NarudzbaId = narudzbaId;
            ViewBag.Datum = narudzba.VrijemeKreiranja;

            var restoranContext = _context.Spisakzanarudzbu
                .Include(s => s.Meni)
                .Include(s => s.Narudzba)
                .Where(x => x.NarudzbaId == narudzbaId);
            return View(await restoranContext.ToListAsync());
        }

        public async Task<IActionResult> Cart(int? narudzbaId)
        {
            var restoranContext = _context.Spisakzanarudzbu
                .Include(k => k.Meni)
                .Include(k => k.Narudzba)
                .Where(x => x.NarudzbaId == narudzbaId);

            ViewBag.NarudzbaId = narudzbaId;

            var brojElemenata = _context.Spisakzanarudzbu.Where(r => r.NarudzbaId == narudzbaId).Count();

            ViewBag.brojElemenata = brojElemenata;

            var narudzba = await _context.Narudzba.FindAsync(narudzbaId);
            if (narudzba == null)
            {
                return NotFound();
            }
            ViewBag.Cijena = narudzba.Cijena;
            return View(await restoranContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spisakzanarudzbu = await _context.Spisakzanarudzbu
                .Include(s => s.Meni)
                .Include(s => s.Narudzba)
                .FirstOrDefaultAsync(m => m.MeniId == id);
            if (spisakzanarudzbu == null)
            {
                return NotFound();
            }

            return View(spisakzanarudzbu);
        }

        public IActionResult Create(int? narudzbaId)
        {
            var meni = _context.Meni
                .Select(x => new
                {
                    Id = x.Id,
                    Podaci = x.TipMenija.Naziv.ToString() + " - " + x.Naziv.ToString()
                });
            ViewData["MeniId"] = new SelectList(meni, "Id", "Podaci");
            ViewBag.NarudzbaId = narudzbaId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int narudzbaId, [Bind("MeniId,NarudzbaId,Kolicina")] Spisakzanarudzbu spisakzanarudzbu)
        {
            if (ModelState.IsValid)
            {
                var stariSpisakZaNarudzbu = await _context.Spisakzanarudzbu.FindAsync(narudzbaId, spisakzanarudzbu.MeniId);
                var meni = await _context.Meni.FindAsync(spisakzanarudzbu.MeniId);
                var narudzba = await _context.Narudzba.FindAsync(narudzbaId);

                decimal novaCijena = (decimal)(spisakzanarudzbu.Kolicina * meni.Cijena);

                if (stariSpisakZaNarudzbu != null)
                {
                    if(meni.Kolicina > spisakzanarudzbu.Kolicina)
                    {
                        stariSpisakZaNarudzbu.Cijena += novaCijena;
                        stariSpisakZaNarudzbu.Kolicina += spisakzanarudzbu.Kolicina;
                        _context.Update(stariSpisakZaNarudzbu);

                        narudzba.Cijena += novaCijena;
                        _context.Update(narudzba);

                        meni.Kolicina -= spisakzanarudzbu.Kolicina;
                        _context.Update(meni);

                        await _context.SaveChangesAsync();
                        return RedirectToAction("Cart", "Spisakzanarudzbu", new { narudzbaId = stariSpisakZaNarudzbu.NarudzbaId});
                    }
                    else
                    {
                        //greska
                    }
                }
                else
                {
                    spisakzanarudzbu.Cijena = novaCijena;
                    _context.Add(spisakzanarudzbu);

                    narudzba.Cijena += novaCijena;
                    _context.Update(narudzba);

                    meni.Kolicina -= spisakzanarudzbu.Kolicina;
                    _context.Update(meni);

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Cart", "Spisakzanarudzbu", new { narudzbaId = spisakzanarudzbu.NarudzbaId });
                }
            }
            ViewData["MeniId"] = new SelectList(_context.Meni, "Id", "Naziv", spisakzanarudzbu.MeniId);
            return View(spisakzanarudzbu);
        }

        public async Task<IActionResult> RemoveNarudzba(int narudzbaId, int meniId)
        {
            var spisakZaNarudzbu = await _context.Spisakzanarudzbu.FindAsync(narudzbaId, meniId);
            var meni = await _context.Meni.FindAsync(meniId);
            var narudzba = await _context.Narudzba.FindAsync(narudzbaId);

            meni.Kolicina += spisakZaNarudzbu.Kolicina;
            _context.Update(meni);

            narudzba.Cijena -= spisakZaNarudzbu.Cijena;
            _context.Update(narudzba);

            _context.Spisakzanarudzbu.Remove(spisakZaNarudzbu);
            await _context.SaveChangesAsync();
            return RedirectToAction("Cart", "Spisakzanarudzbu", new { narudzbaId = spisakZaNarudzbu.NarudzbaId });
        }

        public async Task<ActionResult> Discard(int? narudzbaId)
        {
            if (narudzbaId == null)
            {
                return NotFound();
            }

            var spisakZaNarudzbu = await _context.Spisakzanarudzbu
                .Include(k => k.Meni)
                .Where(m => m.NarudzbaId == narudzbaId)
                .ToListAsync();
            if (spisakZaNarudzbu == null)
            {
                return NotFound();
            }

            foreach (var item in spisakZaNarudzbu)
            {
                var jednaNarudzba = await _context.Spisakzanarudzbu.FindAsync(narudzbaId, item.MeniId);
                var meni = await _context.Meni.FindAsync(item.MeniId);

                meni.Kolicina += jednaNarudzba.Kolicina;
                _context.Update(meni);

                _context.Spisakzanarudzbu.Remove(jednaNarudzba);
            }

            var narudzba = await _context.Narudzba.FindAsync(narudzbaId);

            var sto = await _context.Sto.FindAsync(narudzba.StoId);
            sto.Dostupan = 1;

            _context.Update(sto);
            _context.Remove(narudzba);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Narudzba");
        }

        public async Task<IActionResult> Finish(int narudzbaId)
        {
            var narudzba = await _context.Narudzba.FindAsync(narudzbaId);


            narudzba.VrijemeZavrsetka = DateTime.Now;
            _context.Update(narudzba);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Narudzba");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spisakzanarudzbu = await _context.Spisakzanarudzbu.FindAsync(id);
            if (spisakzanarudzbu == null)
            {
                return NotFound();
            }
            ViewData["MeniId"] = new SelectList(_context.Meni, "Id", "Id", spisakzanarudzbu.MeniId);
            ViewData["NarudzbaId"] = new SelectList(_context.Narudzba, "Id", "Id", spisakzanarudzbu.NarudzbaId);
            return View(spisakzanarudzbu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MeniId,NarudzbaId,Kolicina,Cijena")] Spisakzanarudzbu spisakzanarudzbu)
        {
            if (id != spisakzanarudzbu.MeniId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(spisakzanarudzbu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpisakzanarudzbuExists(spisakzanarudzbu.MeniId))
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
            ViewData["MeniId"] = new SelectList(_context.Meni, "Id", "Id", spisakzanarudzbu.MeniId);
            ViewData["NarudzbaId"] = new SelectList(_context.Narudzba, "Id", "Id", spisakzanarudzbu.NarudzbaId);
            return View(spisakzanarudzbu);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spisakzanarudzbu = await _context.Spisakzanarudzbu
                .Include(s => s.Meni)
                .Include(s => s.Narudzba)
                .FirstOrDefaultAsync(m => m.MeniId == id);
            if (spisakzanarudzbu == null)
            {
                return NotFound();
            }

            return View(spisakzanarudzbu);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var spisakzanarudzbu = await _context.Spisakzanarudzbu.FindAsync(id);
            _context.Spisakzanarudzbu.Remove(spisakzanarudzbu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpisakzanarudzbuExists(int id)
        {
            return _context.Spisakzanarudzbu.Any(e => e.MeniId == id);
        }
    }
}
