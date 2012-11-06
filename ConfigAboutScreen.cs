
using System;
using System.Drawing;

using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace onermlog
{
	public partial class ConfigAboutScreen : UIViewController
	{
		private UIImageView imgIcon;

		private DialogViewController _dvc;
		private RootElement _aboutRoot;
		private Section _aboutSect;

		public ConfigAboutScreen () : base ("ConfigAboutScreen", null)
		{
			this.imgIcon = new UIImageView(RectangleF.FromLTRB(20.0f, 20.0f, 78.0f, 78.0f));

			this._aboutRoot = new RootElement ("Configuration");
			this._dvc = new DialogViewController (UITableViewStyle.Grouped, this._aboutRoot, false);
			
			// load data from list
			this._aboutSect = new Section ("About");
			StringElement recordString = new StringElement ("test");
			this._aboutSect.Add(recordString);
			this._aboutRoot.Add(this._aboutSect);
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

			// Perform any additional setup after loading the view, typically from a nib.
			this.View.BackgroundColor = UIColor.FromPatternImage (UIImage.FromBundle ("images/white_carbon"));
			
			// Bar
			this.NavigationController.NavigationBar.SetBackgroundImage (UIImage.FromBundle ("images/navBar"), UIBarMetrics.Default);

			// image
			if (UIScreen.MainScreen.Scale > 1.0) {
				this.imgIcon.Image = UIImage.FromBundle("icons/icon119.png");
				Console.WriteLine("setting retina image");
			} else {
				this.imgIcon.Image = UIImage.FromBundle("icons/icon58.png");
				Console.WriteLine("setting regular image");
			}

			this.imgIcon.Hidden = false;
			this.View.AddSubview(imgIcon);


			// version #
			this.lblVersion.Text = "one rm log - v. " + NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion").ToString();


			// about screen
			this.viewDVC.AddSubview(this._dvc.View);

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

