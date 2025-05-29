using Microsoft.EntityFrameworkCore;
using Slot4_Demo1.Models;

namespace Slot4_Demo1
{
    public class MyWorldDbContext : DbContext
    {
        public MyWorldDbContext(DbContextOptions<MyWorldDbContext> options) : base(options) { }

        public DbSet<Gadgets> Gadgets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
        }
    }

}
