using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using SQLite;

namespace onermlog
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;
		
		MyTabBarController navigation;


		private SQLiteConnection db;
		private List<Exercise> exercises;

		
		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			CopyDb();
			
			SetupDb();
			
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			
			navigation = new MyTabBarController();
			
			window.RootViewController = navigation;
			window.MakeKeyAndVisible ();
			


			return true;
		}

		public void CopyDb ()
		{
			string dbname = "onerm.db";
			string documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // This goes to the documents directory for your app
			string db = Path.Combine(documents, dbname);
			
			string rootPath = Environment.CurrentDirectory;  // This is the package such as MyApp.app/
			string rootDbPath = Path.Combine(rootPath, dbname);
			
			Console.WriteLine("Root Db Path: " + rootDbPath);
			Console.WriteLine("Final Db Path: " +db);
			
			if(File.Exists(db) == false) {
				Console.WriteLine("Copying DB!");
				File.Copy(rootDbPath, db);
			} else {
				Console.WriteLine("DB Exists, not copying.");
			}
			
		}

		public override void WillTerminate(UIApplication app) {
			//	Settings.Write();
			
		}
		
		private void SetupDb ()
		{
			string dbname = "onerm.db";
			string documents = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // This goes to the documents directory for your app
			string dbPath = Path.Combine (documents, dbname);
			
			db = new SQLiteConnection (dbPath);

			db.CreateTable<Exercise> ();

			this.exercises = db.Query<Exercise> ("select * from Exercise");

			// populate the database
			if (exercises.Count () < 3) {
				Exercise bench = new Exercise {
					Name = "Bench Press"
				};
				Exercise squat = new Exercise {
					Name = "Squat"
				};
				Exercise deadlift = new Exercise {
					Name = "Deadlift"
				};

				db.InsertAll(new[] { bench, squat, deadlift }, false);
				this.exercises = db.Query<Exercise> ("select * from Exercise");
			} 

		}
		
		
	}
	
	public class MyTabBarController : UITabBarController {
		

		//UINavigationController _configController;

		UINavigationController _exerciseOne;
		UINavigationController _exerciseTwo;
		UINavigationController _exerciseThree;

		private SQLiteConnection db;
		private List<Exercise> exercises;
		
		
		public override void ViewDidLoad() {
			base.ViewDidLoad();

			/**
			UIImage aboutIcon = UIImage.FromBundle("icons/tabBarIconAbout");
			this._aboutScreenController = new UINavigationController();
			this._aboutScreenController.TabBarItem = new UITabBarItem();
			this._aboutScreenController.TabBarItem.Title = "About";
			this._aboutScreenController.TabBarItem.Image = aboutIcon;
			this._aboutScreenController.PushViewController(new AboutScreen(), false);
			***/

			string dbname = "onerm.db";
			string documents = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // This goes to the documents directory for your app
			string dbPath = Path.Combine (documents, dbname);
			
			db = new SQLiteConnection (dbPath);
			this.exercises = db.Query<Exercise> ("select * from Exercise");

			this._exerciseOne = new UINavigationController();
			this._exerciseOne.TabBarItem = new UITabBarItem();
			this._exerciseOne.TabBarItem.Title = exercises[0].Name;
			//this._aboutScreenController.TabBarItem.Image = aboutIcon;
			this._exerciseOne.PushViewController(new RepMaxView(exercises[0]), false);

			this._exerciseTwo = new UINavigationController();
			this._exerciseTwo.TabBarItem = new UITabBarItem();
			this._exerciseTwo.TabBarItem.Title = exercises[1].Name;
			//this._aboutScreenController.TabBarItem.Image = aboutIcon;
			this._exerciseTwo.PushViewController(new RepMaxView(exercises[1]), false);

			this._exerciseThree = new UINavigationController();
			this._exerciseThree.TabBarItem = new UITabBarItem();
			this._exerciseThree.TabBarItem.Title = exercises[2].Name;
			//this._aboutScreenController.TabBarItem.Image = aboutIcon;
			this._exerciseThree.PushViewController(new RepMaxView(exercises[2]), false);

			// TODO: Add settings tab

			this.ViewControllers = new UIViewController[] {
				this._exerciseOne,
				this._exerciseTwo,
				this._exerciseThree,
				
			};
			this.SelectedViewController = this._exerciseTwo;

		}
		
	}

}

