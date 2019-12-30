Public Class vMSG
    Public vIntA, vIntB As Integer
    Public Mode As String
    Public arg As String
    Private Sub vMSG_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Button1.Enabled = True
        Select Case Mode
            Case ""
                Button1.Text = "OK"
            Case "cpu_patch_disable"
                Button1.Text = "禁用Intel CPU 幽灵与熔断补丁"
            Case "cpu_patch_enable"
                Button1.Text = "恢复Intel CPU 幽灵与熔断补丁"
            Case "chkdsk"
                Button1.Text = "执行chkdsk磁盘检查"
            Case "del_patch"
                Button1.Text = "删除DNF更新残留安装包"
            Case "del_3rd"
                Button1.Text = "删除自动下载的可执行组件(TX管家等)"
            Case "tguard"
                Button1.Text = "停止并禁用TGuardSvc服务"
            Case "background"
                Button1.Text = "进入后台模式"
            Case "delcfg"
                Button1.Text = "删除配置文件"
            Case "getpremission"
                Button1.Text = "获取权限"
            Case "del_video_rep"
                Button1.Text = "删除rep录像文件"
            Case "maintenance_disabled"
                Button1.Text = "禁用自动维护"
            Case "maintenance_enabled"
                Button1.Text = "恢复自动维护"
            Case Else
                Button1.Text = "OK"
        End Select
    End Sub
    Public Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Select Case Button1.Text
            Case "删除rep录像文件"
                TextBox1.AppendText("--------------------" + vbCrLf)
                Dim vLen As Long
                Button1.Enabled = False
                For Each sFile As String In Public_ArrayList
                    Application.DoEvents()
                    If sFile.ToString.ToLower Like (Main.GamePath.Text + "\video_*.rep").Replace("\\", "\").ToLower Then
                        Try
                            vLen = New IO.FileInfo(sFile).Length
                            IO.File.Delete(sFile)
                            If IO.File.Exists(sFile) Then
                                TextBox1.AppendText("[失败]" + vbTab + "[" + Format(vLen / 1024 / 1024, "#.##") + "MB]" + vbTab + IO.Path.GetFileName(sFile) + vbCrLf)
                            Else
                                TextBox1.AppendText("[成功]" + vbTab + "[" + Format(vLen / 1024 / 1024, "#.##") + "MB]" + vbTab + IO.Path.GetFileName(sFile) + vbCrLf)
                            End If
                        Catch ex As Exception
                            TextBox1.AppendText("[失败]" + vbTab + "[" + Format(vLen / 1024 / 1024, "#.##") + "MB]" + vbTab + IO.Path.GetFileName(sFile) + vbCrLf)
                        End Try
                    End If
                Next
                
                Button1.Enabled = True
                Button1.Text = "OK"
            Case "获取权限"
                Button1.Enabled = False
                TextBox1.AppendText("--------------------" + vbCrLf)
                TextBox1.AppendText("准备中" + vbCrLf)
                vIntB = Get_Files_Count(Main.GamePath.Text)
                vIntA = 0
                TextBox1.WordWrap = False
                Get_Files_Premission(Main.GamePath.Text)
                TextBox1.WordWrap = True
                MsgBox("获取权限完毕")
                Me.Close()
            Case "删除配置文件"
                Try
                    IO.File.Delete(arg)
                    MsgBox("删除成功")
                Catch ex As Exception
                    MsgBox("删除失败[" + ex.Message + "]")
                End Try
                Me.Close()
            Case "OK"
                Me.Close()
            Case "禁用Intel CPU 幽灵与熔断补丁"
                If MsgBox("当前补丁状态为[未配置(默认)]，是否禁用该补丁", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    Set_CPU_Meltdown_Spectre(False)
                    MsgBox("设置成功，请重启电脑以生效")
                    Me.Close()
                End If
            Case "恢复Intel CPU 幽灵与熔断补丁"
                If MsgBox("当前补丁状态为[禁用]，是否恢复该补丁为默认值", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    Set_CPU_Meltdown_Spectre(True)
                    MsgBox("设置成功，请重启电脑以生效")
                    Me.Close()
                End If
            Case "执行chkdsk磁盘检查"
                Try
                    Shell("cmd /c chkdsk /f " + IO.Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System)).Replace("\", ""), AppWinStyle.NormalFocus, True)
                    Shell("cmd /c chkdsk /f " + IO.Path.GetPathRoot(Main.GamePath.Text).Replace("\", ""), AppWinStyle.NormalFocus, True)
                Catch ex As Exception

                End Try
                MsgBox("请重新启动计算机并执行磁盘检查(请勿跳过，否则无效)")
                Me.Close()
            Case "删除DNF更新残留安装包"
                TextBox1.AppendText("--------------------" + vbCrLf)
                Dim vLen As Long
                Button1.Enabled = False
                For Each sFile As String In Public_ArrayList
                    Application.DoEvents()
                    Try
                        vLen = New IO.FileInfo(sFile).Length
                        IO.File.Delete(sFile)
                        If IO.File.Exists(sFile) Then
                            TextBox1.AppendText("[失败]" + vbTab + "[" + Format(vLen / 1024 / 1024, "#.##") + "MB]" + vbTab + IO.Path.GetFileName(sFile) + vbCrLf)
                        Else
                            TextBox1.AppendText("[成功]" + vbTab + "[" + Format(vLen / 1024 / 1024, "#.##") + "MB]" + vbTab + IO.Path.GetFileName(sFile) + vbCrLf)
                        End If

                    Catch ex As Exception

                    End Try
                Next
                Try
                    IO.Directory.Delete(Main.GamePath.Text + "\tgppatches", True)
                    If IO.Directory.Exists(Main.GamePath.Text + "\tgppatches") Then
                        TextBox1.AppendText("[失败]" + vbTab + "\tgppatches")
                    Else
                        TextBox1.AppendText("[成功]" + vbTab + "\tgppatches")
                    End If
                Catch ex As Exception

                End Try
                Button1.Enabled = True
                Button1.Text = "OK"
            Case "删除自动下载的可执行组件(TX管家等)"
                Button1.Enabled = False
                TextBox1.AppendText("--------------------" + vbCrLf)
                For Each vString As String In {"\components", "\TGuard", "\TP_Temp"}
                    Try

                        If Del_Tree(Main.GamePath.Text + vString) Then
                            TextBox1.AppendText("[成功]" + vbTab + vString + vbCrLf)
                        Else
                            TextBox1.AppendText("[失败]" + vbTab + vString + vbTab + "请关闭所有相关软件或重新启动计算机后再尝试" + vbCrLf)
                        End If
                    Catch ex As Exception

                    End Try
                Next
                Button1.Enabled = True
                Button1.Text = "OK"
            Case "停止并禁用TGuardSvc服务"
                Button1.Enabled = False
                Disable_TGuardSvc_All()
                Button1.Enabled = True
                Button1.Text = "OK"
            Case "进入后台模式"
                With Main
                    .NotifyIcon1.Visible = True
                    ShowBalloonTipEx(.NotifyIcon1, 2000, "开启后台模式", "将自动检测[启动器/启动插件]" + vbCrLf + "并在游戏成功运行后自动关闭[启动器/启动插件]", ToolTipIcon.Info)
                    'TGuard_Tried_Sum = 0
                    .Visible = False
                    Auto_Kill_Gameloader_Flag = 0
                    .AutoKill_GameLoader.Start()
                    Me.Close()
                End With
            Case "禁用自动维护"
                If MsgBox("当前状态为[未配置](默认启用)，是否禁用自动维护功能", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    If Set_MaintenanceDisabled_Status(True) Then
                        MsgBox("设置成功，请重启电脑以生效")
                        Me.Close()
                    End If
                End If
            Case "恢复自动维护"
                If MsgBox("当前状态为[禁用]，是否恢复自动维护功能", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    If Set_MaintenanceDisabled_Status(False) Then
                        MsgBox("设置成功，请重启电脑以生效")
                        Me.Close()
                    End If
                End If
        End Select
    End Sub
    Public Sub Get_Files_Premission(ByVal Path As String)
        Dim myEx = New Exception
        For Each vline In IO.Directory.GetFiles(Path)
            Try
                vIntA += 1
                TextBox1.AppendText("[" + Format(Math.Min(vIntA / vIntB, 1) * 100, "0.#") + "% " + vIntA.ToString + "/" + vIntB.ToString + "]")
                TextBox1.AppendText(IO.Path.GetFileName(vline))
                Set_File_Security(vline, True, myEx)
                TextBox1.AppendText("[OK]" + vbCrLf)
            Catch ex As Exception
                TextBox1.AppendText("[BAD]" + vbCrLf)
            End Try
            Application.DoEvents()
        Next
        For Each vline In IO.Directory.GetDirectories(Path)
            Set_File_Security(vline, True, myEx)
            Get_Files_Premission(vline)
        Next
    End Sub

End Class