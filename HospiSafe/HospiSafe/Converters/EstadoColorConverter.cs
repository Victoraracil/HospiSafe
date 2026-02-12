using HospiSafe.Models;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HospiSafe.Converters
{
    public class EstadoColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is EstadoCita estado)
            {
                switch (estado)
                {
                    case EstadoCita.Activa:
                        return new SolidColorBrush(Colors.Green); // Or a specific green
                    case EstadoCita.Cancelada:
                        return new SolidColorBrush(Colors.Red); // Or a specific red
                    default:
                        return new SolidColorBrush(Colors.Gray);
                }
            }
            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
