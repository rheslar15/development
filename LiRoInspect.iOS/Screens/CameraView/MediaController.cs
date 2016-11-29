
using System;
using Foundation;
using UIKit;
using Xamarin.Media;
using ObjCRuntime;

using System.Collections.Generic;
using System.Diagnostics;


namespace LiRoInspect.iOS
{
	public partial class MediaController: UIImagePickerController
	{
		public MediaController()
		{
			
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();


		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			Debug.WriteLine("MediaController - ViewDidLoad");
			//this.clearZoomSliderDelegate(this.View.Subviews);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			this.clearZoomSliderDelegate (this.View.Subviews);

		}

		public void clearZoomSliderDelegate(UIView[] subviews)
		{
			
			foreach (UIView subview in subviews)
			{
				//static type sliderType = Type.GetType ("CAMZoomSlider");
				if( object.Equals(subview.GetType(), Type.GetType ("CAMZoomSlider")))//  is sliderType)
				{
					if(subview.RespondsToSelector(new Selector("setDelegate:")))
					{
						subview.PerformSelector(new Selector("setDelegate:"),null);
					}
					return;
				
				}
				else if (subview.Subviews != null)
				{
					this.clearZoomSliderDelegate(subview.Subviews);
				}
			}
		}



	}

}



