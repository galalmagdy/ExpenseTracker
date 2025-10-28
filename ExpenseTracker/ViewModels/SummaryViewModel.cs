using ExpenseTracker.Services;
using Microcharts;
using SkiaSharp;
using System.Windows.Input;


namespace ExpenseTracker.ViewModels
{
    public class SummaryViewModel : BaseViewModel
    {
        private readonly IExpenseService _expenseService;

        public SummaryViewModel(IExpenseService expenseService)
        {
            _expenseService = expenseService;
            LoadSummaryCommand = new Command(async () => await LoadSummaryAsync());
        }

        private decimal _totalExpense;
        public decimal TotalExpense
        {
            get => _totalExpense;
            set => SetProperty(ref _totalExpense, value);
        }

        private Chart _categoryChart;
        public Chart CategoryChart
        {
            get => _categoryChart;
            set => SetProperty(ref _categoryChart, value);
        }

        public ICommand LoadSummaryCommand { get; }

        public async Task LoadSummaryAsync()
        {
            var expenses = await _expenseService.GetExpensesAsync();

            if (expenses == null || !expenses.Any())
            {
                TotalExpense = 0;
                CategoryChart = null;
                return;
            }

            TotalExpense = expenses.Sum(e => e.Amount);

            var categoryTotals = expenses
                .GroupBy(e => e.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(e => e.Amount) })
                .ToList();

            var entries = categoryTotals.Select(c => new ChartEntry((float)c.Total)
            {
                Label = c.Category,
                ValueLabel = $"{c.Total:F2}",
                Color = SKColor.Parse(GetCategoryColor(c.Category))
            }).ToList();

            CategoryChart = new DonutChart
            {
                Entries = entries,
                HoleRadius = 0.6f,
                LabelTextSize = 36,
            };
        }

        private string GetCategoryColor(string category)
        {
            return category switch
            {
                "Food" => "#4CAF50",
                "Transport" or "Transportation" => "#2196F3",
                "Shopping" => "#9C27B0",
                "Entertainment" => "#FFC107",
                "Bills" => "#FF5722",
                "Healthcare" => "#E91E63",
                "Other" => "#607D8B",
                _ => "#795548"
            };
        }
    }
}

