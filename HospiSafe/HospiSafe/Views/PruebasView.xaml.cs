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
using System.Windows.Shapes;

namespace HospiSafe.Views
{
    /// <summary>
    /// Lógica de interacción para PruebasView.xaml
    /// </summary>
    public partial class PruebasView : UserControl
    {
        public PruebasView()
        {
            InitializeComponent();
            Loaded += PruebasView_Loaded;
        }

        private void PruebasView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is PruebasViewModel vm)
            {
                vm.CargarCommand.Execute(null);
            }
        }
    }

}
