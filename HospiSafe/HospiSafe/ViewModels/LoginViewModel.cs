using HospiSafe.Services;
using HospiSafe.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HospiSafe.ViewModels
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
            if (parameter is PasswordBox passwordBox)
            {
                using (var service = new ServiceUsuario())
                {
                    var usuario = await service.LoginAsync(Correo, passwordBox.Password);

                    if (usuario != null)
                    {
                        // Abrimos la ventana principal
                        Window MainWindow = new MainWindow();
                        MainWindow.Show();

                        // Cerramos la ventana de login
                        Window loginWindow = Window.GetWindow(passwordBox);
                        loginWindow?.Close();
                    }
                    else
                    {
                        MessageBox.Show("Credenciales incorrectas");
                    }
                }
            }
        }
    }
}
