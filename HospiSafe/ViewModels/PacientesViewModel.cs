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
    public class PacientesViewModel : BaseViewModel
    {
        #region Atributos

        private Paciente _paciente;
        private Paciente _pacienteSelected;

        private List<Paciente> _listaPacientes;
        private ObservableCollection<Paciente> _listaPacientesVisible;

        #endregion

        #region Propiedades públicas (Wrappers)

        public int IdPaciente
        {
            get { return _paciente.IdPaciente; }
            set { _paciente.IdPaciente = value; OnPropertyChanged(); }
        }

        public string Nombre
        {
            get { return _paciente.Nombre; }
            set { _paciente.Nombre = value; OnPropertyChanged(); }
        }

        public string Apellidos
        {
            get { return _paciente.Apellidos; }
            set { _paciente.Apellidos = value; OnPropertyChanged(); }
        }

        public string DNI
        {
            get { return _paciente.DNI; }
            set { _paciente.DNI = value; OnPropertyChanged(); }
        }

        public DateTime FechaNacimiento
        {
            get { return _paciente.FechaNacimiento; }
            set { _paciente.FechaNacimiento = value; OnPropertyChanged(); }
        }

        public string Telefono
        {
            get { return _paciente.Telefono; }
            set { _paciente.Telefono = value; OnPropertyChanged(); }
        }

        public string CorreoElectronico
        {
            get { return _paciente.CorreoElectronico; }
            set { _paciente.CorreoElectronico = value; OnPropertyChanged(); }
        }

        public string NumSS
        {
            get { return _paciente.NumSS; }
            set { _paciente.NumSS = value; OnPropertyChanged(); }
        }

        public Paciente PacienteSelected
        {
            get { return _pacienteSelected; }
            set { SetProperty(ref _pacienteSelected, value); }
        }

        public ObservableCollection<Paciente> ListaPacientesVisible
        {
            get { return _listaPacientesVisible; }
            set { SetProperty(ref _listaPacientesVisible, value); }
        }

        #endregion

        #region Comandos

        public ICommand CargarCommand { get; }
        public ICommand SelectedItemChangedCommand { get; }
        public ICommand CrearNuevoPacienteCommand { get; }
        public ICommand GuardarPacienteCommand { get; }
        public ICommand EliminarPacienteCommand { get; }
        public ICommand LimpiarDatosCommand { get; }

        #endregion

        #region Constructor

        public PacientesViewModel()
        {
            _paciente = new Paciente();
            _pacienteSelected = new Paciente();

            _listaPacientes = new List<Paciente>();
            _listaPacientesVisible = new ObservableCollection<Paciente>();

            CargarCommand = new RelayCommand(PerformCargarPacientes);
            SelectedItemChangedCommand = new RelayCommand(PerformSelectedItemChanged);
            CrearNuevoPacienteCommand = new RelayCommand(PerformCrearNuevoPaciente);
            GuardarPacienteCommand = new RelayCommand(PerformGuardarPaciente);
            EliminarPacienteCommand = new RelayCommand(PerformEliminarPaciente);
            LimpiarDatosCommand = new RelayCommand(PerformLimpiarDatos);

            PerformCrearNuevoPaciente();
        }

        #endregion

        #region Métodos

        public async void PerformCargarPacientes(object? parameter = null)
        {
            using var service = new ServicePaciente();

            _listaPacientes = await service.ListarPacientesAsync();

            ListaPacientesVisible.Clear();
            foreach (var paciente in _listaPacientes)
            {
                ListaPacientesVisible.Add(paciente);
            }
        }

        private void PerformSelectedItemChanged(object? parameter = null)
        {
            if (PacienteSelected == null)
                return;

            IdPaciente = PacienteSelected.IdPaciente;
            Nombre = PacienteSelected.Nombre;
            Apellidos = PacienteSelected.Apellidos;
            DNI = PacienteSelected.DNI;
            FechaNacimiento = PacienteSelected.FechaNacimiento;
            Telefono = PacienteSelected.Telefono;
            CorreoElectronico = PacienteSelected.CorreoElectronico;
            NumSS = PacienteSelected.NumSS;
        }

        public void PerformCrearNuevoPaciente(object? parameter = null)
        {
            IdPaciente = 0;
            Nombre = string.Empty;
            Apellidos = string.Empty;
            DNI = string.Empty;
            FechaNacimiento = DateTime.Today;
            Telefono = string.Empty;
            CorreoElectronico = string.Empty;
            NumSS = string.Empty;
        }

        public async void PerformGuardarPaciente(object? parameter = null)
        {
            using var service = new ServicePaciente();

            var pacienteGuardar = new Paciente
            {
                IdPaciente = IdPaciente,
                Nombre = Nombre,
                Apellidos = Apellidos,
                DNI = DNI,
                FechaNacimiento = FechaNacimiento,
                Telefono = Telefono,
                CorreoElectronico = CorreoElectronico,
                NumSS = NumSS
            };

            bool resultado;

            if (IdPaciente == 0)
            {
                resultado = await service.CrearPacienteAsync(pacienteGuardar);

                if (resultado)
                {
                    MessageBox.Show("Paciente creado correctamente");
                    PerformCargarPacientes();
                    PerformCrearNuevoPaciente();
                }
                else
                {
                    MessageBox.Show("Ya existe un paciente con ese DNI");
                }
            }
            else
            {
                resultado = await service.ActualizarPacienteAsync(pacienteGuardar);

                if (resultado)
                {
                    MessageBox.Show("Paciente actualizado correctamente");
                    PerformCargarPacientes();
                    PerformCrearNuevoPaciente();
                }
                else
                {
                    MessageBox.Show("Error actualizando paciente");
                }
            }
        }

        public async void PerformEliminarPaciente(object? parameter = null)
        {
            if (PacienteSelected == null)
                return;

            using var service = new ServicePaciente();

            bool eliminado = await service.EliminarPacienteAsync(PacienteSelected.IdPaciente);

            if (eliminado)
            {
                MessageBox.Show("Paciente eliminado correctamente");
                PerformCargarPacientes();
                PerformCrearNuevoPaciente();
            }
            else
            {
                MessageBox.Show("No se puede eliminar el paciente porque tiene citas asociadas");
            }
        }

        private void PerformLimpiarDatos(object? parameter = null)
        {
            PerformCrearNuevoPaciente();
        }

        #endregion
    }

}
