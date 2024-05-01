using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace GoogleDocClone.Data
{
    public class DocumentsDbContext : IdentityDbContext
    {
        public DocumentsDbContext(DbContextOptions<DocumentsDbContext> options)
            : base(options)
        {
        }
        public DbSet<Entities.Document> Documents { get; set; }
    }
}
