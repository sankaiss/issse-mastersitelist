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
using Azure.Storage.Blobs;
namespace DotNetCoreSqlDb.Controllers
{
  
    [Authorize]
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
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult Create()
        {
            return View();
        }
        // POST: Sites/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Ort,Gatuadress,SiteTyp,GammalAdressEfterFlytt,Leverantör,Status,NätverkskapacitetMbps,NätverkskapacitetGbps,KontaktNamn,ISSKontorSite,Mobilnr,Epostadress,WANUplink,AntalEnheter,Sitestorlek,Kommentarer,TICNummer,IPAdress")] Site site, List<IFormFile> imageFiles)
        {
            if (ModelState.IsValid)
            {
                
                if (imageFiles != null && imageFiles.Count > 0)
                {
                    site.ImageReferences = new List<ImageReference>();
                    foreach (var imageFile in imageFiles)
                    {
                        string imageUrl = await UploadImageToBlobStorage(imageFile);
                        var imageReference = new ImageReference { Url = imageUrl };
                        site.ImageReferences.Add(imageReference);
                    }
                }

                site.LastUpdatedDate = DateTime.UtcNow;

                _context.Add(site);
                await _context.SaveChangesAsync();
                await _cache.RemoveAsync(_SiteItemsCacheKey);
                
                                
                return RedirectToAction(nameof(Index));
            }
            return View(site);
        }
        // GET: Sites/Edit/5
        [Authorize(Roles = "Admin,Editor")]
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,Ort,Gatuadress,SiteTyp,GammalAdressEfterFlytt,Leverantör,Status,NätverkskapacitetMbps,NätverkskapacitetGbps,KontaktNamn,ISSKontorSite,Mobilnr,Epostadress,WANUplink,AntalEnheter,Sitestorlek,Kommentarer,TICNummer,IPAdress")] Site site, List<IFormFile> imageFiles, List<int> imagesToDelete)
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

                            // Lägg till nya bilder
                    if (imageFiles != null && imageFiles.Count > 0)
                    {
                        foreach (var imageFile in imageFiles)
                        {
                            string imageUrl = await UploadImageToBlobStorage(imageFile);
                            var imageReference = new ImageReference { Url = imageUrl };
                            site.ImageReferences.Add(imageReference);
                        }
                    }

                            // Ta bort valda bilder
                    if (imagesToDelete != null && imagesToDelete.Count > 0)
                    {
                        foreach (var imageIdToDelete in imagesToDelete)
                        {
                            var imageReferenceToDelete = site.ImageReferences.FirstOrDefault(img => img.ID == imageIdToDelete);
                            if (imageReferenceToDelete != null)
                            {
                                site.ImageReferences.Remove(imageReferenceToDelete);
                            }
                        }
                    }

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
                    CheckAndLogChange(originalSite, site, "TICNummer");
                    CheckAndLogChange(originalSite, site, "IPAdress");


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
        [Authorize(Roles = "Admin,Editor")]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        private async Task<string> UploadImageToBlobStorage(IFormFile imageFile)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=mastersitebloob;AccountKey=fIs+wCc1RFXP4k6raU5DHul3OltKt88Vo6xW1PT8FeyNKUYbHi9LnMF78Re4kQ7buO7SjSGV545f+AStB1Esqg==;EndpointSuffix=core.windows.net";
            string containerName = "images"; // Skapa en behållare för att lagra bilder

            // Skapa en BlobServiceClient med din anslutningssträng
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            // Hämta eller skapa en behållare (container)
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            // Skapa ett unikt filnamn för bilden
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

            // Skapa en BlobClient för att ladda upp bilden
            BlobClient blobClient = containerClient.GetBlobClient(uniqueFileName);

            // Öppna en ström från IFormFile och ladda upp bilden till Blob Storage
            using (Stream stream = imageFile.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            // Returnera URL:en till den uppladdade bilden
            return blobClient.Uri.ToString();
        }



    }
    
}