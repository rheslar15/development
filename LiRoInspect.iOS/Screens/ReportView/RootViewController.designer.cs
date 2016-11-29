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
    [Register ("RootViewController")]
    partial class RootViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnEdit { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnFinalizeSave { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnHome { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnLogOut { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnNotify { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnPrintPreview { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSync { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView headerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIWebView ImgWebView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView InspectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblAddress1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblAddress2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblCalDate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblCalDay { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblFailMessage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblInspectionDate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblInspectionType { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblInspectorName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField LblNotifyNbr { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblOwnerName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblPathway { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblPhoneNumber { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField lblSyncNumber { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel leftlblInspectionDate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl reportPhotoSegment { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView WebViewScrollView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIWebView WVReport { get; set; }


        [Action ("btnPrintPreview_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnPrintPreview_TouchUpInside (UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnEdit != null) {
                btnEdit.Dispose ();
                btnEdit = null;
            }

            if (btnFinalizeSave != null) {
                btnFinalizeSave.Dispose ();
                btnFinalizeSave = null;
            }

            if (btnHome != null) {
                btnHome.Dispose ();
                btnHome = null;
            }

            if (btnLogOut != null) {
                btnLogOut.Dispose ();
                btnLogOut = null;
            }

            if (btnNotify != null) {
                btnNotify.Dispose ();
                btnNotify = null;
            }

            if (btnPrintPreview != null) {
                btnPrintPreview.Dispose ();
                btnPrintPreview = null;
            }

            if (btnSync != null) {
                btnSync.Dispose ();
                btnSync = null;
            }

            if (headerView != null) {
                headerView.Dispose ();
                headerView = null;
            }

            if (ImgWebView != null) {
                ImgWebView.Dispose ();
                ImgWebView = null;
            }

            if (InspectionView != null) {
                InspectionView.Dispose ();
                InspectionView = null;
            }

            if (lblAddress1 != null) {
                lblAddress1.Dispose ();
                lblAddress1 = null;
            }

            if (lblAddress2 != null) {
                lblAddress2.Dispose ();
                lblAddress2 = null;
            }

            if (lblCalDate != null) {
                lblCalDate.Dispose ();
                lblCalDate = null;
            }

            if (lblCalDay != null) {
                lblCalDay.Dispose ();
                lblCalDay = null;
            }

            if (lblFailMessage != null) {
                lblFailMessage.Dispose ();
                lblFailMessage = null;
            }

            if (lblInspectionDate != null) {
                lblInspectionDate.Dispose ();
                lblInspectionDate = null;
            }

            if (lblInspectionType != null) {
                lblInspectionType.Dispose ();
                lblInspectionType = null;
            }

            if (lblInspectorName != null) {
                lblInspectorName.Dispose ();
                lblInspectorName = null;
            }

            if (LblNotifyNbr != null) {
                LblNotifyNbr.Dispose ();
                LblNotifyNbr = null;
            }

            if (lblOwnerName != null) {
                lblOwnerName.Dispose ();
                lblOwnerName = null;
            }

            if (lblPathway != null) {
                lblPathway.Dispose ();
                lblPathway = null;
            }

            if (lblPhoneNumber != null) {
                lblPhoneNumber.Dispose ();
                lblPhoneNumber = null;
            }

            if (lblSyncNumber != null) {
                lblSyncNumber.Dispose ();
                lblSyncNumber = null;
            }

            if (leftlblInspectionDate != null) {
                leftlblInspectionDate.Dispose ();
                leftlblInspectionDate = null;
            }

            if (reportPhotoSegment != null) {
                reportPhotoSegment.Dispose ();
                reportPhotoSegment = null;
            }

            if (WebViewScrollView != null) {
                WebViewScrollView.Dispose ();
                WebViewScrollView = null;
            }

            if (WVReport != null) {
                WVReport.Dispose ();
                WVReport = null;
            }
        }
    }
}