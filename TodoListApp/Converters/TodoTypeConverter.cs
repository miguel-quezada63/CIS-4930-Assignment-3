using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Data;
namespace TodoListApp.Converters
{
    public class TodoTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ((string) parameter == (string) value);
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? parameter : null;
        }
    }
}
