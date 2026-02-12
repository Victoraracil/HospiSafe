using HospiSafe.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HospiSafe.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel;
        public string UserInitial { get; set; } = "U";
        public string UserName { get; set; } = "Usuario";
        public string UserRole { get; set; } = "Rol";

        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public ICommand LogoutCommand { get; }

        public MainViewModel()
        {
            // Vista inicial es Home
            CurrentViewModel = new HomeViewModel(this);
            LogoutCommand = new RelayCommand(ExecuteLogout);
        }

        private void ExecuteLogout(object obj)
        {
            if (obj is System.Windows.Window window)
            {
                window.Close();
            }
        }
    }
}
