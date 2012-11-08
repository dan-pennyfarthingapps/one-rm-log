using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using PennyFarElements;

using SQLite;

namespace onermlog
{
	public partial class RepMaxView : UIViewController
	{
		private DialogViewController _dvc;
		private RootElement _logRoot;
		private Section _logSect;

		private SQLiteConnection db;

		private Exercise _exercise;
		private List<RmLog> _rms;

		private double largestRMValue;

		public RepMaxView (Exercise exerciseToShow) : base ("RepMaxView", null)
		{
			this._exercise = exerciseToShow;

			largestRMValue = 0.0;

			string dbname = "onerm.db";
			string documents = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // This goes to the documents directory for your app
			string dbPath = Path.Combine (documents, dbname);
			
			db = new SQLiteConnection (dbPath);

			this.LoadRecords();

			this._logRoot = new RootElement ("Records");
			this._dvc = new DialogViewController (UITableViewStyle.Plain, this._logRoot, false);

			// load data from list
			this._logSect = new Section ();
			foreach (RmLog rm in this._rms) {
				StringElement recordString = new StringElement (rm.Weight.ToString(), rm.DateLogged.ToShortDateString());
				this._logSect.Add(recordString);
			}


			this._logRoot.Add(this._logSect);
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
			LargestRMForDisplay();

			// Put the dialog view controller into the UIView
			this.dvcView.AddSubview(this._dvc.View);


			this.btnAddNew.TouchUpInside += delegate(object sender, EventArgs e) {
				
				var addRMScreen = NewRMEntry (largestRMValue);
				addRMScreen.ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
				this.NavigationController.PresentViewController (addRMScreen, true, null);
			};

			// edit button
			UIColor buttonColor = UIColor.FromRGB(39, 113, 205);
			UIBarButtonItem editButton = new UIBarButtonItem(UIBarButtonSystemItem.Edit);
			editButton.TintColor = buttonColor;
			editButton.Clicked += delegate {
				var editNameScreen = EditExerciseName();
				editNameScreen.ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
				this.NavigationController.PresentViewController (editNameScreen, true, null);
			};
			this.NavigationItem.LeftBarButtonItem = editButton;



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
			
			this._rms = db.Query<RmLog> ("select * from RmLog where ExerciseID=? order by DateLogged DESC", this._exercise.ID);


		}

		private void RefreshRecords ()
		{
			this._logSect.RemoveRange(0, this._logSect.Count);

			foreach (RmLog rm in this._rms) {
				StringElement recordString = new StringElement (rm.Weight.ToString(), rm.DateLogged.ToShortDateString());
				this._logSect.Add(recordString);
			}

			this._logRoot.Reload(this._logSect, UITableViewRowAnimation.None);
			LargestRMForDisplay();
		}

		private void LargestRMForDisplay ()
		{
			DateTime largestDate = DateTime.Today;
			double largestRM = 0.0;
			if (this._rms.Count > 0) {
				foreach (RmLog rm in this._rms) {
					if (rm.Weight > largestRM) {
						largestRM = rm.Weight;
						largestDate = rm.DateLogged;
					}
				}

				this.lblMaxOnDate.Text = "on " + largestDate.ToShortDateString ();
				this.lblWeightMax.Text = largestRM.ToString () + " lb.";
				largestRMValue = largestRM;
			} else {
				this.lblMaxOnDate.Text = "add one";
				this.lblWeightMax.Text = "No RM";
			}

		}

		private UINavigationController NewRMEntry (double prevBest) {

			CustomRootElement root = new CustomRootElement(" ");

			CustomDialogViewController dvc = new CustomDialogViewController(root, false);
			UINavigationController nav = new UINavigationController(dvc);

			dvc.BackgroundImage = "images/white_carbon";
			dvc.NavigationBarColor = UIColor.FromRGB(39, 113, 205);
			dvc.NavigationBarImage = UIImage.FromBundle("images/navBar");


			UIColor buttonColor = UIColor.FromRGB(39, 113, 205);
			UIBarButtonItem cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel);
			cancelButton.TintColor = buttonColor;
			cancelButton.Clicked += delegate {
				dvc.NavigationController.DismissViewController(true, null);
			};
			UIBarButtonItem saveButton = new UIBarButtonItem(UIBarButtonSystemItem.Save);
			saveButton.TintColor = buttonColor;
			dvc.NavigationItem.LeftBarButtonItem = cancelButton;
			dvc.NavigationItem.RightBarButtonItem = saveButton;


			Section newRMSection = new Section("Record Details") {};

			string strPrevBest = String.Format("{0:000.0}", prevBest);

			if(prevBest == 0)
				strPrevBest = "100.0";

			

			ResponsiveCounterElement newRMCounter = new ResponsiveCounterElement("New RM", strPrevBest); // TODO: grab the previous best and use that
			newRMCounter.SetCustomBackButton(UIImage.FromBundle("images/backbutton"), RectangleF.FromLTRB (0, 20, 50, 20));

			DateElement newRMDate = new DateElement("Date", DateTime.Today);

			newRMSection.Add(newRMCounter);
			newRMSection.Add(newRMDate);

			root.Add(newRMSection);



			saveButton.Clicked += delegate {
				RmLog newEntry = new RmLog {
					DateLogged = newRMDate.DateValue,
					Weight = Convert.ToDouble(newRMCounter.Value),
					ExerciseID = this._exercise.ID
				};

				dvc.NavigationController.DismissViewController(true, null);

				db.Insert(newEntry);
				this._rms.Add(newEntry);

				// refresh view
				this.RefreshRecords();

			};

			return nav;


		}

		private UINavigationController EditExerciseName () {
			
			CustomRootElement root = new CustomRootElement(" ");
			
			CustomDialogViewController dvc = new CustomDialogViewController(root, false);
			UINavigationController nav = new UINavigationController(dvc);
			
			dvc.BackgroundImage = "images/white_carbon";
			dvc.NavigationBarColor = UIColor.FromRGB(39, 113, 205);
			dvc.NavigationBarImage = UIImage.FromBundle("images/navBar");
			
			
			UIColor buttonColor = UIColor.FromRGB(39, 113, 205);
			UIBarButtonItem cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel);
			cancelButton.TintColor = buttonColor;
			cancelButton.Clicked += delegate {
				dvc.NavigationController.DismissViewController(true, null);
			};
			UIBarButtonItem saveButton = new UIBarButtonItem(UIBarButtonSystemItem.Save);
			saveButton.TintColor = buttonColor;
			dvc.NavigationItem.LeftBarButtonItem = cancelButton;
			dvc.NavigationItem.RightBarButtonItem = saveButton;
			
			
			Section nameSection = new Section("Edit Name") {};
			
			EntryElement nameEdit = new EntryElement("Name", "", this._exercise.Name, false);
			
			nameSection.Add(nameEdit);
			
			root.Add(nameSection);

			// TODO: Add all the logs here and set them to delete mode
			
			
			
			saveButton.Clicked += delegate {
				this._exercise.Name = nameEdit.Value;
				
				dvc.NavigationController.DismissViewController(true, null);
				
				db.Update(this._exercise);

				this.NavigationController.TabBarItem.Title = this._exercise.Name;
				this.lblExName = this._exercise.Name;
			};
			
			return nav;
			
			
		}
	
	}
}

