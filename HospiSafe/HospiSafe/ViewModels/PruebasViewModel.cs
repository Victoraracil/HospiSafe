using HospiSafe.Models;
using HospiSafe.Services;
using HospiSafe.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace HospiSafe.ViewModels
{
    public class PruebasViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainViewModel;

        public string Titulo { get; } = "Laboratorio";

        private ObservableCollection<Prueba> _pruebas = new ObservableCollection<Prueba>();
        public ObservableCollection<Prueba> Pruebas
        {
            get => _pruebas;
            set => SetProperty(ref _pruebas, value);
        }

        private ObservableCollection<Prueba> _pruebasOriginal = new ObservableCollection<Prueba>();

        private ObservableCollection<Paciente> _pacientes = new ObservableCollection<Paciente>();
        public ObservableCollection<Paciente> Pacientes
        {
            get => _pacientes;
            set => SetProperty(ref _pacientes, value);
        }

        private ObservableCollection<Usuario> _usuarios = new ObservableCollection<Usuario>();
        public ObservableCollection<Usuario> Usuarios
        {
            get => _usuarios;
            set => SetProperty(ref _usuarios, value);
        }

        private Paciente _pacienteSeleccionado;
        public Paciente PacienteSeleccionado
        {
            get => _pacienteSeleccionado;
            set => SetProperty(ref _pacienteSeleccionado, value);
        }

        private Usuario _usuarioSeleccionado;
        public Usuario UsuarioSeleccionado
        {
            get => _usuarioSeleccionado;
            set => SetProperty(ref _usuarioSeleccionado, value);
        }

        private string _tipoAnalisis = "";
        public string TipoAnalisis
        {
            get => _tipoAnalisis;
            set => SetProperty(ref _tipoAnalisis, value);
        }

        private string _resultados = "";
        public string Resultados
        {
            get => _resultados;
            set => SetProperty(ref _resultados, value);
        }

        private string _textoBusqueda = "";
        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set
            {
                if (SetProperty(ref _textoBusqueda, value))
                    Filtrar();
            }
        }

        private bool _formVisible;
        public bool FormVisible
        {
            get => _formVisible;
            set => SetProperty(ref _formVisible, value);
        }

        public ICommand CargarCommand { get; }
        public ICommand NuevoCommand { get; }
        public ICommand GuardarCommand { get; }
        public ICommand CancelarCommand { get; }
        public ICommand VolverCommand { get; }

        public PruebasViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            CargarCommand = new RelayCommand(async _ => await CargarAsync());
            NuevoCommand = new RelayCommand(_ => MostrarFormulario());
            GuardarCommand = new RelayCommand(async _ => await GuardarAsync());
            CancelarCommand = new RelayCommand(_ => OcultarFormulario());
            VolverCommand = new RelayCommand(_ => Volver());

            FormVisible = false;
        }

        private void Volver()
        {
            _mainViewModel.CurrentViewModel = new HomeViewModel(_mainViewModel);
        }

        private async Task CargarAsync()
        {
            try
            {
                using (var sPac = new ServicePaciente())
                using (var sUsu = new ServiceUsuario())
                using (var sPrue = new ServicePrueba())
                {
                    var pacientes = await sPac.ListarPacientesAsync();
                    var usuarios = await sUsu.ListarUsuariosAsync();
                    var pruebas = await sPrue.ListarPruebasAsync();

                    Pacientes = new ObservableCollection<Paciente>(pacientes);
                    Usuarios = new ObservableCollection<Usuario>(usuarios);

                    _pruebasOriginal = new ObservableCollection<Prueba>(pruebas);
                    Pruebas = new ObservableCollection<Prueba>(pruebas);

                    PacienteSeleccionado = Pacientes.FirstOrDefault();
                    UsuarioSeleccionado = Usuarios.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
        }

        private void MostrarFormulario()
        {
            FormVisible = true;
            TipoAnalisis = "";
            Resultados = "";
        }

        private void OcultarFormulario()
        {
            FormVisible = false;
        }

        private async Task GuardarAsync()
        {
            try
            {
                if (PacienteSeleccionado == null)
                {
                    MessageBox.Show("Selecciona un paciente.");
                    return;
                }

                if (UsuarioSeleccionado == null)
                {
                    MessageBox.Show("Selecciona un usuario/médico.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(TipoAnalisis))
                {
                    MessageBox.Show("Introduce el tipo de análisis.");
                    return;
                }

                var nueva = new Prueba
                {
                    Fecha = DateTime.Now,
                    TipoAnalisis = TipoAnalisis.Trim(),
                    Estado = EstadoPrueba.Pendiente,
                    Resultados = string.IsNullOrWhiteSpace(Resultados) ? null : Resultados.Trim(),
                    IdPaciente = PacienteSeleccionado.IdPaciente,
                    IdUsuario = UsuarioSeleccionado.IdUsuario
                };

                using (var service = new ServicePrueba())
                {
                    await service.CrearPruebaAsync(nueva);
                }

                OcultarFormulario();
                await CargarAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message);
            }
        }

        private void Filtrar()
        {
            string t = TextoBusqueda?.Trim().ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(t))
            {
                Pruebas = new ObservableCollection<Prueba>(_pruebasOriginal);
                return;
            }

            var filtradas = _pruebasOriginal.Where(p =>
                (p.Paciente != null && ((p.Paciente.Nombre + " " + p.Paciente.Apellidos).ToLower().Contains(t)))
                || (!string.IsNullOrEmpty(p.TipoAnalisis) && p.TipoAnalisis.ToLower().Contains(t))
                || (!string.IsNullOrEmpty(p.Resultados) && p.Resultados.ToLower().Contains(t))
            );

            Pruebas = new ObservableCollection<Prueba>(filtradas);
        }
    }

}
