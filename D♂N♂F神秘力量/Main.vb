﻿Public Class Main

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim tmp As FolderBrowserDialog = New FolderBrowserDialog
        If tmp.ShowDialog = Windows.Forms.DialogResult.OK Then GamePath.Text = tmp.SelectedPath
    End Sub

    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Kill_Process()
        GamePath.Text = Find_DNF_Path()
        Try
            vData = Get_Currend_File_String()
        Catch ex As Exception

        End Try
        If vData Is Nothing Then
            Dim vstring() As String = My.Resources.list.Split(vbCrLf)
            Dim vstring2() As String
            ReDim vData(vstring.Length - 1)
            For i = 0 To vstring.Length - 1
                Try
                    vstring2 = vstring(i).Split("|")
                    vData(i).Name = vstring2(1)
                    vData(i).Path = vstring2(3)
                    vData(i).Info = vstring2(4)
                    vData(i).Value = CBool(vstring2(2))
                Catch ex As Exception

                End Try
            Next
            Try
                IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\" + Application.ProductName + "\list.ini", My.Resources.list, System.Text.Encoding.Unicode)
            Catch ex As Exception

            End Try
        End If
        If vData.Length < 3 Then vData = Get_Currend_File_String(True)
        CanIFEO = Check_IFEO_Permission()
        'If CanIFEO = False Then MsgBox("权限不足，将采用[文件读写]模式" + vbCrLf + "[更新游戏]需要[恢复]！！！" + vbCrLf + "详情参阅帮助")
        Set_Application_Title()

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
            If sData.Value = False Then Continue For
            PPrint("执行禁用[" + sData.Name + "]")
            If CanIFEO Then
                If Set_IFEO(sData.Name, False, VoCytDefenderEx_Path, MyException) Then
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
        Next
        vList.Check_Status(False)
        PAppend("执行结束")
        If Bad = True Then MsgBox("部分插件采用[文件读写]模式禁用，更新游戏建议还原插件，详情参阅[帮助]")
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
                b2 = Set_IFEO(sData.Name, True, VoCytDefenderEx_Path, Ex2)
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
End Class
