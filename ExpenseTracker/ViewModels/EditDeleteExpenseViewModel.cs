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
        private Expense _expense;

        public EditDeleteExpenseViewModel(IExpenseService expenseService)
        {
            _expenseService = expenseService;

            Categories = new ObservableCollection<string>
            {
                "Food", "Transportation", "Shopping", "Entertainment", "Bills", "Healthcare", "Other"
            };

            SaveCommand = new Command(async () => await SaveExpenseAsync());
            DeleteCommand = new Command(async () => await DeleteExpenseAsync());
            CancelCommand = new Command(async () => await CancelAsync());
        }

        public void LoadExpense(Expense expense)
        {
            _expense = expense;
            Amount = expense.Amount.ToString();
            SelectedCategory = expense.Category;
            SelectedDate = expense.Date;
            Description = expense.Description;
        }

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

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand CancelCommand { get; }

        private async Task CancelAsync()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async Task SaveExpenseAsync()
        {
            if (!decimal.TryParse(Amount, out decimal parsedAmt) || parsedAmt <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid amount!", "OK");
                return;
            }

            _expense.Amount = parsedAmt;
            _expense.Category = SelectedCategory;
            _expense.Date = SelectedDate;
            _expense.Description = Description;

            await _expenseService.UpdateExpenseAsync(_expense);

            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async Task DeleteExpenseAsync()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirm Delete",
                "Are you sure you want to delete this expense?",
                "Delete",
                "Cancel");

            if (!confirm) return;

            await _expenseService.DeleteExpenseAsync(_expense.Id);

            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}