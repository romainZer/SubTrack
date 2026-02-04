using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubTrack.Converters
{
    public class NumberToMonthConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int month)
            {
                return month switch
                {
                    1 => "janvier",
                    2 => "février",
                    3 => "mars",
                    4 => "avril",
                    5 => "mai",
                    6 => "juin",
                    7 => "juillet",
                    8 => "août",
                    9 => "septembre",
                    10 => "octobre",
                    11 => "novembre",
                    12 => "décembre",
                    _ => throw new ArgumentOutOfRangeException(nameof(value), "Le nombre doit être entre 1 et 12")
                };
            }
            throw new ArgumentException("La valeur doit être un entier", nameof(value));
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string month)
            {
                return month.ToLower() switch
                {
                    "janvier" => 1,
                    "février" => 2,
                    "mars" => 3,
                    "avril" => 4,
                    "mai" => 5,
                    "juin" => 6,
                    "juillet" => 7,
                    "août" => 8,
                    "septembre" => 9,
                    "octobre" => 10,
                    "novembre" => 11,
                    "décembre" => 12,
                    _ => throw new ArgumentOutOfRangeException(nameof(value), "Le mois doit être un nom de mois valide")
                };
            }
            throw new ArgumentException("La valeur doit être une chaîne de caractères", nameof(value));
        }
    }
}
