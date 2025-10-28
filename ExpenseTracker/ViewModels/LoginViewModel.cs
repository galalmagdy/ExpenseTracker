using ExpenseTracker.Services;
using ExpenseTracker.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExpenseTracker.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;

        private string _email = string.Empty;
        public string Email { get => _email; set => SetProperty(ref _email, value); }

        private string _password = string.Empty;
        public string Password { get => _password; set => SetProperty(ref _password, value); }

        public ICommand LoginCommand { get; }

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
            LoginCommand = new Command(async () => await LoginAsync());
        }

        private async Task LoginAsync()
        {
            bool success = await _authService.LoginAsync(Email, Password);
            if (success)
            {
                Application.Current.MainPage = new AppFlyoutPage();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid credentials", "OK");
            }
        }
    }
}
