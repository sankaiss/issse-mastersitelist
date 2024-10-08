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
    public class IPPlanController : Controller
    {
        private readonly MyDatabaseContext _context;

        public IPPlanController(MyDatabaseContext context)
        {
            _context = context;
        }

        // GET: IPPlan
        public async Task<IActionResult> Index()
        {
              return _context.IPPlan != null ? 
                          View(await _context.IPPlan.ToListAsync()) :
                          Problem("Entity set 'MyDatabaseContext.IPPlan'  is null.");
        }

        // GET: IPPlan/Details/5
        public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.IPPlan == null)
        {
            return NotFound();
        }

        // Hämta IPPlan baserat på det angivna id
        var ipPlan = await _context.IPPlan
            .FirstOrDefaultAsync(m => m.ID == id);

        if (ipPlan == null)
        {
            return NotFound();
        }

        // Hämta IPPlanLogs för den aktuella IPPlan och sortera efter ändringsdatum
        var logs = await _context.IPPlanLogs
            .Where(l => l.IPPlanId == id)
            .OrderByDescending(l => l.ChangeDate)
            .ToListAsync();

        // Returnera vyn med både IPPlan och dess loggar
        return View((IPPlan: ipPlan, Logs: logs));
    }

        // GET: IPPlan/Create
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult CreateIPPlan()
        {
            return View();
        }

        // POST: IPPlan/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateIPPlan([Bind("ID,Nät,Mask,MGMT,WLAN,LAN,Site,GammalSite,Status,LastUpdatedDate,PRNT")] IPPlan iPPlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(iPPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(iPPlan);
        }

        // GET: IPPlan/Edit/5
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.IPPlan == null)
            {
                return NotFound();
            }

            var iPPlan = await _context.IPPlan.FindAsync(id);
            if (iPPlan == null)
            {
                return NotFound();
            }
            return View(iPPlan);
        }

        // POST: IPPlan/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nät,Mask,MGMT,WLAN,LAN,Site,GammalSite,Status,LastUpdatedDate,PRNT")] IPPlan iPPlan)
        {
            if (id != iPPlan.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(iPPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IPPlanExists(iPPlan.ID))
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
            return View(iPPlan);
        }

        // GET: IPPlan/Delete/5
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.IPPlan == null)
            {
                return NotFound();
            }

            var iPPlan = await _context.IPPlan
                .FirstOrDefaultAsync(m => m.ID == id);
            if (iPPlan == null)
            {
                return NotFound();
            }

            return View(iPPlan);
        }

        // POST: IPPlan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.IPPlan == null)
            {
                return Problem("Entity set 'MyDatabaseContext.IPPlan'  is null.");
            }
            var iPPlan = await _context.IPPlan.FindAsync(id);
            if (iPPlan != null)
            {
                _context.IPPlan.Remove(iPPlan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IPPlanExists(int id)
        {
          return (_context.IPPlan?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
