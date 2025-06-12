using NguyenPhuocAn_SE17D10_A01.Models;
using NguyenPhuocAn_SE17D10_A01.Repositories;

namespace NguyenPhuocAn_SE17D10_A01.Services
{
    public class TagService
    {
        private static TagService _instance;
        private readonly IRepository<Tag> _repository;

        private TagService(IRepository<Tag> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public static TagService Instance(IRepository<Tag> repository)
        {
            if (_instance == null)
            {
                _instance = new TagService(repository);
            }
            return _instance;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Tag> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid tag ID.", nameof(id));

            var tag = await _repository.GetByIdAsync(id);
            if (tag == null)
                throw new KeyNotFoundException("Tag not found.");

            return tag;
        }

        public async Task AddAsync(Tag tag)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            if (string.IsNullOrWhiteSpace(tag.Name))
                throw new ArgumentException("Tag name cannot be empty.", nameof(tag.Name));

            await _repository.AddAsync(tag);
        }

        public async Task UpdateAsync(Tag tag)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            if (string.IsNullOrWhiteSpace(tag.Name))
                throw new ArgumentException("Tag name cannot be empty.", nameof(tag.Name));

            var existingTag = await _repository.GetByIdAsync(tag.TagID);
            if (existingTag == null)
                throw new KeyNotFoundException("Tag not found.");

            await _repository.UpdateAsync(tag);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid tag ID.", nameof(id));

            await _repository.DeleteAsync(id);
        }
    }
}
