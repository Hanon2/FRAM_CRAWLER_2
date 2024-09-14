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
	[Register ("GaugeViewController")]
	partial class GaugeViewController
	{
		[Outlet]
		UIKit.UIButton DownloadBatchesButton { get; set; }

		[Outlet]
		UIKit.UITextView LiveReadingTextView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (DownloadBatchesButton != null) {
				DownloadBatchesButton.Dispose ();
				DownloadBatchesButton = null;
			}

			if (LiveReadingTextView != null) {
				LiveReadingTextView.Dispose ();
				LiveReadingTextView = null;
			}
		}
	}
}
