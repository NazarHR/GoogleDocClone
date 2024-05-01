

using GoogleDocClone.Data;
using GoogleDocClone.Entities;
using Microsoft.EntityFrameworkCore;


namespace GoogleDocClone.Services
{

    public class DocumentsRepository : IDocumentsRepository
    {
        private readonly DocumentsDbContext _context;

        public DocumentsRepository(DocumentsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Document>> GetUsersDocuments(string userId)
        {
            return await _context.Documents.Where(d => d.UserId == userId).ToListAsync();
        }

        public async Task AddDocument(Document document)
        {
            _context.Add(document);
            await _context.SaveChangesAsync();
        }

        public async Task<Document?> FindAsync(int? id)
        {
           return await _context.Documents.FindAsync(id);
        }

        public async Task UpdateDocumentAsync(Document document)
        {
            _context.Update(document);
            await _context.SaveChangesAsync();
        }
        public bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.Id == id);
        }

        public async Task DeleteDocumentAsync(Document? document)
        {
            if (document != null)
            {
                _context.Documents.Remove(document);
            }
            await _context.SaveChangesAsync();
        }
    }
}
