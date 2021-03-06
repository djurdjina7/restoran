﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restoran.Models;

namespace Restoran.Controllers
{
    public class TipmenijaController : Controller
    {
        private readonly RestoranContext _context;

        public TipmenijaController(RestoranContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Tipmenija.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipmenija = await _context.Tipmenija
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipmenija == null)
            {
                return NotFound();
            }

            return View(tipmenija);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naziv")] Tipmenija tipmenija)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipmenija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipmenija);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipmenija = await _context.Tipmenija.FindAsync(id);
            if (tipmenija == null)
            {
                return NotFound();
            }
            return View(tipmenija);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv")] Tipmenija tipmenija)
        {
            if (id != tipmenija.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipmenija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipmenijaExists(tipmenija.Id))
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
            return View(tipmenija);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipmenija = await _context.Tipmenija
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipmenija == null)
            {
                return NotFound();
            }

            return View(tipmenija);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipmenija = await _context.Tipmenija.FindAsync(id);
            _context.Tipmenija.Remove(tipmenija);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipmenijaExists(int id)
        {
            return _context.Tipmenija.Any(e => e.Id == id);
        }
    }
}
