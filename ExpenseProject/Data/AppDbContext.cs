using ExpenseProject.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Expense> Expenses { get; set; }

        [ForeignKey("UserId")]
        public DbSet<User> Users { get; set; }

    }
}
