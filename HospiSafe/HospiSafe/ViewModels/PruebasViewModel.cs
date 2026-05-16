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

        //prueba seleccionada para a futuro que abrir el informe
        private Prueba _PruebaSeleccionada;
        public Prueba PruebaSeleccionada
        {
            get => _PruebaSeleccionada;
            set => SetProperty(ref _PruebaSeleccionada, value);
        }

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

        // contenido opcional del informe inicial
        private string _informeContenido = "";
        public string InformeContenido
        {
            get => _informeContenido;
            set => SetProperty(ref _informeContenido, value);
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
        public ICommand VerInformeCommand { get; }

        public PruebasViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            CargarCommand = new RelayCommand(async _ => await InicializarAsync());
            BuscarCommand = new RelayCommand(async _ => await BuscarPacientesAsync());
            NuevoCommand = new RelayCommand(_ => MostrarFormulario());
            GuardarCommand = new RelayCommand(async _ => await GuardarAsync());
            CancelarCommand = new RelayCommand(_ => OcultarFormulario());
            VolverCommand = new RelayCommand(_ => Volver());
            VerInformeCommand = new RelayCommand(param => AbrirInforme(param));

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

        private void AbrirInforme(object? parameter = null)
        {
            Prueba pruebaAbrir = null;

            if (parameter is Prueba p)
                pruebaAbrir = p;
            else if (PruebaSeleccionada != null)
                pruebaAbrir = PruebaSeleccionada;
            else
            {
                MessageBox.Show("Selecciona una prueba.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (pruebaAbrir.Informes == null || pruebaAbrir.Informes.Count == 0)
            {
                MessageBox.Show("Esta prueba aún no tiene informe.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                //var dlg = new Views.InformeDialog(pruebaAbrir.IdPrueba);
                //dlg.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir informe: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.WriteLine($"Error al abrir informe: {ex}");
            }
        }

        private void MostrarFormulario()
        {
            FormVisible = true;
            TipoAnalisis = "";
            InformeContenido = "";
        }

        private void OcultarFormulario()
        {
            FormVisible = false;
            InformeContenido = "";
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
                    IdPaciente = PacienteSeleccionado.IdPaciente,
                    IdUsuario = UsuarioSeleccionado.IdUsuario
                };

                using (var service = new ServicePrueba())
                {
                    // crear prueba y obtener Id generado
                    var idCreada = await service.CrearPruebaAsync(nueva);

                    if (idCreada > 0)
                    {
                        // solo crear informe si el contenido no está vacío tras trim
                        var contenidoTrim = (InformeContenido ?? string.Empty).Trim();
                        if (!string.IsNullOrEmpty(contenidoTrim))
                        {
                            var informe = new Informe
                            {
                                Fecha = DateTime.UtcNow,
                                Contenido = contenidoTrim,
                                IdPrueba = idCreada,
                                IdPaciente = PacienteSeleccionado.IdPaciente
                            };

                            using (var serviceInf = new ServiceInforme())
                            {
                                var creado = await serviceInf.CrearInformeAsync(informe);
                                if (!creado)
                                {
                                    MessageBox.Show("Prueba creada, pero no se pudo crear el informe.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                            }
                        }

                        MessageBox.Show("Prueba creada correctamente.");

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
                    else
                    {
                        MessageBox.Show("Error al crear la prueba.");
                    }
                }
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
