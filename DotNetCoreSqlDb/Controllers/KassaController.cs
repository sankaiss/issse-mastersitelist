using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetCoreSqlDb.Data;
using DotNetCoreSqlDb.Models;
using Microsoft.AspNetCore.Authorization;

namespace DotNetCoreSqlDb.Controllers
{

    [Authorize]
    public class KassaController : Controller
    {
        private readonly MyDatabaseContext _context;

        public KassaController(MyDatabaseContext context)
        {
            _context = context;
        }

        // GET: Kassa
        public async Task<IActionResult> Index()
        {
              return _context.Kassas != null ? 
                          View(await _context.Kassas.ToListAsync()) :
                          Problem("Entity set 'MyDatabaseContext.Kassas'  is null.");
        }

        // GET: Kassa/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Kassas == null)
            {
                return NotFound();
            }

            var kassa = await _context.Kassas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kassa == null)
            {
                return NotFound();
            }

            return View(kassa);
        }

        // GET: Kassa/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kassa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KontorSite,Gatuadress,ÄrendeNrISS,Ort,ISSKontaktperson,EpostISSKontaktperson,TelefonISSKontaktperson,KassaTyp,Uppkopling,Leveraantör,Övrigt")] Kassa kassa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kassa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kassa);
        }

        // GET: Kassa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Kassas == null)
            {
                return NotFound();
            }

            var kassa = await _context.Kassas.FindAsync(id);
            if (kassa == null)
            {
                return NotFound();
            }
            return View(kassa);
        }

        // POST: Kassa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KontorSite,Gatuadress,ÄrendeNrISS,Ort,ISSKontaktperson,EpostISSKontaktperson,TelefonISSKontaktperson,KassaTyp,Uppkopling,Leveraantör,Övrigt")] Kassa kassa)
        {
            if (id != kassa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kassa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KassaExists(kassa.Id))
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
            return View(kassa);
        }

        // GET: Kassa/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Kassas == null)
            {
                return NotFound();
            }

            var kassa = await _context.Kassas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kassa == null)
            {
                return NotFound();
            }

            return View(kassa);
        }

        // POST: Kassa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Kassas == null)
            {
                return Problem("Entity set 'MyDatabaseContext.Kassas'  is null.");
            }
            var kassa = await _context.Kassas.FindAsync(id);
            if (kassa != null)
            {
                _context.Kassas.Remove(kassa);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KassaExists(int id)
        {
          return (_context.Kassas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
