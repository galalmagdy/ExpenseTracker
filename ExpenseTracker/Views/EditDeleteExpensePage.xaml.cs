using ExpenseTracker.Helpers;
using ExpenseTracker.Models;
using ExpenseTracker.Services;
using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views;

public partial class EditDeleteExpensePage : ContentPage
{
    public EditDeleteExpensePage(Expense expenseToEdit)
    {
        InitializeComponent();

        // Resolve the ViewModel using the ServiceContainer
        var viewModel = ServiceContainer.Resolve<EditDeleteExpenseViewModel>();

        // Load the specific expense data into the ViewModel
        viewModel.LoadExpense(expenseToEdit);

        BindingContext = viewModel;
    }
}