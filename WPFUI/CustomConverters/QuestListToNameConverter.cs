using System.Globalization;
using System.Windows.Data;

namespace WPFUI.CustomConverters
{
    public class QuestListToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object paramter, CultureInfo culture)
        {
            if(value == null)
            {
                throw new ArgumentNullException("value");
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
