using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Helpers
{
    public static class ServiceContainer
    {
        public static IServiceProvider Services { get; private set; }

        public static void RegisterServices(IServiceProvider services)
        {
            Services = services;
        }

        public static T Resolve<T>() => Services.GetService<T>();
    }
}
