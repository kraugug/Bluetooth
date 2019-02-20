using InTheHand.Net;
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
	[ValueConversion(typeof(BluetoothAddress), typeof(string))]
	public class BluetoothAddressToFormattedStringConverter : ObjectToFormattedStringConverter<BluetoothAddress>
	{
		#region Methods

		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (value as BluetoothAddress).ToString("C");
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
