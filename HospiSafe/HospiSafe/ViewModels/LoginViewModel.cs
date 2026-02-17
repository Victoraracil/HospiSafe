using HospiSafe.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var mainWindow = new MainWindow();
            //Views.MainWindow mainWindow = new Views.MainWindow();
            mainWindow.Show();
        }
    }
}
