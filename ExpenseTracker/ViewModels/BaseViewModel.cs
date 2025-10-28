using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Backing field for the IsBusy property
        private bool _isBusy;

        /// <summary>
        /// Gets or sets a value indicating whether the view model is busy loading data or executing a command.
        /// This is used for UI feedback (loading spinners) and preventing duplicate command execution.
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        /// <summary>
        /// Helper to raise the PropertyChanged event.
        /// </summary>
        /// <param name="name">The name of the property that changed.</param>
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        /// <summary>
        /// Checks if properties are equal, sets the backing field, and raises the PropertyChanged event.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="backingStore">Reference to the property's backing field.</param>
        /// <param name="value">New value for the property.</param>
        /// <param name="propertyName">Name of the property (automatically populated).</param>
        /// <returns>True if the value was set, False otherwise.</returns>
        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
