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
    public class DrzavaController : Controller
    {
        private readonly RestoranContext _context;

        public DrzavaController(RestoranContext context)
        {
            _context = context;
        }

        // GET: Drzava
        public async Task<IActionResult> Index()
        {
            return View(await _context.Drzava.ToListAsync());
        }

        // GET: Drzava/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drzava = await _context.Drzava
                .FirstOrDefaultAsync(m => m.Id == id);
            if (drzava == null)
            {
                return NotFound();
            }

            return View(drzava);
        }

        // GET: Drzava/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Drzava/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naziv")] Drzava drzava)
        {
            if (ModelState.IsValid)
            {
                _context.Add(drzava);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(drzava);
        }

        // GET: Drzava/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drzava = await _context.Drzava.FindAsync(id);
            if (drzava == null)
            {
                return NotFound();
            }
            return View(drzava);
        }

        // POST: Drzava/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv")] Drzava drzava)
        {
            if (id != drzava.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(drzava);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DrzavaExists(drzava.Id))
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
            return View(drzava);
        }

        // GET: Drzava/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drzava = await _context.Drzava
                .FirstOrDefaultAsync(m => m.Id == id);
            if (drzava == null)
            {
                return NotFound();
            }

            return View(drzava);
        }

        // POST: Drzava/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var drzava = await _context.Drzava.FindAsync(id);
            _context.Drzava.Remove(drzava);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DrzavaExists(int id)
        {
            return _context.Drzava.Any(e => e.Id == id);
        }
    }
}
