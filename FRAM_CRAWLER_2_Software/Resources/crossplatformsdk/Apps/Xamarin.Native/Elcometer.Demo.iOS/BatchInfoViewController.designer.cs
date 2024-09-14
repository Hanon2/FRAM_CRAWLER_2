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
	[Register ("BatchInfoViewController")]
	partial class BatchInfoViewController
	{
		[Outlet]
		UIKit.UITextView BatchInfoTextView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (BatchInfoTextView != null) {
				BatchInfoTextView.Dispose ();
				BatchInfoTextView = null;
			}
		}
	}
}
