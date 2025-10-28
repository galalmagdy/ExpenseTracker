using CommunityToolkit.Maui;
using ExpenseTracker.Helpers;
using ExpenseTracker.Services;
using ExpenseTracker.ViewModels;
using ExpenseTracker.Views;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace ExpenseTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            }).UseMauiCommunityToolkit().UseSkiaSharp();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            // Services
            builder.Services.AddSingleton<IAuthService, MockAuthService>();
            //builder.Services.AddSingleton<IExpenseService, MockExpenseService>();
            //builder.Services.AddSingleton<IExpenseService, SQLiteExpenseService>();
            builder.Services.AddSingleton<MockExpenseService>();
            builder.Services.AddSingleton<SQLiteExpenseService>();
            builder.Services.AddSingleton<IExpenseService, HybridExpenseService>();
            // ViewModels
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<ExpensesViewModel>();
            builder.Services.AddTransient<AddExpenseViewModel>();
            builder.Services.AddTransient<EditDeleteExpenseViewModel>();
            builder.Services.AddTransient<SummaryViewModel>();
            // Views
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ExpensesPage>();
            builder.Services.AddTransient<AppFlyoutPage>();
            builder.Services.AddTransient<AddExpensePage>();
            builder.Services.AddTransient<EditDeleteExpensePage>(); 
            builder.Services.AddTransient<SummaryPage>();

            var app = builder.Build();
            // Register DI globally
            ServiceContainer.RegisterServices(app.Services);
            return app;
        }
    }
}