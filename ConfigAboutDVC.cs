
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

using MonoTouch.Twitter;
using MonoTouch.MessageUI;

using PennyFarElements;

namespace onermlog
{
	public partial class ConfigScreenDVC : DialogViewController
	{
		
		private UIImage navBarBackgroundImage;
		
		public ConfigScreenDVC () : base (UITableViewStyle.Grouped, null)
		{
			MyRootElement configRoot = new MyRootElement("Configuration") { };

			
			Section weightSection = new Section("Weight Options") {};
			
			MyRootElement weightOptions = new MyRootElement ("Units", new RadioGroup ("units", 0)) {
				new Section () { }
				
				
			};
			
			MyRadioElement lbs = new MyRadioElement ("pounds");
			
			lbs.OnSelected += delegate(object sender, EventArgs e) {
				//Settings.DefaultUnits = 0;
				//Settings.Write();
			};
			
			MyRadioElement kgs = new MyRadioElement ("kilograms");
			
			kgs.OnSelected += delegate(object sender, EventArgs e) {
				//Settings.DefaultUnits = 1;
				//Settings.Write();
			};
			
			
			
			
			weightOptions[0].Add(lbs);
			weightOptions[0].Add(kgs);
			
			weightSection.Add(weightOptions);
			
			
			configRoot.Add(weightSection);
			
			Section aboutSection = new Section("About") { };
			
			StyledStringElement pennyFarthing = new StyledStringElement("Penny Farthing Apps", "website");
			//facebookPageBE.Font = UIFont.FromName ("HelveticaNeue-Bold", 17f);
			pennyFarthing.Image = UIImage.FromBundle("icons/contactHP");
			pennyFarthing.Tapped += delegate() {
				UIApplication.SharedApplication.OpenUrl(NSUrl.FromString("http://pennyfarthingapps.com"));
			};


			
			MyRootElement contactUsRoot = new MyRootElement("Contact Us");
			
			Section contactUsSect = new Section("Contact");
			
			StyledStringElement facebookPageBE = new StyledStringElement("Our Facebook Page");
			//facebookPageBE.Font = UIFont.FromName ("HelveticaNeue-Bold", 17f);
			facebookPageBE.Image = UIImage.FromBundle("icons/contactFb");
			

			facebookPageBE.Tapped += delegate() {
				if(UIApplication.SharedApplication.CanOpenUrl(NSUrl.FromString("fb://profile/439101479483557"))) {
					UIApplication.SharedApplication.OpenUrl(NSUrl.FromString("fb://profile/439101479483557"));
				} else {
					UIApplication.SharedApplication.OpenUrl(NSUrl.FromString("http://penfarapps.com/RxuFzI"));
				}
			};
			
			
			StyledStringElement twitterBE = new StyledStringElement("On Twitter");
			twitterBE.Image = UIImage.FromBundle("icons/contactTw");
			twitterBE.Tapped += delegate() {
				ShowTwitterView();
			};
			
			StyledStringElement emailSupBE = new StyledStringElement("Email Support");
			emailSupBE.Image = UIImage.FromBundle("icons/contactEm");
			emailSupBE.Tapped += delegate() {
				ShowEmailView();
			};
			
			
			contactUsSect.Add(emailSupBE);
			contactUsSect.Add(facebookPageBE);
			contactUsSect.Add(twitterBE);
			
			contactUsRoot.Add(contactUsSect);
			
			aboutSection.Add(pennyFarthing);
			aboutSection.Add(contactUsRoot);

			configRoot.Add(aboutSection);

			// TODO: change to JSON from website
			Section	ourAppsSection = new Section("Check out Our Apps");
			StyledStringElement myWODTimeBadge = new StyledStringElement("My WOD Time", "Crossfit Timer");
			myWODTimeBadge.Image = UIImage.FromBundle("icons/myWODTime");
			myWODTimeBadge.Tapped += delegate() {
				UIApplication.SharedApplication.OpenUrl(NSUrl.FromString("itms://itunes.apple.com/us/app/my-wod-time/id568915915?mt=8&uo=4"));
			};

			ourAppsSection.Add(myWODTimeBadge);
			configRoot.Add(ourAppsSection);

			
			Section legalSect = new Section("Disclaimer") { };
			
			MultilineElement disclaimerLines = new MultilineElement("Please consult a physician before starting any exercise program. Always use a spotter, and pick up your weights!");
			
			
			legalSect.Add(disclaimerLines);
			
			configRoot.Add(legalSect);
			
			
			Root = configRoot;
		}
		
		
		
		public override void LoadView ()
		{
			base.LoadView ();
			TableView.BackgroundColor = UIColor.Clear;
			TableView.BackgroundView = null;
			UIImage background = UIImage.FromBundle ("images/white_carbon");
			ParentViewController.View.BackgroundColor = UIColor.FromPatternImage (background);
			
			// Bar
			if (UIDevice.CurrentDevice.CheckSystemVersion (5, 0)) {
				this.navBarBackgroundImage = UIImage.FromBundle ("images/navBarBlank");
				//this.NavigationController.NavigationItem.Title = "";
				this.NavigationController.NavigationBar.SetBackgroundImage (this.navBarBackgroundImage, UIBarMetrics.Default);
			} 
			
			
			
			
			
		}
		
		private void ShowTwitterView() {
			var tvc = new TWTweetComposeViewController();
			tvc.SetInitialText("@PennyFarApps ");
			this.PresentModalViewController(tvc, true);
		}
		
		private void ShowEmailView() {
			MFMailComposeViewController _mailController = new MFMailComposeViewController ();
			_mailController.SetToRecipients (new string[]{"support@pennyfarthingapps.com"});
			_mailController.SetSubject ("support for one rm log");
			_mailController.SetMessageBody ("", false);
			
			_mailController.Finished += ( object s, MFComposeResultEventArgs args) => {
				Console.WriteLine (args.Result.ToString ());
				args.Controller.DismissModalViewControllerAnimated (true); 
			};
			
			this.PresentModalViewController(_mailController, true);
		}
		
		
	}
	
	public class MyRadioElement : RadioElement {
		public MyRadioElement (string s) : base (s) {}
		
		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			base.Selected (dvc, tableView, path);
			var selected = OnSelected;
			if (selected != null)
				selected (this, EventArgs.Empty);
		}
		
		public event EventHandler<EventArgs> OnSelected;
	}

	public class MyRootElement : CustomRootElement {

		public MyRootElement (string caption, RadioGroup rgroup) : base (caption, rgroup)
		{
			SetDefaults();
		}

		public MyRootElement (string caption) : base(caption)
		{
			SetDefaults();
		}

		private void SetDefaults() {
			this.BackgroundImage = "images/white_carbon";
			this.SetCustomBackButton(UIImage.FromBundle("images/backbutton"), RectangleF.FromLTRB (0, 20, 50, 20));
		}
	}
}