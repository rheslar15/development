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
    [Register ("InspectionViewController")]
    partial class InspectionViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView addPictureView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton beginInspectionButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnClearSearch { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnFinish { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnHome { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnLogout { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnNext { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnNotify { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSave { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSearchOptions { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSelectAll { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSync { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView cameraImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton createPunchListBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton docViewBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView headerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint htLblSequence { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView imageCollectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imageShowView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView imagesScrollView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView inspectionViewScrollView { get; set; }

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
        UIKit.UILabel lblInspectionDate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblinspectionName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblinspectionNameRight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblInspectionType { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField LblNotifyNbr { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblOwnerName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblPhoneNumber { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblSearchOptions { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblSequence { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField lblSyncNumber { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel locationIdHeaderLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView OptionsTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ownerDetailsView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton progressInspectionBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton punchAddItemBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton punchFinishButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint rhsHeaderHeightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView rhsTableHeaderView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView sequenceTable { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint spSequenceLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton takePictureBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton takePictureButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView takePictureView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtSearchOptions { get; set; }


        [Action ("btnNext_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnNext_TouchUpInside (UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (addPictureView != null) {
                addPictureView.Dispose ();
                addPictureView = null;
            }

            if (beginInspectionButton != null) {
                beginInspectionButton.Dispose ();
                beginInspectionButton = null;
            }

            if (btnClearSearch != null) {
                btnClearSearch.Dispose ();
                btnClearSearch = null;
            }

            if (btnFinish != null) {
                btnFinish.Dispose ();
                btnFinish = null;
            }

            if (btnHome != null) {
                btnHome.Dispose ();
                btnHome = null;
            }

            if (btnLogout != null) {
                btnLogout.Dispose ();
                btnLogout = null;
            }

            if (btnNext != null) {
                btnNext.Dispose ();
                btnNext = null;
            }

            if (btnNotify != null) {
                btnNotify.Dispose ();
                btnNotify = null;
            }

            if (btnSave != null) {
                btnSave.Dispose ();
                btnSave = null;
            }

            if (btnSearchOptions != null) {
                btnSearchOptions.Dispose ();
                btnSearchOptions = null;
            }

            if (btnSelectAll != null) {
                btnSelectAll.Dispose ();
                btnSelectAll = null;
            }

            if (btnSync != null) {
                btnSync.Dispose ();
                btnSync = null;
            }

            if (cameraImage != null) {
                cameraImage.Dispose ();
                cameraImage = null;
            }

            if (createPunchListBtn != null) {
                createPunchListBtn.Dispose ();
                createPunchListBtn = null;
            }

            if (docViewBtn != null) {
                docViewBtn.Dispose ();
                docViewBtn = null;
            }

            if (headerView != null) {
                headerView.Dispose ();
                headerView = null;
            }

            if (htLblSequence != null) {
                htLblSequence.Dispose ();
                htLblSequence = null;
            }

            if (imageCollectionView != null) {
                imageCollectionView.Dispose ();
                imageCollectionView = null;
            }

            if (imageShowView != null) {
                imageShowView.Dispose ();
                imageShowView = null;
            }

            if (imagesScrollView != null) {
                imagesScrollView.Dispose ();
                imagesScrollView = null;
            }

            if (inspectionViewScrollView != null) {
                inspectionViewScrollView.Dispose ();
                inspectionViewScrollView = null;
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

            if (lblInspectionDate != null) {
                lblInspectionDate.Dispose ();
                lblInspectionDate = null;
            }

            if (lblinspectionName != null) {
                lblinspectionName.Dispose ();
                lblinspectionName = null;
            }

            if (lblinspectionNameRight != null) {
                lblinspectionNameRight.Dispose ();
                lblinspectionNameRight = null;
            }

            if (lblInspectionType != null) {
                lblInspectionType.Dispose ();
                lblInspectionType = null;
            }

            if (LblNotifyNbr != null) {
                LblNotifyNbr.Dispose ();
                LblNotifyNbr = null;
            }

            if (lblOwnerName != null) {
                lblOwnerName.Dispose ();
                lblOwnerName = null;
            }

            if (lblPhoneNumber != null) {
                lblPhoneNumber.Dispose ();
                lblPhoneNumber = null;
            }

            if (lblSearchOptions != null) {
                lblSearchOptions.Dispose ();
                lblSearchOptions = null;
            }

            if (lblSequence != null) {
                lblSequence.Dispose ();
                lblSequence = null;
            }

            if (lblSyncNumber != null) {
                lblSyncNumber.Dispose ();
                lblSyncNumber = null;
            }

            if (locationIdHeaderLabel != null) {
                locationIdHeaderLabel.Dispose ();
                locationIdHeaderLabel = null;
            }

            if (OptionsTableView != null) {
                OptionsTableView.Dispose ();
                OptionsTableView = null;
            }

            if (ownerDetailsView != null) {
                ownerDetailsView.Dispose ();
                ownerDetailsView = null;
            }

            if (progressInspectionBtn != null) {
                progressInspectionBtn.Dispose ();
                progressInspectionBtn = null;
            }

            if (punchAddItemBtn != null) {
                punchAddItemBtn.Dispose ();
                punchAddItemBtn = null;
            }

            if (punchFinishButton != null) {
                punchFinishButton.Dispose ();
                punchFinishButton = null;
            }

            if (rhsHeaderHeightConstraint != null) {
                rhsHeaderHeightConstraint.Dispose ();
                rhsHeaderHeightConstraint = null;
            }

            if (rhsTableHeaderView != null) {
                rhsTableHeaderView.Dispose ();
                rhsTableHeaderView = null;
            }

            if (sequenceTable != null) {
                sequenceTable.Dispose ();
                sequenceTable = null;
            }

            if (spSequenceLabel != null) {
                spSequenceLabel.Dispose ();
                spSequenceLabel = null;
            }

            if (takePictureBtn != null) {
                takePictureBtn.Dispose ();
                takePictureBtn = null;
            }

            if (takePictureButton != null) {
                takePictureButton.Dispose ();
                takePictureButton = null;
            }

            if (takePictureView != null) {
                takePictureView.Dispose ();
                takePictureView = null;
            }

            if (txtSearchOptions != null) {
                txtSearchOptions.Dispose ();
                txtSearchOptions = null;
            }
        }
    }
}