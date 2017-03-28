using Foundation;
using System;
using UIKit;
using SharpMobileCode.ModalPicker;
using System.Collections.Generic;

namespace MobileInvoice.ios
{
    public partial class InvoiceDateCell : UITableViewCell
    {
		public InvoiceViewController callingController;

        public InvoiceDateCell (IntPtr handle) : base (handle)
        {
        }


		async partial void btnIssueDate_UpInside(UIButton sender)
		{
			var modalPicker = new ModalPickerViewController(ModalPickerType.Date, "Select a Date", this.callingController)
			{
				HeaderBackgroundColor = UIColor.FromRGB(26, 188, 156),
				HeaderTextColor = UIColor.White,
				TransitioningDelegate = new ModalPickerTransitionDelegate(),
				ModalPresentationStyle = UIModalPresentationStyle.Custom
			};

			modalPicker.DatePicker.Mode = UIDatePickerMode.Date;

			modalPicker.OnModalPickerDismissed += (s, ea) =>
			{
				var dateFormatter = new NSDateFormatter()
				{
					DateFormat = "MMMM dd, yyyy"
				};

				this.btnIssueDate.SetTitle(dateFormatter.ToString(modalPicker.DatePicker.Date), UIControlState.Normal);

			};

			await callingController.PresentViewControllerAsync(modalPicker, true);
		}

		async partial void btnDueTerm_UpInside(UIButton sender)
		{
			var dueDaysList = new List<string>();

			dueDaysList.Add("Due on receipt");
			dueDaysList.Add("1 day");
			dueDaysList.Add("7 days");
			dueDaysList.Add("14 days");
			dueDaysList.Add("21 days");
			dueDaysList.Add("30 days");
			dueDaysList.Add("60 days");
			dueDaysList.Add("90 days");
			dueDaysList.Add("180 days");
			dueDaysList.Add("365 days");

			var modalPicker = new ModalPickerViewController(ModalPickerType.Custom, "Select a Date", this.callingController)
			{
				HeaderBackgroundColor = UIColor.FromRGB(26, 188, 156),
				HeaderTextColor = UIColor.White,
				TransitioningDelegate = new ModalPickerTransitionDelegate(),
				ModalPresentationStyle = UIModalPresentationStyle.Custom
			};

			modalPicker.PickerView.Model = new CustomPickerModel(dueDaysList);

			modalPicker.OnModalPickerDismissed += (s, ea) =>
			{
				var index = modalPicker.PickerView.SelectedRowInComponent(0);

				btnDueTerm.SetTitle(dueDaysList[(int)index] == "Due on receipt" ? "Due on receipt" : "Due on " + dueDaysList[(int)index], UIControlState.Normal);

			};

			await callingController.PresentViewControllerAsync(modalPicker, true);
		}
	}
}