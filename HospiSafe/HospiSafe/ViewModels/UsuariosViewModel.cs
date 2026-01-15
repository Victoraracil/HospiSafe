using HospiSafe.Models;
using HospiSafe.Services;
using HospiSafe.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HospiSafe.ViewModels
{
    public class UsuariosViewModel : BaseViewModel
    {
        #region Atributos

        // Objeto usuario
        private Usuario _usuario;

        // Usuario seleccionado en la lista
        private Usuario _usuarioSelected;

        // Lista de usuarios de la BBDD
        private List<Usuario> _listaUsuarios;
        private ObservableCollection<Usuario> _listaUsuariosVisible;

        #endregion

        #region Propiedades públicas (Wrappers)

        public int IdUsuario
        {
            get { return _usuario.IdUsuario; }
            set { _usuario.IdUsuario = value; OnPropertyChanged(); }
        }

        public string Nombre
        {
            get { return _usuario.Nombre; }
            set { _usuario.Nombre = value; OnPropertyChanged(); }
        }

        public string Apellidos
        {
            get { return _usuario.Apellidos; }
            set { _usuario.Apellidos = value; OnPropertyChanged(); }
        }

        public string DNI
        {
            get { return _usuario.DNI; }
            set { _usuario.DNI = value; OnPropertyChanged(); }
        }

        public DateTime FechaNacimiento
        {
            get { return _usuario.FechaNacimiento; }
            set { _usuario.FechaNacimiento = value; OnPropertyChanged(); }
        }

        public string Telefono
        {
            get { return _usuario.Telefono; }
            set { _usuario.Telefono = value; OnPropertyChanged(); }
        }

        public string CorreoElectronico
        {
            get { return _usuario.CorreoElectronico; }
            set { _usuario.CorreoElectronico = value; OnPropertyChanged(); }
        }

        public RolUsuario Rol
        {
            get { return _usuario.Rol; }
            set { _usuario.Rol = value; OnPropertyChanged(); }
        }

        // Password plano solo para crear/editar
        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        public Usuario UsuarioSelected
        {
            get { return _usuarioSelected; }
            set { SetProperty(ref _usuarioSelected, value); }
        }

        public ObservableCollection<Usuario> ListaUsuariosVisible
        {
            get { return _listaUsuariosVisible; }
            set { SetProperty(ref _listaUsuariosVisible, value); }
        }

        public Array Roles => Enum.GetValues(typeof(RolUsuario));

        #endregion

        #region Comandos

        public ICommand CargarCommand { get; }
        public ICommand SelectedItemChangedCommand { get; }
        public ICommand CrearNuevoUsuarioCommand { get; }
        public ICommand GuardarUsuarioCommand { get; }
        public ICommand EliminarUsuarioCommand { get; }
        public ICommand LimpiarDatosCommand { get; }

        #endregion

        #region Constructor

        public UsuariosViewModel()
        {
            _usuario = new Usuario();
            _usuarioSelected = new Usuario();
            _listaUsuarios = new List<Usuario>();
            _listaUsuariosVisible = new ObservableCollection<Usuario>();

            CargarCommand = new RelayCommand(PerformCargarUsuarios);
            SelectedItemChangedCommand = new RelayCommand(PerformSelectedItemChanged);
            CrearNuevoUsuarioCommand = new RelayCommand(PerformCrearNuevoUsuario);
            GuardarUsuarioCommand = new RelayCommand(PerformGuardarUsuario);
            EliminarUsuarioCommand = new RelayCommand(PerformEliminarUsuario);
            LimpiarDatosCommand = new RelayCommand(PerformLimpiarDatos);
        }

        #endregion

        #region Métodos

        public async void PerformCargarUsuarios(object? parameter = null)
        {
            var service = new ServiceUsuario();

            _listaUsuarios = await service.ListarUsuariosAsync();

            ListaUsuariosVisible.Clear();
            foreach (var usuario in _listaUsuarios)
            {
                ListaUsuariosVisible.Add(usuario);
            }
        }

        private void PerformSelectedItemChanged(object? parameter = null)
        {
            if (UsuarioSelected != null)
            {
                IdUsuario = UsuarioSelected.IdUsuario;
                Nombre = UsuarioSelected.Nombre;
                Apellidos = UsuarioSelected.Apellidos;
                DNI = UsuarioSelected.DNI;
                FechaNacimiento = UsuarioSelected.FechaNacimiento;
                Telefono = UsuarioSelected.Telefono;
                CorreoElectronico = UsuarioSelected.CorreoElectronico;
                Rol = UsuarioSelected.Rol;

                Password = string.Empty; // nunca mostramos hash
            }
        }

        public void PerformCrearNuevoUsuario(object? parameter = null)
        {
            IdUsuario = 0;
            Nombre = string.Empty;
            Apellidos = string.Empty;
            DNI = string.Empty;
            FechaNacimiento = DateTime.Today;
            Telefono = string.Empty;
            CorreoElectronico = string.Empty;
            Rol = RolUsuario.Personal;
            Password = string.Empty;
        }

        public async void PerformGuardarUsuario(object? parameter = null)
        {
            var service = new ServiceUsuario();

            var usuarioGuardar = new Usuario
            {
                IdUsuario = IdUsuario,
                Nombre = Nombre,
                Apellidos = Apellidos,
                DNI = DNI,
                FechaNacimiento = FechaNacimiento,
                Telefono = Telefono,
                CorreoElectronico = CorreoElectronico,
                Rol = Rol,
                PasswordHash = Password // se hashea en el service
            };

            if (IdUsuario == 0)
            {
                // Usuario nuevo
                var resultado = await service.CrearUsuarioAsync(usuarioGuardar);

                if (resultado)
                {
                    MessageBox.Show("Usuario creado correctamente");
                    PerformCargarUsuarios();
                    PerformCrearNuevoUsuario();
                }
                else
                {
                    MessageBox.Show("Ya existe un usuario con ese correo");
                }
            }
            else
            {
                MessageBox.Show("Edición aún no implementada");
            }
        }

        public async void PerformEliminarUsuario(object? parameter = null)
        {
            if (UsuarioSelected == null) return;

            MessageBox.Show("Eliminar usuario aún no implementado");
        }

        public void PerformLimpiarDatos(object? parameter = null)
        {
            PerformCrearNuevoUsuario();
        }

        #endregion
    }
}
