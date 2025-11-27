using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IPLStore.API.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace IPLStore.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
