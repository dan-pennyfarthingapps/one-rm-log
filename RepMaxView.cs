using System;
using System.Drawing;

using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace onermlog
{
	public partial class RepMaxView : UIViewController
	{
		DialogViewController _dvc;
		RootElement _logRoot;

		public RepMaxView () : base ("RepMaxView", null)
		{
			this._logRoot = new RootElement("Records");
			this._dvc = new DialogViewController(UITableViewStyle.Plain, this._logRoot, false);

			// sample data for testing
			Section logSect = new Section();
			StringElement recordString = new StringElement("200 lb.");
			logSect.Add(recordString);

			this._logRoot.Add(logSect);
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
			this.View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromBundle("images/white_carbon"));

			// Bar
			this.NavigationController.NavigationBar.SetBackgroundImage (UIImage.FromBundle ("images/navBar"), UIBarMetrics.Default);

			// Put the dialog view controller into the UIView
			this.dvcView.AddSubview(this._dvc.View);



		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;
			
			ReleaseDesignerOutlets ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	}
}

