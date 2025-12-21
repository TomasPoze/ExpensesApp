using System.Globalization;

namespace ExpensesApp.MAUI.Converters;

public class CategoryToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var category = value as string;

        if (string.IsNullOrWhiteSpace(category))
            return "icon_default.png";

        return category.ToLower() switch
        {
            "food" => "cutlery.png",
            "groceries" => "shopping_cart.png",
            "transport" => "car.png",
            "fuel" => "car.png",
            "entertainment" => "video.png",
            _ => "question.png"
        };
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // One-way binding only (we never convert an image back to text)
        throw new NotImplementedException();
    }
}