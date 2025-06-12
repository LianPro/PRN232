using Microsoft.EntityFrameworkCore;
using NguyenPhuocAn_SE17D10_A01.DataAccess;
using NguyenPhuocAn_SE17D10_A01.Models;

namespace NguyenPhuocAn_SE17D10_A01.Repositories
{
    public class NewsArticleRepository : IRepository<NewsArticle>
    {
        private readonly NewsManagementContext _context;

        public NewsArticleRepository(NewsManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NewsArticle>> GetAllAsync()
        {
            return await _context.NewsArticles
                .Include(na => na.Category)
                .Include(na => na.Account)
                .Include(na => na.NewsArticleTags)
                .ThenInclude(nat => nat.Tag)
                .ToListAsync();
        }

        public async Task<NewsArticle> GetByIdAsync(int id)
        {
            return await _context.NewsArticles
                .Include(na => na.Category)
                .Include(na => na.Account)
                .Include(na => na.NewsArticleTags)
                .ThenInclude(nat => nat.Tag)
                .FirstOrDefaultAsync(na => na.ArticleID == id);
        }

        public async Task AddAsync(NewsArticle entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.NewsArticles.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NewsArticle entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.NewsArticles.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var article = await _context.NewsArticles.FindAsync(id);
            if (article == null)
                throw new ArgumentException("News article not found.", nameof(id));

            // Remove associated NewsArticleTags
            var tags = _context.NewsArticleTags.Where(nat => nat.ArticleID == id);
            _context.NewsArticleTags.RemoveRange(tags);

            _context.NewsArticles.Remove(article);
            await _context.SaveChangesAsync();
        }
    }
}
