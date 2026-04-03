using HospiSafe.Models;
using HospiSafe.Services;
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
        private Usuario? _currentUser;

        private string _userInitial;
        private string _userName;
        private string _userRole;
        public string UserInitial
        {
            get => _userInitial;
            set => SetProperty(ref _userInitial, value);
        }

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public string UserRole
        {
            get => _userRole;
            set => SetProperty(ref _userRole, value);
        }
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

            // Inicializar desde la sesión actual
            _currentUser = SessionManager.CurrentUser;
            ApplyUserToView(_currentUser);

            SessionManager.CurrentUserChanged += OnSessionUserChanged;
        }

        private void OnSessionUserChanged()
        {
            _currentUser = SessionManager.CurrentUser;
            ApplyUserToView(_currentUser);
        }

        private void ApplyUserToView(Usuario? user)
        {
            if (user == null)
            {
                UserInitial = "U";
                UserName = "Usuario";
                UserRole = "Sin sesión";
            }
            else
            {
                var displayName = $"{user.Nombre} {user.Apellidos}".Trim(); //usuario a mostrar
                UserName = string.IsNullOrWhiteSpace(displayName) ? (user.CorreoElectronico ?? "Usuario") : displayName;
                UserRole = user.Rol.ToString();
                UserInitial = !string.IsNullOrEmpty(user.Nombre) ? user.Nombre.Substring(0, 1).ToUpper() : "U"; //inicial y sino U
            }
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
