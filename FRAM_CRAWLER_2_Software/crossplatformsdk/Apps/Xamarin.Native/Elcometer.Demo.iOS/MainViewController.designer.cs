// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Elcometer.Demo.iOS
{
	[Register ("MainViewController")]
	partial class MainViewController
	{
		[Outlet]
		UIKit.UIButton ConnectBluetoothClassicButton { get; set; }

		[Outlet]
		UIKit.UIButton ConnectBluetoothLEButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UITableView GaugesTable { get; set; }

		[Action ("ConnectBluetoothClassicTouch:")]
		partial void ConnectBluetoothClassicTouch (UIKit.UIButton sender);

		[Action ("ConnectBluetoothLETouch:")]
		partial void ConnectBluetoothLETouch (UIKit.UIButton sender);

		[Action ("ConnectGaugeTouch:")]
		partial void ConnectGaugeTouch (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ConnectBluetoothClassicButton != null) {
				ConnectBluetoothClassicButton.Dispose ();
				ConnectBluetoothClassicButton = null;
			}

			if (ConnectBluetoothLEButton != null) {
				ConnectBluetoothLEButton.Dispose ();
				ConnectBluetoothLEButton = null;
			}

			if (GaugesTable != null) {
				GaugesTable.Dispose ();
				GaugesTable = null;
			}
		}
	}
}
