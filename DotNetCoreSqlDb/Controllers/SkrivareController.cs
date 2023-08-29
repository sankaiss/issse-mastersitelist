using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetCoreSqlDb.Models;
using Microsoft.AspNetCore.Authorization;
using DotNetCoreSqlDb.Data;

namespace DotNetCoreSqlDb.Controllers
{
    [Authorize]
    public class SkrivareController : Controller
    {
        private readonly MyDatabaseContext _context;

        public SkrivareController(MyDatabaseContext context)
        {
            _context = context;
        }

        // GET: Skrivare
        public async Task<IActionResult> Index()
        {
              return _context.Skrivare != null ? 
                          View(await _context.Skrivare.ToListAsync()) :
                          Problem("Entity set 'MyDatabaseContext.Skrivare'  is null.");
        }

        // GET: Skrivare/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Skrivare == null)
            {
                return NotFound();
            }

            var skrivare = await _context.Skrivare
                .FirstOrDefaultAsync(m => m.Id == id);
            if (skrivare == null)
            {
                return NotFound();
            }

            return View(skrivare);
        }

        // GET: Skrivare/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Skrivare/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Gatuadress,Ort,ShareName,Port,Modell")] Skrivare skrivare)
        {
            if (ModelState.IsValid)
            {
                _context.Add(skrivare);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(skrivare);
        }

        // GET: Skrivare/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Skrivare == null)
            {
                return NotFound();
            }

            var skrivare = await _context.Skrivare.FindAsync(id);
            if (skrivare == null)
            {
                return NotFound();
            }
            return View(skrivare);
        }

        // POST: Skrivare/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Gatuadress,Ort,ShareName,Port,Modell")] Skrivare skrivare)
        {
            if (id != skrivare.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(skrivare);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SkrivareExists(skrivare.Id))
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
            return View(skrivare);
        }

        // GET: Skrivare/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Skrivare == null)
            {
                return NotFound();
            }

            var skrivare = await _context.Skrivare
                .FirstOrDefaultAsync(m => m.Id == id);
            if (skrivare == null)
            {
                return NotFound();
            }

            return View(skrivare);
        }

        // POST: Skrivare/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Skrivare == null)
            {
                return Problem("Entity set 'MyDatabaseContext.Skrivare'  is null.");
            }
            var skrivare = await _context.Skrivare.FindAsync(id);
            if (skrivare != null)
            {
                _context.Skrivare.Remove(skrivare);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SkrivareExists(int id)
        {
          return (_context.Skrivare?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
