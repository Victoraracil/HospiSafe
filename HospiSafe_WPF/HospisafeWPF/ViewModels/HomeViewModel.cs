using HospiSafe_WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;

namespace HospiSafe_WPF.ViewModels
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

        public string UserInitial { get; set; } = "U";
        public string UserName { get; set; } = "Usuario";
        public string UserRole { get; set; } = "Rol";

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
            if (parameter is Module module)
            {
                MessageBox.Show($"Abriendo módulo: {module.Title}");
                // Future: Navigate to specific ViewModel
                // _mainViewModel.CurrentViewModel = new PacientesViewModel();
            }
        }
    }
}
