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

        // GET: Spisakzanarudzbu
        public async Task<IActionResult> Index()
        {
            var restoranContext = _context.Spisakzanarudzbu.Include(s => s.Meni).Include(s => s.Narudzba);
            return View(await restoranContext.ToListAsync());
        }

        public async Task<IActionResult> Cart(int? narudzbaId)
        {
            var restoranContext = _context.Spisakzanarudzbu
                .Include(k => k.Meni)
                .Include(k => k.Narudzba)
                .Where(x => x.NarudzbaId == narudzbaId);
            ViewBag.NarudzbaId = narudzbaId;


            var narudzba = await _context.Narudzba.FindAsync(narudzbaId);
            if (narudzba == null)
            {
                return NotFound();
            }
            ViewBag.Cijena = narudzba.Cijena;
            return View(await restoranContext.ToListAsync());
        }

        // GET: Spisakzanarudzbu/Details/5
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

        // GET: Spisakzanarudzbu/Create
        public IActionResult Create(int? narudzbaId)
        {
            ViewData["MeniId"] = new SelectList(_context.Meni, "Id", "Naziv");
            ViewBag.NarudzbaId = narudzbaId;
            return View();
        }

        // POST: Spisakzanarudzbu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int narudzbaId, [Bind("MeniId,NarudzbaId,Kolicina")] Spisakzanarudzbu spisakzanarudzbu)
        {
            if (ModelState.IsValid)
            {
                var staraNarudzba = await _context.Spisakzanarudzbu.FindAsync(spisakzanarudzbu.MeniId, narudzbaId);
                var meni = await _context.Meni.FindAsync(spisakzanarudzbu.MeniId);
                var narudzba = await _context.Narudzba.FindAsync(narudzbaId);
                

                if(staraNarudzba != null)
                {
                    if(meni.Kolicina > spisakzanarudzbu.Kolicina)
                    {
                        decimal cijena = (decimal) (spisakzanarudzbu.Kolicina * meni.Cijena);
                        staraNarudzba.Cijena += cijena;
                        staraNarudzba.Kolicina += spisakzanarudzbu.Kolicina;

                        narudzba.Cijena += cijena;
                        _context.Update(narudzba);
                        _context.Update(staraNarudzba);

                        meni.Kolicina -= spisakzanarudzbu.Kolicina;
                        _context.Update(meni);

                        await _context.SaveChangesAsync();
                        return RedirectToAction("Cart", "Spisakzanarudzbu", new { narudzbaId = staraNarudzba.NarudzbaId});
                    }
                    else
                    {
                        //greska
                    }
                }
                else
                {
                    decimal cijena = (decimal)(spisakzanarudzbu.Kolicina * meni.Cijena);
                    spisakzanarudzbu.Cijena += cijena;

                    narudzba.Cijena += cijena;
                    _context.Update(narudzba);
                    _context.Add(spisakzanarudzbu);

                    meni.Kolicina -= spisakzanarudzbu.Kolicina;
                    _context.Update(meni);

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Cart", "Spisakzanarudzbu", new { narudzbaId = spisakzanarudzbu.NarudzbaId });
                }
            }
            ViewData["MeniId"] = new SelectList(_context.Meni, "Id", "Naziv", spisakzanarudzbu.MeniId);
            return View(spisakzanarudzbu);
        }

        // GET: Spisakzanarudzbu/Edit/5
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

        // POST: Spisakzanarudzbu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Spisakzanarudzbu/Delete/5
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

        // POST: Spisakzanarudzbu/Delete/5
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
