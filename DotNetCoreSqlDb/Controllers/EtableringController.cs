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
    public class EtableringController : Controller
    {
        private readonly MyDatabaseContext _context;

        public EtableringController(MyDatabaseContext context)
        {
            _context = context;
        }

        // GET: Etablering
        public async Task<IActionResult> Index()
        {
              return _context.Etableringar != null ? 
                          View(await _context.Etableringar.ToListAsync()) :
                          Problem("Entity set 'MyDatabaseContext.Etableringar'  is null.");
        }

        // GET: Etablering/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Etableringar == null)
            {
                return NotFound();
            }

            var etablering = await _context.Etableringar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (etablering == null)
            {
                return NotFound();
            }

            return View(etablering);
        }

        // GET: Etablering/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Etablering/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KontorSite,AteaKonsult,Gatuadress,Postnr,Ort,ISSKontaktperson,EpostISSKontaktperson,TelefonISSKontaktperson,TeliaUppkoppling,OvrigInfo,KlartSenast,Status")] Etablering etablering)
        {
            if (ModelState.IsValid)
            {
                _context.Add(etablering);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(etablering);
        }

        // GET: Etablering/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Etableringar == null)
            {
                return NotFound();
            }

            var etablering = await _context.Etableringar.FindAsync(id);
            if (etablering == null)
            {
                return NotFound();
            }
            return View(etablering);
        }

        // POST: Etablering/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KontorSite,AteaKonsult,Gatuadress,Postnr,Ort,ISSKontaktperson,EpostISSKontaktperson,TelefonISSKontaktperson,TeliaUppkoppling,OvrigInfo,KlartSenast,Status")] Etablering etablering)
        {
            if (id != etablering.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(etablering);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EtableringExists(etablering.Id))
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
            return View(etablering);
        }

        // GET: Etablering/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Etableringar == null)
            {
                return NotFound();
            }

            var etablering = await _context.Etableringar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (etablering == null)
            {
                return NotFound();
            }

            return View(etablering);
        }

        // POST: Etablering/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Etableringar == null)
            {
                return Problem("Entity set 'MyDatabaseContext.Etableringar'  is null.");
            }
            var etablering = await _context.Etableringar.FindAsync(id);
            if (etablering != null)
            {
                _context.Etableringar.Remove(etablering);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EtableringExists(int id)
        {
          return (_context.Etableringar?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}