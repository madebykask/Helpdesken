Imports System.Configuration
Imports System.Data.SqlClient
Imports Microsoft.Win32
Imports System.Net
Imports System.IO
Imports System.Net.Mail
Imports System.Text.RegularExpressions

<Serializable()> Public Class SharedFunctions
    Public Enum ConnectionType As Integer
        DB = 0
        HTTP = 1
    End Enum

    Public Enum ComputerType As Integer
        PC = 0
        MAC = 1
    End Enum

    Public Enum ComputerRole As Integer
        PC = 0
        SERVER = 1
    End Enum

    Public Enum ObjectType As Integer
        OS = 1
        Processor = 2
        RAM = 3
        ComputerModel = 4
        NIC = 5
    End Enum

    Public Enum SyncType As Integer
        SyncByCustomer = 1
        SyncByCustomerHTTP = 2
        SyncByDomain = 3
        SyncByDomainHTTP = 4
        SyncByWorkingGroup = 5
    End Enum

    Public Enum EMailType As Integer
        EMailNewCase = 1
        EMailAssignCasePerformer = 2
        EMailCaseClosed = 3
        EMailInformNotifier = 4
        EMailInternalLogNote = 5
        EMailQuestionnaire = 6
        EMailAssignCaseWorkingGroup = 7
        EMailOrder = 8
        EMailWatchDate = 9
        EMailNotifierCaseUpdate = 10
        EMailAssignCasePerformerSMS = 11
        EMailPlanDate = 12
        EMailReminderNotifier = 17
    End Enum

    Public gfDEBUG As Boolean
    Public gsInvoiceFileFolder As String
    Public giCustomer_Id As Int32

    Public Sub Init()
        Dim sTemp As String
        Dim keyValue As String
        Dim regDH_Helpdesk As RegistryKey

        Dim iConnectionType As Int32
        Dim sServer As String
        Dim sDatabase As String
        Dim sDBUId As String
        Dim sPWD As String

        ' Läs regsitret
        Try
            keyValue = "Software\\Datahalland\\DH_Helpdesk"
            regDH_Helpdesk = Registry.LocalMachine.OpenSubKey(keyValue, True)

            If regDH_Helpdesk Is Nothing Then
                Exit Sub
            End If

            giCustomer_Id = regDH_Helpdesk.GetValue("Customer_Id")

            sTemp = regDH_Helpdesk.GetValue("DEBUG")

            If sTemp = "" Then
                gfDEBUG = False
            Else
                gfDEBUG = sTemp
            End If

            sTemp = regDH_Helpdesk.GetValue("ConnectionType")

            If sTemp = "" Then
                iConnectionType = 0
            Else
                iConnectionType = sTemp
            End If

            sServer = regDH_Helpdesk.GetValue("Server")
            sDatabase = regDH_Helpdesk.GetValue("Database")
            sDBUId = regDH_Helpdesk.GetValue("DBUID")
            sPWD = regDH_Helpdesk.GetValue("PWD")

            gsConnectionString = "Data Source=" & sServer & "; Initial Catalog=" & sDatabase & "; User Id=" & sDBUId & "; Password=" & sPWD & ";Network Library=dbmssocn"

            gsInvoiceFileFolder = regDH_Helpdesk.GetValue("InvoiceFileFolder")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Function Call4DateFormat(ByVal sFieldName As String, ByVal iDBType As Integer) As String
        If iDBType = 0 Then
            Return " dbo.FormatISODate(" & sFieldName & ") "
        Else
            Return " FormatISODate(" & sFieldName & ") "
        End If
    End Function

    Public Shared Function convertDateTime(ByVal sDate As DateTime, ByVal iDBType As Int32) As String
        Dim sTemp As String
        Dim sNewDate As String

        If IsDate(sDate) Then
            sNewDate = DatePart(DateInterval.Year, sDate) & "-"

            sTemp = DatePart(DateInterval.Month, sDate)
            sTemp = sTemp.PadLeft(2, "0")
            sNewDate = sNewDate & sTemp & "-"

            sTemp = DatePart(DateInterval.Day, sDate)
            sTemp = sTemp.PadLeft(2, "0")
            sNewDate = sNewDate & sTemp

            If iDBType = 1 Then
                Return "to_date('" & sNewDate & "','YYYY-MM-DD')"
            Else
                Return "'" & sNewDate & "'"
            End If
        Else
            Return "NULL"
        End If
    End Function

    Public Shared Function getDataTable(sConnectionstring As String, sSql As String, ParamArray params As SqlParameter()) As DataTable
        Return DbHelper.getDataTable(sConnectionstring, sSql, params)
    End Function

    Public Shared Sub executeSQL(sConnectionstring As String, sSql As String)
        DbHelper.executeNonQuery(sConnectionString, sSql, CommandType.Text)
    End Sub

    Public Shared Function getMailTemplateIdentifier(ByVal FieldName As String)
        Select Case UCase(FieldName)
            Case "CASENUMBER"
                Return "[#1]"
            Case "CUSTOMERNAME"
                Return "[#2]"
            Case "PERSONS_NAME"
                Return "[#3]"
            Case "CAPTION"
                Return "[#4]"
            Case "DESCRIPTION"
                Return "[#5]"
            Case "FIRSTNAME"
                Return "[#6]"
            Case "SURNAME"
                Return "[#7]"
            Case "PERSONS_EMAIL"
                Return "[#8]"
            Case "PERSONS_PHONE"
                Return "[#9]"
            Case "TEXT_EXTERNAL"
                Return "[#10]"
            Case "TEXT_INTERNAL"
                Return "[#11]"
            Case "PRIORITYNAME"
                Return "[#12]"
            Case "WORKINGGROUPEMAIL"
                Return "[#13]"
            Case "WORKINGGROUP"
                Return "[#15]"
            Case "REGTIME"
                Return "[#16]"
            Case "INVENTORYNUMBER"
                Return "[#17]"
            Case "PERSONS_CELLPHONE"
                Return "[#18]"
            Case "AVAILABLE"
                Return "[#19]"
            Case "PRIORITY_DESCRIPTION"
                Return "[#20]"
            Case "WATCHDATE"
                Return "[#21]"
            Case "LASTCHANGEDBYUSER"
                Return "[#22]"
            Case "MISCELLANEOUS"
                Return "[#23]"
            Case "PLACE"
                Return "[#24]"
            Case "CASETYPE"
                Return "[#25]"
            Case "CATEGORY"
                Return "[#26]"
            Case "REPORTEDBY"
                Return "[#27]"
            Case "PRODUCTAREA"
                Return "[#28]"
            Case "REGUSER"
                Return "[#29]"
            Case "PERFORMER_PHONE"
                Return "[#70]"
            Case "PERFORMER_CELLPHONE"
                Return "[#71]"
            Case "PERFORMER_EMAIL"
                Return "[#72]"
            Case "AUTOCLOSEDAYS"
                Return "[#80]"
            Case Else
                Return ""
        End Select
    End Function

    #Region "Database Helper Methods"

   

    #End Region

    Public Shared Sub executeSQLHTTP(ByVal sSQL As String)
        Dim sURL As String

        sSQL = SharedFunctions.URLEncode(sSQL)

        sURL = gsURL & "/setInfo.asp?SQL=" & sSQL

        Try
            Dim request As WebRequest = WebRequest.Create(sURL)

            Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)

            Dim objStream As New StreamReader(response.GetResponseStream())

            Dim sXML As String = objStream.ReadToEnd()

        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR executeSQLHTTP " & sSQL)
            End If

            Throw ex
        End Try
    End Sub

    Public Shared Function parseEMailAddress(ByVal sEmail As String) As String
        Dim iPos As Int32 = 0

        Try
            ' Leta upp "<"
            iPos = InStr(sEmail, "<", CompareMethod.Text)

            If iPos > 0 Then
                sEmail = Mid(sEmail, iPos + 1)
            End If

            ' Leta upp ">"
            iPos = InStr(sEmail, ">", CompareMethod.Text)

            If iPos > 0 Then
                sEmail = Left(sEmail, iPos - 1)
            End If
        Catch ex As Exception
            Throw ex
        End Try

        Return sEmail
    End Function

    Public Shared Function removeNonNumericValues(ByVal expression As String) As String
        Dim sTemp As String = ""
        Dim sChar As String

        If expression <> "" Then
            For i As Integer = 1 To Len(expression)
                sChar = Mid(expression, i, 1)
                If Asc(sChar) > 47 And Asc(sChar) < 58 Then
                    sTemp = sTemp & sChar
                End If
            Next
        End If

        Return sTemp
    End Function

    Public Shared Function URLEncode(ByVal sURL As String) As String
        ' &
        sURL = Replace(sURL, "&", "%26")

        ' å, Å
        sURL = Replace(sURL, "å", "%e5")
        sURL = Replace(sURL, "Å", "%c5")

        ' ä, Ä
        sURL = Replace(sURL, "ä", "%e4")
        sURL = Replace(sURL, "Ä", "%c4")

        ' ö, Ö
        sURL = Replace(sURL, "ö", "%f6")
        sURL = Replace(sURL, "Ö", "%d6")

        ' é
        sURL = Replace(sURL, "é", "%e9")

        ' ü
        sURL = Replace(sURL, "ü", "%fc")

        Return sURL
    End Function


    Public Shared Function URLDecode(ByVal sURL As String) As String
        ' Koda av %
        sURL = sURL.Replace("%25", "%")

        For i As Integer = 128 To 255
            sURL = sURL.Replace("%" & Hex(i), Chr(i))

        Next

        Return sURL
    End Function

    Public Shared Function getMinuteSign() As String
        If giDBType = 0 Then
            Return "mi"
        Else
            Return "mi"
        End If
    End Function

    Public Shared Function extractAnswerFromBody(ByVal sBodyText As String, ByVal sEMailAnswerSeparator As String) As String
        
        Dim aEMailAnswerSeparator() As String
        Dim iPos As Integer = 0
        Dim iPos_new As Integer = 0

        aEMailAnswerSeparator = Split(sEMailAnswerSeparator, ";")

        For i As Integer = 0 To aEMailAnswerSeparator.Length - 1
            Dim emailSeparatorPattern = aEMailAnswerSeparator(i)

            Dim match As Match = Regex.Match(sBodyText, emailSeparatorPattern, RegexOptions.IgnoreCase Or RegexOptions.Multiline)

            If match.Success 

                iPos = match.Index
            
                ' Compare with prev results which match is the closest one to the beginning
                If iPos > 0 Then
                    If iPos_new = 0 Then
                        iPos_new = iPos
                    Else
                        If iPos < iPos_new Then
                            iPos_new = iPos
                        End If
                    End If

                End If
            End If
            
        Next

        If iPos_new = 0 Then
            Return sBodyText
        Else
            Return Left(sBodyText, iPos_new)
        End If
    End Function
    

    Public Shared Function extractCaseNumberFromSubject(ByVal sSubject As String, ByVal sEMailSubjectPattern As String) As Integer
        Dim iPosCaseNumber As Integer
        Dim iPos As Integer
        Dim sTemp As String = ""

        Try
            Dim vEMailSubjectPattern() As String = sEMailSubjectPattern.Split(";")

            For Index As Integer = 0 To vEMailSubjectPattern.Length - 1
                sTemp = ""
                ' Leta upp var #-tecknet finns
                iPosCaseNumber = InStr(vEMailSubjectPattern(Index), "#", CompareMethod.Text)

                ' Om 1 då finns ärendenumret innan sökmönstret annars efter
                If iPosCaseNumber = 1 Then
                    ' Ta bort # från sökmönstret
                    vEMailSubjectPattern(Index) = Mid(vEMailSubjectPattern(Index), 2)

                    ' Leta upp var möstret börjar
                    iPos = InStr(sSubject, vEMailSubjectPattern(Index), CompareMethod.Text)

                    ' Starta på iPos och stega till vänster tills det inte är ett numeriskt tecken
                    For i As Integer = iPos - 1 To 1 Step -1
                        If IsNumeric(Mid(sSubject, i, 1)) Then
                            sTemp = Mid(sSubject, i, 1) & sTemp
                        Else
                            Exit For
                        End If
                    Next
                ElseIf iPosCaseNumber > 1 Then
                    Dim FirstNumericCharFound As Boolean = False

                    ' Ta bort # från sökmönstret
                    vEMailSubjectPattern(Index) = Left(vEMailSubjectPattern(Index), iPosCaseNumber - 1)

                    ' Leta upp var möstret börjar
                    'iPos = InStr(sSubject, vEMailSubjectPattern(Index), CompareMethod.Text)
                    iPos = InStr(sSubject, vEMailSubjectPattern(Index), CompareMethod.Text)

                    If iPos > 0 Then
                        iPos = iPos + Len(vEMailSubjectPattern(Index))
                        Dim iStart As Integer = iPos

                        ' Starta på iPos och stega till höger tills det inte är ett numeriskt tecken
                        For i As Integer = iPos To Len(sSubject)
                            If iPos = iStart And Mid(sSubject, i, 1) = " " Then
                                Exit For
                            End If
                            If IsNumeric(Mid(sSubject, i, 1)) Then
                                FirstNumericCharFound = True
                                sTemp = sTemp & Mid(sSubject, i, 1)
                            Else
                                If FirstNumericCharFound = True Then
                                    Exit For
                                Else
                                    If iStart = iPos And Mid(sSubject, i, 1) <> " " And Mid(sSubject, i, 1) <> "" Then
                                        Exit For
                                    End If
                                End If

                            End If
                        Next
                    End If

                End If

                If sTemp <> "" Then
                    Return CType(sTemp, Integer)
                End If
            Next

            If sTemp = "" Then
                Return 0
            End If
        Catch ex As Exception
            Return 0
        End Try

    End Function

    Public Shared Function isLastWeekDay() As Boolean
        Dim iDaysPerMonth As Integer

        iDaysPerMonth = System.DateTime.DaysInMonth(Today.Year, Today.Month)

        If iDaysPerMonth - Today.Day < 7 Then
            isLastWeekDay = True
        Else
            isLastWeekDay = False
        End If
    End Function

    Public Shared Function getWeekDayOrder() As Integer
        Dim iDay As Integer = Today.Day

        If iDay >= 1 And iDay <= 7 Then
            Return 1
        ElseIf iDay >= 8 And iDay <= 14 Then
            Return 2
        ElseIf iDay >= 15 And iDay <= 21 Then
            Return 3
        ElseIf iDay >= 22 And iDay <= 28 Then
            Return 4
        Else
            Return 5
        End If
    End Function

    Public Shared Function getDBStringPrefix() As String
        If giDBType = 0 Then
            getDBStringPrefix = "N"
        Else
            getDBStringPrefix = ""
        End If
    End Function

    Public Shared Function IsValidEmailAddress(ByVal email As String) As Boolean
        Try
            Dim ma As New MailAddress(email)
            Return True
        Catch
            Return False
        End Try
    End Function


    Public Shared Function createMessageId(ByVal sEmail As String) As String
        Dim sTemp0 As String
        Dim sTemp1 As String
        Dim sTemp2 As String

        sTemp0 = "<" & DateTime.UtcNow.Year
        sTemp0 = sTemp0 & DateTime.UtcNow.Month.ToString.PadLeft(2, "0")
        sTemp0 = sTemp0 & DateTime.UtcNow.Day.ToString.PadLeft(2, "0")
        sTemp0 = sTemp0 & DateTime.UtcNow.Hour.ToString.PadLeft(2, "0")
        sTemp0 = sTemp0 & DateTime.UtcNow.Minute.ToString.PadLeft(2, "0")
        sTemp0 = sTemp0 & DateTime.UtcNow.Second.ToString.PadLeft(2, "0")

        sTemp1 = Guid.NewGuid().ToString().Substring(0, 8)
        sTemp2 = Replace(sEmail, "@", ".") & ">"

        createMessageId = sTemp0 & sTemp1 & "@" & sTemp2
    End Function

    Public Shared Function GetAppSettingValue(key As String) as String
        Dim val as String = ConfigurationManager.AppSettings(key)
        Return If (IsNullOrEmpty(val), "", val)
    End Function

    Public Shared Function IsNullOrEmpty(val as String, Optional checkForWhitespace As Boolean = True)
        'Extra check for VB.NET
        If val Is Nothing Then 
            Return true
        End If
        
        If (checkForWhitespace) Then
            Return String.IsNullOrWhiteSpace(val)
         Else 
             Return String.IsNullOrEmpty(val)
        End If
    End Function

    Public Shared Function IfNullTheDefault(Of TValue As Class)(value As TValue, defaultValue As TValue) As Object
        If value Is Nothing Then
            Return defaultValue
        Else
            Return value
        End If
    End Function

    Public Shared Function ReplaceSingleApostrophe(ByVal self As String) As String
        If String.IsNullOrEmpty(self) OrElse String.IsNullOrWhiteSpace(self) Then
            Return String.Empty
        End If

        Return Regex.Replace(self, "(?<!')'(?!')", "''")
    End Function


End Class