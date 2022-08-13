using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFlasher.Utils
{
    public class HexToDecConverter : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return System.Convert.ToUInt32(((string)value).Substring(2), 16);
            }
            catch
            {
                return 0;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return "0x" + System.Convert.ToUInt32(value.ToString()).ToString("X").ToLower();
            }
            catch
            {
                return "0x0";
            }
        }
    }
}
