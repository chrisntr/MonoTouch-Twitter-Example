
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace TwitterExample
{
	public partial class TweetWebViewController : UIViewController
	{
		#region Constructors

		// The IntPtr and NSCoder constructors are required for controllers that need 
		// to be able to be created from a xib rather than from managed code

		public TweetWebViewController (IntPtr handle) : base(handle)
		{
			Initialize ();
		}

		[Export("initWithCoder:")]
		public TweetWebViewController (NSCoder coder) : base(coder)
		{
			Initialize ();
		}

		public TweetWebViewController (string url) : base("TweetWebViewController", null)
		{
			Url = url;
			Initialize ();
		}

		void Initialize ()
		{
		}
		
		public string Url {
			get;
			set;
		}
		#endregion
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			Console.WriteLine ("Loading..." + Url);
			webView.LoadRequest(new NSUrlRequest(new NSUrl(Url)));
		}

		
		
		
	}
}
