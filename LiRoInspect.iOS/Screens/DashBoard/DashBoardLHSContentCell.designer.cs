// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace LiRoInspect.iOS
{
    [Register ("DashBoardLHSContentCell")]
    partial class DashBoardLHSContentCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnBeginInspection { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblAddress1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblAddress2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblConstructionType { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblContactNo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblInspectionDate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblInspectionType { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblOwnerNamer { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnBeginInspection != null) {
                btnBeginInspection.Dispose ();
                btnBeginInspection = null;
            }

            if (lblAddress1 != null) {
                lblAddress1.Dispose ();
                lblAddress1 = null;
            }

            if (lblAddress2 != null) {
                lblAddress2.Dispose ();
                lblAddress2 = null;
            }

            if (lblConstructionType != null) {
                lblConstructionType.Dispose ();
                lblConstructionType = null;
            }

            if (lblContactNo != null) {
                lblContactNo.Dispose ();
                lblContactNo = null;
            }

            if (lblInspectionDate != null) {
                lblInspectionDate.Dispose ();
                lblInspectionDate = null;
            }

            if (lblInspectionType != null) {
                lblInspectionType.Dispose ();
                lblInspectionType = null;
            }

            if (lblOwnerNamer != null) {
                lblOwnerNamer.Dispose ();
                lblOwnerNamer = null;
            }
        }
    }
}