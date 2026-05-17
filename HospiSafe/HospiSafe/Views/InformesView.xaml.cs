using HospiSafe.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HospiSafe.Views
{
    /// <summary>
    /// Lógica de interacción para InformesView.xaml
    /// </summary>
    public partial class InformesView : UserControl
    {
        // Constructor por defecto accedemos desde el menu
        public InformesView()
        {
            InitializeComponent();
            Loaded += InformesView_Loaded;
        }

        public InformesView(InformesViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }

        private void InformesView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is InformesViewModel vm)
            {
                vm.CargarCommand?.Execute(null);
            }
        }
    }
}
