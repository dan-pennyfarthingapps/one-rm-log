
using System;
using System.Collections.Generic;
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
			MyRootElement configRoot = new MyRootElement(" ") { };
			//var dvc = new MyDialogViewController (spokeRoot, true);
			
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
			
			CustomRootElement contactUsRoot = new CustomRootElement("Contact Us");
			
			Section contactUsSect = new Section("Contact");
			
			StyledStringElement facebookPageBE = new StyledStringElement("Our Facebook Page");
			//facebookPageBE.Font = UIFont.FromName ("HelveticaNeue-Bold", 17f);
			facebookPageBE.Image = UIImage.FromBundle("icons/contactFb");
			
			// TODO: change to app page
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
			
			Section legalSect = new Section("Disclaimer") { };
			
			MultilineElement disclaimerLines = new MultilineElement("Please check with the manufacturer on the correct torque specs for your components. " +
			                                                        "Please do not ruin your carbon frame! All brand names are properties of their respective owners, used for reference purposes only. " +
			                                                        "Penny Farthing Apps, llc is not related to any company included within the app.");
			
			
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
				this.navBarBackgroundImage = UIImage.FromBundle ("images/navBar");
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
			_mailController.SetSubject ("support for bike mech app");
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
		}
	}
}