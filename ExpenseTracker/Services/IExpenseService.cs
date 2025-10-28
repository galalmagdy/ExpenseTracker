using ExpenseTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Services
{
    public interface IExpenseService
    {
        Task<IEnumerable<Expense>> GetExpensesAsync();
        Task AddExpenseAsync(Expense expense);
        Task UpdateExpenseAsync(Expense expense);
        Task DeleteExpenseAsync(int id);
    }
}
