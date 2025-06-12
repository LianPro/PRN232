using NguyenPhuocAn_SE17D10_A01.Models;
using NguyenPhuocAn_SE17D10_A01.Repositories;

namespace NguyenPhuocAn_SE17D10_A01.Services
{
    public class NewsArticleService
    {
        private static NewsArticleService _instance;
        private readonly IRepository<NewsArticle> _repository;

        private NewsArticleService(IRepository<NewsArticle> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public static NewsArticleService Instance(IRepository<NewsArticle> repository)
        {
            if (_instance == null)
            {
                _instance = new NewsArticleService(repository);
            }
            return _instance;
        }

        public async Task<IEnumerable<NewsArticle>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<NewsArticle> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid news article ID.", nameof(id));

            var article = await _repository.GetByIdAsync(id);
            if (article == null)
                throw new KeyNotFoundException("News article not found.");

            return article;
        }

        public async Task AddAsync(NewsArticle entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(NewsArticle entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var existingArticle = await _repository.GetByIdAsync(entity.ArticleID);
            if (existingArticle == null)
                throw new KeyNotFoundException("News article not found.");

            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid news article ID.", nameof(id));

            await _repository.DeleteAsync(id);
        }
    }
}
