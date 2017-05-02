using Foundation;
using System;
using UIKit;
using SharpMobileCode.ModalPicker;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MobileInvoice.ios
{
    public partial class InvoiceDateCell : UITableViewCell
    {
		public InvoiceViewController callingController;

        public InvoiceDateCell (IntPtr handle) : base (handle)
        {
        }

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			var dateFormatter = new NSDateFormatter()
			{
				DateFormat = "MMMM dd, yyyy"
			};

			this.btnIssueDate.SetTitle(dateFormatter.ToString(Helper.DateTimeToNSDate(callingController.invoice.IssueDate)), UIControlState.Normal);

			this.btnDueTerm.SetTitle(callingController.invoice.DueTerm == "Due on receipt" ? "Due on receipt" : "Due on " + callingController.invoice.DueTerm, UIControlState.Normal);
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

				this.callingController.invoice.IssueDate = Helper.NSDateToDateTime(modalPicker.DatePicker.Date);
				this.callingController.invoice.DueDate = this.callingController.invoice.DueTerm == "Due on receipt" ?
					this.callingController.invoice.IssueDate :
					this.callingController.invoice.IssueDate.AddDays(Convert.ToDouble(Regex.Match(this.callingController.invoice.DueTerm, "\\d+").Value));
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

				this.callingController.invoice.DueTerm = dueDaysList[(int)index];

                this.callingController.invoice.DueDate = this.callingController.invoice.DueTerm == "Due on receipt" ? 
					this.callingController.invoice.IssueDate : 
					this.callingController.invoice.IssueDate.AddDays(Convert.ToDouble(Regex.Match(this.callingController.invoice.DueTerm, "\\d+").Value));
			};

			await callingController.PresentViewControllerAsync(modalPicker, true);
		}
	}
}