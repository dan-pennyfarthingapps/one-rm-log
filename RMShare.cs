using System;
using System.Drawing;
using System.Collections.Generic;

using Mono.Data.Sqlite;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Twitter;
using MonoTouch.FacebookConnect;
using MonoTouch.Dialog;
using System.Json;

namespace onermlog
{
	public class RMShare
	{
		private UIActionSheet _shareSheet;
		private UIViewController _vc;
		
		// TWITTER
		//private bool _canSendTweets;
		
		// Facebook
		public string _fbAppId;
		public Facebook _facebook;
		public FBDialogDelegate _wallDialogHandler;
		
		// social
		private string _socialMessage;
		
		
		public RMShare (UIViewController viewC)
		{
			//this._canSendTweets = TWTweetComposeViewController.CanSendTweet;
			
			// FB - change appid
			this._fbAppId = ""; 
			var sessionDelegate = new SessionDelegate (this); 
			this._facebook = new Facebook (this._fbAppId, sessionDelegate);
			
			var defaults = NSUserDefaults.StandardUserDefaults;
			if (defaults ["FBAccessTokenKey"] != null && defaults ["FBExpirationDateKey"] != null){
				this._facebook.AccessToken = defaults ["FBAccessTokenKey"] as NSString;
				this._facebook.ExpirationDate = defaults ["FBExpirationDateKey"] as NSDate;
			}
			
			
			this._vc = viewC;
			

			
			this._socialMessage = "";
			
			// ValidateSetup();
		}
		
		public void ShowShareSheet (UIView view) {
			NetworkStatus status = Reachability.InternetConnectionStatus();
			if(status == NetworkStatus.NotReachable) {
				// Put alternative content/message here
				ShowMessage ("Network Error", "Sharing requires an Internet connection. Please check your network settings and try again.");
			}
			else
			{
				// Put Internet Required Code here
				this._shareSheet.ShowInView(view);
			}
			
			
			
			
		}
		
		public string SocialMessage {
			get { return this._socialMessage; }
			set { this._socialMessage = value; }
			
		}
		
		public void ShowTwitterView () {

			NetworkStatus status = Reachability.InternetConnectionStatus();
			if(status == NetworkStatus.NotReachable) {
				// Put alternative content/message here
				ShowMessage ("Network Error", "Sharing requires an Internet connection. Please check your network settings and try again.");
			}
			else
			{
				var tvc = new TWTweetComposeViewController();
				tvc.SetInitialText("I just got a new one rep max " + this.SocialMessage + " #onermlog");
				this._vc.PresentModalViewController(tvc, true);
			}

			
		}
		
		public void ShowFacebookView ()
		{
			
			if (this._facebook.IsSessionValid)
				PostToWall("test");
			else
				Login ();
			
		}
		
		
		/** Facebook Methods **/
		public void Login ()
		{
			if (!this._facebook.IsSessionValid)
				this._facebook.Authorize (new string [] { "offline_access" });
			else
				PostToWall("test");
		}
		
		public void UninstallFromFacebook ()
		{
			this._facebook.GraphRequest ("me/permissions", new NSMutableDictionary (), "DELETE", Handler (delegate {
				ShowMessage ("Success", "The application has been uninstalled from Facebook");
				
				// Clear out authentication values
				this._facebook.AccessToken = null;
				this._facebook.ExpirationDate = null;
				ShowLoggedOut ();
			}));
		}
		
		
		
		public void ShowLoggedIn () {
			// implement for a config page
			PostToWall("test");
			
		}
		
		public void ShowLoggedOut () {
			// implement for a config page
		}
		/** For later
		public Section FacebookConfig (UILabel label)
		{
			Section facebookConfig = new Section(label);
				if (this._facebook.IsSessionValid) {
					facebookConfig.Add(new StringElement ("Uninstall app", this.UninstallFromFacebook));
				} else {
					facebookConfig.Add(new StringElement ("Login to Facebook", this.Login));
				}

			return facebookConfig;
		}
		**/
		
		public void PostToWall (string wallPost)
		{
			Console.WriteLine("post to wall launched");
			
			var json = new JsonObject ();
			json.Add ("name", new JsonPrimitive ("About My WOD Time"));
			json.Add ("link", new JsonPrimitive ("http://wodti.me"));
			
			var parameters = NSMutableDictionary.FromObjectsAndKeys (
				new object [] { "Completed a WOD", "using My WOD Time", "Just completed " + this.SocialMessage, 
				"http://penfarapps.com/SU4Bie", "http://www.wodti.me/img/facebook_icon_large.png", json.ToString ()},
			new object [] { "name", "caption", "description", "link", "picture", "actions"});
			
			_wallDialogHandler = DialogCallback (url => {
				
				if (url.Query == null)
					return;
				
				var pars = System.Web.HttpUtility.ParseQueryString (url.Query);
				if (pars ["post_id"] != null)
					ShowMessage ("Success", "Posted to your wall");
			});
			
			this._facebook.Dialog ("feed", parameters, _wallDialogHandler);
		}
		
		public void SaveAuthorization () {
			
			var defaults = NSUserDefaults.StandardUserDefaults;
			defaults ["FBAccessTokenKey"] = new NSString (this._facebook.AccessToken);
			defaults ["FBExpirationDateKey"] = this._facebook.ExpirationDate;
			defaults.Synchronize ();
		}
		
		public void ClearAuthorization () {
			
			var defaults = NSUserDefaults.StandardUserDefaults;
			defaults.RemoveObject ("FBAccessTokenKey");
			defaults.RemoveObject ("FBExpirationDateKey");
			defaults.Synchronize ();
		}
		
		public void ShowMessage (string caption, string message, NSAction callback = null)
		{
			var alert = new UIAlertView (caption, message, null, "Ok");
			if (callback != null)
			alert.Dismissed += delegate { callback (); };
			alert.Show ();
		}
		
		
		public void SetupError (string msg)
		{
			ShowMessage ("Setup Error", msg, ()=>{Environment.Exit (1); });
		}
		
		//
		// This method merely validates that the basic components of a Facebook
		// app are complete, it should not be needed in a complete application,
		// but will help in the first stages of debugging your Facebook
		// interop
		//
		public void ValidateSetup ()
		{
			if (this._fbAppId == null)
				SetupError ("Missing AppId,  You can not run the app until this is setup");
			
			// Validate the callback "fb[APPID]://authorize is in the Info.plist
			// which is what facebook uses to call us back
			bool urlFound = false;
			try {
				var arr = NSArray.FromArray<NSObject> (NSBundle.MainBundle.ObjectForInfoDictionary ("CFBundleURLTypes") as NSArray);
				if (arr != null && arr.Length > 0){
					var dict = arr [0] as NSDictionary;
					arr = NSArray.FromArray<NSString> (dict [(NSString) "CFBundleURLSchemes"] as NSArray);
					if (arr != null && arr.Length > 0){
						var fbvalue = arr [0].ToString ();
						if (fbvalue.StartsWith ("fb" + this._fbAppId))
							urlFound = true; 
					}
				} else 
					SetupError ("Missing fbXXXX URL handler in Info.plist's CFBundleURLTypes section");
			} catch {
				SetupError ("Invalid contents of Info.plist file");
			}
			
			if (!urlFound)
				SetupError ("Missing correct fbXXXX i the Info.plist's setup");
			
			// Check if the authorization callback will work
			if (!UIApplication.SharedApplication.CanOpenUrl (new NSUrl ("fb" + this._fbAppId + "://authorize")))
				SetupError ("Invalid or missing URL scheme. You cannot run the app until you set up a valid URL scheme in your .plist.");
		}
		
		
		// Pre-4.2 callback
		// This method is called back when Facebook has authenticated us in the Web UI
		public bool HandleOpenURL (UIApplication application, NSUrl url)
		{
			return this._facebook.HandleOpenURL (url);
		}
		
		// Post 4.2 callback
		public bool OpenUrl (UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
		{
			Console.WriteLine ("Got: {0} and {1}", url, this._facebook.Handle);
			try 
			{
				this._facebook.HandleOpenURL (url);
				return true;
			}
			catch (Exception ex) 
			{   
				Console.WriteLine ("A very bad thing happened : " + ex.Message );
				return false;
			}
			
		}
		
		
		
		public FBRequestDelegate Handler (Action<FBRequest,NSObject> handler)
		{
			return new RequestHandler (handler);
		}
		
		public FBDialogDelegate DialogCallback (Action<NSUrl> callback)
		{
			return new DialogHandler (callback);
		}
		
		// Method called back by all of the various Requests* methods
		public void RequestsCallback (NSUrl url)
		{
			var collection = System.Web.HttpUtility.ParseQueryString (url.Query);
			int count = 0;
			foreach (var k in collection.AllKeys){
				if (k.StartsWith ("request_ids"))
					count++;
			}
			ShowMessage ("Request Result", "Successfully sent " + count + " requests");
		}
		
		
	}

	
	class SessionDelegate : FBSessionDelegate 
	{
		RMShare container;
		
		public SessionDelegate (RMShare container)
		{
			this.container = container;
		}
		
		public override void DidLogin ()
		{
			//Console.WriteLine("did log in");
			container.ShowLoggedIn ();
			container.SaveAuthorization ();
		}
		public override void DidLogout ()
		{
			container.ClearAuthorization ();
			container.ShowLoggedOut ();
		}
		
		public override void DidNotLogin (bool cancelled)
		{
			// TODO: Implement - see: http://go-mono.com/docs/index.aspx?link=T%3aMonoTouch.Foundation.ModelAttribute
		}
		
		public override void SessionInvalidated ()
		{
			// TODO: Implement - see: http://go-mono.com/docs/index.aspx?link=T%3aMonoTouch.Foundation.ModelAttribute
		}
	}
	
	
	//
	// Proxy class that turns method invocations into delegate calls
	//
	class RequestHandler : FBRequestDelegate {
		static List<RequestHandler> handlers = new List<RequestHandler> ();
		Action<FBRequest, NSObject> loadedHandler;
		
		public RequestHandler (Action<FBRequest, NSObject> loadedHandler)
		{
			handlers.Add (this);
			this.loadedHandler = loadedHandler;
		}
		
		public override void FailedWithError (FBRequest request, NSError error)
		{
			var u = new UIAlertView ("Request Error", "Failed with " + error.ToString (), null, "ok");
			u.Dismissed += delegate {
				handlers.Remove (this);
			};
			
			Console.WriteLine(error.Description.ToString());
			u.Show ();
		}
		
		public override void RequestLoaded (FBRequest request, NSObject result)
		{
			loadedHandler (request, result);
			handlers.Remove (this);
		}
	}
	
	// 
	// Proxy call that turns method invocation into delegate calls
	//
	class DialogHandler : FBDialogDelegate {
		Action<NSUrl> callback;
		
		public DialogHandler (Action<NSUrl> callback)
		{
			this.callback = callback;
		}
		public override void CompletedWithUrl (NSUrl url)
		{
			callback (url);
		}
	}

}

