using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
// ... andra nödvändiga using-anvisningar

namespace DotNetCoreSqlDb.Controllers
{
    public class SiteController : Controller
    {
        // Om du behöver en logger eller några andra tjänster kan du injicera dem här. 
        // Till exempel: private readonly ILogger<SiteController> _logger;

        public SiteController()  // Du kan även lägga till logger eller andra tjänster här.
        {
            // _logger = logger; (om du lägger till en logger)
        }

        public IActionResult Index()
        {
            return View();
        }

        // Lägg till andra actions om nödvändigt
    }
}
