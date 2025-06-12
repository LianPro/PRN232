using Microsoft.EntityFrameworkCore;
using NguyenPhuocAn_SE17D10_A01.DataAccess;
using NguyenPhuocAn_SE17D10_A01.Models;

namespace StudentName_ClassCode_A01_BE.Services
{
    public class DatabaseInitializer : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public DatabaseInitializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<NewsManagementContext>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            await context.Database.MigrateAsync();

            var adminEmail = configuration["AdminAccount:Email"];
            var adminPassword = configuration["AdminAccount:Password"];
            if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
            {
                throw new InvalidOperationException("Admin account email or password is missing in appsettings.json.");
            }

            var adminAccount = await context.Accounts.FirstOrDefaultAsync(a => a.Email == adminEmail);
            if (adminAccount == null)
            {
                context.Accounts.Add(new Account { Email = adminEmail, Password = adminPassword, Role = 0 });
            }
            else
            {
                adminAccount.Password = adminPassword;
                adminAccount.Role = 0;
                context.Accounts.Update(adminAccount);
            }
            await context.SaveChangesAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}