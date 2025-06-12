using Microsoft.EntityFrameworkCore;
using NguyenPhuocAn_SE17D10_A01.DataAccess;
using NguyenPhuocAn_SE17D10_A01.Models;

namespace NguyenPhuocAn_SE17D10_A01.Repositories
{
    public class TagRepository : IRepository<Tag>
    {
        private readonly NewsManagementContext _context;

        public TagRepository(NewsManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Tag> GetByIdAsync(int id)
        {
            return await _context.Tags.FindAsync(id);
        }

        public async Task AddAsync(Tag entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.Tags.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tag entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.Tags.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
                throw new ArgumentException("Tag not found.", nameof(id));

            // Remove associated NewsArticleTags
            var articleTags = _context.NewsArticleTags.Where(nat => nat.TagID == id);
            _context.NewsArticleTags.RemoveRange(articleTags);

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }
    }
}
