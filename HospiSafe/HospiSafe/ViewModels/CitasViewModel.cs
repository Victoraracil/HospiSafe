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
    public class CitasViewModel : BaseViewModel
    {
        private ServiceCita _serviceCita;
        private ServicePaciente _servicePaciente;
        private ServiceUsuario _serviceUsuario;

        private ObservableCollection<Cita> _listaCitas;
        public ObservableCollection<Cita> ListaCitas
        {
            get => _listaCitas;
            set => SetProperty(ref _listaCitas, value);
        }

        private Cita _citaSeleccionada;
        public Cita CitaSeleccionada
        {
            get => _citaSeleccionada;
            set
            {
                if (SetProperty(ref _citaSeleccionada, value) && value != null)
                {
                    // Llenar formulario con datos de la selección
                    Fecha = value.Fecha;
                    Estado = value.Estado;
                    PacienteSeleccionado = ListaPacientes?.FirstOrDefault(p => p.IdPaciente == value.IdPaciente);
                    UsuarioSeleccionado = ListaUsuarios?.FirstOrDefault(u => u.IdUsuario == value.IdUsuario);
                }
            }
        }

        // Form properties
        private DateTime _fecha = DateTime.Now;
        public DateTime Fecha
        {
            get => _fecha;
            set => SetProperty(ref _fecha, value);
        }

        private EstadoCita _estado;
        public EstadoCita Estado
        {
            get => _estado;
            set => SetProperty(ref _estado, value);
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

        // ComboBox Lists
        private List<Paciente> _listaPacientes;
        public List<Paciente> ListaPacientes
        {
            get => _listaPacientes;
            set => SetProperty(ref _listaPacientes, value);
        }

        private List<Usuario> _listaUsuarios;
        public List<Usuario> ListaUsuarios
        {
            get => _listaUsuarios;
            set => SetProperty(ref _listaUsuarios, value);
        }

        public IEnumerable<EstadoCita> EstadosCita => Enum.GetValues(typeof(EstadoCita)).Cast<EstadoCita>();


        // Commands
        public ICommand CargarCommand { get; }
        public ICommand GuardarCommand { get; }
        public ICommand EliminarCommand { get; }
        public ICommand LimpiarCommand { get; }

        public CitasViewModel()
        {
            _serviceCita = new ServiceCita();
            _servicePaciente = new ServicePaciente();
            _serviceUsuario = new ServiceUsuario();

            CargarCommand = new RelayCommand(async _ => await CargarDatos());
            GuardarCommand = new RelayCommand(async _ => await GuardarCita());
            EliminarCommand = new RelayCommand(async _ => await EliminarCita());
            LimpiarCommand = new RelayCommand(_ => LimpiarFormulario());

            // Initial load maybe? Or wait for view Loaded event
        }

        private async Task CargarDatos()
        {
            var citas = await _serviceCita.ListarCitasAsync();
            ListaCitas = new ObservableCollection<Cita>(citas);

            ListaPacientes = await _servicePaciente.ListarPacientesAsync();
            ListaUsuarios = await _serviceUsuario.ListarUsuariosAsync();
        }

        private async Task GuardarCita()
        {
            if (PacienteSeleccionado == null || UsuarioSeleccionado == null)
            {
                MessageBox.Show("Debe seleccionar un paciente y un usuario (doctor).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (CitaSeleccionada != null)
                {
                    // Update
                    CitaSeleccionada.Fecha = Fecha;
                    CitaSeleccionada.Estado = Estado;
                    CitaSeleccionada.IdPaciente = PacienteSeleccionado.IdPaciente;
                    CitaSeleccionada.IdUsuario = UsuarioSeleccionado.IdUsuario;

                    await _serviceCita.ActualizarCitaAsync(CitaSeleccionada);
                    MessageBox.Show("Cita actualizada correctamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Create
                    var nuevaCita = new Cita
                    {
                        Fecha = Fecha,
                        Estado = Estado,
                        IdPaciente = PacienteSeleccionado.IdPaciente,
                        IdUsuario = UsuarioSeleccionado.IdUsuario
                    };

                    await _serviceCita.CrearCitaAsync(nuevaCita);
                    MessageBox.Show("Cita creada correctamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                await CargarDatos();
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task EliminarCita()
        {
            if (CitaSeleccionada == null) return;

            var result = MessageBox.Show("¿Está seguro de eliminar esta cita?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _serviceCita.EliminarCitaAsync(CitaSeleccionada.IdCita);
                    await CargarDatos();
                    LimpiarFormulario();
                    MessageBox.Show("Cita eliminada correctamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LimpiarFormulario()
        {
            CitaSeleccionada = null;
            Fecha = DateTime.Now;
            Estado = EstadoCita.Activa;
            PacienteSeleccionado = null;
            UsuarioSeleccionado = null;
        }
    }
}
