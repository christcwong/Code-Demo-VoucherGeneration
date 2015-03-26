<%@ Page Title="" Language="VB" MasterPageFile="~/signatureadmin/Admin.master" AutoEventWireup="false" CodeFile="VoucherManagement.aspx.vb" Inherits="signatureadmin_VoucherManagement" Async="true" EnableEventValidation="false" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <link rel="stylesheet" href="/Scripts/fancybox/jquery.fancybox-1.3.4.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="/Scripts/imgareaselect/imgareaselect-default.css" type="text/css" media="screen" />
    <script src="/Scripts/jquery.imgareaselect.min.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="Server">
    <div style="text-align: center;">

        <asp:Label ID="Label2" runat="server" Text="VOUCHER & PROMOTION TYPE" CssClass="RedShadowText" Font-Size="18"></asp:Label>

        <br />
        <div class="DarkDotHr" style="margin: 0px 100px;"></div>
        <br />

        <asp:Panel ID="Panel1" runat="server" Style="text-align: left; display: inline-block; *display: inline; zoom: 1;">
            <asp:DropDownList ID="ddlBranch" runat="server" DataSourceID="edsBranch"
                DataTextField="Name" DataValueField="BranchID" AutoPostBack="True" AppendDataBoundItems="true">
            </asp:DropDownList>
            <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True">
                <asp:ListItem>Active</asp:ListItem>
                <asp:ListItem>Expired</asp:ListItem>
                <asp:ListItem>Deleted</asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:ListView ID="lvVoucherType" runat="server" DataKeyNames="VoucherTypeID" InsertItemPosition="LastItem"
                DataSourceID="edsVoucherType">
                <EditItemTemplate>
                    <tr style="background-color: #008A8C; color: #FFFFFF;">
                        <td></td>
                        <td>
                            <asp:TextBox ID="NameTextBox" runat="server" Text='<%# Bind("Name") %>' />
                        </td>
                        <td>
                            <asp:TextBox ID="DescriptionTextBox" runat="server" Text='<%# Bind("Description") %>' />
                        </td>
                        <td>
                            <asp:TextBox ID="PromoCodeTextBox" runat="server" Text='<%# Bind("PromoCode") %>' Width="50" />
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("CreateDate") %>' />
                        </td>
                        <td>
                            <asp:FileUpload ID="fuPhoto" runat="server" />
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Image") %>' />
                        </td>
                        <td>
                            <%--<a id="EditImageQRXY" href='<%# "/Data/Voucher/VoucherImage/" + Eval("Image")%>' style="padding:0px">
                                Click to Edit XY of QR Code<br />--%>
                            <asp:Image ID="Image2" runat="server" ImageUrl='<%# "/Data/Voucher/VoucherImage/" + Eval("Image")%>' Width="40" Height="40" />
                            <%--</a>--%>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlQRDimension" runat="server" Text='<%# Bind("QRDimension")%>' Width="50"
                                onchange="hideQRSelector()">
                                <asp:ListItem>100</asp:ListItem>
                                <asp:ListItem>150</asp:ListItem>
                                <asp:ListItem>200</asp:ListItem>
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("CodeXPosition") %>' Width="50" />
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("CodeYPosition") %>' Width="50" />
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("StartDate") %>' Width="100" />
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("ExpiryDate") %>' Width="100" />
                        </td>
                        <td>
                            <div style="width: 60px">
                                <asp:CheckBox ID="cbMonUsable" runat="server" Checked='<%# Bind("MonUsable") %>' Text="Mon" /><br />
                                <asp:CheckBox ID="cbTueUsable" runat="server" Checked='<%# Bind("TueUsable") %>' Text="Tue" /><br />
                                <asp:CheckBox ID="cbWedUsable" runat="server" Checked='<%# Bind("WedUsable") %>' Text="Wed" /><br />
                                <asp:CheckBox ID="cbThuUsable" runat="server" Checked='<%# Bind("ThuUsable") %>' Text="Thu" /><br />
                                <asp:CheckBox ID="cbFriUsable" runat="server" Checked='<%# Bind("FriUsable") %>' Text="Fri" /><br />
                                <asp:CheckBox ID="cbSatUsable" runat="server" Checked='<%# Bind("SatUsable") %>' Text="Sat" /><br />
                                <asp:CheckBox ID="cbSunUsable" runat="server" Checked='<%# Bind("SunUsable") %>' Text="Sun" /><br />
                            </div>
                        </td>
                        <td>
                            <%--<asp:TextBox ID="TextBox7" runat="server" Text='<%# Bind("Status") %>' Width="100" />--%>
                            <asp:DropDownList ID="ddlStatus" runat="server" Text='<%# Bind("Status") %>' Width="100"
                                onchange="hideQRSelector()">
                                <asp:ListItem>Active</asp:ListItem>
                                <asp:ListItem>Expired</asp:ListItem>
                                <asp:ListItem>Deleted</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="UpdateButton" runat="server" CommandName="Update"
                                Text="Update" />
                            <asp:Button ID="CancelButton" runat="server" CommandName="Cancel"
                                Text="Cancel" />
                        </td>
                    </tr>
                </EditItemTemplate>
                <EmptyDataTemplate>
                    <table id="Table3" runat="server" style="">
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="No data was returned." Font-Bold="True" Font-Size="15pt" Style="display: inline-block; text-align: left;" ForeColor="Maroon"></asp:Label></td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <InsertItemTemplate>
                    <tr style="">
                        <td>Only numbers and letters:
                        </td>
                        <td>
                            <asp:TextBox ID="NameTextBox" runat="server" Width="200" Text='<%# Bind("Name") %>' />
                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="NameTextBox" FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ">
                            </ajaxToolkit:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="DescriptionTextBox" runat="server" Width="300" Text='<%# Bind("Description") %>' />
                        </td>
                        <td>
                            <asp:TextBox ID="PromoCodeTextBox" runat="server" Text='<%# Bind("PromoCode") %>' Width="50" />
                        </td>
                        <td></td>
                        <td>
                            <%--<ajaxToolkit:AsyncFileUpload runat="server" OnUploadedComplete="ProcessUpload" OnClientUploadComplete="asyncUploadComplete" ID="asyncFuPhoto" ThrobberID="spanUploading" ClientIDMode="AutoID" Width="200"/><br />
                            <span id="spanUploading" runat="server">Uploading...</span>
                            <asp:HiddenField runat="server" ID="fileName"/>--%>
                            <asp:FileUpload ID="fuPhoto" runat="server" />
                        </td>
                        <td>
                            <%--<asp:LinkButton runat="server" ID="btnXY" OnClick="btnXY_Click" Visible="false">Select Image First</asp:LinkButton>
                            <asp:Image runat="server" ID="insertCanvas"/>
                            <img src="" id="imgUpload" alt="" /> --%>
                            <%--<html5:Canvas ID="insertCanvas" runat="server" Visible="false" Height="300" Width="400">
                                Canvas Place Holder
                            </html5:Canvas>--%>
                            <%--<canvas id="insertCanvas" width="400" height="300">Canvas Place Holder</canvas>--%>

                        </td>
                        <td>
                            <asp:DropDownList ID="ddlQRDimension" runat="server" Text='<%# Bind("QRDimension")%>' Width="50">
                                <asp:ListItem>100</asp:ListItem>
                                <asp:ListItem>150</asp:ListItem>
                                <asp:ListItem>200</asp:ListItem>
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox8" runat="server"
                                Text='<%# Bind("CodeXPosition") %>' Width="50" />
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox9" runat="server"
                                Text='<%# Bind("CodeYPosition") %>' Width="50" />
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox10" runat="server"
                                Text='<%# Bind("StartDate") %>' Width="100" />
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox11" runat="server"
                                Text='<%# Bind("ExpiryDate") %>' Width="100" />
                        </td>
                        <td>
                            <div style="width: 60px">
                                <asp:CheckBox ID="cbMonUsable" runat="server" Checked='<%# Bind("MonUsable") %>' Text="Mon" /><br />
                                <asp:CheckBox ID="cbTueUsable" runat="server" Checked='<%# Bind("TueUsable") %>' Text="Tue" /><br />
                                <asp:CheckBox ID="cbWedUsable" runat="server" Checked='<%# Bind("WedUsable") %>' Text="Wed" /><br />
                                <asp:CheckBox ID="cbThuUsable" runat="server" Checked='<%# Bind("ThuUsable") %>' Text="Thu" /><br />
                                <asp:CheckBox ID="cbFriUsable" runat="server" Checked='<%# Bind("FriUsable") %>' Text="Fri" /><br />
                                <asp:CheckBox ID="cbSatUsable" runat="server" Checked='<%# Bind("SatUsable") %>' Text="Sat" /><br />
                                <asp:CheckBox ID="cbSunUsable" runat="server" Checked='<%# Bind("SunUsable") %>' Text="Sun" /><br />
                            </div>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox12" runat="server"
                                Text='Active' Enabled="False" Width="50" />
                        </td>
                        <td>
                            <asp:LinkButton ID="InsertButton" runat="server" CommandName="Insert"
                                Text="Insert" />
                            <asp:LinkButton ID="CancelButton" runat="server" CommandName="Cancel"
                                Text="Clear" />
                        </td>
                    </tr>
                </InsertItemTemplate>
                <ItemTemplate>
                    <tr style="">
                        <td>
                            <asp:HiddenField ID="hfVoucherTypeID" runat="server" Value='<%# Eval("VoucherTypeID") %>' />
                            <asp:Label ID="BranchNameLabel" runat="server" Text='<%# Eval("Branch").Name %>' />
                        </td>
                        <td>
                            <asp:Label ID="NameLabel" runat="server" Text='<%# Eval("Name") %>' />
                        </td>
                        <td>
                            <asp:Label ID="DescriptionLabel" runat="server" Text='<%# Eval("Description") %>' />
                        </td>
                        <td>
                            <asp:Label ID="PromoCodeLabel" runat="server" Text='<%# Eval("PromoCode") %>' Width="50" />
                        </td>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("CreateDate", "{0:dd-MMM-yyyy}") %>' />
                        </td>
                        <td>
                            <a class="EnlargeImage" href='<%# "/Data/Voucher/VoucherImage/" + Eval("Image") %>'>
                                <asp:Image ID="Image2" runat="server" ImageUrl='<%# "~/Data/Voucher/VoucherImage/" + Eval("Image") %>' Width="20" Height="20" />
                            </a>
                        </td>
                        <td>
                            <%--<a class="EnlargeImage" href='<%# "/Data/Voucher/VoucherImage/" + Eval("Image") %>'>Pick XY</a>--%>
                        </td>
                        <td>
                            <asp:Label ID="Label10" runat="server" Text='<%# Eval("QRDimension")%>' />
                        </td>
                        <td>
                            <asp:Label ID="Label6" runat="server" Text='<%# Eval("CodeXPosition") %>' />
                        </td>
                        <td>
                            <asp:Label ID="Label7" runat="server" Text='<%# Eval("CodeYPosition") %>' />
                        </td>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text='<%# Eval("StartDate", "{0:dd-MMM-yyyy}") %>' />
                        </td>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text='<%# Eval("ExpiryDate", "{0:dd-MMM-yyyy}") %>' />
                        </td>
                        <td>
                            <div style="width: 60px">
                                <asp:CheckBox ID="cbMonUsable" runat="server" Checked='<%# Bind("MonUsable") %>' Enabled="false" Text="Mon" /><br />
                                <asp:CheckBox ID="cbTueUsable" runat="server" Checked='<%# Bind("TueUsable") %>' Enabled="false" Text="Tue" /><br />
                                <asp:CheckBox ID="cbWedUsable" runat="server" Checked='<%# Bind("WedUsable") %>' Enabled="false" Text="Wed" /><br />
                                <asp:CheckBox ID="cbThuUsable" runat="server" Checked='<%# Bind("ThuUsable") %>' Enabled="false" Text="Thu" /><br />
                                <asp:CheckBox ID="cbFriUsable" runat="server" Checked='<%# Bind("FriUsable") %>' Enabled="false" Text="Fri" /><br />
                                <asp:CheckBox ID="cbSatUsable" runat="server" Checked='<%# Bind("SatUsable") %>' Enabled="false" Text="Sat" /><br />
                                <asp:CheckBox ID="cbSunUsable" runat="server" Checked='<%# Bind("SunUsable") %>' Enabled="false" Text="Sun" /><br />
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text='<%# Eval("Status") %>' />
                        </td>
                        <td>
                            <asp:LinkButton ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                            <%--<asp:LinkButton ID="DeleteButton" runat="server" CommandName="Delete" OnClientClick='return confirm("Are you sure you want to delete this item?");'
                                Text="Delete" />--%>
                        </td>
                    </tr>
                </ItemTemplate>
                <LayoutTemplate>
                    <table id="Table4" runat="server" class="CBForm">
                        <tr id="Tr4" runat="server">
                            <td id="Td3" runat="server">
                                <table id="Table5" runat="server" border="1"
                                    style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;">
                                    <tr id="Tr5" runat="server" style="background-color: #000000; color: #FFFFFF;">
                                        <th id="Th4" runat="server">Branch Name</th>
                                        <th id="Th13" runat="server">Voucher Name</th>
                                        <th id="Th14" runat="server">Description</th>
                                        <th id="Th23" runat="server">Promo Code</th>
                                        <th id="Th16" runat="server">Create Date</th>
                                        <th id="Th17" runat="server">Image</th>
                                        <th id="Th24" runat="server">QR Location Picker</th>
                                        <th id="Th25" runat="server">QR Code Dimension</th>
                                        <th id="Th18" runat="server">Code X Position</th>
                                        <th id="Th19" runat="server">Code Y Position</th>
                                        <th id="Th20" runat="server">Start Date</th>
                                        <th id="Th21" runat="server">Expiry Date</th>
                                        <th id="Th26" runat="server">Usable WeekDay</th>
                                        <th id="Th15" runat="server">Status</th>
                                        <th id="Th22" runat="server"></th>
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="Tr7" runat="server">
                            <td id="Td4" runat="server"
                                style="text-align: center; background-color: #CCCCCC; font-family: Verdana, Arial, Helvetica, sans-serif; color: #000000;"></td>
                        </tr>
                    </table>
                </LayoutTemplate>
            </asp:ListView>
        </asp:Panel>

        <br />
        <br />
        <br />

        <asp:Label ID="Label3" runat="server" Text="GENERATE VOUCHER " CssClass="RedShadowText" Font-Size="18"></asp:Label>

        <br />
        <div class="DarkDotHr" style="margin: 0px 100px;"></div>
        <br />
        <asp:Panel ID="Panel4" runat="server" Style="text-align: left; display: inline-block; *display: inline; zoom: 1;">
            <asp:DropDownList ID="ddlVoucherType" runat="server"
                DataSourceID="edsVoucherType" DataTextField="Name"
                DataValueField="VoucherTypeID">
            </asp:DropDownList>
            <br />
            <table>
                <tr>
                    <th>Customer Name</th>
                    <th>Email</th>
                    <th>Contact</th>
                    <th></th>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtCustomerName" runat="server" Enabled="False"></asp:TextBox></td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" Enabled="False"></asp:TextBox></td>
                    <td>
                        <asp:TextBox ID="txtContact" runat="server" Enabled="False"></asp:TextBox></td>
                    <td></td>
                </tr>
            </table>

            <br />

            Upload csv file with above headers: 
            <asp:FileUpload ID="fuCsv" runat="server" />

            <asp:Button ID="btnUpload" runat="server" Text="Save and Send" />

            <asp:Button ID="btnUploadOnly" runat="server" Text="Save Only" />

            <asp:Button ID="btnUploadQROnly" runat="server" Text="Save QR Only" />

            <%--<asp:Button ID="btnUploadWithQR" runat="server" Text="Save with own QR" />

            <asp:Button ID="btnSubscriber2013" runat="server" Text="Save and Send Subscriber 2013" ToolTip="upload random csv" />--%>
            <br />
            <br />
            <asp:Label runat="server">Notice : For "Save and Send" Function, Please modify the Promotion Email Template before use! </asp:Label><asp:HyperLink runat="server" NavigateUrl="~/signatureadmin/EmailTemplateManagement.aspx">Click Here</asp:HyperLink>
        </asp:Panel>
        <br />
        <br />
        <asp:UpdatePanel ID="Panel2" runat="server" Style="text-align: left; display: inline-block; *display: inline; zoom: 1;">
            <ContentTemplate>
                <asp:Timer ID="Timer1" runat="server" Interval="15000" Enabled="True" OnTick="Timer1_Tick">
                </asp:Timer>
                <asp:ListView ID="lvVoucherGenerationRequest" runat="server" DataKeyNames="VoucherGenerationRequestID" DataSourceID="edsVoucherGenerationRequest">
                    <ItemTemplate>
                        <tr style="">
                            <td>
                                <asp:Label ID="lblReqCreateTime" runat="server" Text='<%# Eval("RequestCreateUTC")%>' />
                            </td>
                            <td>
                                <asp:Label ID="lblVoucherTypeBranch" runat="server" Text='<%# Eval("VoucherType").Branch.Name%>' />
                            </td>
                            </td>
                        <td>
                            <asp:Label ID="lblVoucherTypeID" runat="server" Text='<%# Eval("VoucherType").Name%>' />
                        </td>
                            <td>
                                <asp:HyperLink ID="hlCsvFilePath" runat="server" NavigateUrl='<%# Eval("CsvPath")%>'>Click</asp:HyperLink>
                            </td>
                            <td>
                                <asp:Label ID="lblReqStatus" runat="server" Text='<%# Eval("Status") %>' />
                            </td>
                            <td>
                                <asp:Label ID="lblReqStartTime" runat="server" Text='<%# Eval("RequestStartUTC")%>' />
                            </td>
                            <td>
                                <asp:Label ID="lblReqLastModifyTime" runat="server" Text='<%# Eval("LastModifyUTC") %>' />
                            </td>
                            <td>
                                <asp:HyperLink ID="hlZipFileLink" runat="server" NavigateUrl='<%# Eval("ZipFileLink")%>'><%# If(String.IsNullOrEmpty(Eval("ZipFileLink")), "", "Click")%></asp:HyperLink>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <table id="Table1" runat="server" style="">
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="No Generation Request History was returned." Font-Bold="True" Font-Size="15pt" Style="display: inline-block; text-align: left;" ForeColor="Maroon"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="Table2" runat="server" class="CBForm">
                            <tr>
                                <td colspan="8" align="right">Last Update : <%= DateTime.Now%>
                                </td>
                            </tr>
                            <tr id="Tr1" runat="server">
                                <td id="Td1" runat="server">
                                    <table id="itemPlaceholderContainer" runat="server" border="1"
                                        style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;">
                                        <tr id="Tr2" runat="server" style="background-color: #000000; color: #FFFFFF;">
                                            <th id="Th1" runat="server">Request Create Time (UTC)</th>
                                            <th id="Th26" runat="server">Branch Name</th>
                                            <th id="Th27" runat="server">Voucher Name</th>
                                            <th id="Th3" runat="server">CSV File</th>
                                            <th id="Th5" runat="server">Status</th>
                                            <th id="Th6" runat="server">Request Start Time (UTC)</th>
                                            <th id="Th10" runat="server">Last Modify Time (UTC)</th>
                                            <th id="Th28" runat="server">Zip File Link</th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="Tr3" runat="server">
                                <td id="Td2" runat="server"
                                    style="text-align: center; background-color: #CCCCCC; font-family: Verdana, Arial, Helvetica, sans-serif; color: #000000;">
                                    <asp:DataPager ID="DataPager2" runat="server">
                                        <Fields>
                                            <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True"
                                                ShowLastPageButton="False" ShowPreviousPageButton="True" ShowNextPageButton="False" />
                                            <asp:NumericPagerField />
                                            <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="False"
                                                ShowLastPageButton="True" ShowPreviousPageButton="False" ShowNextPageButton="True" />
                                        </Fields>
                                    </asp:DataPager>
                                </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                </asp:ListView>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <asp:Label ID="Label11" runat="server" Text="VOUCHERS" CssClass="RedShadowText" Font-Size="18"></asp:Label>
        <br />
        <div class="DarkDotHr" style="margin: 0px 100px;"></div>
        <br />
        <asp:Panel ID="Panel3" runat="server" Style="text-align: left; display: inline-block; *display: inline; zoom: 1;">
            <%--<asp:UpdatePanel ID="Panel3" runat="server" Style="text-align: left; display: inline-block; *display: inline; zoom: 1;">
            <ContentTemplate>--%>
            <%--<asp:Timer ID="Timer2" runat="server" Interval="30000" Enabled="True" OnTick="Timer2_Tick">
                </asp:Timer>--%>
            <asp:ListView ID="lvVoucher" runat="server" DataKeyNames="VoucherID"
                DataSourceID="edsVoucher">
                <EmptyDataTemplate>
                    <table id="Table1" runat="server" style="">
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="No data was returned." Font-Bold="True" Font-Size="15pt" Style="display: inline-block; text-align: left;" ForeColor="Maroon"></asp:Label></td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <ItemTemplate>
                    <tr style="">
                        <td>
                            <asp:HiddenField ID="hfVoucherID" runat="server" Value='<%# Eval("VoucherID") %>' />
                            <asp:Label ID="BranchNameLabel" runat="server" Text='<%# Eval("VoucherType").Branch.Name %>' />
                        </td>
                        <td>
                            <asp:Label ID="CodeLabel" runat="server" Text='<%# Common.CutDesc(Eval("Code"),5) %>' title='<%# Eval("Code") %>' />
                        </td>
                        <td>
                            <asp:Label ID="NameLabel" runat="server" Text='<%# Eval("VoucherType").Name %>' />
                        </td>
                        <td>
                            <asp:Label ID="CustomerNameLabel" runat="server" Text='<%# Eval("CustomerName") %>' />
                        </td>
                        <td>
                            <asp:Label ID="EmailLabel" runat="server" Text='<%# Eval("Email") %>' />
                        </td>
                        <td>
                            <asp:Label ID="ContactLabel" runat="server" Text='<%# Eval("Contact") %>' />
                        </td>
                        <td>
                            <a class="EnlargeImage" href='<%# "/CustomerVoucherImage/" + Eval("Image") %>'>
                                <asp:Image ID="Image1" runat="server" ImageUrl='<%# "/CustomerVoucherImage/" + Eval("Image") %>' Width="20" Height="20" />
                            </a>
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
                        <td>
                            <asp:LinkButton ID="DeleteButton" runat="server" CommandName="Delete" OnClientClick='return confirm("Are you sure you want to delete this item?");'
                                Text="Delete" />
                        </td>
                    </tr>
                </ItemTemplate>
                <LayoutTemplate>
                    <table id="Table2" runat="server" class="CBForm">
                        <tr id="Tr1" runat="server">
                            <td id="Td1" runat="server">
                                <table id="itemPlaceholderContainer" runat="server" border="1"
                                    style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;">
                                    <tr id="Tr2" runat="server" style="background-color: #000000; color: #FFFFFF;">
                                        <th id="Th1" runat="server">Branch Name</th>
                                        <th id="Th11" runat="server">Code</th>
                                        <th id="Th2" runat="server">Voucher Name</th>
                                        <th id="Th3" runat="server">Customer Name</th>
                                        <th id="Th5" runat="server">Email</th>
                                        <th id="Th6" runat="server">Contact</th>
                                        <th id="Th10" runat="server">Image</th>
                                        <th id="Th7" runat="server">Generate Date</th>
                                        <th id="Th8" runat="server">Use Date</th>
                                        <th id="Th9" runat="server">Status</th>
                                        <th id="Th12" runat="server"></th>
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="Tr3" runat="server">
                            <td id="Td2" runat="server"
                                style="text-align: center; background-color: #CCCCCC; font-family: Verdana, Arial, Helvetica, sans-serif; color: #000000;">
                                <asp:DataPager ID="DataPager1" runat="server">
                                    <Fields>
                                        <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True"
                                            ShowLastPageButton="False" ShowPreviousPageButton="True" ShowNextPageButton="False" />
                                        <asp:NumericPagerField />
                                        <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="False"
                                            ShowLastPageButton="True" ShowPreviousPageButton="False" ShowNextPageButton="True" />
                                    </Fields>
                                </asp:DataPager>
                            </td>
                        </tr>
                    </table>
                </LayoutTemplate>
            </asp:ListView>
            <%--    </ContentTemplate>
        </asp:UpdatePanel>--%>
        </asp:Panel>

        <asp:EntityDataSource ID="edsVoucher" runat="server"
            ContextTypeName="DAL.ChinaBarDBEntities" EnableDelete="True"
            EnableFlattening="False" EnableInsert="True" EnableUpdate="True"
            EntitySetName="Vouchers" OrderBy="it.Generatedate desc">
        </asp:EntityDataSource>

        <asp:EntityDataSource ID="edsVoucherType" runat="server"
            ContextTypeName="DAL.ChinaBarDBEntities" EnableDelete="True"
            EnableFlattening="False" EnableInsert="True" EnableUpdate="True"
            EntitySetName="VoucherTypes" OrderBy="it.CreateDate DESC"
            ConnectionString="name=ChinaBarDBEntities" AutoGenerateWhereClause="True"
            DefaultContainerName="ChinaBarDBEntities">
            <WhereParameters>
                <asp:ControlParameter Name="BranchID" ControlID="ddlBranch" PropertyName="SelectedValue" Type="Int32" />
                <asp:ControlParameter Name="Status" ControlID="ddlStatus" PropertyName="SelectedValue" />
            </WhereParameters>
        </asp:EntityDataSource>

        <asp:EntityDataSource ID="edsVoucherGenerationRequest" runat="server"
            ContextTypeName="DAL.ChinaBarDBEntities" EnableDelete="False"
            EnableFlattening="False" EnableInsert="True" EnableUpdate="False"
            EntitySetName="VoucherGenerationRequests" OrderBy="it.LastModifyUTC DESC">
        </asp:EntityDataSource>

        <asp:EntityDataSource ID="edsBranch" runat="server"
            ContextTypeName="DAL.ChinaBarDBEntities" EnableDelete="True"
            EnableFlattening="False" EnableInsert="True" EnableUpdate="True"
            EntitySetName="Branches">
        </asp:EntityDataSource>

    </div>
    <script type="text/javascript">
        // function showImgAreaSelect() {
        //     if ($('#fancybox-img').is(':visible')) {
        //         $('#fancybox-img').imgAreaSelect({
        //          aspectRatio: '1:1',
        //            maxWidth: '100',
        //              maxHeight: '100',
        //                handles: true,
        //                  parent: '#fancybox-content'
        //  ,
        //    onSelectEnd: function (img, selection) {
        //         $('%= lvVoucherType.Items[lvVoucherType.SelectedIndex].FindControl("TextBox3").ClientID()%>').val(Math.floor(selection.x1));
        //            $('%= lvVoucherType.Items[lvVoucherType.SelectedIndex].FindControl("TextBox4").ClientID()%>').val(Math.floor(selection.y1));
        //         }
        //      });
        //       $('#fancybox-content').unbind('click');
        //       $('#lightbox-nav').remove();
        //    } else
        //        setTimeout(showImgAreaSelect, 50);
        //};
        function asyncUploadComplete(sender, args) {
            var filename = args.get_fileName();
            var contentType = args.get_contentType();
            var text = "Size of " + filename + " is " + args.get_length() + " bytes";
            if (contentType.length > 0) {
                text += " and content type is '" + contentType + "'.";
            }
            //alert(text);
        }
        $(document).ready(function () {
            $("a.EnlargeImage").fancybox();
            //$("a.EnlargeImage").click(showImgAreaSelect);
            //var canvas = $('#= lvVoucherType.InsertItem.FindControl("insertCanvas").ClientID()%>');
            //var ctx = canvas[0].getContext("2d");
            //ctx.fillStyle = "#000000";
            //ctx.fillRect(0, 0, 50, 50);
            // $('#<= lvVoucherType.InsertItem.FindControl("btnXY").ClientID()%>').hide();
            //$('#< lvVoucherType.InsertItem.FindControl("insertCanvas").ClientID()%>').hide();
            //$('#<= lvVoucherType.InsertItem.FindControl("fuPhoto").ClientID()%>').change(function () {
            //   $('#<= lvVoucherType.InsertItem.FindControl("btnXY").ClientID()%>').click();
            //  alert('fuPhoto ID '+$('#<=lvVoucherType.InsertItem.FindControl("fuPhoto").ClientID%>').attr("id"));
            //   var filePath = $('#<=lvVoucherType.InsertItem.FindControl("fuPhoto").ClientID%>').attr("value");
            //alert('filePath '+filePath);
            // var fileName = filePath.split('\\').pop().split('/').pop();
            //alert('fileName ' + fileName);
            //var tempPath = "~/Data/Voucher/VoucherImage/temp/" + fileName;
            //alert('file changed! ' + tempPath);
            //if (fileName != '') {
            //var uploadPath = Server.MapPath("~/Data/Voucher/VoucherImage/temp/" + CType(lvVoucherType.InsertItem.FindControl("NameTextBox"), TextBox).Text + CType(lvVoucherType.InsertItem.FindControl("fuPhoto"), FileUpload).PostedFile.FileName)
            //$('#< lvVoucherType.InsertItem.FindControl("insertCanvas").ClientID()%>').attr("src", tempPath);
            //  $('#< lvVoucherType.InsertItem.FindControl("btnXY").ClientID()%>').show();
            //    }
            //);
            //$('#< lvVoucherType.InsertItem.FindControl("insertCanvas").ClientID()%>').hide();
            //alert($('#< lvVoucherType.InsertItem.FindControl("insertCanvas").ClientID()%>').attr("id"));
            //$('#< lvVoucherType.InsertItem.FindControl("insertCanvas").ClientID()%>').imgAreaSelect(
            //  {
            //       aspectRatio: '1:1',
            //      handles: true,
            //     onSelectEnd: function (img, selection) {
            //        $('#<= lvVoucherType.InsertItem.FindControl("TextBox8").ClientID()%>').val(selection.x1);
            //       $('#<= lvVoucherType.InsertItem.FindControl("TextBox9").ClientID()%>').val(selection.y1);
            //  }
            //});
        });
    </script>
</asp:Content>

