using HospiSafe.Models;
using HospiSafe.Services;
using HospiSafe.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HospiSafe.ViewModels
{
    public class PruebasViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainViewModel;

        public string Titulo { get; } = "Laboratorio";

        // estado de la busqueda
        private bool _busquedaRealizada = false;
        public bool BusquedaRealizada
        {
            get => _busquedaRealizada;
            set => SetProperty(ref _busquedaRealizada, value);
        }

        // texto de busqueda
        private string _textoBusqueda = "";
        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set
            {
                if (SetProperty(ref _textoBusqueda, value))
                    FiltrarPacientes();
            }
        }

        // pacientes filtrados segun la busqueda
        private ObservableCollection<Paciente> _pacientesFiltrados = new ObservableCollection<Paciente>();
        public ObservableCollection<Paciente> PacientesFiltrados
        {
            get => _pacientesFiltrados;
            set => SetProperty(ref _pacientesFiltrados, value);
        }

        // Paciente seleccionado en la busqueda
        private Paciente _pacienteSeleccionado;
        public Paciente PacienteSeleccionado
        {
            get => _pacienteSeleccionado;
            set
            {
                if (SetProperty(ref _pacienteSeleccionado, value))
                {
                    if (value != null)
                        CargarPruebasDelPaciente(value.IdPaciente);
                    else
                        Pruebas.Clear();
                }
            }
        }

        // pruebas del paciente
        private ObservableCollection<Prueba> _pruebas = new ObservableCollection<Prueba>();
        public ObservableCollection<Prueba> Pruebas
        {
            get => _pruebas;
            set => SetProperty(ref _pruebas, value);
        }

        /* prueba seleccionada para a futuro que abrir el informe
        private Prueba _pruebaSeleccionada;
        public Prueba PruebaSeleccionada
        {
            get => _pruebaSeleccionada;
            set
            {
                if (SetProperty(ref _pruebaSeleccionada, value))
                {
                    if (value != null)
                        VerInformePrueba(value);
                }
            }
        }*/

        // usuarios para nuevo informe 
        /*

        REVISARRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR
        CARGAR TODOS USUARIOS?? SOLO PACIENTES? SOLO MEDICOS

        */
        private ObservableCollection<Usuario> _usuarios = new ObservableCollection<Usuario>();
        public ObservableCollection<Usuario> Usuarios
        {
            get => _usuarios;
            set => SetProperty(ref _usuarios, value);
        }

        // crear nueva prueba, usuario seleccionado
        private Usuario _usuarioSeleccionado;
        public Usuario UsuarioSeleccionado
        {
            get => _usuarioSeleccionado;
            set => SetProperty(ref _usuarioSeleccionado, value);
        }

        // form nueva prueba
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

        private bool _formVisible;
        public bool FormVisible
        {
            get => _formVisible;
            set => SetProperty(ref _formVisible, value);
        }

        // Comandos
        public ICommand CargarCommand { get; }
        public ICommand BuscarCommand { get; }
        public ICommand NuevoCommand { get; }
        public ICommand GuardarCommand { get; }
        public ICommand CancelarCommand { get; }
        public ICommand VolverCommand { get; }

        public PruebasViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            CargarCommand = new RelayCommand(async _ => await InicializarAsync());
            BuscarCommand = new RelayCommand(async _ => await BuscarPacientesAsync());
            NuevoCommand = new RelayCommand(_ => MostrarFormulario());
            GuardarCommand = new RelayCommand(async _ => await GuardarAsync());
            CancelarCommand = new RelayCommand(_ => OcultarFormulario());
            VolverCommand = new RelayCommand(_ => Volver());

            FormVisible = false;
            BusquedaRealizada = false;
        }

        private async Task InicializarAsync()
        {
            try
            {
                using (var serviceUsuario = new ServiceUsuario())
                {
                    var usuarios = await serviceUsuario.ListarUsuariosAsync();
                    Usuarios = new ObservableCollection<Usuario>(usuarios);
                    UsuarioSeleccionado = Usuarios.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task FiltrarPacientesAsync()
        {
            string t = TextoBusqueda?.Trim().ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(t))
            {
                PacientesFiltrados.Clear();
                BusquedaRealizada = false;
                return;
            }

            try
            {
                using (var servicePaciente = new ServicePaciente())
                {
                    var todosPacientes = await servicePaciente.ListarPacientesAsync();

                    var filtrados = todosPacientes.Where(p =>
                        (p.Nombre + " " + p.Apellidos).ToLower().Contains(t)
                        || p.DNI.ToLower().Contains(t)
                        || p.NumSS.ToLower().Contains(t)
                    ).ToList(); // lista pacientes filtrados

                    PacientesFiltrados = new ObservableCollection<Paciente>(filtrados);
                    BusquedaRealizada = true; //completada la busqueda
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar pacientes.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FiltrarPacientes()
        {
            _ = FiltrarPacientesAsync();
        }

        private async Task BuscarPacientesAsync()
        {
            await FiltrarPacientesAsync();
        }

        // cargar pruebas del paciente seleccionado
        private async void CargarPruebasDelPaciente(int idPaciente)
        {
            try
            {
                using (var servPruebas = new ServicePrueba())
                {
                    var pruebas = await servPruebas.ListarPruebasAsync();
                    var pruebasDelPaciente = pruebas.Where(p => p.IdPaciente == idPaciente).ToList();

                    Pruebas = new ObservableCollection<Prueba>(pruebasDelPaciente);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar pruebas del paciente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /* ver informe
        private void VerInformePrueba(Prueba prueba)
        {
            //Implementar vista o box window con el informe
        }*/

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
                    var creada = await service.CrearPruebaAsync(nueva);
                }

                // log
                try
                {
                    using var slog = new ServiceLog();
                    await slog.CrearLogAsync(SessionManager.CurrentUser?.IdUsuario,
                        $"Creó prueba de laboratorio TipoAnalisis={TipoAnalisis} PacienteId={PacienteSeleccionado.IdPaciente}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error al registrar log: {ex}");
                }

                OcultarFormulario();
                CargarPruebasDelPaciente(PacienteSeleccionado.IdPaciente);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message);
            }
        }

        private void Volver()
        {
            _mainViewModel.CurrentViewModel = new HomeViewModel(_mainViewModel);
        }
    }
}
