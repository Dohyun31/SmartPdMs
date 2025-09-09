using System.Globalization;
using Microsoft.Maui.Controls;

namespace SmartPdM.Converters;

/// <summary>
/// ConverterParameter="OFF|ON" 처럼 사용
/// </summary>
public class BoolToTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var param = (parameter as string) ?? "OFF|ON";
        var parts = param.Split('|');
        var off = parts.Length > 0 ? parts[0] : "OFF";
        var on = parts.Length > 1 ? parts[1] : "ON";
        return (value is bool b && b) ? on : off;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
