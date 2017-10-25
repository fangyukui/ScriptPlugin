using System;
using System.Globalization;
using System.Windows.Data;
using ScriptPlugin.Common.Extensions;

namespace ScriptPlugin.Common.Converters
{
    public class EnumToDescConverter<T> : IValueConverter where T : struct
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                T state;
                Enum.TryParse(value.ToString(), out state);
                return typeof(T).GetEnumDesc(System.Convert.ToInt32(state));
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
