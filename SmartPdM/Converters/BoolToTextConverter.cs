using System.Globalization;

namespace SmartPdM.Converters;

public class BoolToTextConverter : IValueConverter
{
    // ConverterParameter: "TrueText|FalseText" 형식
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var s = parameter?.ToString() ?? "켜짐|꺼짐";
        var parts = s.Split('|');
        var trueText = parts.Length > 0 ? parts[0] : "켜짐";
        var falseText = parts.Length > 1 ? parts[1] : "꺼짐";
        return (value is bool b && b) ? trueText : falseText;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
