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

		[Outlet]
		MonoTouch.UIKit.UIButton btnAddNew { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnFb { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnTw { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblExName { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblWeightMax { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblMaxOnDate { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (dvcView != null) {
				dvcView.Dispose ();
				dvcView = null;
			}

			if (btnAddNew != null) {
				btnAddNew.Dispose ();
				btnAddNew = null;
			}

			if (btnFb != null) {
				btnFb.Dispose ();
				btnFb = null;
			}

			if (btnTw != null) {
				btnTw.Dispose ();
				btnTw = null;
			}

			if (lblExName != null) {
				lblExName.Dispose ();
				lblExName = null;
			}

			if (lblWeightMax != null) {
				lblWeightMax.Dispose ();
				lblWeightMax = null;
			}

			if (lblMaxOnDate != null) {
				lblMaxOnDate.Dispose ();
				lblMaxOnDate = null;
			}
		}
	}
}
