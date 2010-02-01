
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Net;
using System.Xml.Linq;

namespace TwitterExample
{
	public partial class TweetListViewController : UIViewController
	{
		#region Constructors

		// The IntPtr and NSCoder constructors are required for controllers that need 
		// to be able to be created from a xib rather than from managed code

		public TweetListViewController (IntPtr handle) : base(handle)
		{
			Initialize ();
		}

		[Export("initWithCoder:")]
		public TweetListViewController (NSCoder coder) : base(coder)
		{
			Initialize ();
		}

		public TweetListViewController () : base("TweetListViewController", null)
		{
			Initialize ();
		}

		void Initialize ()
		{
		}
		
		#endregion
		
		public List<Tweet> ListOfTweets {
			get;
			set;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			Title = "DDD8 on Twitter";
			var client = new WebClient();
			var keyWord = "ddd8";
			client.DownloadStringCompleted += delegate(object sender, DownloadStringCompletedEventArgs e) {
				
				var twitterResult = e.Result;
				XDocument xDoc = XDocument.Parse(twitterResult);
				XNamespace ns = "http://www.w3.org/2005/Atom";
				var tweets = from x in xDoc.Descendants(ns + "entry")
				select new Tweet
				{
					Content = x.Descendants( ns + "title").First().Value,
					Url = x.Descendants( ns + "link").Attributes("href").First().Value,
					User = x.Descendants( ns + "author" ).Descendants(ns + "name").First().Value
				};
			
				ListOfTweets = tweets.ToList();
				tableView.ReloadData();
				
			};
			client.DownloadStringAsync(new Uri(String.Format("http://search.twitter.com/search.atom?q={0}&show-user=true", keyWord)));
			
			ListOfTweets = new List<Tweet>();
			tableView.Source = new TweetListSource(this);
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}

		

	}
	
	public class TweetListSource : UITableViewSource
	{
		TweetListViewController _tlvc;
		public TweetListSource(TweetListViewController tlvc)
		{
			_tlvc = tlvc;
		}
		
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			var selectedUrl = _tlvc.ListOfTweets[indexPath.Row].Url;
			var tweetWebView = new TweetWebViewController(selectedUrl);
			_tlvc.NavigationController.PushViewController(tweetWebView, true);
		}

		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
            {
				var kCellIdentifier = "MyIdentifier";
                var cell = tableView.DequeueReusableCell (kCellIdentifier);
                if (cell == null)
                {
                    cell = new UITableViewCell (UITableViewCellStyle.Default, kCellIdentifier);
                }
                cell.Accessory = UITableViewCellAccessory.None;
				cell.TextLabel.Text = _tlvc.ListOfTweets[indexPath.Row].Content;
                return cell;
            }
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			return _tlvc.ListOfTweets.Count();
		}
		
		public override int NumberOfSections (UITableView tableView)
		{
			return 1;
		}



	}
}
