<%@ Page Title="" Language="VB" MasterPageFile="~/signatureadmin/Admin.master" AutoEventWireup="false" CodeFile="Voucher.aspx.vb" Inherits="signatureadmin_Voucher"  enableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <script>
        $(document).ready(function () {
            $("a.EnlargeImage").fancybox();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div style="text-align: center;">

    <asp:Label ID="Label2" runat="server" Text="USE VOUCHER" CssClass="RedShadowText" Font-Size="18"></asp:Label>

    <br /><div class="DarkDotHr" style="margin: 0px 100px;"></div><br />
    
    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnCheck">        
        <asp:Label ID="Label1" runat="server" Text="SCAN BARCODE:" 
            Font-Bold="True" Font-Size="15pt" style="display:inline-block; vertical-align: middle;"></asp:Label>            
        <asp:TextBox ID="txtBarCode" runat="server" CssClass="WhiteRadiusBox" Width="465" Font-Size="15pt" style="display: inline-block; vertical-align: middle;"></asp:TextBox>
        <asp:Button ID="btnCheck" runat="server" Text="CHECK" Font-Size="15pt" />
        <br />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* Required" ControlToValidate="txtBarCode"></asp:RequiredFieldValidator>
    </asp:Panel>

    <br /><br /><br />
    
    <div style="text-align: left; display: inline-block; *display: inline; zoom: 1;">
            
        <asp:ListView ID="lvVoucher" runat="server" DataKeyNames="VoucherID" 
            DataSourceID="">
            <EmptyDataTemplate>
                <table runat="server" style="">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="No data was returned." Font-Bold="True" Font-Size="15pt" style="display: inline-block; text-align: left;" ForeColor="Maroon"></asp:Label></td>
                    </tr>
                </table>
            </EmptyDataTemplate>
            <ItemTemplate>
                <tr style="">
                    <td>
                        <asp:HiddenField ID="hfVoucherID" runat="server" Value='<%# Eval("VoucherID") %>' />
                        <asp:HiddenField ID="hfBranchID" runat="server" Value='<%# Eval("VoucherType").BranchID %>' />
                        <asp:Label ID="BranchNameLabel" runat="server" Text='<%# Eval("VoucherType").Branch.Name %>' />
                    </td>
                    <td>
                        <asp:Label ID="NameLabel" runat="server" Text='<%# Eval("VoucherType").Name %>' />
                    </td>                    
                    <td>
                        <asp:Label ID="CustomerNameLabel" runat="server" Text='<%# Eval("CustomerName") %>' />
                        <br />
                        <asp:Label ID="EmailLabel" runat="server" Text='<%# Eval("Email") %>' />
                        <br />
                        <asp:Label ID="ContactLabel" runat="server" Text='<%# Eval("Contact") %>' />
                    </td>
                    <td>
                        <asp:Label ID="DescriptionLabel" runat="server" Text='<%# Eval("VoucherType").Description %>' />
                    </td>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text='<%# Eval("VoucherType.StartDate", "{0:dd-MMM-yyyy}") %>' />
                    </td>
                    <td>
                        <asp:Label ID="ExpiryDateLabel" runat="server" Text='<%# Eval("VoucherType.ExpiryDate", "{0:dd-MMM-yyyy}") %>' />
                    </td>
                    <td>
                        <asp:Label ID="GenerateDateLabel" runat="server" Text='<%# Eval("GenerateDate") %>' />
                    </td>
                    <td>
                        <asp:Label ID="UseDateLabel" runat="server" Text='<%# Eval("UseDate") %>' />
                    </td>
                    <td>
                        <asp:Label ID="StatusLabel" runat="server" Text='<%# Eval("Status") %>' />
                    </td>
                </tr>
            </ItemTemplate>
            <LayoutTemplate>
                <div style="text-align:left;">
                    <asp:Label ID="Label3" runat="server" Text="OUR RECORD:" 
                        Font-Bold="True" Font-Size="15pt" style="display: inline-block; text-align: left;"></asp:Label>
                    <asp:Label ID="lblMessage" runat="server" Text="" Font-Bold="True" Font-Size="15pt" style="display: inline-block; text-align: left;" ForeColor="#CC0000"></asp:Label>
                </div>
                <br />
                <table runat="server" class="CBForm">
                    <tr runat="server">
                        <td runat="server">
                            <table ID="itemPlaceholderContainer" runat="server" border="1" 
                                style="background-color: #FFFFFF;border-collapse: collapse;border-color: #999999;border-style:none;border-width:1px;font-family: Verdana, Arial, Helvetica, sans-serif;">
                                <tr id="Tr1" runat="server" style="background-color:#000000;color: #FFFFFF;">
                                    <th runat="server">
                                        Branch Name</th>
                                    <th runat="server">
                                        Name</th>
                                    <th runat="server">
                                        Customer Details</th>
                                    <th id="Th1" runat="server">
                                        Description</th>    
                                    <th id="Th3" runat="server">
                                        Start Date</th> 
                                    <th id="Th4" runat="server">
                                        Expiry Date</th>                      
                                    <th runat="server">
                                        Generate Date</th>
                                    <th runat="server">
                                        Use Date</th>
                                    <th runat="server">
                                        Status</th>
                                </tr>
                                <tr ID="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr runat="server">
                        <td runat="server" style="">
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
        </asp:ListView>
        
    </div>

    <br />

    <asp:Panel ID="Panel2" runat="server" Visible="False" Font-Bold="True" 
        Font-Size="15pt" EnableViewState="False">
        <br />
        Please kindly check the ID of customer to make sure the above details are matched.
        <br />
        <asp:Button ID="btnAccept" runat="server" Text="ACCEPT" Font-Size="15pt" OnClientClick='return confirm("Are you sure you want to accept this voucher?");' />
    </asp:Panel>       
    
    <br />

    <asp:Panel ID="Panel3" runat="server" Visible="False" Font-Bold="True" 
        Font-Size="15pt" EnableViewState="False" style="text-align: left; display: inline-block; *display: inline; zoom: 1;">
        <br />
        <asp:Label ID="lblInsertVoucher" runat="server" Text="INSERT NEW VOUCHER" CssClass="RedShadowText" Font-Size="18"></asp:Label>
        <br />
        <%--Only key in if customer is presenting "<a style="text-decoration: underline;" class="EnlargeImage" href="/Data/Voucher/VoucherImage/Our%20Deal%20$16%20Lunch%20VoucherScreen%20shot%202012-05-03%20at%2012.11.36%20PM.png">Facebook</a>" Offer Voucher--%>
        Only key in if customer is presenting "Facebook" Offer Voucher
        <table>
            <tr>
                <th>Voucher ID</th>
                <th>Customer Name</th>
                <th>Email</th>
                <th>Contact</th>
                <th></th>
            </tr>
            <tr>
                <td><asp:TextBox ID="txtVoucherID" runat="server" Width="200"></asp:TextBox></td>
                <td><asp:TextBox ID="txtCustomerName" runat="server" Width="200"></asp:TextBox></td>
                <td><asp:TextBox ID="txtEmail" runat="server" Width="200"></asp:TextBox></td>
                <td>
                    04
                    <asp:TextBox ID="txtContact" runat="server" MaxLength="8" Width="200"></asp:TextBox>
                </td>
                <td><asp:Button ID="btnAddVoucher" runat="server" Text="Add this code into 'FB OFFER'" /></td>
            </tr>
        </table>
        <br />
    </asp:Panel>

    <asp:Panel ID="Panel4" runat="server" Visible="False" Font-Bold="True" 
        Font-Size="14" EnableViewState="False" style="text-align: left; display: inline-block; *display: inline; zoom: 1;">
        <br />
        <asp:Label ID="Label5" runat="server" Text="Activate Gift Card" CssClass="RedShadowText" Font-Size="18"></asp:Label>
        <br />
        Gift card is not activated yet, enter below customer information and activate it.
        <br />When customer use gift card, please double check the generate date is no more than 1 year ago.
        <br /><br />
        <table>
            <tr>
                <th>Voucher ID</th>
                <th>Customer Name</th>
                <th>Email</th>
                <th>Contact</th>
                <th>Subscribe?</th>
                <th></th>
            </tr>
            <tr>
                <td><asp:TextBox ID="txtGiftCardVoucherID" runat="server" Width="200"></asp:TextBox></td>
                <td><asp:TextBox ID="txtGiftCardName" runat="server" Width="200"></asp:TextBox></td>
                <td><asp:TextBox ID="txtGiftCardEmail" runat="server" Width="200"></asp:TextBox></td>
                <td>
                    04
                    <asp:TextBox ID="txtGiftCardContact" runat="server" MaxLength="8" Width="200"></asp:TextBox>
                </td>
                <td><asp:CheckBox ID="chkSubscribe" runat="server" /></td>
                <td><asp:Button ID="btnActivate" runat="server" Text="Activate" /></td>
            </tr>
        </table>
        <br />
    </asp:Panel>

    <asp:EntityDataSource ID="edsVoucher" runat="server" 
        ContextTypeName="DAL.ChinaBarDBEntities" 
        EnableFlattening="False" 
        EntitySetName="Vouchers" OrderBy="it.GenerateDate" AutoGenerateWhereClause="True" 
        Where="" ConnectionString="name=ChinaBarDBEntities" 
        DefaultContainerName="ChinaBarDBEntities">
    </asp:EntityDataSource>  
    

</div>
</asp:Content>

