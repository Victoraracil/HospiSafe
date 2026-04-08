using HospiSafe.Services;
using HospiSafe.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HospiSafe.ViewModels
{
    //Plantilla de modulo
    public class Modulo
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
    }

    public class HomeViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainViewModel;
        private ObservableCollection<Modulo> _modulos;

        //atributo publico de _modulos
        public ObservableCollection<Modulo> Modulos
        {
            get => _modulos;
            set => SetProperty(ref _modulos, value);
        }

        public ICommand LogoutCommand => _mainViewModel.LogoutCommand;
        public ICommand AbrirModuloCommand { get; }

        public HomeViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            //Inicializamos _modulos y cargamos
            _modulos = new ObservableCollection<Modulo>();
            LoadModules();

            AbrirModuloCommand = new RelayCommand(ExecuteAbrirModulo);

            // cambios de sesion si el usuario ha cambiado
            SessionManager.CurrentUserChanged += OnSessionChanged;
        }

        private void OnSessionChanged()
        {
            LoadModules(); //carga los modulos en caso de cambiar la sesion
        }

        //Carga modulos
        private void LoadModules()
        {
            var todosModulos = new List<Modulo>
            {
                new Modulo { Title = "Pacientes", Icon = "👥", Description = "Gestión de historial clínico y datos personales" },
                new Modulo { Title = "Citas", Icon = "📅", Description = "Programación y control de agenda médica" },
                new Modulo { Title = "Laboratorio", Icon = "🔬", Description = "Resultados de laboratorios y diagnósticos" },
                new Modulo { Title = "Radiologia", Icon = "⚡", Description = "Estudios de radiología y pruebas de imagen" },
                new Modulo { Title = "Usuarios", Icon = "⚙️", Description = "Administración de personal y permisos" }
            };

            var modulosPermitidos = todosModulos.Where(m => PuedeAcceder(m.Title)).ToList();

            // Actualizar la lista
            Modulos.Clear();
            foreach (var modulo in modulosPermitidos)
            {
                Modulos.Add(modulo);
            }
        }

        //comprueba si el usuario puede acceder
        private bool PuedeAcceder(string modulo)
        {
            return modulo switch
            {
                "Pacientes" => _mainViewModel.PuedeVerPacientes,
                "Citas" => _mainViewModel.PuedeVerCitas,
                "Laboratorio" => _mainViewModel.PuedeVerLaboratorio,
                "Radiologia" => _mainViewModel.PuedeVerRadiologia,
                "Usuarios" => _mainViewModel.PuedeVerUsuarios,
                _ => false //default
            };
        }

        private void ExecuteAbrirModulo(object parameter)
        {
            if (parameter is not Modulo modulo)
                return;

            switch (modulo.Title)
            {
                case "Pacientes":
                    _mainViewModel.CurrentViewModel = new PacientesViewModel();
                    break;

                case "Laboratorio":
                    _mainViewModel.CurrentViewModel = new PruebasViewModel(_mainViewModel);
                    break;

                case "Radiologia":
                    MessageBox.Show("Módulo de Radiología en desarrollo", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;

                case "Usuarios":
                    _mainViewModel.CurrentViewModel = new UsuariosViewModel();
                    break;

                case "Citas":
                    _mainViewModel.CurrentViewModel = new CitasViewModel();
                    break;

                default:
                    MessageBox.Show("Módulo no implementado");
                    break;
            }
        }

    }
}
