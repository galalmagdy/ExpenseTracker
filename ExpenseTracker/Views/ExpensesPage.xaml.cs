using ExpenseTracker.Helpers;
using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views{

    public partial class ExpensesPage : ContentPage
    {
        public ExpensesPage()
        {
            InitializeComponent();
            BindingContext = ServiceContainer.Resolve<ExpensesViewModel>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Get the ViewModel (BindingContext is only set once in the constructor)
            if (BindingContext is ExpensesViewModel vm)
            {
                // FIX: Invoke the command directly on the main thread context.
                // This ensures ObservableCollection updates are correctly received by the CollectionView.
                if (vm.LoadExpensesCommand.CanExecute(null))
                {
                    vm.LoadExpensesCommand.Execute(null);
                }
            }
        }
    }
}
