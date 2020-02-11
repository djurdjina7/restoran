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

        public async Task<IActionResult> Index()
        {
            return View(await _context.Drzava.ToListAsync());
        }

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

        public IActionResult Create()
        {
            return View();
        }

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
