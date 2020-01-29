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
    public class ZaposleniController : Controller
    {
        private readonly RestoranContext _context;

        public ZaposleniController(RestoranContext context)
        {
            _context = context;
        }

        // GET: Zaposleni
        public async Task<IActionResult> Index()
        {
            var restoranContext = _context.Zaposleni.Include(z => z.Grad);
            return View(await restoranContext.ToListAsync());
        }

        // GET: Zaposleni/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zaposleni = await _context.Zaposleni
                .Include(z => z.Grad)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zaposleni == null)
            {
                return NotFound();
            }

            return View(zaposleni);
        }

        // GET: Zaposleni/Create
        public IActionResult Create()
        {
            ViewData["GradId"] = new SelectList(_context.Grad, "Id", "Id");
            return View();
        }

        // POST: Zaposleni/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ime,Prezime,BrojTelefona,MaticniBroj,GradId,Adresa")] Zaposleni zaposleni)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zaposleni);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GradId"] = new SelectList(_context.Grad, "Id", "Id", zaposleni.GradId);
            return View(zaposleni);
        }

        // GET: Zaposleni/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zaposleni = await _context.Zaposleni.FindAsync(id);
            if (zaposleni == null)
            {
                return NotFound();
            }
            ViewData["GradId"] = new SelectList(_context.Grad, "Id", "Id", zaposleni.GradId);
            return View(zaposleni);
        }

        // POST: Zaposleni/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ime,Prezime,BrojTelefona,MaticniBroj,GradId,Adresa")] Zaposleni zaposleni)
        {
            if (id != zaposleni.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zaposleni);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZaposleniExists(zaposleni.Id))
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
            ViewData["GradId"] = new SelectList(_context.Grad, "Id", "Id", zaposleni.GradId);
            return View(zaposleni);
        }

        // GET: Zaposleni/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zaposleni = await _context.Zaposleni
                .Include(z => z.Grad)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zaposleni == null)
            {
                return NotFound();
            }

            return View(zaposleni);
        }

        // POST: Zaposleni/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zaposleni = await _context.Zaposleni.FindAsync(id);
            _context.Zaposleni.Remove(zaposleni);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZaposleniExists(int id)
        {
            return _context.Zaposleni.Any(e => e.Id == id);
        }
    }
}
