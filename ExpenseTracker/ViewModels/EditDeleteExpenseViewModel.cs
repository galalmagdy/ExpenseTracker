using ExpenseTracker.Models;
using ExpenseTracker.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExpenseTracker.ViewModels
{
    // Reuses core functionality and adds support for Delete and Update modes.
    public class EditDeleteExpenseViewModel : BaseViewModel
    {
        private readonly IExpenseService _expenseService;
        private Expense _currentExpense; // Stores the original expense data

        public EditDeleteExpenseViewModel(IExpenseService expenseService)
        {
            _expenseService = expenseService;
            Categories = new ObservableCollection<string>
{
    "Food", "Transportation", "Shopping", "Entertainment", "Bills", "Healthcare", "Other"
};
            // Initialize Commands
            SaveCommand = new Command(async () => await SaveExpenseAsync(), () => !IsBusy);
            DeleteCommand = new Command(async () => await DeleteExpenseAsync(), () => !IsBusy);
            CancelCommand = new Command(async () => await CancelAsync(),() => !IsBusy);
        }

        // --- Bindable Properties (Mirroring the Expense Model) ---

        // Note: The original AddExpenseViewModel used string for Amount, we'll keep that for consistency with input.
        private string _amount;
        public string Amount
        {
            get => _amount;
            set
            {
                if (SetProperty(ref _amount, value))
                    ((Command)SaveCommand).ChangeCanExecute();
            }
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
                SetProperty(ref _errorMessage, value);
                OnPropertyChanged(nameof(HasError));
            }
        }
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        // Public list of categories for the Picker control
        public ObservableCollection<string> Categories { get; }

        // --- Commands ---
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public ICommand CancelCommand { get; }

        // --- Public Methods ---

        // Method called by the View to initialize the data when navigating to this page
        public void LoadExpense(Expense expense)
        {
            if (expense == null) return;

            // Store the original object for update/delete operations
            _currentExpense = expense;

            // Populate the View Model properties from the expense object
            Amount = expense.Amount.ToString();
            SelectedCategory = expense.Category;
            SelectedDate = expense.Date;
            Description = expense.Description;
        }

        // --- Command Handlers ---
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
            if (IsBusy) return;
            IsBusy = true;
            ((Command)SaveCommand).ChangeCanExecute();

            try
            {
                if (!ValidateInputs()) return;
                ErrorMessage = string.Empty;

                // Update the current expense object with new data
                _currentExpense.Amount = decimal.Parse(Amount);
                _currentExpense.Category = SelectedCategory;
                _currentExpense.Date = SelectedDate;
                _currentExpense.Description = Description;

                await _expenseService.UpdateExpenseAsync(_currentExpense);

                await Application.Current.MainPage.DisplayAlert("Success", "Expense updated successfully!", "OK");

                // Pop the modal page to return to the list
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error updating expense: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Update Error: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                ((Command)SaveCommand).ChangeCanExecute();
            }
        }

        private async Task DeleteExpenseAsync()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirm Deletion",
                $"Are you sure you want to delete this expense ({_currentExpense.Description})?",
                "Yes", "No");

            if (!confirm) return;

            if (IsBusy) return;
            IsBusy = true;
            ((Command)DeleteCommand).ChangeCanExecute();

            try
            {
                await _expenseService.DeleteExpenseAsync(_currentExpense.Id);

                await Application.Current.MainPage.DisplayAlert("Success", "Expense deleted successfully!", "OK");

                // Pop the modal page to return to the list
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error deleting expense: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Delete Error: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                ((Command)DeleteCommand).ChangeCanExecute();
            }
        }

        // --- Validation Logic (Copied from AddExpenseViewModel for consistency) ---
        private bool ValidateInputs()
        {
            ErrorMessage = string.Empty;

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