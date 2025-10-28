using ExpenseTracker.Models;
using SQLite;


namespace ExpenseTracker.Services
{
    public class SQLiteExpenseService : IExpenseService
    {
        private readonly SQLiteAsyncConnection _database;
        private bool _initialized = false;

        public SQLiteExpenseService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "expenses.db3");
            _database = new SQLiteAsyncConnection(dbPath);
        }

        private async Task InitializeAsync()
        {
            if (_initialized)
                return;

            await _database.CreateTableAsync<Expense>();
            _initialized = true;
        }

        public async Task<IEnumerable<Expense>> GetExpensesAsync()
        {
            await InitializeAsync();
            return await _database.Table<Expense>().OrderByDescending(e => e.Date).ToListAsync();
        }

        public async Task AddExpenseAsync(Expense expense)
        {
            await InitializeAsync();
            await _database.InsertAsync(expense);
        }

        public async Task UpdateExpenseAsync(Expense expense)
        {
            await InitializeAsync();
            await _database.UpdateAsync(expense);
        }

        public async Task DeleteExpenseAsync(int id)
        {
            await InitializeAsync();
            await _database.DeleteAsync<Expense>(id);
        }
    }
}
