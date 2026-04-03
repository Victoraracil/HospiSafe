using HospiSafe.Services;
using System.Configuration;
using System.Data;
using System.Text;
using System.Windows;

namespace HospiSafe
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //POPUP CON hashes para insertarlos en sql hardcodeados, luego barajamos registro real
        /*
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            StringBuilder sb = new StringBuilder();

            for (int i = 1; i <= 10; i++)
            {
                string hash = PasswordHelper.HashPassword("1234");
                sb.AppendLine($"-- Usuario {i}");
                sb.AppendLine($"'{hash}',");
            }

            MessageBox.Show(sb.ToString(), "HASHES GENERADOS");
        }
        */
    }
}
