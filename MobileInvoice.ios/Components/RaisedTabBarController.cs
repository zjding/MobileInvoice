using System;
using System.Collections.Generic;
using UIKit;

namespace MobileInvoice.ios
{
	public class RaisedTabBarController: UITabBarController 
	{
		UIButton button; 

		public RaisedTabBarController()
		{
		}

		public RaisedTabBarController(IntPtr handle) : base (handle)
        {

		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
		}

		public override void ViewDidLayoutSubviews()
		{
			button.Center = this.TabBar.Center;
			button.Layer.ZPosition = 1;
		}

		public void BringButtonToFront()
		{
			this.View.BringSubviewToFront(button);
		}

		public void InsertEmptyTabItem(string title, int i)
		{
			var vc = new UIViewController();

			vc.TabBarItem = new UITabBarItem(title, null, 0);
			vc.TabBarItem.Enabled = false;

			List<UIViewController> viewCtlList = new List<UIViewController>(this.ViewControllers);

			viewCtlList.Insert(i, vc);

			this.ViewControllers = viewCtlList.ToArray();
		}

		public void RemoveEmptyTabItem(int i)
		{
			List<UIViewController> viewCtlList = new List<UIViewController>(this.ViewControllers);
			viewCtlList.RemoveAt(i);
			this.ViewControllers = viewCtlList.ToArray();
		}

		public void AddRaisedButton(UIImage buttonImage, UIImage highlightImage)
		{
			button = new UIButton(UIButtonType.Custom);

			button.AutoresizingMask = UIViewAutoresizing.FlexibleRightMargin |
				UIViewAutoresizing.FlexibleLeftMargin |
				UIViewAutoresizing.FlexibleBottomMargin |
				UIViewAutoresizing.FlexibleTopMargin;

			button.Frame = new CoreGraphics.CGRect(0, 0, buttonImage.Size.Width, buttonImage.Size.Height);

			button.SetBackgroundImage(buttonImage, UIControlState.Normal);
			button.SetBackgroundImage(highlightImage, UIControlState.Highlighted);

			var heightDifference = buttonImage.Size.Height - this.TabBar.Frame.Size.Height;

			if (heightDifference < 0)
			{
				button.Center = new CoreGraphics.CGPoint(this.TabBar.Center.X, this.TabBar.Center.Y);
			}
			else
			{
				var center = this.TabBar.Center;
				center.Y -= (nfloat)(heightDifference / 2.0);
				button.Center = new CoreGraphics.CGPoint(center.X, center.Y);;
			}

			button.TouchUpInside += onRaisedButton_TouchUpInside;

			button.Tag = 1001;
			//button.Layer.ZPosition = 1000;



			this.View.AddSubview(button);
		}

		public void RemoveRaiseButton()
		{
			button.RemoveFromSuperview();
		}

		public virtual void onRaisedButton_TouchUpInside(object sender, EventArgs e)
		{

		}
	}
}
