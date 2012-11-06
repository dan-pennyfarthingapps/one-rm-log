// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace onermlog
{
	[Register ("ConfigAboutScreen")]
	partial class ConfigAboutScreen
	{
		[Outlet]
		MonoTouch.UIKit.UIView viewDVC { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblVersion { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (viewDVC != null) {
				viewDVC.Dispose ();
				viewDVC = null;
			}

			if (lblVersion != null) {
				lblVersion.Dispose ();
				lblVersion = null;
			}
		}
	}
}
