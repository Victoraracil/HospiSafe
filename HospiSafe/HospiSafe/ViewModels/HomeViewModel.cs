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
    public class Module
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
    }

    public class HomeViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainViewModel;

        public ObservableCollection<Module> Modules { get; set; }

        public ICommand LogoutCommand => _mainViewModel.LogoutCommand;
        public ICommand OpenModuleCommand { get; }

        public HomeViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            LoadModules();
            OpenModuleCommand = new RelayCommand(ExecuteOpenModule);
        }

        private void LoadModules()
        {
            Modules = new ObservableCollection<Module>
            {
                new Module { Title = "Pacientes", Icon = "👥", Description = "Gestión de historial clínico y datos personales" },
                new Module { Title = "Citas", Icon = "📅", Description = "Programación y control de agenda médica" },
                new Module { Title = "Pruebas", Icon = "🔬", Description = "Resultados de laboratorios y diagnósticos" },
                new Module { Title = "Usuarios", Icon = "⚙️", Description = "Administración de personal y permisos" }
            };
        }

        private void ExecuteOpenModule(object parameter)
        {
            if (parameter is not Module module)
                return;

            switch (module.Title)
            {
                case "Pacientes":
                    _mainViewModel.CurrentViewModel = new PacientesViewModel();
                    break;

                case "Citas":
                    _mainViewModel.CurrentViewModel = new CitasViewModel();
                    break;

                case "Pruebas":
                    _mainViewModel.CurrentViewModel = new PruebasViewModel();
                    break;

                case "Usuarios":
                    _mainViewModel.CurrentViewModel = new UsuariosViewModel();
                    break;

                default:
                    MessageBox.Show("Módulo no implementado");
                    break;
            }
        }

    }
}
