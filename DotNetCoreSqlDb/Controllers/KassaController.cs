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
using Microsoft.AspNetCore.Identity;

namespace DotNetCoreSqlDb.Controllers
{

    [Authorize]
    public class KassaController : Controller
    {
        private readonly MyDatabaseContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public KassaController(MyDatabaseContext context , UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

            var logs = await _context.KassaLogs.Where(l => l.KassaId == id).OrderByDescending(l => l.ChangeDate).ToListAsync();
            return View((Kassa: kassa, Logs: logs));
        }

        // GET: Kassa/Create
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kassa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KontorSite,Gatuadress,Status,Ort,ISSKontaktperson,EpostISSKontaktperson,TelefonISSKontaktperson,KassaTyp,Uppkopling,Leveraantör,Övrigt")] Kassa kassa)
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
        [Authorize(Roles = "Admin,Editor")]
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,KontorSite,Gatuadress,Status,Ort,ISSKontaktperson,EpostISSKontaktperson,TelefonISSKontaktperson,KassaTyp,Uppkopling,Leveraantör,Övrigt")] Kassa kassa)
        {
            var originalKassa = await _context.Kassas.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);


            if (id != kassa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kassa);

                    CheckAndLogChange(originalKassa, kassa, "KontorSite");
                    CheckAndLogChange(originalKassa, kassa, "Gatuadress");
                    CheckAndLogChange(originalKassa, kassa, "Status");
                    CheckAndLogChange(originalKassa, kassa, "Ort");
                    CheckAndLogChange(originalKassa, kassa, "ISSKontaktperson");
                    CheckAndLogChange(originalKassa, kassa, "EpostISSKontaktperson");
                    CheckAndLogChange(originalKassa, kassa, "TelefonISSKontaktperson");
                    CheckAndLogChange(originalKassa, kassa, "KassaTyp");
                    CheckAndLogChange(originalKassa, kassa, "Uppkopling");
                    CheckAndLogChange(originalKassa, kassa, "Leveraantör");
                    CheckAndLogChange(originalKassa, kassa, "Övrigt");

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
        [Authorize(Roles = "Admin,Editor")]
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

        private void CheckAndLogChange(Kassa original, Kassa updated, string propertyName)
            {
                var originalValue = original.GetType().GetProperty(propertyName).GetValue(original)?.ToString();
                var updatedValue = updated.GetType().GetProperty(propertyName).GetValue(updated)?.ToString();

                if (originalValue != updatedValue && !(string.IsNullOrEmpty(originalValue) && string.IsNullOrEmpty(updatedValue)))
                {
                    LogChange(updated.Id, propertyName, originalValue, updatedValue);
                }
            }

            private void LogChange(int kassaId, string propertyName, string oldValue, string newValue)
            {
                var userId = _userManager.GetUserId(User); 
                var userName = _userManager.GetUserName(User);
                var log = new PrinterLog 
                {
                    PrinterId = kassaId,
                    PropertyName = propertyName,
                    OldValue = oldValue,
                    NewValue = newValue,
                    ChangeDate = DateTime.UtcNow,
                    UserId = userId,
                    UserName = userName
                };

                _context.PrinterLogs.Add(log);
            }
    }
}
