using ExpenseTracker.Helpers;
using ExpenseTracker.Models;
using ExpenseTracker.Services;
using ExpenseTracker.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExpenseTracker.ViewModels
{
    public class ExpensesViewModel : BaseViewModel
    {
        private readonly IExpenseService _expenseService;
        private Expense _selectedExpense;
        private string _searchText = string.Empty;
        private string _selectedCategoryFilter = "All";
        private DateTime? _startDate;
        private DateTime? _endDate;

        public ObservableCollection<Expense> Expenses { get; } = new();
        public ObservableCollection<string> AvailableCategories { get; } = new(
            new[] { "All", "Food", "Transport", "Shopping", "Entertainment", "Bills", "Other" });

        // Commands
        public ICommand LoadExpensesCommand { get; }
        public ICommand AddExpenseCommand { get; }
        public ICommand RefreshCommand { get; }

        // ### ADDED ###
        // This command will be triggered by the TapGestureRecognizer
        public ICommand SelectExpenseCommand { get; }

        public ExpensesViewModel(IExpenseService expenseService)
        {
            _expenseService = expenseService;

            LoadExpensesCommand = new Command(async () => await LoadExpensesAsync());
            RefreshCommand = new Command(async () => await LoadExpensesAsync());

            // Navigate to AddExpensePage
            AddExpenseCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(
                    new NavigationPage(new AddExpensePage()));
            });

            // ### ADDED ###
            // Initialize the new command to call our selection method
            SelectExpenseCommand = new Command<Expense>(async (expense) => await OnExpenseSelected(expense));
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                    LoadExpensesCommand.Execute(null);
            }
        }

        public string SelectedCategoryFilter
        {
            get => _selectedCategoryFilter;
            set
            {
                if (SetProperty(ref _selectedCategoryFilter, value))
                    LoadExpensesCommand.Execute(null);
            }
        }

        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                if (SetProperty(ref _startDate, value))
                    LoadExpensesCommand.Execute(null);
            }
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                if (SetProperty(ref _endDate, value))
                    LoadExpensesCommand.Execute(null);
            }
        }

        public Expense SelectedExpense
        {
            get => _selectedExpense;
            set
            {
                // ### MODIFIED ###
                // All navigation logic is removed from the setter.
                // It's now just a simple property.
                SetProperty(ref _selectedExpense, value);
            }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        private async Task LoadExpensesAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var allExpenses = await _expenseService.GetExpensesAsync();
                var filtered = allExpenses.Where(FilterExpense).ToList();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Expenses.Clear();
                    foreach (var e in filtered)
                        Expenses.Add(e);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading expenses: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        private bool FilterExpense(Expense expense)
        {
            if (SelectedCategoryFilter != "All" && expense.Category != SelectedCategoryFilter)
                return false;

            if (!string.IsNullOrWhiteSpace(SearchText) &&
                !(expense.Description?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false))
                return false;

            if (StartDate.HasValue && expense.Date < StartDate.Value)
                return false;

            if (EndDate.HasValue && expense.Date > EndDate.Value)
                return false;

            return true;
        }

        // ### ADDED ###
        // This method now handles the tap.
        private async Task OnExpenseSelected(Expense expense)
        {
            if (expense == null)
                return;

            // Call your existing navigation method
            await OpenEditDeletePageAsync(expense);

            // De-select the item after navigation so it can be tapped again
            // We run this on the main thread just to be safe
            MainThread.BeginInvokeOnMainThread(() => SelectedExpense = null);
        }

        private async Task OpenEditDeletePageAsync(Expense expense)
        {
            if (expense == null) return;

            await Application.Current.MainPage.Navigation.PushModalAsync(
                new NavigationPage(new EditDeleteExpensePage(expense)));
        }
    }

}