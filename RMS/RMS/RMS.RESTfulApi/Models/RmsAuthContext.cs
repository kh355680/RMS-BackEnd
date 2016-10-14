using Microsoft.AspNet.Identity.EntityFramework;

namespace RMS.RESTfulApi.Models
{
    public class RmsAuthContext : IdentityDbContext
    {
        public RmsAuthContext() : base("DefaultConnection")
        {
            
        }
    }
}