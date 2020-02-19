Public Class Main

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim tmp As FolderBrowserDialog = New FolderBrowserDialog
        If tmp.ShowDialog = Windows.Forms.DialogResult.OK Then GamePath.Text = tmp.SelectedPath
    End Sub

    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Kill_Process()
        Dim lastpath As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\" + Application.ProductName + "\last-path.ini"
        If IO.File.Exists(lastpath) Then
            Try
                GamePath.Text = IO.File.ReadAllText(lastpath)
            Catch ex As Exception
                GamePath.Text = Find_DNF_Path()
            End Try
        Else
            GamePath.Text = Find_DNF_Path()
        End If

        CanIFEO = Check_IFEO_Permission()

        If IO.File.Exists(INI_Path) Then
            vData = String_to_Data(IO.File.ReadAllText(INI_Path))
        Else
            vData = String_to_Data("")
        End If
        禁用更新ToolStripMenuItem.Checked = Not IO.File.Exists(Path_Info + "\noUpdate")
        气泡提示ToolStripMenuItem.Checked = Not IO.File.Exists(Path_Info + "\noBalloonTip")
        自动删除rep文件ToolStripMenuItem.Checked = Not IO.File.Exists(Path_Info + "\noDelrepFile")
        开机启动ToolStripMenuItem.Checked = IO.File.Exists(Startup_Path + "\" + Application.ProductName + ".bat")

        'If CanIFEO = False Then MsgBox("权限不足，将采用[文件读写]模式" + vbCrLf + "[更新游戏]需要[恢复]！！！" + vbCrLf + "详情参阅帮助")
        Set_Application_Title()

        Dim Args_Background = False
        Dim Args_Update = Not 禁用更新ToolStripMenuItem.Checked
        For Each vline In My.Application.CommandLineArgs
            Select Case vline.ToLower
                Case "-b", "-bg", "-background"
                    Args_Background = True
                Case "-nu", "-noupgrade"
                    Args_Update = False
                Case Else
                    If vline.ToLower.StartsWith("-path=") Then
                        GamePath.Text = Mid(vline, 7)
                    End If
            End Select
        Next
        If Args_Update And Not DEBUG_MODE Then
            Dim nc As New mt
            nc.isMessage = True
            Dim nt As New Threading.Thread(AddressOf nc.Check_for_Update)
            nt.Start()
        End If
        If Args_Background Then
            NotifyIcon1.Visible = True
            ShowBalloonTipEx(NotifyIcon1, 2000, "开启后台模式", "将自动检测[启动器/启动插件]" + vbCrLf + "并在游戏成功运行后自动关闭[启动器/启动插件]", ToolTipIcon.Info)
            Me.Visible = False
            'Me.Hide()
            Hide_Run = True
            Auto_Kill_Gameloader_Flag = 0
            AutoKill_GameLoader.Start()
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        vList.Set_Window(600, 500, False)
        If vList.Check_Status < 3 Then
            PAppend("[警告]有效组件数量过少，请检查游戏路径")
        End If
    End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        vList.Set_Window(600, 500, False)
        If vList.Check_Status(False) < 3 Then
            PAppend("[警告]有效组件数量过少，请检查游戏路径")
            Exit Sub
        End If
        Dim Bad As Boolean = False
        Dim MyException As Exception = New Exception("")
        Dim sData As My_Data_Type
        For i = 0 To vList.Exl.Rows.Count - 1
            sData = vData(vList.Exl.Rows(i).Cells(0).Value)
            'If sData.Value = False Then Continue For
            PPrint("执行禁用[" + sData.Name + "]")

            Select Case sData.Value
                Case 0
                    PAppend("[跳过]")
                Case 1
                    Bad = True
                    If Set_File_Security(GamePath.Text + "\" + sData.Path, False, MyException) Then
                        PAppend("[成功]")
                    Else
                        PAppend("[失败][" + MyException.Message + "]")
                    End If
                Case 2
                    If CanIFEO Then
                        If Set_IFEO(sData.Name, False, VCD_Path, MyException) Then
                            PAppend("[成功]")
                        Else
                            PAppend("[失败][" + MyException.Message + "][转用常规模式(NTFS权限)]")
                            Bad = True
                            If Set_File_Security(GamePath.Text + "\" + sData.Path, False, MyException) Then
                                PAppend("[成功]")
                            Else
                                PAppend("[失败][" + MyException.Message + "]")
                            End If
                        End If
                    Else
                        Bad = True
                        If Set_File_Security(GamePath.Text + "\" + sData.Path, False, MyException) Then
                            PAppend("[成功]")
                        Else
                            PAppend("[失败][" + MyException.Message + "]")
                        End If
                    End If
                    
            End Select

        Next
        vList.Check_Status(False)
        PAppend("执行结束")
        If Bad = True Then PAppend("部分插件采用[文件读写]模式禁用，更新游戏建议还原插件，详情参阅[帮助]")
        Disable_TGuardSvc_All(False)
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        vList.Set_Window(600, 500, False)
        If vList.Check_Status(False) < 3 Then
            PAppend("[警告]有效组件数量过少，请检查游戏路径")
            Exit Sub
        End If
        Dim b1 As Boolean = True
        Dim b2 As Boolean = True
        Dim Ex1 As Exception = New Exception("")
        Dim Ex2 As Exception = New Exception("")
        Dim sData As My_Data_Type
        For i = 0 To vList.Exl.Rows.Count - 1
            sData = vData(vList.Exl.Rows(i).Cells(0).Value)
            PPrint("执行恢复[" + sData.Name + "]")
            If (sData.CanRead = False) Or (sData.CanWrite = False) Then
                b1 = Set_File_Security(GamePath.Text + "\" + sData.Path, True, Ex1)
            End If
            If sData.CanRun = False Then
                b2 = Set_IFEO(sData.Name, True, VCD_Path, Ex2)
            End If
            If b1 And b2 Then
                PAppend("[成功]")
            Else
                If b1 = False Then PPrint("[失败][" + Ex1.Message + "]")
                If b2 = False Then PPrint("[失败][" + Ex2.Message + "]")
                PAppend("")
            End If
        Next
        vList.Check_Status(False)
        PAppend("执行结束")
    End Sub
    Public Sub Check_And_Add(ByVal Index As Integer, ByRef InData As Object, ByVal InDataGridView As DataGridView)
        Dim B1, B2 As String
        Dim vMod As Integer = Check_File_Single(Me.GamePath.Text + InData.Path)
        If vMod Mod 2 <> 0 Then InData.CanRead = True Else InData.CanRead = False
        If vMod Mod 3 <> 0 Then InData.CanWrite = True Else InData.CanWrite = False
        If vMod Mod 5 <> 0 Then InData.CanRun = True Else InData.CanRun = False

        If InData.CanRead And InData.CanWrite Then B1 = "禁用" Else B1 = "恢复"
        If InData.Name.ToString.ToLower.EndsWith(".exe") = True Then
            If InData.CanRun Then B2 = "禁用" Else B2 = "恢复"
            InDataGridView.Rows.Add(Index, InData.Name, ctxt(InData.CanRead And InData.CanWrite And InData.CanRun), ctxt(InData.CanRead And InData.CanWrite), ctxt(InData.CanRun), InData.Info, B1, B2, InData.Path)
        Else
            InDataGridView.Rows.Add(Index, InData.Name, ctxt(InData.CanRead And InData.CanWrite), ctxt(InData.CanRead And InData.CanWrite), "不支持", InData.Info, B1, "不支持", InData.Path)
        End If
    End Sub
    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        vList.Set_Window(800, 600, True)
        vList.Check_Status(True, True)
        
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click

        Try
            Dim tpf = IO.Path.GetTempPath
            tpf += Application.ProductName + "帮助文档.txt"
            IO.File.Delete(tpf)
            IO.File.WriteAllText(tpf, My.Resources.help)
            Diagnostics.Process.Start(tpf)

        Catch ex As Exception
            vMSG.Mode = ""
            vMSG.TextBox1.Text = My.Resources.help
            vMSG.TextBox1.Select(0, 0)
            vMSG.Show()
        End Try


    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        With vMSG.TextBox1
            .Clear()
            .AppendText("补丁简介：" + vbCrLf)
            .AppendText("Meltdown(熔断)允许低权限、用户级别的应用程序""越界""访问系统级的内存，从而造成数据泄露" + vbCrLf)
            .AppendText("Spectre(幽灵)可以骗过安全检查程序，使得应用程序访问内存的任意位置" + vbCrLf + vbCrLf)
            .AppendText("上述两个补丁在Win10最新的1803及以上版本中默认开启，但会导致CPU略微性能下降，在5代之前intel的CPU性能下降尤为明显" + vbCrLf + vbCrLf)
            .AppendText("关闭补丁会导致系统稳定性降低，谨慎关闭" + vbCrLf)
            .AppendText("-------------------" + vbCrLf)
            .AppendText("补丁状态:" + vbCrLf)
            If Get_CPU_Meltdown_Spectre() = True Then
                .AppendText("未配置(默认)")
                vMSG.Mode = "cpu_patch_disable"
            Else
                .AppendText("禁用")
                vMSG.Mode = "cpu_patch_enable"
            End If
        End With

        vMSG.Show()
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        With vMSG.TextBox1
            .Clear()
            .AppendText("如遇到提示请按照下列操作：" + vbCrLf)
            .AppendText("--------------------" + vbCrLf)
            .AppendText("是否强制卸除该卷：[选择否(N)]" + vbCrLf)
            .AppendText("是否计划在下一次系统重新启动时检查此卷：[选择是(Y)]" + vbCrLf)
            .AppendText("--------------------" + vbCrLf)
            .AppendText("重新启动计算机，并让系统执行磁盘检查（请勿跳过！！！）" + vbCrLf)
        End With
        vMSG.Mode = "chkdsk"
        vMSG.Show()
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        With vMSG.TextBox1
            .Clear()
            .AppendText("该功能将删除游戏目录下tgppatches文件夹内更新安装包：" + vbCrLf)
            .AppendText("--------------------" + vbCrLf)
            Try
                Public_ArrayList = New ArrayList
                Scan_Files(GamePath.Text + "\tgppatches", Public_ArrayList)
                If Public_ArrayList.Count = 0 Then
                    .AppendText("无" + vbCrLf)
                Else
                    For Each sFile In Public_ArrayList
                        .AppendText("[" + Format(New IO.FileInfo(sFile).Length / 1024 / 1024, "#.##") + "MB]" + vbTab + IO.Path.GetFileName(sFile) + vbCrLf)
                    Next
                End If


            Catch ex As Exception

            End Try
        End With
        vMSG.Mode = "del_patch"
        vMSG.Show()
    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        With vMSG.TextBox1
            .Clear()
            .AppendText("该功能将删除游戏目录下列文件夹：" + vbCrLf)
            .AppendText("--------------------" + vbCrLf)
            .AppendText("components：TX管家" + vbCrLf)
            .AppendText("TGuard：TP3Helper独立后台" + vbCrLf)
            .AppendText("TP_Temp：TP临时文件夹" + vbCrLf)

            Try
                Public_ArrayList = New ArrayList
                If Public_ArrayList.Count = 0 Then
                    .AppendText("无" + vbCrLf)
                Else
                    For Each sFile In Public_ArrayList
                        .AppendText("[" + Format(New IO.FileInfo(sFile).Length / 1024 / 1024, "#.##") + "MB]" + vbTab + IO.Path.GetFileName(sFile) + vbCrLf)
                    Next
                End If


            Catch ex As Exception

            End Try
        End With
        vMSG.Mode = "del_3rd"
        vMSG.Show()
    End Sub


    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        With vMSG.TextBox1
            .Clear()
            .AppendText("该功能将停止并禁用DNF注册的TGuardSvc服务" + vbCrLf)
            .AppendText("--------------------" + vbCrLf)
        End With
        vMSG.Mode = "tguard"
        vMSG.Show()
    End Sub

    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        With vMSG.TextBox1
            .Clear()
            .AppendText("该功能将自动检测并关闭下列启动器及插件：" + vbCrLf)
            .AppendText("下载器：TenioDL.exe" + vbCrLf)
            .AppendText("启动器：GameLoader.exe" + vbCrLf)
            .AppendText("后台服务：TesService.exe" + vbCrLf)
            .AppendText("注册服务：TGuardSvc.exe、TGuard.exe" + vbCrLf)
            .AppendText("--------------------" + vbCrLf)
            .AppendText("如需运行软件自动进入后台模式，可在运行本程序时，添加 -b参数，如：" + vbCrLf)
            .AppendText("D♂N♂F神秘力量.exe -b" + vbCrLf)
            .AppendText("--------------------" + vbCrLf)
            .AppendText("如需开机自启动后台模式，请在开启后台模式后，右键左下角图标进行设置" + vbCrLf)
            .AppendText("启动文件目录位于：" + Startup_Path)
        End With
        vMSG.Mode = "background"
        vMSG.Show()
    End Sub

    Private Sub AutoKill_GameLoader_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoKill_GameLoader.Tick

        Try
            '删除rep
            If Process_dnf IsNot Nothing Then
                If 自动删除rep文件ToolStripMenuItem.Checked = True Then
                    Dim repfiles = IO.Directory.GetFiles(GamePath.Text)
                    For Each vline In repfiles
                        If IO.Path.GetFileName(vline).ToLower Like "video_*.rep" Then
                            Try
                                IO.File.Delete(vline)
                            Catch ex As Exception

                            End Try
                        End If
                    Next
                End If
                If Process_dnf.HasExited = False Then Exit Sub
            End If
        Catch ex As Exception
        End Try
        Dim vProcess() As Process = Process.GetProcesses
        Try
            Select Case Auto_Kill_Gameloader_Flag
                Case 0
                    If 自动删除rep文件ToolStripMenuItem.Checked = True Then
                        Try
                            Dim repfiles = IO.Directory.GetFiles(GamePath.Text)
                            For Each vline In repfiles
                                If IO.Path.GetFileName(vline).ToLower Like "video_*.rep" Then
                                    Try
                                        IO.File.Delete(vline)
                                    Catch ex2 As Exception

                                    End Try
                                End If
                            Next
                        Catch ex As Exception

                        End Try
                    End If
                        For Each vline As Process In vProcess
                            Select Case vline.ProcessName.ToLower
                                Case "teniodl", "gameloader" ', "tesservice"
                                    ShowBalloonTipEx(NotifyIcon1, 2000, "提示", "检测到" + vline.ProcessName + "启动", ToolTipIcon.Info)
                                    Auto_Kill_Gameloader_Flag = 1
                                    AutoKill_GameLoader.Interval = 1000
                                    Public_Date = Now
                                Case "tguard", "tguardsvc"
                                    If vline.MainModule.FileName.ToLower.Contains(TGuardSvc_Path.ToLower) Then
                                        ShowBalloonTipEx(NotifyIcon1, 2000, "提示", "检测到TGuardSvc服务", ToolTipIcon.Info)
                                        Disable_TGuardSvc_All(False)
                                        ShowBalloonTipEx(NotifyIcon1, 2000, "提示", "禁用TGuardSvc服务", ToolTipIcon.Info)
                                    End If
                            End Select
                        Next
                Case 1
                        If DateDiff(DateInterval.Second, Public_Date, Now) > 60 * 3 Then
                            Auto_Kill_Gameloader_Flag = 0
                            AutoKill_GameLoader.Stop()
                            AutoKill_Kill.Start()
                            AutoKill_GameLoader.Interval = 5000
                            Auto_Kill_Gameloader_Flag = 0
                            Exit Sub
                        End If
                        For Each vline As Process In vProcess
                            Select Case vline.ProcessName.ToLower
                                Case "dnf"
                                    ShowBalloonTipEx(NotifyIcon1, 2000, "提示", "检测到" + vline.ProcessName + "启动" + vbCrLf + "等待游戏载入", ToolTipIcon.Info)
                                    Auto_Kill_Gameloader_Flag = 2
                            End Select
                        Next
                Case 2
                        For Each vline As Process In vProcess
                            Select Case vline.ProcessName.ToLower
                                Case "dnf"
                                    'vline.Start()
                                    If vline.MainWindowTitle = "地下城与勇士" And DateDiff("s", vline.StartTime, Now) > 60 Then
60:                                     ShowBalloonTipEx(NotifyIcon1, 2000, "提示", vline.ProcessName + "启动成功" + vbCrLf + "将于10秒后结束残余启动器", ToolTipIcon.Info)
                                        AutoKill_GameLoader.Stop()
                                        AutoKill_Kill.Start()
                                        AutoKill_GameLoader.Interval = 5000
                                        Auto_Kill_Gameloader_Flag = 0
                                        Process_dnf = vline
                                    End If
                            End Select
                        Next
            End Select
        Catch ex As Exception
            'NotifyIcon1.ShowBalloonTip(2000, "错误", ex.Message, ToolTipIcon.Error)
        End Try


    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        AutoKill_GameLoader.Stop()
        Me.Visible = True
        NotifyIcon1.Visible = False
    End Sub

    Private Sub 还原ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 还原ToolStripMenuItem.Click
        NotifyIcon1_MouseDoubleClick(Nothing, Nothing)
    End Sub

    Private Sub 关闭ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 关闭ToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub Main_Activated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        If Hide_Run Then
            Hide_Run = False
            Me.Hide()
        End If
    End Sub

    Private Sub AutoKill_Kill_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoKill_Kill.Tick
        Dim vProcess() As Process = Process.GetProcesses
        Dim sProcess() As Process
        Try
            For Each vline As Process In vProcess
                Select Case vline.ProcessName.ToLower
                    Case "teniodl", "gameloader", "tesservice"
                        sProcess = Process.GetProcessesByName(vline.ProcessName)
                        For Each vline2 As Process In sProcess
                            If vline2.MainModule.FileName IsNot Nothing Then
                                If vline2.MainModule.FileName.ToString.ToLower.Contains(GamePath.Text.ToLower) Or _
                                vline2.MainModule.FileName.ToString.ToLower.Contains("tencent") Or _
                                vline2.MainModule.FileName.ToString.ToLower.Contains("tgp") Or _
                                vline2.MainModule.FileName.ToString.ToLower.Contains("wegame") Then
                                    Try
                                        vline2.Kill()
                                        ShowBalloonTipEx(NotifyIcon1, 2000, "提示", "结束" + vline2.ProcessName + "成功", ToolTipIcon.Info)
                                    Catch ex2 As Exception
                                        'NotifyIcon1.ShowBalloonTip(2000, "警告", "结束" + vline2.ProcessName + "失败" + vbCrLf + ex2.Message, ToolTipIcon.Warning)
                                    End Try
                                End If
                            End If
                        Next
                    Case "tguardsvc", "tguard"
                        Disable_TGuardSvc_All(False)
                        ShowBalloonTipEx(NotifyIcon1, 2000, "提示", "禁用TGuardSvc服务", ToolTipIcon.Info)
                End Select
            Next
        Catch ex As Exception
            'NotifyIcon1.ShowBalloonTip(2000, "错误", ex.Message, ToolTipIcon.Error)
        End Try
        AutoKill_Kill.Stop()
        AutoKill_GameLoader.Start()
        AutoKill_Kill.Enabled = False
    End Sub

    Private Sub Main_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Try
            Dim vpath As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\" + Application.ProductName
            Dim lastpath As String = vpath + "\last-path.ini"
            If GamePath.Text <> "" Then IO.File.WriteAllText(lastpath, GamePath.Text)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
        Dim str As String = Find_DNF_Path()
        If str = "" Then
            MsgBox("[注册表/默认安装路径/常规路径]均无DNF路径信息，无法检测DNF游戏路径" + vbCrLf + vbCrLf + "请手动指定DNF游戏路径")
        Else
            GamePath.Text = str
        End If
    End Sub

    Private Sub Button15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button15.Click
        If MsgBox("暴力模式会扫描DNF目录下所有相关的全家桶组件并禁用，有可能会造成游戏无法启动" + vbCrLf + vbCrLf + _
                  "如提示：您的游戏环境异常，请重启机器后再试" + vbCrLf + vbCrLf + _
                  "如提示异常,或者相关组件(如连发)不可用，请进入[手动模式]，还原相关插件，或者点击[一键还原]" + _
                  vbCrLf + vbCrLf + "是否使用暴力禁用？", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub

        Dim vData_Arraylist = New ArrayList
        Dim vFile_Keys = New ArrayList
        For Each vline In Addon_List_String
            vFile_Keys.Add(vline.ToLower)
        Next

        Scan_For_Addon(GamePath.Text, vFile_Keys, vData_Arraylist)
        vData = vData_Arraylist.ToArray(GetType(My_Data_Type))
        Save_Data()
        Button2_Click(Me, e)

    End Sub

    Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button16.Click
        With vMSG.TextBox1
            .Clear()
            .AppendText("该功能将删除DNF默认配置文件：" + vbCrLf)
            Dim cfgpath As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\.."
            If IO.File.Exists(cfgpath + "\LocalLow\DNF\DNF.cfg") Then
                cfgpath = IO.Path.GetFullPath(cfgpath + "\LocalLow\DNF\DNF.cfg")
                .AppendText(cfgpath + vbCrLf)
                .AppendText("--------------------" + vbCrLf)
                .AppendText("配置文件删除后，将初始化下列数据（包括但不限于）：" + vbCrLf)
                .AppendText("最后一次登录频道" + vbCrLf)
                .AppendText("游戏窗口设置" + vbCrLf)
                .AppendText("游戏分辨率设置" + vbCrLf)
                .AppendText("本地布局" + vbCrLf)
                vMSG.arg = cfgpath
                vMSG.Mode = "delcfg"
            Else
                If IO.File.Exists(cfgpath + "\Local\DNF\DNF.cfg") Then
                    cfgpath = IO.Path.GetFullPath(cfgpath + "\Local\DNF\DNF.cfg")
                Else
                    .AppendText("配置文件未找到！！" + vbCrLf)
                    .AppendText("--------------------" + vbCrLf)
                    .AppendText("通常配置文件位于：" + vbCrLf)
                    .AppendText("%userprofile%\AppData\LocalLow\DNF" + vbCrLf)
                    .AppendText("文件名：" + vbCrLf)
                    .AppendText("DNF.cfg")
                    vMSG.Mode = ""
                End If
            End If
        End With

        vMSG.Show()
    End Sub

    Private Sub 打开目录ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 打开目录ToolStripMenuItem.Click
        Diagnostics.Process.Start(Path_Info)
    End Sub

    Private Sub 重置配置ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 重置配置ToolStripMenuItem.Click
        For Each vline In IO.Directory.GetFiles(Path_Info)
            If IO.Path.GetExtension(vline).ToLower = ".ini" Then
                Try
                    IO.File.Delete(vline)
                Catch ex As Exception

                End Try
            End If
        Next
        MsgBox("重置完毕，请重新打开软件")
        Me.Close()
    End Sub

    Private Sub Button17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button17.Click
        With vMSG.TextBox1
            .Clear()
            .AppendText("该功能将获取游戏目录中所有文件访问权，并设置[正常访问]" + vbCrLf)
            .AppendText("如需屏蔽插件，请待该操作结束后，再次禁用相关插件" + vbCrLf)
            .AppendText("操作结束后，将获取游戏目录内所有文件所有者，并删除所有限制权限，并添加Everyone为完全访问" + vbCrLf)
            .AppendText("因目录权限限制，文件总数统计可能存在不准确性，该操作耗时较长（3~5分钟），请耐心等待" + vbCrLf)
            .AppendText("--------------------" + vbCrLf)
            .AppendText("Windows 10系统胡乱启用administrator管理员账号导致权限异常无法禁用文件的解决方法：" + vbCrLf)
            .AppendText("1.右键游戏目录>属性>安全>高级" + vbCrLf)
            .AppendText("2.点击禁用继承>从此对象中删除所有已继承的权限" + vbCrLf)
            .AppendText("3.添加>选择主体>输入everyone>勾选完全控制" + vbCrLf)
            .AppendText("4.回到高级选项卡>勾选使用可从此对象继承的权限项目替换所有子对象的权限项目>确定" + vbCrLf)
            .AppendText("5.弹窗提示这将使来自XXX的可继承权限替换此对象所有子对象的明确定义权限, 你想继续吗 > 是" + vbCrLf)
            .AppendText("6.确定" + vbCrLf)

            vMSG.Mode = "getpremission"
            vMSG.arg = (IO.Path.GetTempPath + "\takeown.exe").Replace("\\", "\")
        End With
        vMSG.Show()
    End Sub

    Private Sub 交流群421483534ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 交流群421483534ToolStripMenuItem.Click
        Diagnostics.Process.Start("https://jq.qq.com/?_wv=1027&k=53KHmZj")
    End Sub

    Private Sub 赞助ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 赞助ToolStripMenuItem.Click
        showmethemoney.Show()
    End Sub

    Private Sub GroupBox2_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox2.Resize
        'Dim leg = (sender.Width - 20) / 2
        Dim myButtonLeft = New ArrayList
        Dim myButtonRight = New ArrayList

        '后台模式
        myButtonLeft.Add(Button13)
        'chkdsk
        myButtonLeft.Add(Button8)
        '访问权
        myButtonLeft.Add(Button17)
        'cpu
        myButtonLeft.Add(Button7)
        '删除tx管家
        myButtonLeft.Add(Button10)
        '删除安装包
        myButtonLeft.Add(Button9)


        'dnf.cfg
        myButtonRight.Add(Button16)
        'TGuardSvc
        myButtonRight.Add(Button12)
        'Update
        myButtonRight.Add(Button11)
        '卸载
        myButtonRight.Add(Button19)
        '蓝屏
        myButtonRight.Add(Button18)
        Dim mySize = New Point((sender.Width - 20) / 2, 23)
        For Each vline In myButtonLeft
            vline.Size = mySize
        Next
        For Each vline In myButtonRight
            vline.Size = mySize
            vline.left = myButtonLeft(0).Left + myButtonLeft(0).Width + 8
        Next
    End Sub

    Private Sub 气泡提示ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 气泡提示ToolStripMenuItem.Click
        If 气泡提示ToolStripMenuItem.Checked Then
            IO.File.Create(Path_Info + "\NoBalloonTip").Close()
        Else
            IO.File.Delete(Path_Info + "\NoBalloonTip")
        End If
        气泡提示ToolStripMenuItem.Checked = Not 气泡提示ToolStripMenuItem.Checked
    End Sub

    Private Sub 开机启动ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 开机启动ToolStripMenuItem.Click
        Dim path = Startup_Path + "\" + Application.ProductName + ".bat"
        If 开机启动ToolStripMenuItem.Checked Then
            IO.File.Delete(path)
        Else
            Dim txt = "start /d """ + Application.StartupPath + """ " + IO.Path.GetFileName(Application.ExecutablePath) + " -b"
            IO.File.WriteAllText(path, txt, System.Text.Encoding.Default)
        End If
        开机启动ToolStripMenuItem.Checked = Not 开机启动ToolStripMenuItem.Checked
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        With vMSG.TextBox1
            .Clear()
            .AppendText("该功能将删除游戏主目录下所有rep录像文件：" + vbCrLf)
            .AppendText("--------------------" + vbCrLf)
            Try
                Public_ArrayList = New ArrayList
                Scan_Files(GamePath.Text, Public_ArrayList, False)
                If Public_ArrayList.Count = 0 Then
                    .AppendText("无" + vbCrLf)
                Else

                    For Each sFile In Public_ArrayList
                        If sFile.ToString.ToLower Like (GamePath.Text + "\video_*.rep").Replace("\\", "\").ToLower Then
                            .AppendText("[" + Format(New IO.FileInfo(sFile).Length / 1024 / 1024, "#.##") + "MB]" + vbTab + IO.Path.GetFileName(sFile) + vbCrLf)
                        End If
                    Next
                End If


            Catch ex As Exception

            End Try
        End With
        vMSG.Mode = "del_video_rep"
        vMSG.Show()
    End Sub

    Private Sub 自动删除rep文件ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 自动删除rep文件ToolStripMenuItem.Click
        If 自动删除rep文件ToolStripMenuItem.Checked Then
            IO.File.Create(Path_Info + "\noDelrepFile").Close()
        Else
            IO.File.Delete(Path_Info + "\noDelrepFile")
        End If
        自动删除rep文件ToolStripMenuItem.Checked = Not 自动删除rep文件ToolStripMenuItem.Checked
    End Sub

    Private Sub Button18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button18.Click
        If MsgBox("此按钮是给某些勇士使用的，如果您不是天选之勇士，请取消", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            If MsgBox("确定要触发蓝屏吗(请保存好所有正在编辑的文档)", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                If MsgBox("真的要触发蓝屏吗（这句话不是开玩笑）", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    vMSG.Show()
                    vBSOD()
                End If
            End If
        End If
    End Sub

    Private Sub Button19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button19.Click
        If IO.File.Exists(GamePath.Text + "\地下城与勇士卸载.exe") Then
            Shell((GamePath.Text + "\地下城与勇士卸载.exe").Replace("\\", "\"))
        End If
    End Sub

    '-----------------------------
    Private Sub Button23_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button23.Click
        If Is64Bit() = False Then
            MsgBox("仅64位系统可用")
            Exit Sub
        End If
        vLimit.ShowDialog()
    End Sub
    Private Function Is64Bit() As Boolean
        Dim addr = ""
        Try
            Dim vContOption = New System.Management.ConnectionOptions
            Dim vMS = New System.Management.ManagementScope("\\localhost", vContOption)
            Dim vQuery = New System.Management.ObjectQuery("select AddressWidth from Win32_Processor")
            Dim vSearcher = New System.Management.ManagementObjectSearcher(vMS, vQuery)
            Dim vObjectCollection = vSearcher.Get()
            For Each vObject In vObjectCollection
                addr = vObject("AddressWidth").ToString()
            Next
            If Int32.Parse(addr) = 64 Then
                Return True
            End If
        Catch ex As Exception

        End Try
        Return False
    End Function

    Private Sub Button20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button20.Click
        With vMSG.TextBox1
            .Clear()
            .AppendText("功能简介：" + vbCrLf)
            .AppendText("在一段时间不操作电脑后（挂机），Win10会触发自动维护，潜在诱发蓝屏" + vbCrLf)
            .AppendText("关闭自动维护，会少许降低系统流畅度，但是可以缓解因为自动维护带来的蓝屏/卡死困扰" + vbCrLf + vbCrLf)
            .AppendText("禁用自动维护服务的注册表地址[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance]，键[MaintenanceDisabled]，值[DWORD:1]" + vbCrLf)
            .AppendText("-------------------" + vbCrLf)
            .AppendText("当前状态状态:" + vbCrLf)
            If Get_MaintenanceDisabled_Status() = False Then
                .AppendText("未配置(默认启用)")
                vMSG.Mode = "maintenance_disabled"
            Else
                .AppendText("禁用")
                vMSG.Mode = "maintenance_enabled"
            End If
        End With
        vMSG.Show()
    End Sub

    Private Sub 更新链接ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 更新链接ToolStripMenuItem.Click
        Dim ms = New mt
        ms.isMessage = False
        ms.Check_for_Update()
        If ms.updURL = "" Then
            MsgBox("获取更新地址失败")
        Else
            Diagnostics.Process.Start(ms.updURL)
        End If
    End Sub

    Private Sub Button21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button21.Click
        'Set_File_Security(GamePath.Text, True, Nothing)
        'Exit Sub
        Try
            Dim tpf = IO.Path.GetTempPath
            tpf += Application.ProductName + "说明文档.txt"
            IO.File.Delete(tpf)
            IO.File.WriteAllText(tpf, My.Resources.说明文档)
            Diagnostics.Process.Start(tpf)

        Catch ex As Exception
            vMSG.Mode = ""
            vMSG.TextBox1.Text = My.Resources.说明文档
            vMSG.TextBox1.Select(0, 0)
            vMSG.Show()
        End Try



    End Sub

    Private Sub 检测更新ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 检测更新ToolStripMenuItem.Click
        Dim nc As New mt
        nc.isMessage = True
        Dim nt As New Threading.Thread(AddressOf nc.Check_for_Update)
        nt.Start()
    End Sub

    Private Sub 禁用更新ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 禁用更新ToolStripMenuItem.Click
        If 禁用更新ToolStripMenuItem.Checked Then
            IO.File.Create(Path_Info + "\noUpdate").Close()
        Else
            IO.File.Delete(Path_Info + "\noUpdate")
        End If
        禁用更新ToolStripMenuItem.Checked = Not 禁用更新ToolStripMenuItem.Checked
    End Sub
End Class
