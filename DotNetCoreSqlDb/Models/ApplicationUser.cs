using Microsoft.AspNetCore.Identity;

namespace DotNetCoreSqlDb.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
