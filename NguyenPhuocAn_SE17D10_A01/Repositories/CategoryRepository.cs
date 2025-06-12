using Microsoft.EntityFrameworkCore;
using NguyenPhuocAn_SE17D10_A01.DataAccess;
using NguyenPhuocAn_SE17D10_A01.Models;

namespace NguyenPhuocAn_SE17D10_A01.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly NewsManagementContext _context;

        public CategoryRepository(NewsManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task AddAsync(Category entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.Categories.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                throw new ArgumentException("Category not found.", nameof(id));

            // Check if the category is associated with any news articles
            var hasArticles = await _context.NewsArticles.AnyAsync(na => na.CategoryID == id);
            if (hasArticles)
            {
                throw new InvalidOperationException("Cannot delete category because it is associated with one or more news articles.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
