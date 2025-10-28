using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Services
{
    public class MockAuthService : IAuthService
    {
        private bool _isLoggedIn = false;

        public async Task<bool> LoginAsync(string email, string password)
        {
            await Task.Delay(400); // Simulate network delay
            _isLoggedIn = (email == "demo@user.com" && password == "1234");
            return _isLoggedIn;
        }

        public void Logout() => _isLoggedIn = false;
    }
}
