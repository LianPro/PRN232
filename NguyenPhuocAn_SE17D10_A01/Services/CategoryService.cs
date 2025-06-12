using NguyenPhuocAn_SE17D10_A01.Models;
using NguyenPhuocAn_SE17D10_A01.Repositories;

namespace NguyenPhuocAn_SE17D10_A01.Services
{
    public class CategoryService
    {
        private static CategoryService _instance;
        private readonly IRepository<Category> _repository;

        private CategoryService(IRepository<Category> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public static CategoryService Instance(IRepository<Category> repository)
        {
            if (_instance == null)
            {
                _instance = new CategoryService(repository);
            }
            return _instance;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid category ID.", nameof(id));

            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException("Category not found.");

            return category;
        }

        public async Task AddAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            if (string.IsNullOrWhiteSpace(category.Name))
                throw new ArgumentException("Category name cannot be empty.", nameof(category.Name));

            await _repository.AddAsync(category);
        }

        public async Task UpdateAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            if (string.IsNullOrWhiteSpace(category.Name))
                throw new ArgumentException("Category name cannot be empty.", nameof(category.Name));

            var existingCategory = await _repository.GetByIdAsync(category.CategoryID);
            if (existingCategory == null)
                throw new KeyNotFoundException("Category not found.");

            await _repository.UpdateAsync(category);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid category ID.", nameof(id));

            try
            {
                await _repository.DeleteAsync(id);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Cannot delete category because it is associated with one or more news articles.", ex);
            }
        }
    }
}
