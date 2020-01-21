Module disabketguardsvc
    Dim myError As Exception
    Public Function Stop_TGuardSvc_Command(Optional ByVal ServicesName As String = "TGuardSvc") As Boolean
        Try
            Shell("cmd.exe /c sc stop " + ServicesName, AppWinStyle.Hide)
            Return True
        Catch ex As Exception
            myError = ex
            Return False
        End Try

    End Function
    Public Function Disable_TGuardSvc_Command(Optional ByVal ServicesName As String = "TGuardSvc") As Boolean
        Try
            Shell("cmd.exe /c sc config " + ServicesName + " start= disabled", AppWinStyle.Hide)
            Return True
        Catch ex As Exception
            myError = ex
            Return False
        End Try
    End Function
    Public Function Stop_TGuardSvc_Net(ByVal ServicesName As ServiceProcess.ServiceController) As Boolean
        Try
            ServicesName.Stop()
            Return True
        Catch ex As Exception
            myError = ex
            Return False
        End Try
    End Function
    Public Function Disable_TGuardSvc_Net(ByVal ServicesName As ServiceProcess.ServiceController) As Boolean
        Try
            Dim regist As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine
            Dim svcreg As Microsoft.Win32.RegistryKey = regist.OpenSubKey("SYSTEM\CurrentControlSet\Services\" + ServicesName.DisplayName, True)
            svcreg.SetValue("Start", 4, Microsoft.Win32.RegistryValueKind.DWord)
            svcreg.Flush()
            Return True
        Catch ex As Exception
            myError = ex
            Return False
        End Try
    End Function
    Public Sub Kill_TGuardSvc()
        Dim vProcess() As Process = Process.GetProcesses
        For Each vline In vProcess
            Select Case vline.ProcessName.ToLower
                Case "tguard", "tguardsvc"
                    Try
                        If vline.MainModule.FileName.ToLower.Contains(TGuardSvc_Path.ToLower) Then vline.Kill()
                    Catch ex As Exception
                        myError = ex
                    End Try

            End Select
        Next
    End Sub
    Public Sub Disable_TGuardSvc_All(Optional ByVal isPrint As Boolean = True)
        Dim svc() = ServiceProcess.ServiceController.GetServices
        Dim vline As ServiceProcess.ServiceController = Nothing
        For i = 0 To svc.Length - 1
            If svc(i).DisplayName = "TGuardSvc" Then
                vline = svc(i)
            End If
        Next
        If vline Is Nothing Then
            If isPrint Then vMSG.TextBox1.AppendText("服务状态：未找到" + vbCrLf + "设置禁用：")
            If Disable_TGuardSvc_Command() = True Then
                If isPrint Then vMSG.TextBox1.AppendText("成功")
            Else
                If isPrint Then vMSG.TextBox1.AppendText("失败[" + myError.Message + "]")
            End If

        Else
            If vline.Status = ServiceProcess.ServiceControllerStatus.Stopped Then
                If isPrint Then vMSG.TextBox1.AppendText("服务状态：已停止" + vbCrLf)
            Else
                If isPrint Then vMSG.TextBox1.AppendText("服务状态：运行中" + vbCrLf)
                If Stop_TGuardSvc_Net(vline) And Stop_TGuardSvc_Command() Then
                    If isPrint Then vMSG.TextBox1.AppendText("停止成功" + vbCrLf)
                Else
                    If isPrint Then vMSG.TextBox1.AppendText("失败[" + myError.Message + "]" + vbCrLf)
                End If
            End If
            If Disable_TGuardSvc_Net(vline) And Disable_TGuardSvc_Command() Then
                If isPrint Then vMSG.TextBox1.AppendText("设置禁用：成功")
            Else
                If isPrint Then vMSG.TextBox1.AppendText("失败[" + myError.Message + "]" + vbCrLf)
            End If

        End If


    End Sub

End Module
