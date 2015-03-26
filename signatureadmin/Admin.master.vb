
Partial Class signatureadmin_admin
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Trim(My.User.Name) = "burwoodreception" Then
            lblBranch.Text = "Burwood East"
            hfBranchID.Value = "1"
        End If
        If Trim(My.User.Name) = "cityreception" Then
            lblBranch.Text = "Chinatown"
            hfBranchID.Value = "2"
        End If
        If Trim(My.User.Name) = "eppingreception" Then
            lblBranch.Text = "Pacific Epping"
            hfBranchID.Value = "4"
        End If
        If Trim(My.User.Name) = "burwoodmanager" Then
            lblBranch.Text = "Burwood East"
            hfBranchID.Value = "1"
        End If
        If Trim(My.User.Name) = "citymanager" Then
            lblBranch.Text = "Chinatown"
            hfBranchID.Value = "2"
        End If
        If Trim(My.User.Name) = "eppingmanager" Then
            lblBranch.Text = "Pacific Epping"
            hfBranchID.Value = "4"
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If My.User.IsInRole("Administrator") Then
            lblRole.Text = "ADMINISTRATION"
            LoginStatus1.Visible = True
        ElseIf My.User.IsInRole("Receptionist") Then
            lblRole.Text = "RECEPTION"
            LoginStatus1.Visible = True
        ElseIf My.User.IsInRole("Manager") Then
            lblRole.Text = "MANAGER"
            LoginStatus1.Visible = True
        End If
    End Sub
End Class

