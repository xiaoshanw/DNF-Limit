Module Code
    Public CheckBoxArray As New ArrayList
    Public LabelArray_Normal As New ArrayList
    Public LabelArray_Ext As New ArrayList

    Private Function Get_Application_Version() As String
        Try
            Return System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString
        Catch ex As Exception
            Return "0.0.0.0"
        End Try
    End Function
    Public Sub Set_Application_Title()
        Main.Text = "D♂N♂F神秘力量 Ver" + Get_Application_Version() + " Powered by VoCyt"
    End Sub
    Public Function Get_Windows_Version() As String
        Select Case Environment.OSVersion.Version.Major
            Case 3
                Return "Windows NT 351"
            Case 4
                Return "Windows NT 40"
            Case 5
                Select Case Environment.OSVersion.Version.Minor
                    Case 0
                        Return "Windows 2000"
                    Case 1
                        Return "Windows XP"
                    Case 2
                        Return "Windows 2003"
                    Case Else
                        Return "Unknown Windows NT 5.x"
                End Select
            Case 6
                Select Case Environment.OSVersion.Version.Minor
                    Case 0
                        Return "Windows Vista"
                    Case 1
                        Return "Windows 7"
                    Case 2
                        Return "Windows 8"
                    Case 3
                        Return "Windows 8.1"
                    Case 4
                        Return "Windows 10"
                    Case Else
                        Return "Unknown Windows 6.x"
                End Select
            Case 10
                Return "Windows 10"
            Case Else
                Return "Unknown Windows"
        End Select

    End Function
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
    Public Function Set_File_Security(ByVal vfilefullpath As String, ByVal vEnabled As Boolean, ByRef vEx_Catch As Exception) As Boolean
        Try
            Dim vfilepath As String = vfilefullpath.Replace("\\", "\")
            If IO.File.Exists(vfilepath) = False Then Throw New Exception("File not found.")
            Dim MyFileInfo As IO.FileInfo = New IO.FileInfo(vfilepath)
            Dim MyFileSecurity As Security.AccessControl.FileSecurity = MyFileInfo.GetAccessControl
            'For Each rule as Security.AccessControl.FileSystemAccessRule In MyFileSecurity.GetAccessRules(True, True, GetType(System.Security.Principal.NTAccount))
            ' MsgBox(rule.IdentityReference.Value)
            ' MsgBox(rule.FileSystemRights.ToString)
            ' Next
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
    Public Function ReadWriteTest(ByVal vfilepath As String) As String
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
    Public Sub Find_DNF_Path()
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
                        If CheckGamePath(vString_Tmp) Then
                            Main.GamePath.Text = vString_Tmp
                            Exit Sub
                        End If
                    Case "path"
                        If vString2.Length <> 2 Then Continue For
                        vDriverInfo = IO.DriveInfo.GetDrives()
                        For j = 0 To vDriverInfo.Length - 1
                            Select Case vDriverInfo(j).DriveType
                                Case IO.DriveType.CDRom, IO.DriveType.Ram, IO.DriveType.Removable
                                    Continue For
                                Case Else
                                    If vDriverInfo(j).DriveType = IO.DriveType.Network Then Continue For
                                    If vDriverInfo(j).IsReady = False Then Continue For
                                    vString_Tmp = Replace(Replace(vString2(1), "$(driver)", vDriverInfo(j).Name), "\\", "\")
                                    If CheckGamePath(vString_Tmp) Then
                                        Main.GamePath.Text = vString_Tmp
                                        Exit Sub
                                    End If
                            End Select
                        Next
                    Case Else
                        Continue For
                End Select
            Next
        Catch ex As Exception

        End Try

    End Sub
    Private Function CheckGamePath(ByVal vpath As String) As Boolean
        If IO.Directory.Exists(vpath) = False Then Return False
        Dim detail() As IO.DirectoryInfo = New IO.DirectoryInfo(vpath).GetDirectories
        Dim eax = 0
        For i = 0 To detail.Length - 1
            If detail(i).Name.ToLower = "ImagePacks2".ToLower Then eax += 1
            If detail(i).Name.ToLower = "SoundPacks".ToLower Then eax += 1
        Next
        If eax = 2 Then Return True Else Return False
    End Function
    Public Sub CheckStatus()
        Dim IsRead, IsWrite, isIFEO As Boolean
        Dim vMod As Integer
        Dim filepath As String
        For i = 0 To CheckBoxArray.Count - 1
            filepath = Main.GamePath.Text.ToString + CheckBoxArray(i).Tag.ToString
            If IO.File.Exists(filepath) = False Then LabelArray_Normal(i).Text = "未找到" : LabelArray_Ext(i).Text = "未找到" : Continue For
            vMod = ReadWriteTest(filepath)
            If vMod Mod 2 <> 0 Then IsRead = True Else IsRead = False
            If vMod Mod 3 <> 0 Then IsWrite = True Else IsWrite = False
            If vMod Mod 5 <> 0 Then isIFEO = True Else isIFEO = False
            If IsRead And IsWrite Then LabelArray_Normal(i).Text = "正常" Else LabelArray_Normal(i).Text = "禁用"
            If isIFEO Then LabelArray_Ext(i).Text = "正常" Else LabelArray_Ext(i).Text = "禁用"
        Next
    End Sub
    Public Sub Kill_Process()
        Dim s As String = "下列正在运行的程序可能会影响本软件运行，是否关闭？" + vbCrLf + "----------" + vbCrLf
        Dim vDNF_List As String = "CrossProxy,TPHelper,TQMCenter,tgp_gamead,GameLoader,DNF"
        Dim vProcess() As Process = Process.GetProcesses
        Dim vMessage As String = s
        For Each sProcess As Process In vProcess
            If InStr(vDNF_List.ToLower, sProcess.ProcessName.ToLower) > 0 Then
                vMessage += sProcess.ProcessName + vbCrLf
            End If
        Next
        If vMessage <> s Then
            If MsgBox(vMessage, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                For Each sProcess As Process In vProcess
                    If InStr(vDNF_List.ToLower, sProcess.ProcessName.ToLower) > 0 Then
                        sProcess.Kill()
                    End If
                Next
            End If
        End If
    End Sub
    Public Sub Set_TX(ByVal vEnabled As Boolean)
        With MyMsg
            .Clear()
            .Append("当前系统:" + Get_Windows_Version())
            Dim ex As Exception = New Exception
            If Main.RadioButton1.Checked = True Then
                '普通模式
                .Append("常规模式(基于NTFS分区权限)")
                For Each vdata As CheckBox In CheckBoxArray
                    .Add("[" + vdata.Text + "]")
                    If vdata.Checked = False Then .Append("跳过") : Continue For
                    .Add("配置文件权限")
                    If Set_File_Security(Main.GamePath.Text + vdata.Tag, vEnabled, ex) = True Then .Append("[成功]") Else .Append("[失败]" + ex.Message)
                Next
            Else
                '增强模式
                .Append("增强模式(基于IFEO技术)")
                For Each vdata As CheckBox In CheckBoxArray
                    .Add("[" + vdata.Text + "]")
                    If vdata.Checked = False Then .Append("跳过") : Continue For
                    .Add("配置IFEO")
                    If Set_IFEO(vdata.Text, vEnabled, ExtractVCD, ex) = True Then .Append("[成功]") Else .Append("[失败]" + ex.Message)
                Next
            End If
            .Append("完成")
        End With
    End Sub
    Private Function ExtractVCD() As String
        Dim f As String = IO.Path.GetTempPath + "VoCytDefenderEx.exe"
        If IO.File.Exists(f) Then Return f
        IO.File.WriteAllBytes(f, My.Resources.VoCytDefenderEx)
        Return f
    End Function
    Private Function Set_IFEO(ByVal vName As String, ByVal vEnabled As Boolean, ByVal vDebugger_Path As String, ByRef vEx_Catch As Exception) As Boolean
        Try
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

End Module
