using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetCoreSqlDb;
using DotNetCoreSqlDb.Data;
using DotNetCoreSqlDb.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Authorization;

namespace DotNetCoreSqlDb.Controllers
{
  
    [Authorize]
    [ActionTimerFilter]
    public class SitesController : Controller
    {
        private readonly MyDatabaseContext _context;
        private readonly IDistributedCache _cache;
        private readonly string _SiteItemsCacheKey = "SiteItemsList";

        public SitesController(MyDatabaseContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: Sites
        public async Task<IActionResult> Index()
        {
            var sites = new List<Site>();
            byte[]? SiteListByteArray;

            SiteListByteArray = await _cache.GetAsync(_SiteItemsCacheKey);
            if (SiteListByteArray != null && SiteListByteArray.Length > 0)
            { 
                sites = await _context.Site.Where(s => !s.IsArchived).ToListAsync();
            }
            else 
            {
                sites = await _context.Site.ToListAsync();
                SiteListByteArray = ConvertData<Site>.ObjectListToByteArray(sites);
                await _cache.SetAsync(_SiteItemsCacheKey, SiteListByteArray);
            }

            return View(sites);
        }

        // GET: Sites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            byte[]? todoItemByteArray;
            Site? site;

            if (id == null)
            {
                return NotFound();
            }

            todoItemByteArray = await _cache.GetAsync(GetSiteItemCacheKey(id));

            if (todoItemByteArray != null && todoItemByteArray.Length > 0)
            {
                site = ConvertData<Site>.ByteArrayToObject(todoItemByteArray);
            }
            else 
            {
                site = await _context.Site
                .FirstOrDefaultAsync(m => m.ID == id);
            if (site == null)
            {
                return NotFound();
            }

                todoItemByteArray = ConvertData<Site>.ObjectToByteArray(site);
                await _cache.SetAsync(GetSiteItemCacheKey(id), todoItemByteArray);
            }

            

            return View(site);
        }

        // GET: Sites/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Ort,Gatuadress,SiteTyp,GammalAdressEfterFlytt,Leverantör,Status,NätverkskapacitetMbps,NätverkskapacitetGbps,KontaktNamn,ISSKontorSite,Mobilnr,Epostadress,WANUplink,AntalEnheter,Sitestorlek,Kommentarer")] Site site)

        {
            if (ModelState.IsValid)
            {
                site.LastUpdatedDate = DateTime.UtcNow;
                _context.Add(site);
                await _context.SaveChangesAsync();
                await _cache.RemoveAsync(_SiteItemsCacheKey);
                return RedirectToAction(nameof(Index));
            }
            return View(site);
        }

        // GET: Sites/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var site = await _context.Site.FindAsync(id);
            if (site == null)
            {
                return NotFound();
            }
            return View(site);
        }

        // POST: Sites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Ort,Gatuadress,SiteTyp,GammalAdressEfterFlytt,Leverantör,Status,NätverkskapacitetMbps,NätverkskapacitetGbps,KontaktNamn,ISSKontorSite,Mobilnr,Epostadress,WANUplink,AntalEnheter,Sitestorlek,Kommentarer")] Site site)
        {
            if (id != site.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    site.LastUpdatedDate = DateTime.UtcNow;
                    _context.Update(site);
                    await _context.SaveChangesAsync();
                    await _cache.RemoveAsync(GetSiteItemCacheKey(site.ID));
                    await _cache.RemoveAsync(_SiteItemsCacheKey);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SiteExists(site.ID))
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
            return View(site);
        }

        // GET: Sites/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var site = await _context.Site
                .FirstOrDefaultAsync(m => m.ID == id);
            if (site == null)
            {
                return NotFound();
            }

            return View(site);
        }

        // POST: Sites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var site = await _context.Site.FindAsync(id);
            if (site == null)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = "Site hittades inte." });
            }

            if (site.IsArchived)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = "Site är redan arkiverad." });
            }

            site.IsArchived = true;
            _context.Update(site);
            await _context.SaveChangesAsync();
            await _cache.RemoveAsync(GetSiteItemCacheKey(site.ID));
            await _cache.RemoveAsync(_SiteItemsCacheKey);

            return RedirectToAction(nameof(Index));
        }



        private bool SiteExists(int id)
        {
            return _context.Site.Any(e => e.ID == id);
        }

        private string GetSiteItemCacheKey(int? id)
        {
            return _SiteItemsCacheKey+"_&_"+id;
            
        }
        // GET: Sites/Archive
        public async Task<IActionResult> Archived()
        {
            var archivedSites = await _context.Site.Where(s => s.IsArchived).ToListAsync();
            return View(archivedSites);
        }
        
        // GET: Sites/Restore/5
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = "Site ID är obligatoriskt." });
            }

            var site = await _context.Site.FindAsync(id);
            if (site == null)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = "Site hittades inte." });
            }

            if (!site.IsArchived)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = "Site är inte arkiverad och kan inte återställas." });
            }

            return View(site);
        }


        // POST: Sites/Restore/5
        [HttpPost, ActionName]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(int id)
        {
            var site = await _context.Site.FindAsync(id);
            if (site != null)
            {
                site.IsArchived = false;
                _context.Update(site);
                await _context.SaveChangesAsync();
                await _cache.RemoveAsync(GetSiteItemCacheKey(site.ID));
                await _cache.RemoveAsync(_SiteItemsCacheKey);
            }
            return RedirectToAction(nameof(Archived));
        }


    }

    
}
