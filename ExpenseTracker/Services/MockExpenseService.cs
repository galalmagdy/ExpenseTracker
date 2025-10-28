using ExpenseTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Services
{
    public class MockExpenseService : IExpenseService
    {
        private readonly List<Expense> _expenses = new();
        private int _nextId = 1;

        public MockExpenseService()
        {
            // Seed with sample data

            _expenses.Add(new Expense { Id = _nextId++, Amount = 100, Category = "Food", Date = DateTime.Today, Description = "Lunch" });
            _expenses.Add(new Expense { Id = _nextId++, Amount = 250, Category = "Bills", Date = DateTime.Today.AddDays(-1), Description = "Electricity" });

            // Generated random expenses
            _expenses.Add(new Expense { Id = _nextId++, Amount = 45.50m, Category = "Transportation", Date = DateTime.Today.AddDays(-2), Description = "Gas station" });
            _expenses.Add(new Expense { Id = _nextId++, Amount = 89.99m, Category = "Shopping", Date = DateTime.Today.AddDays(-3), Description = "Clothing" });
            _expenses.Add(new Expense { Id = _nextId++, Amount = 25.00m, Category = "Entertainment", Date = DateTime.Today.AddDays(-1), Description = "Movie tickets" });
            _expenses.Add(new Expense { Id = _nextId++, Amount = 150.00m, Category = "Healthcare", Date = DateTime.Today.AddDays(-4), Description = "Pharmacy" });
            _expenses.Add(new Expense { Id = _nextId++, Amount = 75.80m, Category = "Food", Date = DateTime.Today.AddDays(-2), Description = "Groceries" });
            _expenses.Add(new Expense { Id = _nextId++, Amount = 120.00m, Category = "Bills", Date = DateTime.Today.AddDays(-5), Description = "Internet" });
            _expenses.Add(new Expense { Id = _nextId++, Amount = 15.00m, Category = "Food", Date = DateTime.Today.AddDays(-1), Description = "Coffee shop" });
            _expenses.Add(new Expense { Id = _nextId++, Amount = 60.00m, Category = "Transportation", Date = DateTime.Today.AddDays(-3), Description = "Bus pass" });
            _expenses.Add(new Expense { Id = _nextId++, Amount = 200.00m, Category = "Shopping", Date = DateTime.Today.AddDays(-6), Description = "Electronics" });
            _expenses.Add(new Expense { Id = _nextId++, Amount = 45.00m, Category = "Entertainment", Date = DateTime.Today.AddDays(-2), Description = "Restaurant" });
            _expenses.Add(new Expense { Id = _nextId++, Amount = 95.50m, Category = "Healthcare", Date = DateTime.Today.AddDays(-7), Description = "Doctor visit" });
            _expenses.Add(new Expense { Id = _nextId++, Amount = 300.00m, Category = "Bills", Date = DateTime.Today.AddDays(-8), Description = "Rent" });
            _expenses.Add(new Expense { Id = _nextId++, Amount = 35.25m, Category = "Food", Date = DateTime.Today.AddDays(-3), Description = "Takeout" });
            _expenses.Add(new Expense { Id = _nextId++, Amount = 80.00m, Category = "Transportation", Date = DateTime.Today.AddDays(-4), Description = "Taxi" });
            _expenses.Add(new Expense { Id = _nextId++, Amount = 55.00m, Category = "Other", Date = DateTime.Today.AddDays(-2), Description = "Gift" });
        }

        public async Task<IEnumerable<Expense>> GetExpensesAsync()
        {
            await Task.Delay(400);
            return _expenses.OrderByDescending(e => e.Date);
        }

        public async Task AddExpenseAsync(Expense expense)
        {
            await Task.Delay(300);
            expense.Id = _nextId++;
            _expenses.Add(expense);
        }

        public async Task DeleteExpenseAsync(int id)
        {
            await Task.Delay(300);
            var item = _expenses.FirstOrDefault(x => x.Id == id);
            if (item != null)
                _expenses.Remove(item);
        }

        public async Task UpdateExpenseAsync(Expense expense)
        {
            await Task.Delay(300);
            var existing = _expenses.FirstOrDefault(x => x.Id == expense.Id);
            if (existing != null)
            {
                existing.Amount = expense.Amount;
                existing.Category = expense.Category;
                existing.Date = expense.Date;
                existing.Description = expense.Description;
            }
        }
    }
}
