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
                    1 => "Janvier",
                    2 => "Février",
                    3 => "Mars",
                    4 => "Avril",
                    5 => "Mai",
                    6 => "Juin",
                    7 => "Juillet",
                    8 => "Août",
                    9 => "Septembre",
                    10 => "Octobre",
                    11 => "Novembre",
                    12 => "Décembre",
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
