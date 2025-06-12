using Microsoft.EntityFrameworkCore;
using NguyenPhuocAn_SE17D10_A01.DataAccess;
using NguyenPhuocAn_SE17D10_A01.Models;

namespace NguyenPhuocAn_SE17D10_A01.Repositories
{
    public class AccountRepository : IRepository<Account>
    {
        private readonly NewsManagementContext _context;

        public AccountRepository(NewsManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<Account> GetByIdAsync(int id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task AddAsync(Account entity)
        {
            await _context.Accounts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Account entity)
        {
            _context.Accounts.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null && !await _context.NewsArticles.AnyAsync(na => na.AccountID == id))
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Cannot delete account with associated news articles.");
            }
        }
    }
}
