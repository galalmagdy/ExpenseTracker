using ExpenseTracker.Helpers;
using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views
{

    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = ServiceContainer.Resolve<LoginViewModel>();
        }
    }
}