using HospiSafe.ViewModels.Base;
using HospiSafe.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        public ICommand VolverInicioCommand { get; }

        public MainViewModel()
        {
            // Vista inicial es Home
            CurrentViewModel = new HomeViewModel(this);
            LogoutCommand = new RelayCommand(PerformExecuteLogout);
            VolverInicioCommand = new RelayCommand(PerformVolverInicio);
        }

        private void PerformExecuteLogout(object obj)
        {
            var loginWindow = new LoginView(); //objeto de login

            Application.Current.MainWindow = loginWindow; //asociamos a ventana principal para que la muestre

            loginWindow.Show();

            if (obj is Window mainWindow)
            {
                mainWindow.Close(); //cierra
            }
        }

        private void PerformVolverInicio(object obj)
        {
            CurrentViewModel = new HomeViewModel(this); //volver a la vista inicial
        }
    }
}
