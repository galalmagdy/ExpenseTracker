using ExpenseTracker.Helpers;
using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views;

public partial class SummaryPage : ContentPage
{
	public SummaryPage()
	{
		InitializeComponent();
        BindingContext = ServiceContainer.Resolve<SummaryViewModel>();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is SummaryViewModel vm)
        {

            vm.LoadSummaryAsync();
        }
            
    }
}