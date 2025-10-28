using ExpenseTracker.Models;
using ExpenseTracker.Services;
using System.Collections.ObjectModel;

using System.Windows.Input;

namespace ExpenseTracker.ViewModels
{
    public class AddExpenseViewModel : BaseViewModel
    {
        private readonly IExpenseService _expenseService;

        public AddExpenseViewModel(IExpenseService expenseService)
        {
            _expenseService = expenseService;

            Categories = new ObservableCollection<string>
{
    "Food", "Transportation", "Shopping", "Entertainment", "Bills", "Healthcare", "Other"
};

            SelectedDate = DateTime.Now;
            SaveCommand = new Command(async () => await SaveExpenseAsync());
            CancelCommand = new Command(async () => await CancelAsync(), () => !IsBusy);


        }

        // Properties
        public ObservableCollection<string> Categories { get; }

        private string _amount;
        public string Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        private string _selectedCategory;
        public string SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasError));
            }
        }

        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private async Task CancelAsync()
        {
            // Check if the application's Main Page has a Navigation stack 
            // and pop the modal page to return to the list (ExpensesPage).
            if (Application.Current.MainPage != null)
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }
        private async Task SaveExpenseAsync()
        {
            try
            {
                if (!ValidateInputs())
                    return;

                ErrorMessage = string.Empty;

                var newExpense = new Expense
                {
                    Amount = decimal.Parse(Amount),
                    Category = SelectedCategory,
                    Date = SelectedDate,
                    Description = Description
                };

                await _expenseService.AddExpenseAsync(newExpense);

                await Application.Current.MainPage.DisplayAlert("Success", "Expense added successfully!", "OK");
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(Amount))
            {
                ErrorMessage = "Amount is required.";
                return false;
            }

            if (!decimal.TryParse(Amount, out var parsedAmount) || parsedAmount <= 0)
            {
                ErrorMessage = "Amount must be a positive number.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(SelectedCategory))
            {
                ErrorMessage = "Category is required.";
                return false;
            }

            if (SelectedDate == default)
            {
                ErrorMessage = "Date is required.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(Description) && Description.Length > 200)
            {
                ErrorMessage = "Description cannot exceed 200 characters.";
                return false;
            }

            return true;
        }
    }
}
