using HospiSafe_WPF.ViewModels.Base;
using System.Windows.Input;

namespace HospiSafe_WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel;

        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public ICommand LogoutCommand { get; }

        public MainViewModel()
        {
            // Initial view is Home
            CurrentViewModel = new HomeViewModel(this);
            LogoutCommand = new RelayCommand(ExecuteLogout);
        }

        private void ExecuteLogout(object obj)
        {
            // Logic to handle logout, e.g., closing Main Window handled by View or Messenger
            // For now, simpler approach: The View can close itself if needed, or we just switch view
            // But user wants a window for Login and a window for Main.
            // So Logout would involve closing this Window.
            // We will handle this via Binding in the View or a Service.
            // For simplicity in this step:
             if (obj is System.Windows.Window window)
            {
                window.Close();
                // We should also open LoginView again, but typically LoginView opens MainView.
                // If MainView closes, App might exit if ShiftMode is OnLastWindowClose.
                // We will handle re-opening Login in the View code-behind or a NavigationService later if requested.
                // For now, let's just Close.
            }
        }
    }
}
