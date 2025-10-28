using ExpenseTracker.Models;
using ExpenseTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Helpers
{
    public class HybridExpenseService : IExpenseService
    {
        private readonly MockExpenseService _mock;
        private readonly SQLiteExpenseService _sqlite;

        public HybridExpenseService()
        {
            _mock = new MockExpenseService();
            _sqlite = new SQLiteExpenseService();
        }

        public async Task<IEnumerable<Expense>> GetExpensesAsync()
        {
            var data = await _sqlite.GetExpensesAsync();
            if (!data.Any())
            {
                foreach (var exp in await _mock.GetExpensesAsync())
                    await _sqlite.AddExpenseAsync(exp);
            }
            return await _sqlite.GetExpensesAsync();
        }

        // Forward all other calls to SQLite
        public Task AddExpenseAsync(Expense expense) => _sqlite.AddExpenseAsync(expense);
        public Task UpdateExpenseAsync(Expense expense) => _sqlite.UpdateExpenseAsync(expense);
        public Task DeleteExpenseAsync(int id) => _sqlite.DeleteExpenseAsync(id);
    }
}
