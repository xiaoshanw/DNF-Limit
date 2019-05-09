Public Class Main

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim tmp As FolderBrowserDialog = New FolderBrowserDialog
        If tmp.ShowDialog = Windows.Forms.DialogResult.OK Then GamePath.Text = tmp.SelectedPath
    End Sub

    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim nc As New mt
        Dim nt As New Threading.Thread(AddressOf nc.Check_for_Update)
        nt.Start()


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
        If IO.File.Exists(INI_Path) Then
            vData = String_to_Data(IO.File.ReadAllText(INI_Path))
        Else
            vData = String_to_Data("")
        End If

        CanIFEO = Check_IFEO_Permission()
        'If CanIFEO = False Then MsgBox("权限不足，将采用[文件读写]模式" + vbCrLf + "[更新游戏]需要[恢复]！！！" + vbCrLf + "详情参阅帮助")
        Set_Application_Title()

        Select Case My.Application.CommandLineArgs.Count
            Case 1
                If My.Application.CommandLineArgs(0).ToLower.Trim = "-b" Then
                    NotifyIcon1.Visible = True
                    NotifyIcon1.ShowBalloonTip(2000, "开启后台模式", "将自动检测[启动器/启动插件]" + vbCrLf + "并在游戏成功运行后自动关闭[启动器/启动插件]", ToolTipIcon.Info)
                    Me.Visible = False
                    'Me.Hide()
                    Hide_Run = True
                    Auto_Kill_Gameloader_Flag = 0
                    AutoKill_GameLoader.Start()
                End If
        End Select



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
        vMSG.Button1.Text = "停止并禁用TGuardSvc服务"
        vMSG.Button1_Click(Me, e)
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
        vMSG.Mode = ""
        vMSG.TextBox1.Text = My.Resources.help
        vMSG.TextBox1.Select(0, 0)
        vMSG.Show()
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
            Else
                .AppendText("禁用")
            End If
        End With
        vMSG.Mode = "cpu_patch"
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

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        With vMSG.TextBox1
            .Clear()
            .AppendText("Win10用户频繁蓝屏，且蓝屏代码基本为下列两种：" + vbCrLf)
            .AppendText("IRQL_NOT_LESS_OR_EQUAL" + vbCrLf)
            .AppendText("DRIVER_IRQL_NOT_LESS_OR_EQUAL" + vbCrLf)
            .AppendText("多为windows内核ntoskrnl.exe引发的蓝屏，ntoskrnl.exe为NT架构核心调度文件" + vbCrLf)
            .AppendText("该蓝屏多为TP组件跟Win10内核冲突所致，无需怀疑你的系统，也无需怀疑TX的TP，总而言之是个玄学问题" + vbCrLf)
            .AppendText("目前没有任何一个方法或者手段完全不让这个冲突导致蓝屏(至少这半年我没发现)" + vbCrLf)
            .AppendText("包括更换VSDDrvDll.dll等方法，均有概率诱发蓝屏，表现为TP蓝框读条结束蓝屏，进游戏蓝屏，反复蓝屏" + vbCrLf)
            .AppendText("--------------------" + vbCrLf)
            .AppendText("解决方案：" + vbCrLf)
            .AppendText("更新Win10 1809正式版" + vbCrLf)
            .AppendText("更新Win10 1809正式版后，根据基友反馈，蓝屏概率大幅降低，应该与Win10内核审核权限有关" + vbCrLf)
            .AppendText("偶尔蓝屏，重启计算机再登陆即恢复正常，而1803及之前的版本表现为反复蓝屏" + vbCrLf)
            .AppendText("如果您不愿意更新，那么抱歉，游戏跟Win10选一个，或者与蓝为伴" + vbCrLf)
            .AppendText("如果您收到了Win10 1809的推送，请尽快更新" + vbCrLf)
            .AppendText("如果您未收到了Win10 1809的推送，可以按照如下方法开启预览更新" + vbCrLf)
            .AppendText("--------------------" + vbCrLf)
            .AppendText("Win10 1809预览更新：" + vbCrLf)
            .AppendText("设置-更新和安全-Windows 预览体验计划" + vbCrLf)
            .AppendText("获取Insider Preview内部版本-开始-选择账户-链接账户-登陆Microsoft账户" + vbCrLf)
            .AppendText("希望接受哪类内容-跳到下一个Windows版本" + vbCrLf)
            .AppendText("等待更新或手动更新" + vbCrLf)
            .AppendText("--------------------" + vbCrLf)
            .AppendText("注意，各个版本Win10打开预览更新的流程可能不相同，且我的Microsoft账户是开发者账户，所以流程上可能有细微差别")
        End With
        vMSG.Mode = ""
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
            .AppendText("TenioDL.exe" + vbCrLf)
            .AppendText("GameLoader.exe" + vbCrLf)
            .AppendText("TesService.exe" + vbCrLf)
            .AppendText("--------------------" + vbCrLf)
            .AppendText("如需运行软件自动进入后台模式，可在运行本程序时，添加 -b参数，如：" + vbCrLf)
            .AppendText("D♂N♂F神秘力量.exe -b" + vbCrLf)
        End With
        vMSG.Mode = "background"
        vMSG.Show()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoKill_GameLoader.Tick

        Try
            If Process_dnf IsNot Nothing Then
                If Process_dnf.HasExited = False Then Exit Sub
            End If
        Catch ex As Exception
        End Try
        Dim vProcess() As Process = Process.GetProcesses
        Try
            Select Case Auto_Kill_Gameloader_Flag
                Case 0
                    For Each vline As Process In vProcess
                        Select Case vline.ProcessName.ToLower
                            Case "teniodl", "gameloader" ', "tesservice"
                                NotifyIcon1.ShowBalloonTip(2000, "提示", "检测到" + vline.ProcessName + "启动", ToolTipIcon.Info)
                                Auto_Kill_Gameloader_Flag = 1
                                AutoKill_GameLoader.Interval = 1000
                        End Select
                    Next
                Case 1
                    For Each vline As Process In vProcess
                        Select Case vline.ProcessName.ToLower
                            Case "dnf"
                                NotifyIcon1.ShowBalloonTip(2000, "提示", "检测到" + vline.ProcessName + "启动" + vbCrLf + "等待游戏载入", ToolTipIcon.Info)
                                Auto_Kill_Gameloader_Flag = 2
                        End Select
                    Next
                Case 2
                    For Each vline As Process In vProcess
                        Select Case vline.ProcessName.ToLower
                            Case "dnf"
                                'vline.Start()
                                If vline.MainWindowTitle = "地下城与勇士" And DateDiff("s", vline.StartTime, Now) > 60 Then
                                    NotifyIcon1.ShowBalloonTip(2000, "提示", vline.ProcessName + "启动成功" + vbCrLf + "将于10秒后结束残余启动器", ToolTipIcon.Info)
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
                                        NotifyIcon1.ShowBalloonTip(2000, "提示", "结束" + vline2.ProcessName + "成功", ToolTipIcon.Info)
                                    Catch ex2 As Exception
                                        'NotifyIcon1.ShowBalloonTip(2000, "警告", "结束" + vline2.ProcessName + "失败" + vbCrLf + ex2.Message, ToolTipIcon.Warning)
                                    End Try
                                End If
                            End If
                        Next
                    Case "tguardsvc", "tguard"
                        vMSG.Disable_TGuardSvc()
                        NotifyIcon1.ShowBalloonTip(2000, "提示", "禁用TGuardSvc服务", ToolTipIcon.Info)
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
            MsgBox("[注册表/默认安装路径/常规路径]均无DNF路径信息，无法检测DNF游戏路径" + +vbCrLf + vbCrLf + "请手动指定DNF游戏路径")
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
End Class
