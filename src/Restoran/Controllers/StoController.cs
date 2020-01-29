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

        // GET: Sto
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sto.ToListAsync());
        }

        // GET: Sto/Details/5
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

        // GET: Sto/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Sto/Edit/5
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

        // POST: Sto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Sto/Delete/5
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

        // POST: Sto/Delete/5
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
