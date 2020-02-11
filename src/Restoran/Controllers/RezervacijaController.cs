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
    public class RezervacijaController : Controller
    {
        private readonly RestoranContext _context;

        public RezervacijaController(RestoranContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var restoranContext = await _context.Rezervacija.Include(r => r.Sto)
                .Where(x => DateTime.Compare(x.Datum.Value, DateTime.Now) > 0
                || (DateTime.Compare(x.Datum.Value.Date, DateTime.Now.Date) == 0
                    && x.VrijemeDo.Value.TimeOfDay >= DateTime.Now.TimeOfDay)).ToListAsync();
            return View(restoranContext);
        }

        public async Task<IActionResult> AllReservations()
        {
            var restoranContext = await _context.Rezervacija.Include(r => r.Sto).ToListAsync();
            return View(restoranContext);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacija
                .Include(r => r.Sto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

        public IActionResult Create()
        {
            var sto = _context.Sto
                .Where(x => x.Dostupan == 1)
                .Select(x => new
                {
                    Id = x.Id,
                    Podaci = x.BrojStola.ToString() + " [" + x.BrojMjesta.ToString() + " mjesta]"
                });
            ViewData["StoId"] = new SelectList(sto, "Id", "Podaci");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StoId,PodaciGosta,Datum,VrijemeOd,VrijemeDo,BrojOsoba")] Rezervacija rezervacija)
        {
            if (ModelState.IsValid)
            {
                var sto = await _context.Sto.FindAsync(rezervacija.StoId);
                sto.Dostupan = 0;
                _context.Update(sto);

                _context.Add(rezervacija);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var stoList = _context.Sto
                .Where(x => x.Dostupan == 1)
                .Select(x => new
                {
                    Id = x.Id,
                    Podaci = x.BrojStola.ToString() + " [" + x.BrojMjesta.ToString() + " mjesta]"
                });
            ViewData["StoId"] = new SelectList(stoList, "Id", "Podaci", rezervacija.StoId);
            return View(rezervacija);
        }

        public async Task<IActionResult> Edit(int? id, int? stariStoId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacija.FindAsync(id);
            if (rezervacija == null)
            {
                return NotFound();
            }
            var stoList = _context.Sto
               .Where(x => x.Id == stariStoId)
               .Select(x => new
               {
                   Id = x.Id,
                   Podaci = x.BrojStola.ToString() + " [" + x.BrojMjesta.ToString() + " mjesta]"
               }).ToList();

            var stoList1 = _context.Sto
               .Where(x => x.Dostupan == 1)
               .Select(x => new
               {
                   Id = x.Id,
                   Podaci = x.BrojStola.ToString() + " [" + x.BrojMjesta.ToString() + " mjesta]"
               }).ToList();

            stoList.AddRange(stoList1);

            ViewData["StoId"] = new SelectList(stoList, "Id", "Podaci");
            ViewData["StariStoId"] = stariStoId;
            return View(rezervacija);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int stariStoId, [Bind("Id,StoId,PodaciGosta,Datum,VrijemeOd,VrijemeDo,BrojOsoba")] Rezervacija rezervacija)
        {
            if (id != rezervacija.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(stariStoId == rezervacija.StoId)
                    {
                        _context.Update(rezervacija);
                    }
                    else
                    {
                        var sto = await _context.Sto.FindAsync(rezervacija.StoId);
                        sto.Dostupan = 0;
                        _context.Update(sto);

                        var stariSto = await _context.Sto.FindAsync(stariStoId);
                        stariSto.Dostupan = 1;
                        _context.Update(stariSto);

                        _context.Update(rezervacija);
                    }
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RezervacijaExists(rezervacija.Id))
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
            var stoList = _context.Sto
                .Where(x => x.Dostupan == 1)
                .Select(x => new
                {
                    Id = x.Id,
                    Podaci = x.BrojStola.ToString() + " [" + x.BrojMjesta.ToString() + " mjesta]"
                });
            ViewData["StoId"] = new SelectList(stoList, "Id", "Podaci", rezervacija.StoId);
            return View(rezervacija);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacija
                .Include(r => r.Sto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rezervacija = await _context.Rezervacija.FindAsync(id);
            _context.Rezervacija.Remove(rezervacija);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RezervacijaExists(int id)
        {
            return _context.Rezervacija.Any(e => e.Id == id);
        }
    }
}
