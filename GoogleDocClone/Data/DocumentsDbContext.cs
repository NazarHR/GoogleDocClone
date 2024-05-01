using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace GoogleDocClone.Data
{
    public class DocumentsDbContext : IdentityDbContext
    {
        public DocumentsDbContext() { 
        }
        public DocumentsDbContext(DbContextOptions<DocumentsDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Entities.Document> Documents { get; set; }
    }
}
