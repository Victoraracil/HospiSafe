using HospiSafe.Models;
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
        private string _errorMessage;
        private bool _isLoading;

        public string Correo
        {
            get => _correo;
            set => SetProperty(ref _correo, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(PerformExecuteLogin);
        }

        private async void PerformExecuteLogin(object? parameter = null)
        {
            ErrorMessage = string.Empty;

            if (parameter is not PasswordBox passwordBox) //parameter recibe
            {
                ErrorMessage = "Error interno: no se pudo obtener la contraseña.";
                return;
            }

            var password = passwordBox.Password ?? string.Empty;
            if (string.IsNullOrWhiteSpace(Correo) || string.IsNullOrWhiteSpace(password))
            {
                ErrorMessage = "Correo y contraseña son obligatorios.";
                return;
            }

            IsLoading = true; //cargando
            await Task.Delay(2000); //delay para ver progress bar

            try
            {
                using (var service = new ServiceUsuario())
                {
                    var usuario = await service.LoginAsync(Correo.Trim(), password);

                    if (usuario == null) //si falla y no devuelve usuario
                    {
                        ErrorMessage = "Credenciales incorrectas.";
                        return;
                    }

                    if (usuario.Rol == RolUsuario.Sin_Asignar) //Sin rol
                    {
                        ErrorMessage = "Cuenta sin rol asignado. Contacte al administrador.";
                        return;
                    }

                    // autenticacion correcta
                    SessionManager.CurrentUser = usuario;

                    var mainWindow = new MainWindow();

                    Application.Current.MainWindow = mainWindow;

                    mainWindow.Show();

                    // cerrar ventana de login
                    var win = Window.GetWindow(passwordBox);
                    win?.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error durante el proceso de autenticación.";
                //DEBUG DE ERRORES
                System.Diagnostics.Debug.WriteLine(ex);
            }
            finally
            {
                IsLoading = false; //vuelve a false
                passwordBox.Clear(); //limpia el password por seguridad
            }
        }
    }
}
