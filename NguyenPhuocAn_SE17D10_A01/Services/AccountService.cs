using NguyenPhuocAn_SE17D10_A01.Models;
using NguyenPhuocAn_SE17D10_A01.Repositories;
using System.Text.RegularExpressions;

namespace NguyenPhuocAn_SE17D10_A01.Services
{
    public class AccountService
    {
        private static AccountService _instance;
        private readonly IRepository<Account> _repository;

        private AccountService(IRepository<Account> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public static AccountService Instance(IRepository<Account> repository)
        {
            if (_instance == null)
            {
                _instance = new AccountService(repository);
            }
            return _instance;
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Account> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid account ID.", nameof(id));

            var account = await _repository.GetByIdAsync(id);
            if (account == null)
                throw new KeyNotFoundException("Account not found.");

            return account;
        }

        public async Task AddAsync(Account account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));

            if (string.IsNullOrWhiteSpace(account.Email))
                throw new ArgumentException("Email cannot be empty.", nameof(account.Email));

            if (!IsValidPassword(account.Password))
                throw new ArgumentException("Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one number, and one special character.");

            await _repository.AddAsync(account);
        }

        public async Task UpdateAsync(Account account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));

            if (string.IsNullOrWhiteSpace(account.Email))
                throw new ArgumentException("Email cannot be empty.", nameof(account.Email));

            if (!IsValidPassword(account.Password))
                throw new ArgumentException("Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one number, and one special character.");

            var existingAccount = await _repository.GetByIdAsync(account.AccountID);
            if (existingAccount == null)
                throw new KeyNotFoundException("Account not found.");

            await _repository.UpdateAsync(account);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid account ID.", nameof(id));

            try
            {
                await _repository.DeleteAsync(id);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Cannot delete account because it is associated with one or more news articles.", ex);
            }
        }

        public async Task<Account> AuthenticateAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Email and password cannot be empty.");

            return await _repository.GetAllAsync()
                .ContinueWith(t => t.Result.FirstOrDefault(a => a.Email == email && a.Password == password));
        }

        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
            return regex.IsMatch(password);
        }
    }
}
