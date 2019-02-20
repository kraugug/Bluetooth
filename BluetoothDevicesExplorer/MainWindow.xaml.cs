using InTheHand;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BluetoothDevicesExplorer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Fields

		public static readonly RoutedCommand CommandDeviceProperties = new RoutedCommand();
		public static readonly RoutedCommand CommandRefreshDeviceList = new RoutedCommand();

		#endregion

		#region Properties

		public List<BluetoothDeviceInfo> Devices
		{
			get { return (List<BluetoothDeviceInfo>)GetValue(DevicesProperty); }
			set { SetValue(DevicesProperty, value); }
		}
		public static readonly DependencyProperty DevicesProperty = DependencyProperty.Register("Devices", typeof(List<BluetoothDeviceInfo>), typeof(MainWindow), new UIPropertyMetadata(new List<BluetoothDeviceInfo>()));

		public bool IsDiscovering
		{
			get { return (bool)GetValue(IsDiscoveringProperty); }
			set { SetValue(IsDiscoveringProperty, value); }
		}
		public static readonly DependencyProperty IsDiscoveringProperty = DependencyProperty.Register("IsDiscovering", typeof(bool), typeof(MainWindow), new UIPropertyMetadata(false));

		public bool IsGatheringInformation
		{
			get { return (bool)GetValue(IsGatheringInformationProperty); }
			set { SetValue(IsGatheringInformationProperty, value); }
		}
		public static readonly DependencyProperty IsGatheringInformationProperty = DependencyProperty.Register("IsGatheringInformation", typeof(bool), typeof(MainWindow), new UIPropertyMetadata(false));

		#endregion

		#region Methods

		private void CommandDeviceProperties_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			BluetoothDeviceInfo device = ListViewDevices.SelectedItem as BluetoothDeviceInfo;
			e.CanExecute = device != null && device.Connected;
		}

		private void CommandDeviceProperties_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			BluetoothDeviceInfo device = ListViewDevices.SelectedItem as BluetoothDeviceInfo;
			device.ShowDialog();
		}

		private void CommandRefreshDeviceList_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = !IsDiscovering;
		}

		private void CommandRefreshDeviceList_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Devices.Clear();
			IsDiscovering = true;
			ListViewDevices.UnselectAll();
			TextBoxInfo.Clear();
			Task.Factory.StartNew(() =>
			{
				BluetoothDeviceInfo[] devices = null;
				using (BluetoothClient btClient = new BluetoothClient())
				{
					devices = btClient.DiscoverDevices(100, Properties.Settings.Default.Authentificated, Properties.Settings.Default.Remembered,
						Properties.Settings.Default.Unknown, Properties.Settings.Default.DiscoverableOnly);
				}
				Dispatcher.Invoke(new Action(() =>
				{
					Devices = new List<BluetoothDeviceInfo>(devices.Where(i => Properties.Settings.Default.ConnectedOnly ? i.Connected : true));
					IsDiscovering = false;
					CommandManager.InvalidateRequerySuggested();
				}));
			});
		}

		private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			BluetoothDeviceInfo device = (sender as ListView).SelectedItem as BluetoothDeviceInfo;
			if (device != null && device.Connected)
				device.ShowDialog();
		}

		private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			BluetoothDeviceInfo device = (sender as ListView).SelectedItem as BluetoothDeviceInfo;
			if (device != null)
			{
				if (device.InstalledServices.Length > 0)
				{
					Task.Factory.StartNew(() =>
					{
						Dispatcher.Invoke(new Action(() =>
						{
							IsGatheringInformation = true;
							TextBoxInfo.Clear();
						}));
						try
						{
							foreach (var guid in device.InstalledServices)
							{
								ServiceRecord[] serviceRecords = device.GetServiceRecords(guid);
								Dispatcher.Invoke(new Action(() =>
								{
									TextBoxInfo.Text += string.Format("=========== Service: {0} ({1}) ==========={2}{2}", BluetoothService.GetName(guid), guid, Environment.NewLine);
									foreach (var serviceRecord in serviceRecords)
									{
										TextBoxInfo.Text += ServiceRecordUtilities.Dump(serviceRecord) + Environment.NewLine;
									}
								}));
							}
						}
						catch { Dispatcher.Invoke(new Action(() => { TextBoxInfo.Text = string.Format("No available information for '{0}'. Try to connect/turn-on the device.", device.DeviceName); })); }
					}).ContinueWith((Task task) => { Dispatcher.Invoke(new Action(() => { IsGatheringInformation = false; })); });
				}
				else
					TextBoxInfo.Text = "No installed services.";
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Properties.Settings.Default.Save();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			CommandRefreshDeviceList.Execute(null, null);
		}

		#endregion

		#region Constructor

		public MainWindow()
		{
			DataContext = this;
			InitializeComponent();
		}

		#endregion
	}
}
