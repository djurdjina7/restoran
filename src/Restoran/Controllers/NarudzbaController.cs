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
    public class NarudzbaController : Controller
    {
        private readonly RestoranContext _context;

        public NarudzbaController(RestoranContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var restoranContext = _context.Narudzba.Include(n => n.Sto).Include(n => n.Zaposleni);
            return View(await restoranContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var narudzba = await _context.Narudzba
                .Include(n => n.Sto)
                .Include(n => n.Zaposleni)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (narudzba == null)
            {
                return NotFound();
            }

            return View(narudzba);
        }

        public IActionResult Create()
        {
            var rezervacija = _context.Rezervacija
                .Where(x => DateTime.Compare(x.Datum.Value, DateTime.Now) > 0
                    || (DateTime.Compare(x.Datum.Value.Date, DateTime.Now.Date) == 0 
                        && x.VrijemeDo.Value.TimeOfDay >= DateTime.Now.TimeOfDay))
                .Select(x => x.StoId)
                .ToList();

            var sto = _context.Sto.Where(x => x.Dostupan == 1)
                .Select(x => new
                {
                    Id = x.Id,
                    Podaci = x.BrojStola.ToString() + " [" + x.BrojMjesta.ToString() + " mjesta]"
                }).ToList();

            var stoRezervacije = _context.Sto.Where(x => x.Dostupan == 0 && rezervacija.Contains(x.Id))
                .Select(x => new
                {
                    Id = x.Id,
                    Podaci = "REZERVACIJA: " + _context.Rezervacija.Where(y => y.StoId == x.Id).Select(y => y.PodaciGosta).FirstOrDefault().ToString()
                }).ToList();

            sto.AddRange(stoRezervacije);

            ViewData["StoId"] = new SelectList(sto, "Id", "Podaci");
            var zaposleni = _context.Zaposleni
                .Select(x => new
                {
                    Id = x.Id,
                    Podaci = x.Ime.ToString() + " " + x.Prezime.ToString()
                });
            ViewData["ZaposleniId"] = new SelectList(zaposleni, "Id", "Podaci");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ZaposleniId,StoId,VrijemeKreiranja,VrijemeZavrsetka,Cijena")] Narudzba narudzba)
        {
            if (ModelState.IsValid)
            {
                narudzba.Cijena = 0;
                narudzba.VrijemeKreiranja = DateTime.Now;
                _context.Add(narudzba);

                var sto = await _context.Sto.FindAsync(narudzba.StoId);
                sto.Dostupan = 0;
                _context.Update(sto);

                await _context.SaveChangesAsync();

                return RedirectToAction("Cart", "Spisakzanarudzbu", new { narudzbaId = narudzba.Id});
            }

            var stoList = _context.Sto
                .Where(x => x.Dostupan == 1)
                .Select(x => new
                {
                    Id = x.Id,
                    Podaci = x.BrojStola.ToString() + " [" + x.BrojMjesta.ToString() + " mjesta]"
                });
            ViewData["StoId"] = new SelectList(stoList, "Id", "Podaci", narudzba.StoId);

            var zaposleni = _context.Zaposleni
                .Select(x => new
                {
                    Id = x.Id,
                    Podaci = x.Ime.ToString() + " " + x.Prezime.ToString()
                });
            ViewData["ZaposleniId"] = new SelectList(zaposleni, "Id", "Podaci", narudzba.ZaposleniId);

            return View(narudzba);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var narudzba = await _context.Narudzba.FindAsync(id);
            if (narudzba == null)
            {
                return NotFound();
            }
            ViewData["StoId"] = new SelectList(_context.Sto, "Id", "BrojStola", narudzba.StoId);
            ViewData["ZaposleniId"] = new SelectList(_context.Zaposleni, "Id", "Id", narudzba.ZaposleniId);
            return View(narudzba);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ZaposleniId,StoId,VrijemeKreiranja,VrijemeZavrsetka,Cijena")] Narudzba narudzba)
        {
            if (id != narudzba.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(narudzba);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NarudzbaExists(narudzba.Id))
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
            ViewData["StoId"] = new SelectList(_context.Sto, "Id", "BrojStola", narudzba.StoId);
            ViewData["ZaposleniId"] = new SelectList(_context.Zaposleni, "Id", "Id", narudzba.ZaposleniId);
            return View(narudzba);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var narudzba = await _context.Narudzba
                .Include(n => n.Sto)
                .Include(n => n.Zaposleni)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (narudzba == null)
            {
                return NotFound();
            }

            return View(narudzba);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var narudzba = await _context.Narudzba.FindAsync(id);
            _context.Narudzba.Remove(narudzba);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NarudzbaExists(int id)
        {
            return _context.Narudzba.Any(e => e.Id == id);
        }
    }
}
