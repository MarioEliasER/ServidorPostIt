using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ServidorPostIt.Helpers
{
    internal class PorcentajeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //recibo un valor entre 0 y 100.
            //parametro ancho o alto.
            double porcentaje = (double)value;
            double medida = (double)parameter;

            return porcentaje * medida / 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
