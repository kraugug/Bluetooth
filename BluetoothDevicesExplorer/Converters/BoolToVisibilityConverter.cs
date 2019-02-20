using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using BluetoothDevicesExplorer.Converters.Generic;

namespace BluetoothDevicesExplorer.Converters
{
	[ValueConversion(typeof(bool), typeof(Visibility))]
	public class BoolToVisibilityConverter : MarkupValueConverter
	{
		public Visibility FalseValue { get; set; }

		public bool IsInverted { get; set; }

		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is bool))
				throw new ArgumentException("Must be type of bool", nameof(value));
			if (IsInverted)
				return (bool)value ? FalseValue : Visibility.Visible;
			return (bool)value ? Visibility.Visible : FalseValue;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is Visibility))
				throw new ArgumentException("Must be type of Visibility", nameof(value));
			if (IsInverted)
				return (Visibility)value != Visibility.Visible;
			return (Visibility)value == Visibility.Visible;
		}

		public BoolToVisibilityConverter()
		{
			FalseValue = Visibility.Collapsed;
		}
	}
}
