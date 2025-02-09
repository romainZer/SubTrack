using System.Globalization;

namespace SubTrack.Converters
{
    /// <summary>
    /// Transforme une dépense booléenne en string et inversement
    /// </summary>
    public class ExpenseToTextConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool expenseState)
            {
                return expenseState == true ? "Recurrent" : "Unique";
            }
            throw new Exception("This value is not a boolean and cannot be converted");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not null and string expenseString)
            {
                string loweredExpenseString = expenseString.ToLower();
                switch (loweredExpenseString)
                {
                    case "recurrent":
                        return true;
                    case "not recurrent":
                        return false;
                    default:
                        throw new Exception($"Expected \"recurrent\" or \"not recurrent\" and got {loweredExpenseString}");
                }
            }
            throw new Exception("This value doesnt fit the required type, and cannot be converted.");
        }
    }
}
