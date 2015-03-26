
Partial Class signatureadmin_Voucher
    Inherits System.Web.UI.Page

    Protected Sub btnCheck_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCheck.Click
        edsVoucher.WhereParameters.Clear()
        edsVoucher.WhereParameters.Add("Code", txtBarCode.Text)
        lvVoucher.DataSourceID = "edsVoucher"
        lvVoucher.DataBind()
        If lvVoucher.Items.Count = 1 Then
            If CType(lvVoucher.Items(0).FindControl("StatusLabel"), Label).Text = "unuse" Then
                'Check Expiry date and Branch
                Dim hfMasterBranchID As String = CType(Master.FindControl("hfBranchID"), HiddenField).Value
                Dim hfBranchID As String = CType(lvVoucher.Items(0).FindControl("hfBranchID"), HiddenField).Value
                Dim lblExpiryDate As String = CType(lvVoucher.Items(0).FindControl("ExpiryDateLabel"), Label).Text
                'If hfMasterBranchID <> hfBranchID Or lblExpiryDate < Today Then
                '    CType(lvVoucher.FindControl("lblMessage"), Label).Text = "This Voucher is Expired or invalid Branch ."
                'Else
                CType(lvVoucher.FindControl("lblMessage"), Label).Text = "This Voucher CAN be used if the below expiry date is valid."
                'Show Accept Button
                Panel2.Visible = True
                'End If

                'Human Validate gift card, voucher type id of 42 43 44
                Dim context As New DAL.ChinaBarDBEntities
                Dim details = (From o In context.Vouchers
                   Where o.Code = txtBarCode.Text
                   Select o).FirstOrDefault
                If details.HumanValidate = True Then
                    CType(lvVoucher.FindControl("lblMessage"), Label).Text = "This is Gift Card, and NOT Activate yet."
                    Panel2.Visible = False
                    'Show activate gift card panel
                    txtGiftCardVoucherID.Text = txtBarCode.Text
                    Panel4.Visible = True
                End If
            Else
                CType(lvVoucher.FindControl("lblMessage"), Label).Text = "USED BEFORE!"
            End If
        ElseIf lvVoucher.Items.Count = 0 Then
            'Show input voucher for Facebook Offer
            If txtBarCode.Text.Substring(0, 2) = "fb" And IsNumeric(txtBarCode.Text.Substring(2, 4)) Then
                Dim tempEmail As String = txtBarCode.Text.Substring(7, txtBarCode.Text.Length - 7)
                Panel3.Visible = True
                txtVoucherID.Text = txtBarCode.Text
                txtEmail.Text = tempEmail
            End If
        Else
            Dim lbl As Label = CType(lvVoucher.FindControl("lblMessage"), Label)
            If Not lbl Is Nothing Then
                lbl.Text = "Duplicate Vouchers Detected."
            End If
        End If
    End Sub

    Protected Sub btnAccept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        Dim sID As Integer = CType(lvVoucher.Items(0).FindControl("hfVoucherID"), HiddenField).Value
        Dim context As New DAL.ChinaBarDBEntities
        Dim details = (From o In context.Vouchers
                       Where o.VoucherId = sID
                       Select o).FirstOrDefault
        details.UseDate = DateTime.Now
        details.Status = "used"
        details.UseBranchID = CType(Master.FindControl("hfBranchID"), HiddenField).Value
        context.SaveChanges()
        lvVoucher.DataBind()
        Dim lbl As Label = CType(lvVoucher.FindControl("lblMessage"), Label)
        If Not lbl Is Nothing Then
            lbl.Text = "Done."
        End If
    End Sub

    Protected Sub btnAddVoucher_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddVoucher.Click
        Dim context As New DAL.ChinaBarDBEntities
        Dim voucher As New DAL.Voucher
        voucher.VoucherTypeID = 57
        voucher.Code = txtVoucherID.Text
        voucher.CustomerName = txtCustomerName.Text
        voucher.Email = txtEmail.Text
        voucher.Contact = "614" + txtContact.Text
        voucher.GenerateDate = Today
        voucher.UseDate = DateTime.Now
        voucher.Status = "used"
        context.AddToVouchers(voucher)

        'Insert into Subscribe
        Dim subscribe As New DAL.Subscribe
        subscribe.Name = txtCustomerName.Text
        subscribe.Email = txtEmail.Text
        subscribe.Contact = "614" + txtContact.Text
        subscribe.Type = "Facebook Buy 1 get 1 Free Lunch Voucher"
        subscribe.Date = Today.Date
        context.AddToSubscribes(subscribe)

        'Insert into Mobile
        Dim mobile As New DAL.Mobile
        mobile.Name = txtCustomerName.Text
        mobile.MobileNumber = "614" + txtContact.Text
        context.AddToMobiles(mobile)

        context.SaveChanges()

        Dim strScript As String = "alert('Voucher Inserted'); window.location.href = 'Voucher.aspx';"
        ClientScript.RegisterStartupScript(GetType(String), "loadScript", strScript, True)

    End Sub

    Protected Sub btnActivate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnActivate.Click
        Dim context As New DAL.ChinaBarDBEntities
        Dim details = (From o In context.Vouchers
           Where o.Code = txtBarCode.Text
           Select o).FirstOrDefault
        If details.HumanValidate = True Then
            details.HumanValidate = Nothing
            details.GenerateDate = DateTime.Now
            details.CustomerName = txtGiftCardName.Text
            details.Email = txtGiftCardEmail.Text
            details.Contact = "61" + txtGiftCardContact.Text
            If chkSubscribe.Checked Then
                If txtGiftCardContact.Text <> "" And txtGiftCardContact.Text.Length = 8 Then
                    Dim mobile As New DAL.Mobile
                    mobile.Name = txtGiftCardName.Text
                    mobile.MobileNumber = "614" + txtGiftCardContact.Text
                    context.AddToMobiles(mobile)
                End If

                Dim subscribe As New DAL.Subscribe
                subscribe.Name = txtGiftCardName.Text
                subscribe.Email = txtGiftCardEmail.Text
                subscribe.Contact = "614" + txtGiftCardContact.Text
                subscribe.Type = "Gift Card"
                subscribe.Date = Today.Date
                context.AddToSubscribes(subscribe)
            End If

            context.SaveChanges()

            CType(lvVoucher.FindControl("lblMessage"), Label).Text = "Gift Card Activated"
            lvVoucher.DataBind()


        End If
    End Sub
End Class
