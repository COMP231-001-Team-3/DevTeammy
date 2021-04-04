using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace teammy
{
    public class NameToCharsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] nameWords = value.ToString().Split(' ');
            string profChars;

            bool emptyName = nameWords[0].Equals("");

            //If Project name has two or more words...then
            if (nameWords.Length >= 2 && !nameWords[1].Equals(""))
            {
                profChars = nameWords[0][0] + "" + nameWords[1][0];
            }
            else if (!emptyName)
            {
                profChars = nameWords[0][0] + "" + nameWords[0][1];
            }
            else
            {
                profChars = "";
            }
            return profChars;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
