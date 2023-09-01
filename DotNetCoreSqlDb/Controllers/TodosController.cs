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
using Microsoft.AspNetCore.Identity;
namespace DotNetCoreSqlDb.Controllers
{
  
    [Authorize (Roles = "Admin,Editor")]
    [ActionTimerFilter]
    public class SitesController : Controller
    {
        private readonly MyDatabaseContext _context;
        private readonly IDistributedCache _cache;
        private readonly string _SiteItemsCacheKey = "SiteItemsList";
        private readonly UserManager<ApplicationUser> _userManager;
        public SitesController(MyDatabaseContext context, IDistributedCache cache, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _cache = cache;
            _userManager = userManager;
        }
        // GET: Sites
        public async Task<IActionResult> Index()
        {
            var sites = new List<Site>();
            byte[]? SiteListByteArray;
            SiteListByteArray = await _cache.GetAsync(_SiteItemsCacheKey);
            if (SiteListByteArray != null && SiteListByteArray.Length > 0)
            { 
                sites = ConvertData<Site>.ByteArrayToObjectList(SiteListByteArray);
            }
            else 
            {
                sites = await _context.Site.Where(s => !s.IsArchived).ToListAsync();
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

            // Steg 1: Hämta ändringsloggarna för den specifika Site
            var logs = await _context.Sitelogs.Where(l => l.SiteId == id).OrderByDescending(l => l.ChangeDate).ToListAsync();
    
            
            return View((Site: site, Logs: logs));
        }
        // GET: Sites/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Sites/Create
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Ort,Gatuadress,SiteTyp,GammalAdressEfterFlytt,Leverantör,Status,NätverkskapacitetMbps,NätverkskapacitetGbps,KontaktNamn,ISSKontorSite,Mobilnr,Epostadress,WANUplink,AntalEnheter,Sitestorlek,Kommentarer")] Site site)
        {
            if (id != site.ID)
            {
                return NotFound();
            }

            var originalSite = await _context.Site.AsNoTracking().FirstOrDefaultAsync(s => s.ID == id);


            if (ModelState.IsValid)
            {
                try
                {
                    site.IsArchived = originalSite.IsArchived;

                    CheckAndLogChange(originalSite, site, "Ort");
                    CheckAndLogChange(originalSite, site, "Gatuadress");
                    CheckAndLogChange(originalSite, site, "SiteTyp");
                    CheckAndLogChange(originalSite, site, "GammalAdressEfterFlytt");
                    CheckAndLogChange(originalSite, site, "Leverantör");
                    CheckAndLogChange(originalSite, site, "Status");
                    CheckAndLogChange(originalSite, site, "NätverkskapacitetMbps");
                    CheckAndLogChange(originalSite, site, "KontaktNamn");
                    CheckAndLogChange(originalSite, site, "ISSKontorSite");
                    CheckAndLogChange(originalSite, site, "Mobilnr");
                    CheckAndLogChange(originalSite, site, "Epostadress");
                    CheckAndLogChange(originalSite, site, "WANUplink");
                    CheckAndLogChange(originalSite, site, "AntalEnheter");
                    CheckAndLogChange(originalSite, site, "Sitestorlek");
                    CheckAndLogChange(originalSite, site, "Kommentarer");


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

        private void CheckAndLogChange(Site original, Site updated, string propertyName)
        {
            var originalValue = original.GetType().GetProperty(propertyName).GetValue(original)?.ToString();
            var updatedValue = updated.GetType().GetProperty(propertyName).GetValue(updated)?.ToString();

            if (originalValue != updatedValue && !(string.IsNullOrEmpty(originalValue) && string.IsNullOrEmpty(updatedValue)))
            {
                LogChange(updated.ID, propertyName, originalValue, updatedValue);
            }
        }

        private void LogChange(int siteId, string propertyName, string oldValue, string newValue)
        {

            var userId = _userManager.GetUserId(User); 
            var userName = _userManager.GetUserName(User);
            var log = new SiteLog 
            {
                SiteId = siteId,
                PropertyName = propertyName,
                OldValue = oldValue,
                NewValue = newValue,
                ChangeDate = DateTime.UtcNow,
                UserId = userId,
                UserName = userName
            };

            _context.Sitelogs.Add(log);
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
        [HttpPost]
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

        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RealDeleteConfirmed(int id)
        {
            var site = await _context.Site.FindAsync(id);
            _context.Site.Remove(site);
            await _context.SaveChangesAsync();
            await _cache.RemoveAsync(GetSiteItemCacheKey(site.ID));
            await _cache.RemoveAsync(_SiteItemsCacheKey);
            return RedirectToAction(nameof(Archived));
        }


    }
    
}