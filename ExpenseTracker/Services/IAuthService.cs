using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string email, string password);
        void Logout();
    }
}
