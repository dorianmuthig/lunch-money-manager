using LunchMoneyManager.Library.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace LunchMoneyManager.Library
{
    public class LunchMoneyDbContext: DbContext
    {
        public DbSet<Debtor> Debtors { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public string DbPath { get; }

        public LunchMoneyDbContext()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folder = "Lunch Money Manager"; // name of program, do not translate!
            string directory = Path.Join(path, folder);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            DbPath = Path.Join(directory, "data.sqlite");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
        }
    }
}
