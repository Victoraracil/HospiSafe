using HospiSafe_WPF.Models;
using HospiSafe_WPF.Services;
using HospiSafe_WPF.ViewModels.Base;
using HospiSafe_WPF.Views;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HospiSafe_WPF.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _correo;

        public string Correo
        {
            get => _correo;
            set => SetProperty(ref _correo, value);
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        private async void ExecuteLogin(object? parameter = null)
        {
            /*if (parameter is PasswordBox passwordBox) //Comprueba el tipo y crea la variable automaticamente
            {
                using (var service = new ServiceUsuario())
                {
                    var usuario = await service.LoginAsync(Correo, passwordBox.Password);
                    if (usuario != null)
                    {
                         Views.MainWindow mainWindow = new Views.MainWindow();
                         mainWindow.Show();
                         
                         // Close LoginView
                         if (parameter is PasswordBox pb)
                         {
                             Window loginWindow = Window.GetWindow(pb);
                             loginWindow?.Close();
                         }
                    }
                    else
                    {
                        MessageBox.Show("Credenciales incorrectas");
                    }
                }
            }*/
            Views.MainWindow mainWindow = new Views.MainWindow();
            mainWindow.Show();
        }
    }
}
