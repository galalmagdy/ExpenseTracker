using ExpenseTracker.Helpers;
using ExpenseTracker.Services;

namespace ExpenseTracker.Views
{

    public partial class AppFlyoutPage : FlyoutPage
    {
        public AppFlyoutPage()
        {
            InitializeComponent();
        }

        private void OnExpensesClicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new ExpensesPage());
            IsPresented = false;
        }

        private void OnLogoutClicked(object sender, EventArgs e)
        {
            var authService = ServiceContainer.Resolve<IAuthService>();
            authService.Logout();
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
        private void OnSummaryClicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(ServiceContainer.Resolve<SummaryPage>());
            IsPresented = false;
        }
    }
}