// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace onermlog
{
	[Register ("RepMaxView")]
	partial class RepMaxView
	{
		[Outlet]
		MonoTouch.UIKit.UIView dvcView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (dvcView != null) {
				dvcView.Dispose ();
				dvcView = null;
			}
		}
	}
}
