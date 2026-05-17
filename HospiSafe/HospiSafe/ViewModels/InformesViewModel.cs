using HospiSafe.Models;
using HospiSafe.Services;
using HospiSafe.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HospiSafe.ViewModels
{
    public class InformesViewModel : BaseViewModel
    {
        private readonly MainViewModel _main;
        private readonly int? _inicioPacienteId;
        private readonly int? _inicioInformeId;

        public ObservableCollection<Paciente> PacientesFiltrados { get; } = new ObservableCollection<Paciente>();
        public ObservableCollection<Informe> Informes { get; } = new ObservableCollection<Informe>();

        private Paciente _pacienteSeleccionado;
        public Paciente PacienteSeleccionado
        {
            get => _pacienteSeleccionado;
            set
            {
                if (SetProperty(ref _pacienteSeleccionado, value))
                {
                    if (value != null) _ = CargarInformesDelPacienteAsync(value.IdPaciente);
                }
            }
        }

        private Informe _informeSeleccionado;
        public Informe InformeSeleccionado
        {
            get => _informeSeleccionado;
            set
            {
                if (SetProperty(ref _informeSeleccionado, value))
                {
                    ContenidoActual = _informeSeleccionado?.Contenido ?? string.Empty;
                }
            }
        }

        private string _terminoBusqueda = string.Empty;
        public string TerminoBusqueda
        {
            get => _terminoBusqueda;
            set
            {
                if (SetProperty(ref _terminoBusqueda, value))
                {
                    _ = FiltrarPacientesAsync();
                }
            }
        }

        private string _contenidoActual = string.Empty;
        public string ContenidoActual
        {
            get => _contenidoActual;
            set => SetProperty(ref _contenidoActual, value);
        }

        public ICommand CargarCommand { get; }
        public ICommand BuscarCommand { get; }
        public ICommand SeleccionarPacienteCommand { get; }
        public ICommand SeleccionarInformeCommand { get; }
        public ICommand GuardarCommand { get; }
        public ICommand DescargarCommand { get; }

        public InformesViewModel(MainViewModel main, int? pacienteId = null, int? informeId = null)
        {
            _main = main;
            _inicioPacienteId = pacienteId;
            _inicioInformeId = informeId;

            CargarCommand = new RelayCommand(async _ => await InicializarAsync());
            BuscarCommand = new RelayCommand(async _ => await FiltrarPacientesAsync());
            SeleccionarPacienteCommand = new RelayCommand(async p => await SeleccionarPacienteAsync(p as Paciente));
            SeleccionarInformeCommand = new RelayCommand(i => SeleccionarInforme(i as Informe));
            GuardarCommand = new RelayCommand(async _ => await GuardarInformeAsync());
            DescargarCommand = new RelayCommand(_ => DescargarInforme());

            _ = InicializarAsync();
        }

        private async Task InicializarAsync()
        {
            try
            {
                // si llega paciente inicial: cargar solo el y sus informes
                if (_inicioPacienteId.HasValue)
                {
                    using var svcP = new ServicePaciente();
                    var todos = await svcP.ListarPacientesAsync();
                    var paciente = todos.FirstOrDefault(x => x.IdPaciente == _inicioPacienteId.Value);
                    if (paciente != null)
                    {
                        PacientesFiltrados.Clear();
                        PacientesFiltrados.Add(paciente);
                        PacienteSeleccionado = paciente;
                        await CargarInformesDelPacienteAsync(paciente.IdPaciente);

                        if (_inicioInformeId.HasValue)
                        {
                            InformeSeleccionado = Informes.FirstOrDefault(i => i.IdInforme == _inicioInformeId.Value)
                                                 ?? Informes.FirstOrDefault();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar Informes: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async Task FiltrarPacientesAsync()
        {
            try
            {
                using var svc = new ServicePaciente();

                var todos = await svc.ListarPacientesAsync();

                var t = (TerminoBusqueda ?? string.Empty).Trim().ToLower();

                // si no hay texto, limpiar resultados
                if (string.IsNullOrWhiteSpace(t))
                {
                    PacientesFiltrados.Clear();
                    return;
                }

                var filtrados = todos.Where(p =>
                    (
                    p.Nombre + " " + p.Apellidos).ToLower().Contains(t) ||
                    p.DNI.ToLower().Contains(t) ||
                    p.NumSS.ToLower().Contains(t)
                    ).ToList();

                PacientesFiltrados.Clear();

                foreach (var p in filtrados)
                {
                    PacientesFiltrados.Add(p);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar pacientes: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task SeleccionarPacienteAsync(Paciente paciente)
        {
            if (paciente == null) return;
            PacienteSeleccionado = paciente;
            await CargarInformesDelPacienteAsync(paciente.IdPaciente);
        }

        private void SeleccionarInforme(Informe informe)
        {
            if (informe == null) return;
            InformeSeleccionado = informe;
        }

        private async Task CargarInformesDelPacienteAsync(int idPaciente)
        {
            try
            {
                using var svc = new ServiceInforme();
                var all = await svc.ListarInformesAsync();
                var list = all.Where(i => i.IdPaciente == idPaciente)
                              .OrderByDescending(i => i.Fecha)
                              .ToList();

                Informes.Clear();
                foreach (var inf in list) Informes.Add(inf);
                if (_inicioInformeId.HasValue)
                {
                    InformeSeleccionado = Informes.FirstOrDefault(i => i.IdInforme == _inicioInformeId.Value) ?? Informes.FirstOrDefault();
                }
                else
                {
                    InformeSeleccionado = Informes.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar informes: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task GuardarInformeAsync()
        {
            if (InformeSeleccionado == null)
            {
                MessageBox.Show("Selecciona un informe.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var contenidoTrim = (ContenidoActual ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(contenidoTrim))
            {
                MessageBox.Show("El contenido del informe no puede estar vacío.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                InformeSeleccionado.Contenido = contenidoTrim;
                InformeSeleccionado.Fecha = DateTime.UtcNow;

                using var svc = new ServiceInforme();
                var ok = await svc.ActualizarInformeAsync(InformeSeleccionado);
                if (ok)
                {
                    MessageBox.Show("Informe guardado.", "Ok", MessageBoxButton.OK, MessageBoxImage.Information);
                    await CargarInformesDelPacienteAsync(InformeSeleccionado.IdPaciente);
                }
                else
                {
                    MessageBox.Show("No se pudo guardar el informe.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                //sacamos por debug y vemos
                Debug.WriteLine("BUG INFORMES -> " + ex + " ");
            }
        }

        private void DescargarInforme()
        {
            // metemos el questpdf y que descargue el informe en pdc
            MessageBox.Show("Funcionalidad de descarga pendiente de implementar.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}