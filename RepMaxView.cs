using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using SQLite;

namespace onermlog
{
	public partial class RepMaxView : UIViewController
	{
		private DialogViewController _dvc;
		private RootElement _logRoot;

		private SQLiteConnection db;

		private Exercise _exercise;
		private List<RmLog> _rms;

		public RepMaxView (Exercise exerciseToShow) : base ("RepMaxView", null)
		{
			this._exercise = exerciseToShow;

			string dbname = "onerm.db";
			string documents = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // This goes to the documents directory for your app
			string dbPath = Path.Combine (documents, dbname);
			
			db = new SQLiteConnection (dbPath);

			this.LoadRecords();

			this._logRoot = new RootElement ("Records");
			this._dvc = new DialogViewController (UITableViewStyle.Plain, this._logRoot, false);

			// load data from list
			Section logSect = new Section ();
			foreach (RmLog rm in this._rms) {
				StringElement recordString = new StringElement (rm.Weight.ToString());
				logSect.Add(recordString);
			}


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

			// labels
			this.lblExName.Text = this._exercise.Name;

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
			return (toInterfaceOrientation != UIInterfaceOrientation.LandscapeLeft && toInterfaceOrientation != UIInterfaceOrientation.LandscapeRight);
		}

		private void LoadRecords ()
		{
			db.CreateTable<RmLog> ();
			
			this._rms = db.Query<RmLog> ("select * from RmLog where ExerciseID=?", this._exercise.ID);

			// temp for testing
			if (this._rms.Count == 0) {
				this._rms.Add(new RmLog { ExerciseID = this._exercise.ID, DateLogged = DateTime.Now, Weight = 200.0, });
				this._rms.Add(new RmLog { ExerciseID = this._exercise.ID, DateLogged = DateTime.Now, Weight = 190.0, });
			}
		}
	
	}
}

