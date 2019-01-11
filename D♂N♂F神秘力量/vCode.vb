Module vCode
    Public vData() As My_Data_Type
    Public CanIFEO As Boolean
    Public VoCytDefenderEx_Path As String
    Public Public_ArrayList As ArrayList
    Dim vCurrend_File As String
    Public Structure My_Data_Type
        Dim Name As String
        Dim Value As Boolean
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
    Public Function Get_Currend_File_String(Optional ByVal Direct As Boolean = False) As My_Data_Type()
        Dim vpath As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\" + Application.ProductName
        vCurrend_File = vpath + "\list.ini"
        VoCytDefenderEx_Path = vpath + "\VoCytDefenderEx.exe"
        If IO.Directory.Exists(vpath) = False Then IO.Directory.CreateDirectory(vpath)
        If IO.Directory.Exists(VoCytDefenderEx_Path) = False Then IO.File.WriteAllBytes(VoCytDefenderEx_Path, My.Resources.VoCytDefenderEx)

        If (IO.File.Exists(vCurrend_File) = False) And (Direct = False) Then
            IO.File.WriteAllText(vCurrend_File, My.Resources.list, System.Text.Encoding.Unicode)
        End If
        Dim vstring() As String = IO.File.ReadAllText(vCurrend_File, System.Text.Encoding.Unicode).Split(vbCrLf)
        Dim vstring2() As String
        Dim vret(vstring.Length - 1) As My_Data_Type
        For i = 0 To vstring.Length - 1
            Try
                vstring2 = vstring(i).Split("|")
                vret(i).Name = vstring2(1)
                vret(i).Path = vstring2(3)
                vret(i).Info = vstring2(4)
                vret(i).Value = CBool(vstring2(2))
            Catch ex As Exception

            End Try
        Next
        Return vret
    End Function
    Public Sub Set_Application_Title()
        Main.Text = "D♂N♂F神秘力量 Ver " + Get_Application_Version() + " "
        If CanIFEO Then Main.Text += "[IFEO]" Else Main.Text += "[文件读写]"
        Main.Text += " Powered by VoCyt"
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
                            sProcess.Kill()
                        End If
                    Next
                Next
            End If
        End If
    End Sub
    Private Function Get_Application_Version() As String
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
            Dim vfilepath As String = vfilefullpath.Replace("\\", "\")
            If IO.File.Exists(vfilepath) = False Then Throw New Exception("File not found.")
            Dim MyFileInfo As IO.FileInfo = New IO.FileInfo(vfilepath)
            Dim MyFileSecurity As Security.AccessControl.FileSecurity
            Try
                MyFileSecurity = MyFileInfo.GetAccessControl
            Catch ex2 As Exception
                Take_Owner(vfilefullpath)
                MyFileSecurity = MyFileInfo.GetAccessControl
            End Try


            Dim rule_deny As Security.AccessControl.FileSystemAccessRule = New Security.AccessControl.FileSystemAccessRule("Everyone", Security.AccessControl.FileSystemRights.FullControl, Security.AccessControl.AccessControlType.Deny)
            Dim rule_allow As Security.AccessControl.FileSystemAccessRule = New Security.AccessControl.FileSystemAccessRule("Everyone", Security.AccessControl.FileSystemRights.FullControl, Security.AccessControl.AccessControlType.Allow)
            If vEnabled = False Then
                MyFileSecurity.RemoveAccessRule(rule_allow)
                MyFileSecurity.AddAccessRule(rule_deny)
            Else
                MyFileSecurity.RemoveAccessRule(rule_deny)
                MyFileSecurity.AddAccessRule(rule_allow)
            End If
            MyFileInfo.SetAccessControl(MyFileSecurity)
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
        Shell("cmd.exe /c """ + take_own_exe + " /f " + inString.Replace("\\", "\") + "")
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
            IO.File.Delete(vCurrend_File)
            Dim vStr As String = ""
            For Each sData As My_Data_Type In vData
                vStr += "|" + sData.Name + "|" + sData.Value.ToString + "|" + sData.Path + "|" + sData.Info + vbCrLf
            Next
            IO.File.WriteAllText(vCurrend_File, vStr, System.Text.Encoding.Unicode)
        Catch ex As Exception

        End Try
    End Sub

End Module
