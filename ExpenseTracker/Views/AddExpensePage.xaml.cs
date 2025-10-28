using ExpenseTracker.Helpers;
using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views;

public partial class AddExpensePage : ContentPage
{
    public AddExpensePage()
    {
        InitializeComponent();
        BindingContext = ServiceContainer.Resolve<AddExpenseViewModel>();
    }
}