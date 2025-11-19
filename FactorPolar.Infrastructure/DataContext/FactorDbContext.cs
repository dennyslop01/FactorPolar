using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

namespace FactorPolar.Infrastructure.DataContext
{
    public class FactorDbContext(DbContextOptions<FactorDbContext> options) : DbContext(options)
    {
        public DbSet<Credential> Credentials { get; set; }
    }
}
