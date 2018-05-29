namespace ExampleBrowser.View
{
	using System;
	using System.Globalization;
	using System.Windows.Data;

	public class VisibleToWidthConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var input = (bool)value;
			return input ? 200 : 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var input = (float)value;
			return input > 0;
		}
	}
}
