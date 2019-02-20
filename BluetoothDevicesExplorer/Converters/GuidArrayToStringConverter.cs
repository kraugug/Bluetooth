using BluetoothDevicesExplorer.Converters.Generic;
using InTheHand.Net.Bluetooth;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BluetoothDevicesExplorer.Converters
{
	[ValueConversion(typeof(Guid[]), typeof(string))]
	public class GuidArrayToStringConverter : MarkupValueConverter
	{
		#region Methods

		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string temp = string.Empty;
			foreach (var guid in value as Guid[])
			{
				string name = BluetoothService.GetName(guid);
				temp += (string.IsNullOrEmpty(name) ? guid.ToString() : name) + ", ";
			}
			return temp;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
