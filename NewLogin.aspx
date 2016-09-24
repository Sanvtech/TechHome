<%@ Page Language="VB" AutoEventWireup="false" CodeFile="NewLogin.aspx.vb" Inherits="NewLogin" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>:: Maward - Login :: </title>
    <link href="css/login-maward.css" rel="stylesheet" type="text/css">
    <link href="Css/ModalPopup.css" rel="stylesheet" />
    <link href="Css/ControlStyles.css" rel="stylesheet" />

    <!--Alert Box-->
    <script src="Css/SweetAlert/sweetalert-dev.js"></script>
    <link href="Css/SweetAlert/sweetalert.css" rel="stylesheet" />

    <!--Jquery here-->
    <script src="Css/Jqueryjs/JavascriptFunctions.js"></script>
    <script src="Css/Jqueryjs/jquery-ui.js"></script>
    <script src="Css/Jqueryjs/jquery-ui.min.js"></script>
    <script src="Css/Jqueryjs/jquery.js"></script>
    <link href="Css/jquery-ui.css" rel="stylesheet" />

    <link href='http://fonts.googleapis.com/css?family=Oswald' rel='stylesheet' type='text/css' />
    <style type="text/css">
        a:link {
            color: #069;
        }

        a:visited {
            color: #069;
        }

        a:hover {
            color: #06C;
        }

        a:active {
            color: #069;
        }
    </style>
    <script lang="ja" type="text/javascript">

        window.moveTo(0, 0)
        window.resizeTo(screen.availWidth, screen.availHeight)

        function FormValidation(state) {
            if 
                (document.getElementById("<%=ddlDivName.ClientID%>").value == "[Select]") {
                document.getElementById("<%=ddlDivName.ClientID%>").focus();
                alert("Please select branch!!!");
                return false;
            }
            else
                return true;
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div id="login-con">
        <div id="logo-con">
            <img src="images/logo1.png" alt="" width="585" height="398" />
        </div>
        <div id="login-hd">Log In</div>

                <div id="form-con">
            User Code :
            <asp:TextBox ID="txtUserName1" runat="server" CssClass="logintxtbox" TabIndex="1" MaxLength="20" AccessKey="a"></asp:TextBox>
            Password :
            <asp:TextBox ID="txtPassword" runat="server" CssClass="logintxtbox" TextMode="Password" TabIndex="2" MaxLength="20" AccessKey="z"></asp:TextBox>
        </div>

               
                <div id="form-con-check">
                    <br />
                    <asp:CheckBox ID="chkRemember" runat="server" Text="Remember me next time." Width="183px" CssClass="td_cell" TabIndex="3" Visible="false"/>
                </div>
                <div id="form-con-select">
                    Branch:<br />
                                    <asp:DropDownList ID="ddlDivName" class="logindrpdown" runat="server" TabIndex="4" AutoPostBack="true"></asp:DropDownList>

                </div>
                <div id="form-con-submit">

                    <asp:Button ID="btnLogIn" runat="server" Text="Log In" TabIndex="5" />&nbsp;
                </div>

                <div id="forgt-chang-psswrd">
                    <asp:LinkButton ID="LnkFgtPwd" runat="server" style="float: left;" >Forgot Password?</asp:LinkButton>

                    &nbsp;&nbsp;
    
                <asp:LinkButton ID="lnkChgPwd" runat="server" style="float: right;">Change Password?</asp:LinkButton>
                </div>

                <%--<div style="float: left;" id="divSubmit" runat="server" visible="false">
                    <div id="left-log-name-con">
                        User Name :<br />
                    </div>

                    <div class="right-text-fld" style="padding-top: 5px">
                        &nbsp; &nbsp;<asp:Button ID="" OnClick="" runat="server" Text="Submit" CssClass="btn" BorderStyle="Solid" BorderWidth="1px" />
                    </div>

                </div>--%>
            
        <%--FOrgot Password--%>
        <asp:Panel ID="pnl_forgotpwd" runat="server" CssClass="modalPopup" Style="display: none;font-family : 'Roboto', sans-serif; font-size:13px;" align="center">
            <div class="header">
                Forgot Password<p class="closebutton" id="ImgClose">X</p>
            </div>
            <br />
            <div id="popup-body">
                <div id="sml-hd">
                    Enter your User Code to receive your password by Mail
                </div>
                <br /><br />
                <div id="Div1" style="font-family : 'Roboto', sans-serif; font-size:13px;">
                    <span>User Code:<abbr style="color: red">*</abbr></span>
                    <asp:TextBox ID="txtFUSerName" runat="server" CssClass="logintxtbox" MaxLength="20"></asp:TextBox><br /><br />
                    <asp:Label ID="lbl_msg" runat="server" ForeColor="Red" Text=""></asp:Label>
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn"  OnClick="btnSubmit_Click" />
                </div>
                <br />
            </div>
        </asp:Panel>
        <ajx:modalpopupextender id="ModalPopupExtender1" runat="server" popupcontrolid="pnl_forgotpwd"
            targetcontrolid="LnkFgtPwd" backgroundcssclass="modalBackground" CancelControlID ="ImgClose">
        </ajx:modalpopupextender>
            </div>
    </form>
</body>
</html>
