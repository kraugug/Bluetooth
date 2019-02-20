using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using BluetoothDevicesExplorer.Converters.Generic;

namespace BluetoothDevicesExplorer.Converters
{
	[ValueConversion(typeof(bool), typeof(bool))]
	public class InvertedBoolConverter : MarkupValueConverter
	{
		#region Methods

		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is bool))
				throw new ArgumentException("Must be type of bool", nameof(value));
			return !(bool)value;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is bool))
				throw new ArgumentException("Must be type of bool", nameof(value));
			return !(bool)value;
		}

		#endregion
	}
}
