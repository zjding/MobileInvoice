﻿using System;
using CoreGraphics;
using UIKit;

namespace MobileInvoice.ios
{
	public class LoadingOverlay : UIView
	{
		// control declarations
		UIActivityIndicatorView activitySpinner;
		//UILabel loadingLabel;

		public LoadingOverlay(CGRect frame) : base(frame)
		{
			// configurable bits
			//BackgroundColor = UIColor.White;
			//Alpha = 0.25f;
			//AutoresizingMask = UIViewAutoresizing.All;

			//nfloat labelHeight = 22;
			//nfloat labelWidth = Frame.Width - 20;

			// derive the center x and y
			nfloat centerX = Frame.Width / 2;
			nfloat centerY = Frame.Height / 2;

			// create the activity spinner, center it horizontall and put it 5 points above center x
			activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray);
			//activitySpinner.Frame = new CGRect(
			//	centerX - (activitySpinner.Frame.Width / 2),
			//	centerY - activitySpinner.Frame.Height - 40,
			//	activitySpinner.Frame.Width,
			//	activitySpinner.Frame.Height);
			//activitySpinner.AutoresizingMask = UIViewAutoresizing.All;

			activitySpinner.Frame = new CGRect(
				centerX - (activitySpinner.Frame.Width / 2),
				centerY - activitySpinner.Frame.Height - 60,
				50,
				50);
			activitySpinner.Layer.CornerRadius = 05;
			activitySpinner.Opaque = true;

			activitySpinner.BackgroundColor = new UIColor((System.nfloat)0.0, (System.nfloat)0.7);
			activitySpinner.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.White;

			AddSubview(activitySpinner);
			activitySpinner.StartAnimating();

			// create and configure the "Loading Data" label
			//loadingLabel = new UILabel(new CGRect(
			//	centerX - (labelWidth / 2),
			//	centerY + 20,
			//	labelWidth,
			//	labelHeight
			//	));
			//loadingLabel.BackgroundColor = UIColor.Clear;
			//loadingLabel.TextColor = UIColor.White;
			//loadingLabel.Text = "Loading Data...";
			//loadingLabel.TextAlignment = UITextAlignment.Center;
			//loadingLabel.AutoresizingMask = UIViewAutoresizing.All;
			//AddSubview(loadingLabel);

		}

		/// <summary>
		/// Fades out the control and then removes it from the super view
		/// </summary>
		public void Hide()
		{
			UIView.Animate(
				0.2, // duration
				() => { Alpha = 0; },
				() => { RemoveFromSuperview(); }
			);
		}
	}
}
