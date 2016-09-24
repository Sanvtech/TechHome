Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Partial Class NewLogin
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objEmail As New clsEmail
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



        Session("DBName") = ""

        'Session("Userpwd") = ""

        'FormsAuthentication.SignOut()

        'clsDBConnect.webdb = ddlDbName.Items(ddlDbName.SelectedIndex).Value
        'Session.Add("dbconnectionName", ddlDbName.Items(ddlDbName.SelectedIndex).Value)

        If Page.IsPostBack = False Then

            btnLogIn.Attributes.Add("onclick", "return FormValidation('Login')")

            Session.Abandon()

            SetFocus(txtUserName1)

            If Not (Request.Cookies("user") Is Nothing) Then

                txtUserName1.Text = Request.Cookies("user").Item("userId")

            End If

            Dim ds As DataSet
            Dim cmd As SqlCommand
            Dim da As SqlDataAdapter
            ds = New DataSet
            Dim scon As New SqlConnection(ConfigurationManager.ConnectionStrings("strDBConnection").ConnectionString)

            cmd = New SqlCommand("select divisionname, divisioncode from T_Division_Master where active = 1 order by divisionname", scon)
            da = New SqlDataAdapter(cmd)
            da.Fill(ds)
            cmd = Nothing

            ddlDivName.DataSource = ds
            ddlDivName.DataTextField = "divisionname"
            ddlDivName.DataValueField = "divisioncode"
            ddlDivName.DataBind()
            ddlDivName.SelectedIndex = 2    'For setting default value as Dubai

            ds = Nothing
            da = Nothing
            scon.Close()

            If Page.IsPostBack = False Then
                Session.Add("dbConnectionName", "strDBConnection")
                SetFocus(txtUserName1)
                If Not (Request.Cookies("user") Is Nothing) Then
                    txtUserName1.Text = Request.Cookies("user").Item("userId")
                End If
                clsDBConnect.webdb = Session("dbConnectionName") '"strDBConnection"
            End If

        End If
    End Sub
#End Region

#Region "Protected Sub btnLogIn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogIn.Click"
    Protected Sub btnLogIn_Click(sender As Object, e As EventArgs) Handles btnLogIn.Click
        Try

            Session.Add("dbconnectionName", "strDBConnection")
            Session.Add("dbconnectionName1", "strDBConnection1")

            If objUser.ValidateUser(Session("dbconnectionName"), txtUserName1.Text.Trim, txtPassword.Text.Trim) = True Then

                Session.Add("GlobalUserName", txtUserName1.Text.Trim)
                Session.Add("Userpwd", txtPassword.Text.Trim)
                'Session.Add("CompanyName", CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select top 1 companyName from T_CompanyMaster"), String))
                Session.Add("CompanyName", "Maward")
                Session.Add("changeyear", Now.Year.ToString)
                Session.Add("fdate", CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select top 1 fdate from T_CompanyMaster"), String))
                Session.Add("Division", ddlDivName.Items(ddlDivName.SelectedIndex).Value)
                Session.Add("LoggedDateTime", System.DateTime.Now.ToString("dd/MM/yyyy  :  hh:mm:ss"))
                Session.Add("LoggedDate", System.DateTime.Now.ToString("dd/MM/yyyy"))
                Session.Add("LoggedTime", System.DateTime.Now.ToString("hh:mm tt"))

                'To get group id for Group Rights - Functional Rights
                Session.Add("UserGroup", CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), " select groupid from T_UserMaster where usercode=N'" + txtUserName1.Text.Trim + "' and active=1"), String))

                'If chkRemember.Checked Then
                '    addcookie()
                'End If
                FormsAuthentication.SetAuthCookie(txtUserName1.Text.Trim, False)
                Response.Redirect("Home.aspx", False)
            Else
                txtUserName1.Text = ""
                txtPassword.Text = ""
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "Warning", "WarningMsg('','Please enter valid user name and password');", True)
                SetFocus(txtUserName1)
                'Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")
                'objUtils.MessageBox(ClientIP, Me.Page)
            End If

            ''Starts the changes for the crystal report according to the database selection
            If txtUserName1.Text.Trim <> "" And txtPassword.Text.Trim <> "" Then
                Dim ds1 As DataSet
                Dim cmd1 As SqlCommand
                Dim dbSqlReader As SqlDataReader
                Dim scon1 As New SqlConnection(ConfigurationManager.ConnectionStrings("strDBConnection3").ConnectionString)

                If scon1.State = Data.ConnectionState.Closed Then
                    scon1.Open()
                End If

                cmd1 = New SqlCommand("select username,password,servername,databasename from crystal_connection where conn_str='" & clsDBConnect.webdb & "'", scon1)
                dbSqlReader = cmd1.ExecuteReader(CommandBehavior.CloseConnection)
                cmd1 = Nothing

                If dbSqlReader.HasRows Then
                    If dbSqlReader.Read() = True Then

                        If IsDBNull(dbSqlReader("username")) = False Then
                            Session.Add("dbUserName", CType(dbSqlReader("username"), String))
                        End If

                        If IsDBNull(dbSqlReader("password")) = False Then
                            Session.Add("dbPassword", CType(dbSqlReader("password"), String))
                        End If

                        If IsDBNull(dbSqlReader("servername")) = False Then
                            Session.Add("dbServerName", CType(dbSqlReader("servername"), String))
                        End If

                        If IsDBNull(dbSqlReader("databasename")) = False Then
                            Session.Add("dbDatabaseName", CType(dbSqlReader("databasename"), String))
                        End If
                    End If
                End If

                ds1 = Nothing
                scon1.Close()
            End If

            'If txtUserName.Text.Trim = "admin" And txtPassword.Text.Trim = "admin" Then
            '    Session.Add("GlobalUserName", "Admin")
            '    Response.Redirect("ModuleMainPage.aspx", False)
            'Else
            '    txtUserName.Text = ""
            '    txtPassword.Text = ""
            '    objUtils.MessageBox("Please enter valid user name and password.", Me.Page)
            '    SetFocus(txtUserName)
            'End If


            '''''''''
            'Initialize Cookie value to english
            Dim cookie As HttpCookie
            cookie = New HttpCookie("CultureInfo")
            cookie.Value = "en-us"
            Response.Cookies.Add(cookie)
            ''''''''''''''

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("NewLogin.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region

    Protected Sub lnkChgPwd_Click(sender As Object, e As EventArgs) Handles lnkChgPwd.Click

        Dim mysqlreader As SqlDataReader

        Session("dbconnectionName") = "strDBConnection"
        Try
            If txtUserName1.Text <> "" Then
                mysqlreader = objUtils.GetDataFromReadernew(Session("dbconnectionName"), "Select usercode from T_UserMaster where active=1 and  usercode=N'" & txtUserName1.Text & "'")

                If Not mysqlreader.HasRows Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Warning", "WarningMsg('','Please enter a valid username.');", True)
                    '        '   
                    txtUserName1.Text = ""
                    Exit Sub
                End If
                'redirect to pwd change page
                Dim strpop As String = ""
                strpop = "window.open('ChangeUserPassword.aspx?UserCode=" & txtUserName1.Text & "','ChangePassword',' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                ClientScript.RegisterStartupScript(Me.GetType(), "script", strpop, True)
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Warning", "WarningMsg('','Please enter Username.');", True)
                SetFocus(txtUserName1.ClientID)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("NewLogin.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs)

        'Dim mysqlreader As SqlDataReader
        'Dim fromMail As String = ""
        'Dim toMail As String = ""
        'Dim strcust As String = ""
        'Dim subject As String = ""

        ''clsDBConnect.webdb = "strDBConnection"

        'Session("dbconnectionName") = "strDBConnection"

        'Try
        '    If txtFUSerName.Text <> "" Then
        '        'mysqlreader = objUtils.GetDataFromReadernew(Session("dbconnectionName"), "Select usercode,username,dbo.pwddecript(userpwd) userpwd ,usemail from usermaster where active=1 and  usercode='" & txtFUserName.Text & "'")
        '        mysqlreader = objUtils.GetDataFromReadernew(Session("dbconnectionName"), "Select usercode,username, userpwd ,usemail from usermaster where active=1 and  usercode='" & txtFUSerName.Text & "'")

        '        If Not mysqlreader.HasRows Then

        '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter a valid username.');", True)
        '            '        '   
        '            txtFUSerName.Text = ""

        '            Exit Sub
        '        End If

        '        mysqlreader.Read()

        '        If mysqlreader.Item("usemail") = "" Then
        '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Email Address unavailable.');", True)
        '            Exit Sub
        '        End If

        '        fromMail = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id =1070")
        '        'objEmail.SendEmail(txtFrom.Value.Trim, txtTo.Value.Trim, txtSubject.Value.Trim, txtbody.Text.Trim)
        '        toMail = mysqlreader.Item("usemail")

        '        strcust = "<table style='font-family: Verdana'><tr><td > Dear " & mysqlreader.Item("username") & ", <br/><br/> Please  note  your following  T3-Group account Info: "
        '        strcust += "<br/><br/>Username:" & mysqlreader.Item("usercode") & "<br/> "
        '        strcust += "Password:" & mysqlreader.Item("userpwd") & "<br/><br/>"
        '        strcust += " <br /><br />Thanks and Best Regards<br /><br />T3 group </td></tr></table>"
        '        subject = "Password Recovery"
        '        If fromMail <> "" And toMail <> "" Then
        '            If objEmail.SendEmailCC(fromMail, toMail, "", subject, strcust) Then
        '                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Email has been successfully sent');", True)
        '                txtFUSerName.Text = ""
        '            End If
        '        End If
        '    Else
        '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter the username');", True)
        '    End If
        'Catch ex As Exception
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        'End Try

        Dim mysqlreader As SqlDataReader
        Dim fromMail As String = ""
        Dim toMail As String = ""
        Dim strcust As String = ""
        Dim subject As String = ""

        'clsDBConnect.webdb = "strDBConnection"
        Session("dbconnectionName") = "strDBConnection"

        Try

            If txtFUSerName.Text <> "" Then

                mysqlreader = objUtils.GetDataFromReadernew(Session("dbconnectionName"), "Select usercode, username, dbo.pwddecript(userpwd) userpwd, usemail from T_UserMaster where active=1 and usercode=N'" & txtFUSerName.Text & "'")
                'mysqlreader = objUtils.GetDataFromReadernew(Session("dbconnectionName"), "Select usercode, username, userpwd, usemail from T_UserMaster where active=1 and  usercode='" & txtFUserName.Text & "'")

                If Not mysqlreader.HasRows Then
                    ModalPopupExtender1.Show()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Error", "ErrorMsg('User not found','Please enter a valid username!!!');", True)
                    txtFUSerName.Text = ""
                    Exit Sub
                End If

                mysqlreader.Read()

                If mysqlreader.Item("usemail") = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Error", "ErrorMsg('mail not sent','Email Address unavailable!!!');", True)
                    Exit Sub
                End If

                fromMail = "itzdubai@gmail.com"

                toMail = mysqlreader.Item("usemail")

                strcust = "<table style='font-family: Verdana'><tr><td > Dear " & mysqlreader.Item("username") & ", <br/><br/> Please  note  your following Maward account Info: "
                strcust += "<br/><br/>Username:" & mysqlreader.Item("usercode") & "<br/> "
                strcust += "Password:" & mysqlreader.Item("userpwd") & "<br/><br/>"
                strcust += " <br /><br />Thanks and Best Regards<br /><br /></td></tr></table>"

                subject = "Password Recovery"

                SendEmail(fromMail, toMail, subject, strcust)

                If fromMail <> "" And toMail <> "" Then
                    If SendEmailCC(fromMail, toMail, "", subject, strcust) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Warning", "WarningMsg('','Login credentials has been sent to the registered Email successfully!!!');", True)
                        txtFUSerName.Text = ""
                    End If
                End If

            Else
                ModalPopupExtender1.Show()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Warning", "WarningMsg('','UserName canot be empty!!!');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try


    End Sub
    Public Function SendEmail(ByVal strFrom As String, ByVal strTo As String, ByVal strSubject As String, ByVal strMsg As String) As Boolean

        Dim Mail_Message As New MailMessage
        Dim FromAddress As New MailAddress(strFrom)
        Dim msClient As New SmtpClient

        Try

            'Set From Email id
            Mail_Message.From = FromAddress
            'Set To Email id
            Mail_Message.To.Add(strTo)
            Mail_Message.Subject = strSubject

            Mail_Message.Body = strMsg

            'Mail_Message.Priority = MailPriority.Normal
            Mail_Message.IsBodyHtml = True

            msClient.Port = 25
            msClient.Host = "smtp.gmail.com"
            msClient.EnableSsl = True
            msClient.Credentials = New System.Net.NetworkCredential(strFrom, "itz@1234")

            'comment by csn since no firewall need to uncomment on instalation
            msClient.Send(Mail_Message)
            SendEmail = True
        Catch ex As Exception
            SendEmail = False
        End Try
    End Function

    Public Function SendEmailCC(ByVal strFrom As String, ByVal strTo As String, ByVal strToCC As String, ByVal strSubject As String, ByVal strMsg As String) As Boolean

        Dim Mail_Message As New MailMessage
        Dim FromAddress As New MailAddress(strFrom)
        Dim msClient As New SmtpClient
        Dim strarr() As String
        Dim strarrCC() As String

        Dim i, j As Integer

        Try
            strarr = strTo.Split(",")
            strarrCC = strToCC.Split(",")


            'Set From Email id
            Mail_Message.From = FromAddress
            'Set To Email id
            j = 0
            For i = 1 To strarr.Length
                Mail_Message.To.Add(strarr(j))
                j = j + 1
            Next

            If strToCC <> "" Then
                j = 0
                For i = 1 To strarrCC.Length
                    Mail_Message.CC.Add(strarrCC(j))
                    j = j + 1
                Next
            End If

            'Set Subject
            'Dim attachFile As New Attachment(txtAttachmentPath.Text)
            'MyMessage.Attachments.Add(attachFile)

            Mail_Message.Subject = strSubject
            'Set Msg Body
            Mail_Message.Body = strMsg

            'Mail_Message.Priority = MailPriority.Normal
            Mail_Message.IsBodyHtml = True
            msClient.Port = 25

            '            msClient.Host = "127.0.0.1"
            msClient.Host = "smtp.gmail.com"
            msClient.Credentials = New System.Net.NetworkCredential("itzdubai@gmail.com", "itz@1234") '

            'comment by csn since no firewall need to uncomment on instalation
            'msClient.Send(Mail_Message)
            SendEmailCC = True
        Catch ex As Exception
            SendEmailCC = False
        End Try

    End Function

    'Protected Sub LnkFgtPwd_Click(sender As Object, e As EventArgs) Handles LnkFgtPwd.Click

    '    If divSubmit.Visible = True Then
    '        divSubmit.Visible = False
    '    Else
    '        divSubmit.Visible = True
    '    End If

    'End Sub

End Class
