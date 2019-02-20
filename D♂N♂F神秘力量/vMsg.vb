Public Class vMSG
    Public Mode As String
    Private Sub vMSG_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Select Case Mode
            Case ""
                Button1.Text = "OK"
            Case "cpu_patch"
                If Get_CPU_Meltdown_Spectre() Then
                    Button1.Text = "禁用Intel CPU 幽灵与熔断补丁"
                Else
                    Button1.Text = "恢复Intel CPU 幽灵与熔断补丁"
                End If
            Case "chkdsk"
                Button1.Text = "执行chkdsk磁盘检查"
            Case "del_patch"
                Button1.Text = "删除DNF更新残留安装包"
            Case "del_3rd"
                Button1.Text = "删除自动下载的可执行组件(TX管家等)"
            Case "tguard"
                Button1.Text = "停止并禁用DNFTGuardSvc服务"
        End Select
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Select Case Button1.Text
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
                For Each sFile As String In Public_ArrayList
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
                Button1.Text = "OK"
            Case "删除自动下载的可执行组件(TX管家等)"
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
                Button1.Text = "OK"
            Case "停止并禁用DNFTGuardSvc服务"
                Dim svc() As ServiceProcess.ServiceController = ServiceProcess.ServiceController.GetServices
                For Each vline As ServiceProcess.ServiceController In svc
                    If vline.DisplayName = "TGuardSvc" Then
                        TextBox1.AppendText("服务状态：")
                        If vline.Status <> ServiceProcess.ServiceControllerStatus.Stopped Then
                            TextBox1.AppendText("正在运行" + vbCrLf)
                            TextBox1.AppendText("尝试停止TGuardSvc服务")
                            Try
                                vline.Stop()
                                TextBox1.AppendText("[成功]" + vbCrLf)

                            Catch ex As Exception
                                TextBox1.AppendText("[失败][" + ex.Message + "]" + vbCrLf)
                            End Try
                        Else
                            TextBox1.AppendText("已停止" + vbCrLf)
                        End If
                        Try
                            TextBox1.AppendText("尝试禁用TGuardSvc服务")

                            Dim regist As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine
                            Dim svcreg As Microsoft.Win32.RegistryKey = regist.OpenSubKey("SYSTEM\CurrentControlSet\Services\TGuardSvc", True)
                            svcreg.SetValue("Start", 4, Microsoft.Win32.RegistryValueKind.DWord)

                            TextBox1.AppendText("[成功]" + vbCrLf)
                            Button1.Text = "OK"
                        Catch ex As Exception
                            TextBox1.AppendText("[失败][" + ex.Message + "]" + vbCrLf)
                        End Try
                    End If
                Next
        End Select
    End Sub
End Class