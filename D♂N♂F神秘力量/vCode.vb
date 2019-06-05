Module vCode
    Public vData() As My_Data_Type
    Public CanIFEO As Boolean
    Public Data_Version As Double = 2.4
    Public Update_URL As String = "http://vocyt-dnf-limit.oss-cn-qingdao.aliyuncs.com/application-update"
    Public Update_Page As String = "http://bbs.colg.cn/thread-7393386-1-1.html"
    'Public VoCytDefenderEx_Path As String
    Public Public_ArrayList As ArrayList
    'Public vCurrend_File As String
    Public Hide_Run As Boolean = False
    Public Auto_Kill_Gameloader_Flag As Integer
    Public Process_dnf As Process
    Public Path_Info As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\" + Application.ProductName
    Public INI_Path = Path_Info + ("\list.ini").Replace("\\", "\")
    Public VCD_Path = (Path_Info + "\VoCytDefenderEx.exe").Replace("\\", "\")
    Public Addon_List_String() As String = {"TP3Helper.exe", _
                                            "TPHelper.exe", _
                                            "TPWeb.exe", _
                                            "tgp_gamead.exe", _
                                            "AdvertDialog.exe", _
                                            "AdvertTips.exe", _
                                            "BackgroundDownloader.exe", _
                                            "TenSafe.exe", _
                                            "TPSvc.exe", _
                                            "qbclient.exe", _
                                            "DNFADApp.dll", _
                                            "GameDataPlatformClient.dll", _
                                            "res.vfs", _
                                            "DNFTips.dll"}

    Public Structure My_Data_Type
        Dim Name As String
        Dim Value As Integer
        Dim Path As String
        Dim Info As String
        Dim CanRead As Boolean
        Dim CanWrite As Boolean
        Dim CanRun As Boolean
    End Structure
    Public Function Find_DNF_Path() As String
        Try
            Dim vString() As String = System.Text.Encoding.Unicode.GetString(My.Resources.DNF_Path_Info).Split("/")
            Dim vString2() As String
            Dim vDriverInfo() As IO.DriveInfo
            Dim vReg As Microsoft.Win32.RegistryKey
            Dim vString_Tmp As String
            For i = 0 To vString.Length - 1
                If vString(i).Length = 0 Then Continue For
                vString2 = vString(i).Split("|")
                Select Case vString2(0).ToLower.Trim(" ")
                    Case "reg"
                        If vString2.Length <> 4 Then Continue For
                        Select Case vString2(1).ToLower
                            Case "hklm"
                                vReg = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(vString2(2), False)
                            Case "hkcu"
                                vReg = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(vString2(2), False)
                            Case Else
                                Continue For
                        End Select
                        If vReg Is Nothing Then Continue For
                        vString_Tmp = vReg.GetValue(vString2(3), "").ToString
                        If Check_Game_Path(vString_Tmp) Then
                            Return vString_Tmp
                        End If
                    Case "path"
                        If vString2.Length <> 2 Then Continue For
                        vDriverInfo = IO.DriveInfo.GetDrives()
                        For j = 0 To vDriverInfo.Length - 1
                            Select Case vDriverInfo(j).DriveType
                                Case IO.DriveType.CDRom, IO.DriveType.Ram ', IO.DriveType.Removable
                                    Continue For
                                Case Else
                                    If vDriverInfo(j).DriveType = IO.DriveType.Network Then Continue For
                                    If vDriverInfo(j).IsReady = False Then Continue For
                                    vString_Tmp = Replace(Replace(vString2(1), "$(driver)", vDriverInfo(j).Name), "\\", "\")
                                    If Check_Game_Path(vString_Tmp) Then
                                        Return vString_Tmp
                                    End If
                            End Select
                        Next
                    Case Else
                        Continue For
                End Select
            Next
            Return ""
        Catch ex As Exception

        End Try
        Return ""
    End Function

    Public Sub Set_Application_Title()
        Main.Text = "D♂N♂F神秘力量 Ver " + Get_Application_Version() + " "
        If CanIFEO Then Main.Text += "[IFEO]" Else Main.Text += "[文件读写]"
        Main.Text += " Powered by VoCyt" '   Alpha Test for 0.0.2.7
    End Sub
    Public Sub Kill_Process()
        Dim s As String = "下列正在运行的程序可能会影响本软件运行，是否关闭？" + vbCrLf + "----------" + vbCrLf
        Dim vDNF_List() As String = {"CrossProxy", "TPHelper", "TQMCenter", "tgp_gamead", "GameLoader", "DNF"}
        Dim vProcess() As Process = Process.GetProcesses
        Dim vMessage As String = s
        For Each sProcess As Process In vProcess
            For Each sString As String In vDNF_List
                If sString.ToLower = sProcess.ProcessName.ToLower Then
                    vMessage += sProcess.ProcessName + vbCrLf
                End If
            Next
        Next
        If vMessage <> s Then
            If MsgBox(vMessage, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                For Each sProcess As Process In vProcess
                    For Each sString As String In vDNF_List
                        If sString.ToLower = sProcess.ProcessName.ToLower Then
                            Try
                                sProcess.Kill()
                            Catch ex As Exception

                            End Try
                        End If
                    Next
                Next
            End If
        End If
    End Sub
    Friend Function Get_Application_Version() As String
        Try
            Return System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString
        Catch ex As Exception
            Return "0.0.0.0"
        End Try
    End Function
    Private Function Check_Game_Path(ByVal vpath As String) As Boolean
        If IO.Directory.Exists(vpath) = False Then Return False
        Dim detail() As IO.DirectoryInfo = New IO.DirectoryInfo(vpath).GetDirectories
        Dim eax = 0
        For i = 0 To detail.Length - 1
            If detail(i).Name.ToLower = "ImagePacks2".ToLower Then eax += 1
            If detail(i).Name.ToLower = "SoundPacks".ToLower Then eax += 1
        Next
        If eax = 2 Then Return True Else Return False
    End Function
    Public Function Check_File_Single(ByVal vfilepath As String) As String
        Dim IsRead, IsWrite, IsIFEO As Integer
        IsRead = 2
        IsWrite = 3
        IsIFEO = 1
        Dim fs As IO.FileStream
        Try
            fs = IO.File.OpenRead(vfilepath)
            fs.Close()
            IsRead = 1
        Catch ex As Exception

        End Try
        Try
            fs = IO.File.OpenWrite(vfilepath)
            fs.Close()
            IsWrite = 1
        Catch ex As Exception

        End Try
        Try
            Dim MyReg As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options")
            Dim tmp As String
            For Each tmp In MyReg.GetSubKeyNames
                If tmp.ToLower = IO.Path.GetFileName(vfilepath).ToLower Then
                    MyReg = MyReg.OpenSubKey(tmp)
                    If MyReg.GetValue("debugger", "").ToString = "" Then
                        Exit For
                    Else
                        IsIFEO = 5
                        Exit For
                    End If
                Else
                    Continue For
                End If
            Next
        Catch ex As Exception

        End Try

        Return IsRead * IsWrite * IsIFEO
    End Function
    Public Function ctxt(ByVal inBoolean As Boolean) As String
        If inBoolean Then Return "正常" Else Return "禁用"
    End Function
    Public Sub PAppend(ByVal InString As String)
        vList.Log.AppendText(InString + vbCrLf)
    End Sub
    Public Sub PClear()
        vList.Log.Text = ""
    End Sub
    Public Sub PLine()
        vList.Log.AppendText("--------------------")
    End Sub
    Public Sub PPrint(ByVal InString As String)
        vList.Log.AppendText(InString)
    End Sub
    Public Function Set_File_Security(ByVal vfilefullpath As String, ByVal vEnabled As Boolean, ByRef vEx_Catch As Exception) As Boolean
        Try
            Dim MyObjectSecurity
            Dim MyObjectInfo
            'Dim rule_deny
            'Dim rule_allow
            If IO.File.Exists(vfilefullpath) Then
                MyObjectInfo = New IO.FileInfo(vfilefullpath.Replace("\\", "\"))
                'rule_deny = New Security.AccessControl.FileSystemAccessRule("Everyone", Security.AccessControl.FileSystemRights.FullControl, Security.AccessControl.AccessControlType.Deny)
                'rule_allow = New Security.AccessControl.FileSystemAccessRule("Everyone", Security.AccessControl.FileSystemRights.FullControl, Security.AccessControl.AccessControlType.Allow)
            ElseIf IO.Directory.Exists(vfilefullpath) Then
                MyObjectInfo = New IO.DirectoryInfo(vfilefullpath.Replace("\\", "\"))
                'rule_deny = New Security.AccessControl.FileSystemAccessRule("Everyone", Security.AccessControl.FileSystemRights.FullControl, Security.AccessControl.AccessControlType.Deny)
                'rule_allow = New Security.AccessControl.FileSystemAccessRule("Everyone", Security.AccessControl.FileSystemRights.FullControl, Security.AccessControl.AccessControlType.Allow)
            Else
                Throw New Exception("File not found.")
            End If
            'Dim vfilepath As String = vfilefullpath.Replace("\\", "\")
            'Dim MyFileInfo As IO.FileInfo = New IO.FileInfo(vfilepath)
            'Dim MyObjectSecurity As Security.AccessControl.FileSecurity
            Try
                MyObjectSecurity = MyObjectInfo.GetAccessControl
            Catch ex2 As Exception
                Take_Owner(vfilefullpath)
                MyObjectSecurity = MyObjectInfo.GetAccessControl
            End Try

            Dim rule_deny As Security.AccessControl.FileSystemAccessRule = New Security.AccessControl.FileSystemAccessRule("Everyone", Security.AccessControl.FileSystemRights.FullControl, Security.AccessControl.AccessControlType.Deny)
            Dim rule_allow As Security.AccessControl.FileSystemAccessRule = New Security.AccessControl.FileSystemAccessRule("Everyone", Security.AccessControl.FileSystemRights.FullControl, Security.AccessControl.AccessControlType.Allow)
            If vEnabled = False Then
                MyObjectSecurity.RemoveAccessRule(rule_allow)
                MyObjectSecurity.AddAccessRule(rule_deny)
            Else
                MyObjectSecurity.RemoveAccessRule(rule_deny)
                MyObjectSecurity.AddAccessRule(rule_allow)
            End If
            MyObjectInfo.SetAccessControl(MyObjectSecurity)
            Return True
        Catch ex As Exception
            vEx_Catch = ex
            Return False
        End Try

    End Function
    Public Function Set_IFEO(ByVal vName As String, ByVal vEnabled As Boolean, ByVal vDebugger_Path As String, ByRef vEx_Catch As Exception) As Boolean
        Try
            If vName.ToLower.EndsWith(".exe") = False Then Return False
            Dim vReg As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options", True)
            vReg = vReg.CreateSubKey(vName)
            If vEnabled = False Then
                vReg.SetValue("Debugger", vDebugger_Path + " -f ""VoCyt Tools"" -fx """" ""VoCyt Defender"" """" ""主面板"" ""Receiver"" -n " + vName + " -e")
            Else
                vReg.DeleteValue("Debugger", False)
            End If
            Return True
        Catch ex As Exception
            vEx_Catch = ex
            Return False
        End Try

    End Function
    Public Function Check_IFEO_Permission() As Boolean
        Try
            Dim vReg As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options", True)
            vReg = vReg.CreateSubKey("VoCytTest")
            vReg.SetValue("Debugger", "c:\windows\cmd.exe")
            vReg = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options", True)
            vReg.DeleteSubKey("VoCytTest")
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Sub Take_Owner(ByVal inString As String)
        Dim take_own_exe As String = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\takeown.exe"
        If IO.File.Exists(take_own_exe) = False Then
            take_own_exe = IO.Path.GetTempPath() + "\takeown.exe"
            If IO.File.Exists(take_own_exe) = False Then
                IO.File.WriteAllBytes(take_own_exe, My.Resources.takeown)
            End If
        End If
        Shell("cmd.exe /c """ + take_own_exe + " /f " + inString.Replace("\\", "\") + "", AppWinStyle.Hide, True)
    End Sub

    Public Function Get_CPU_Meltdown_Spectre() As Boolean
        Dim MyReg As Microsoft.Win32.RegistryKey
        MyReg = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management")
        If MyReg Is Nothing Then
            Return True
        Else
            Dim FeatureSettingsOverride As Integer = MyReg.GetValue("FeatureSettingsOverride", -999)
            Dim FeatureSettingsOverrideMask As Integer = MyReg.GetValue("FeatureSettingsOverrideMask", -999)
            If FeatureSettingsOverride = -999 And FeatureSettingsOverrideMask = -999 Then
                Return True
            ElseIf FeatureSettingsOverride = 3 And FeatureSettingsOverrideMask = 3 Then
                Return False
            Else
                Return True
            End If
        End If
    End Function
    Public Sub Set_CPU_Meltdown_Spectre(ByVal vBoolean As Boolean)
        Dim MyReg As Microsoft.Win32.RegistryKey
        If vBoolean = True Then
            MyReg = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", True)
            If MyReg Is Nothing Then Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree)
            If MyReg.GetValue("FeatureSettingsOverride_Backup", "Nothing").ToString = "Nothing" Then
                MyReg.DeleteValue("FeatureSettingsOverride")
                MyReg.DeleteValue("FeatureSettingsOverride_Backup", False)
            Else
                MyReg.SetValue("FeatureSettingsOverride", MyReg.GetValue("FeatureSettingsOverride_Backup"))
                MyReg.DeleteValue("FeatureSettingsOverride_Backup", False)
            End If
            If MyReg.GetValue("FeatureSettingsOverrideMask_Backup", "Nothing").ToString = "Nothing" Then
                MyReg.DeleteValue("FeatureSettingsOverrideMask")
                MyReg.DeleteValue("FeatureSettingsOverrideMask_Backup", False)
            Else
                MyReg.SetValue("FeatureSettingsOverrideMask", MyReg.GetValue("FeatureSettingsOverrideMask_Backup"))
                MyReg.DeleteValue("FeatureSettingsOverrideMask_Backup", False)
            End If
        Else
            MyReg = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", True)
            If MyReg Is Nothing Then Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree)
            MyReg.SetValue("FeatureSettingsOverride_Backup", MyReg.GetValue("FeatureSettingsOverride", "Nothing"))
            MyReg.SetValue("FeatureSettingsOverride", 3, Microsoft.Win32.RegistryValueKind.DWord)
            MyReg.SetValue("FeatureSettingsOverrideMask_Backup", MyReg.GetValue("FeatureSettingsOverrideMask", "Nothing"))
            MyReg.SetValue("FeatureSettingsOverrideMask", 3, Microsoft.Win32.RegistryValueKind.DWord)
        End If
    End Sub
    Public Sub Scan_Files(ByVal vPath As String, ByRef vFile As ArrayList)
        Try
            For Each sPath As String In IO.Directory.GetDirectories(vPath)
                Scan_Files(sPath, vFile)
            Next
            For Each sFile As String In IO.Directory.GetFiles(vPath)
                vFile.Add(sFile)
            Next
        Catch ex As Exception

        End Try
    End Sub
    Public Function Del_Tree(ByVal vPath As String) As Boolean
        Try
            IO.Directory.Delete(vPath, True)

        Catch ex As Exception

        End Try

        If IO.Directory.Exists(vPath) Then Return False
        Return True
    End Function
    Public Sub Save_Data()
        Try
            IO.File.Delete(INI_Path)
            Dim vStr As String = "Version" + Data_Version.ToString + vbCrLf
            For Each sData As My_Data_Type In vData
                vStr += sData.Name + "|" + sData.Value.ToString + "|" + sData.Path + "|" + sData.Info + vbCrLf
            Next
            If vStr.EndsWith(vbCrLf) Then vStr = Left(vStr, vStr.Length - 2)
            IO.File.WriteAllText(INI_Path, vStr, System.Text.Encoding.UTF8)
        Catch ex As Exception

        End Try
    End Sub
    Public Function String_to_Data(ByVal InString As String) As My_Data_Type()
        If IO.Directory.Exists(Path_Info) = False Then IO.Directory.CreateDirectory(Path_Info)

        Try
            If IO.File.Exists(VCD_Path) = False Then IO.File.WriteAllBytes(VCD_Path, My.Resources.VoCytDefenderEx)
        Catch ex As Exception
            Try
                VCD_Path = (IO.Path.GetTempPath + "\VoCytDefenderEx.exe").Replace("\\", "\")
                If IO.File.Exists(VCD_Path) = False Then IO.File.WriteAllBytes(VCD_Path, My.Resources.VoCytDefenderEx)
            Catch ex2 As Exception
                CanIFEO = False
            End Try
        End Try
        

        Dim vline() As String = Split(InString, vbCrLf)
        If vline.Length < 2 Then IO.File.WriteAllText(INI_Path, My.Resources.list, System.Text.Encoding.UTF8) : Return String_to_Data(My.Resources.list)
        If IsNumeric(vline(0).Replace("Version", "")) = False Then IO.File.WriteAllText(INI_Path, My.Resources.list, System.Text.Encoding.UTF8) : Return String_to_Data(My.Resources.list)
        If CDbl(vline(0).Replace("Version", "")) < Data_Version Then IO.File.WriteAllText(INI_Path, My.Resources.list, System.Text.Encoding.UTF8) : Return String_to_Data(My.Resources.list)
        Dim vRet(vline.Length - 2) As My_Data_Type
        Dim vline_2() As String
        For i = 0 To vline.Length - 2
            Try
                vline_2 = vline(i + 1).Split("|")
                vRet(i).Name = vline_2(0)
                vRet(i).Path = vline_2(2)
                vRet(i).Info = vline_2(3)
                vRet(i).Value = CInt(vline_2(1))
            Catch ex As Exception

            End Try
        Next
        Return vRet

    End Function

    Public Sub Scan_For_Addon(ByVal path As String, ByVal FileNames As ArrayList, ByRef InData As ArrayList)
        Dim sg As My_Data_Type
        For Each vline In IO.Directory.GetFiles(path)
            If FileNames.Contains(IO.Path.GetFileName(vline).ToLower) Then
                sg = New My_Data_Type
                sg.Name = IO.Path.GetFileName(vline)
                For Each vline2 In Split(My.Resources.list, vbCrLf)
                    If vline2.ToLower.Contains(sg.Name.ToLower) Then sg.Info = vline2.Split("|")(3)
                Next
                If sg.Name = "" Then sg.Name = "自动扫描"
                sg.Path = ("\" + Mid(vline, Main.GamePath.Text.Length + 1, vline.Length)).Replace("\\", "\")

                Select Case sg.Name.ToLower
                    Case "tensafe.exe", "tpsvc.exe"
                        sg.Value = 0
                    Case "res.vfs"
                        If sg.Path.ToLower.Contains("Apps\DNFAD".ToLower) _
                        Or sg.Path.ToLower.Contains("Apps\DNFTips".ToLower) Then
                            sg.Value = 1
                        Else
                            sg.Value = 0
                        End If
                    Case "qbclient.exe"
                        sg.Value = 1
                    Case Else
                        If sg.Name.ToLower.EndsWith(".exe") Then
                            sg.Value = 2
                        Else
                            sg.Value = 1
                        End If

                End Select
            
            InData.Add(sg)
            End If
        Next
        For Each vline In IO.Directory.GetDirectories(path)
            Scan_For_Addon(vline, FileNames, InData)
        Next
    End Sub
    Public Function Get_Files_Count(ByVal Path As String) As Integer
        Dim eax As Integer = 0
        Try
            eax += IO.Directory.GetFiles(Path).Length
            For Each vline In IO.Directory.GetDirectories(Path)
                eax += Get_Files_Count(vline)
            Next
        Catch ex As Exception

        End Try


        
        Return eax
    End Function
End Module
Class mt
    Public Sub Check_for_Update()
        Dim nowversion As Integer = vCode.Get_Application_Version().Replace(".", "")
        Dim tmp As String = IO.Path.GetTempFileName
        Dim latestversion As Integer = 0
        Try
            My.Computer.Network.DownloadFile(Update_URL, tmp, "", "", False, 500, True)
            tmp = IO.File.ReadAllText(tmp)
            If IsNumeric(tmp.Replace(".", "")) Then latestversion = CInt(tmp.Replace(".", ""))
        Catch ex As Exception

        End Try
        If latestversion > nowversion Then
            If MsgBox("已有新版本更新，是否跳转至程序发布/更新网页？", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                Diagnostics.Process.Start(Update_Page)
            End If
        End If




    End Sub
End Class
