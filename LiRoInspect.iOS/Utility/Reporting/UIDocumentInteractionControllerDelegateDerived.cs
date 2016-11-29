using System;
using UIKit;

namespace LiRoInspect.iOS
{
	public class UIDocumentInteractionControllerDelegateDerived:
	UIDocumentInteractionControllerDelegate 
	{
	private UIViewController mViewController; 
	public event EventHandler doneWithPreview;
	public UIDocumentInteractionControllerDelegateDerived(UIViewController
		viewController) 
	{ 
		mViewController = viewController; 
	} 

	public override UIViewController ViewControllerForPreview
	(UIDocumentInteractionController controller) 
	{ 
		return mViewController; 
	} 

	public override UIView ViewForPreview (UIDocumentInteractionController
		controller) 
	{ 
		return mViewController.View; 
	} 
	public override void DidEndPreview (UIDocumentInteractionController controller)
	{
		if(null!=doneWithPreview)
		{
			doneWithPreview (null, null);
		}
	} 
}
}

