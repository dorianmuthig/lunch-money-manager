using LunchMoneyManager.Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LunchMoneyManager.Library
{
    public sealed class LunchMoneyManager
    {
        private static readonly LunchMoneyManager instance = new LunchMoneyManager();

        static LunchMoneyManager()
        {
            using (LunchMoneyDbContext context = new LunchMoneyDbContext())
            {
                if (!(new FileInfo(context.DbPath)).Directory.Exists || !File.Exists(context.DbPath) || context.Database.GetPendingMigrations().Count() > 0)
                {
                    context.Database.Migrate();
                }
            }
        }
        private LunchMoneyManager() { }

        public static LunchMoneyManager Instance
        {
            get
            {
                return instance;
            }
        }

        ///<summary>
        /// Adds a new debt.
        ///</summary>
        public async Task AddDebt(Guid debtorId, string reference, float amount, float interest)
        {
            using (LunchMoneyDbContext context = new LunchMoneyDbContext())
            {
                Debtor debtor = await context.Debtors.Where(x => x.Id == debtorId).FirstOrDefaultAsync();
                if (debtor == null)
                {
                    throw new KeyNotFoundException("The specified debtor does not exist.");
                }
                context.Transactions.Add(new Transaction
                {
                    Debtor = debtor,
                    Reference = reference,
                    Amount = Math.Abs(amount),
                    InterestRate = interest,
                });
                context.ChangeTracker.DetectChanges();
                await context.SaveChangesAsync();
            }
        }
        ///<summary>
        /// Repays (part of) a debt.
        ///</summary>
        public async Task RepayDebt(Guid debtorId, string reference, float amount)
        {
            using (LunchMoneyDbContext context = new LunchMoneyDbContext())
            {
                Debtor debtor = await context.Debtors.Where(x => x.Id == debtorId).FirstOrDefaultAsync();
                if (debtor == null)
                {
                    throw new KeyNotFoundException("The specified debtor does not exist.");
                }
                context.Transactions.Add(new Transaction
                {
                    Debtor = debtor,
                    Reference = reference,
                    Amount = Math.Abs(amount) * -1,
                    InterestRate = 0F,
                });
                context.ChangeTracker.DetectChanges();
                await context.SaveChangesAsync();
            }
        }
        ///<summary>
        /// Creates a debtor. A person that owes you money.
        ///</summary>
        ///<returns>Debtor ID as <c>Guid</c></returns>
        public async Task<Guid> CreateDebtor(string name)
        {
            using (LunchMoneyDbContext context = new LunchMoneyDbContext())
            {
                Debtor debtor = new Debtor
                {
                    Name = name,
                };
                context.Debtors.Add(debtor);
                context.ChangeTracker.DetectChanges();
                await context.SaveChangesAsync();
                return debtor.Id;
            }
        }

        ///<summary>
        /// Edits a debtor. Useful for changing their name.
        ///</summary>
        public async Task<Debtor> EditDebtor(Guid debtorId, string name)
        {
            throw new NotImplementedException("Editing things is not implemented.");
        }

        ///<summary>
        /// Deletes a debtor.
        ///</summary>
        public async Task DeleteDebtor(Guid debtorId)
        {
            throw new NotImplementedException("Deleting things is not implemented.");
        }

        ///<summary>
        /// Edits a transaction.
        ///</summary>
        public async Task<Transaction> EditTransaction(Guid transactionId, string reference, float amount, float interestRate)
        {
            throw new NotImplementedException("Editing things is not implemented.");
        }

        ///<summary>
        /// Deletes a transaction.
        ///</summary>
        public async Task DeleteTransaction(Guid transactionId)
        {
            throw new NotImplementedException("Deleting things is not implemented.");
        }

        ///<summary>
        /// Calculates the total amount of debt for a debtor.
        ///</summary>
        public async Task<float> CalculateDebt(Guid debtorId)
        {
            throw new NotImplementedException("Interest calculation and debt summation is not implemented.");
        }

        ///<summary>
        /// Calculates the total amount of debt.
        ///</summary>
        public async Task<float> CalculateDebt()
        {
            throw new NotImplementedException("Interest calculation and debt summation is not implemented.");
        }

        ///<summary>
        /// Retrieves a list of debtors.
        ///</summary>
        public async Task<List<Debtor>> GetDebtors(uint limit = 10, uint page = 1)
        {
            using (LunchMoneyDbContext context = new LunchMoneyDbContext())
            {
                return await context.Debtors.OrderBy(x => x.Name).Skip((int)((page - 1) * limit)).Take((int)limit).ToListAsync();
            }
        }

        ///<summary>
        /// Retrieves the most recent transactions.
        ///</summary>
        public async Task<List<Transaction>> GetTransactions(uint limit = 10, uint page = 1)
        {
            using (LunchMoneyDbContext context = new LunchMoneyDbContext())
            {
                return await context.Transactions.OrderByDescending(x => x.TimeStamp).Skip((int)((page - 1) * limit)).Take((int)limit).ToListAsync();
            }
        }

        ///<summary>
        /// Retrieves the most recent transactions for a debtor.
        ///</summary>
        public async Task<List<Transaction>> GetTransactions(Guid debtorId, uint limit = 10, uint page = 1)
        {
            using (LunchMoneyDbContext context = new LunchMoneyDbContext())
            {
                return await context.Transactions.Where(x => x.Id == debtorId).OrderByDescending(x => x.TimeStamp).Skip((int)((page - 1) * limit)).Take((int)limit).ToListAsync();
            }
        }
    }
}
