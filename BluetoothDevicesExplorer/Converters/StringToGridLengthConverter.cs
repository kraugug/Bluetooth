using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using BluetoothDevicesExplorer.Converters.Generic;

namespace BluetoothDevicesExplorer.Converters
{
	[ValueConversion(typeof(string), typeof(GridLength))]
	public class StringToGridLengthConverter : MarkupValueConverter
	{
		private GridLengthConverter m_Converter = new GridLengthConverter();

		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string v = value.ToString().Replace("*", string.Empty);
			return m_Converter.ConvertFrom(v == string.Empty ? value : v);
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return m_Converter.ConvertTo(value, targetType);
		}
	}
}
