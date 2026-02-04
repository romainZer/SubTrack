using System.Globalization;

namespace SubTrack.Converters
{
    /// <summary>
    /// Transforme une opération booléenne en string et inversement
    /// </summary>
    public class OperationToTextConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool operationState)
            {
                return operationState == true ? "Recurrent" : "Unique";
            }
            throw new Exception("This value is not a boolean and cannot be converted");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not null and string operationString)
            {
                string loweredOperationString = operationString.ToLower();
                switch (loweredOperationString)
                {
                    case "recurrent":
                        return true;
                    case "not recurrent":
                        return false;
                    default:
                        throw new Exception($"Expected \"recurrent\" or \"not recurrent\" and got {loweredOperationString}");
                }
            }
            throw new Exception("This value doesnt fit the required type, and cannot be converted.");
        }
    }
}
