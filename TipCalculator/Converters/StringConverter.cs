using System.Globalization;

namespace TipCalculator.Converters;

public class StringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return "This is a converted value: " + value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value.ToString();
    }
}