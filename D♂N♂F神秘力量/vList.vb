Public Class vList
    Public Sub Set_Color()
        '2,3,4
        For i = 0 To Exl.Rows.Count - 1
            For j = 2 To 4
                Select Case Exl.Rows(i).Cells(j).Value.ToString
                    Case "禁用"
                        Exl.Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Case Else
                        Exl.Rows(i).Cells(j).Style.ForeColor = Color.Green
                End Select
            Next
        Next
    End Sub
    Public Function Check_Status(Optional ByVal Visible As Boolean = True, Optional ByVal Button_Visible As Boolean = False) As Integer
        Me.Show()
        If Visible Then PClear()
        If Visible Then PAppend("正在检测组件状态")
        Exl.Columns(6).Visible = Button_Visible
        Exl.Columns(7).Visible = Button_Visible
        Exl.Rows.Clear()
        If Visible Then PAppend("读取组件清单，共有[" + vData.Length.ToString + "]个")
        For i = 0 To vData.Length - 1
            If IO.File.Exists(Main.GamePath.Text + vData(i).Path) Then
                If Visible Then PAppend("检测组件[" + vData(i).Name + "]")
                Main.Check_And_Add(i, vData(i), Exl)
            End If
        Next
        If Visible Then PAppend("调整列宽")
        Exl.AutoResizeColumns()
        If Visible Then PAppend("优化可视化界面")
        Set_Color()
        If Visible Then PAppend("检查结束")

        Return Exl.Rows.Count
    End Function

    Private Sub Exl_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Exl.CellContentClick
        '6,7
        Dim vEnabled As Boolean
        Dim MyException As Exception = New Exception
        Select Case e.ColumnIndex
            Case 6, 7
                If Exl.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = "不支持" Then Exit Sub
                If Exl.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString = "禁用" Then
                    vEnabled = False
                    PPrint("执行禁用[" + Exl.Rows(e.RowIndex).Cells(1).Value.ToString + "]")
                Else
                    vEnabled = True
                    PPrint("执行恢复[" + Exl.Rows(e.RowIndex).Cells(1).Value.ToString + "]")
                End If
                If e.ColumnIndex = 6 Then
                    '6
                    If Set_File_Security(Main.GamePath.Text + "\" + Exl.Rows(e.RowIndex).Cells(8).Value.ToString, vEnabled, MyException) Then
                        PAppend("[成功]")
                    Else
                        PAppend("[失败][" + MyException.Message + "]")
                    End If
                Else
                    '7

                    If Set_IFEO(Exl.Rows(e.RowIndex).Cells(1).Value.ToString, vEnabled, VoCytDefenderEx_Path, MyException) Then
                        PAppend("[成功]")
                    Else
                        PAppend("[失败][" + MyException.Message + "]")
                    End If
                End If
                Check_Status(False, True)
            Case Else
                Exit Sub
        End Select
    End Sub

    Public Sub Set_Window(ByVal X As Integer, ByVal Y As Integer, ByVal Right_Click As Boolean)
        Me.Size = New Drawing.Size(X, Y)
        If Right_Click Then
            Exl.ContextMenuStrip = ContextMenuStrip1
        Else
            Exl.ContextMenuStrip = Nothing
        End If

    End Sub

  
    Private Sub Exl_CellMouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles Exl.CellMouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Exl.Rows(e.RowIndex).Selected = True
            Try
                If vData(Exl.SelectedRows(0).Cells(0).Value).Value = False Then
                    ContextMenuStrip1.Items(0).Text = "设置默认[禁用]"
                Else
                    ContextMenuStrip1.Items(0).Text = "设置默认[不禁用]"
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub 设置ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 设置ToolStripMenuItem.Click
        Try
            vData(Exl.SelectedRows(0).Cells(0).Value).Value = Not vData(Exl.SelectedRows(0).Cells(0).Value).Value
            PPrint("设置插件[" + vData(Exl.SelectedRows(0).Cells(0).Value).Name + "]默认")
            If vData(Exl.SelectedRows(0).Cells(0).Value).Value Then PAppend("[禁用]") Else PAppend("[不禁用]")
            Save_Data()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub 添加组件ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 添加组件ToolStripMenuItem.Click
        Dim fd As OpenFileDialog = New OpenFileDialog
        fd.InitialDirectory = Main.GamePath.Text
        fd.Title = "选择文件"
        If fd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If fd.FileName.ToLower.StartsWith(Main.GamePath.Text.ToLower) Then
                Dim info As String = ""
                While info = ""
                    info = InputBox("输入对" + fd.SafeFileName + "组件的描述")
                End While
                ReDim Preserve vData(vData.Length)
                With vData(vData.Length - 1)
                    .Name = fd.SafeFileName
                    .Path = ("\" + Mid(fd.FileName, Main.GamePath.Text.Length + 1, fd.FileName.Length)).Replace("\\", "\")
                    .Value = True
                    .Info = info
                    Check_Status(False, True)
                    PAppend("添加组件[" + .Name + "]")
                    Save_Data()
                End With

            Else
                MsgBox("游戏路径:" + Main.GamePath.Text + vbCrLf + "组件路径:" + fd.FileName + vbCrLf + "不允许选择非游戏目录下组件")
            End If
        End If

    End Sub

    Private Sub 删除组件ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 删除组件ToolStripMenuItem.Click
        If Exl.Rows.Count <= 3 Then
            MsgBox("原则上清单不得低于3个组件")
            Exit Sub
        End If
        Dim fData As ArrayList = New ArrayList
        For Each sData As My_Data_Type In vData
            fData.Add(sData)
        Next
        PAppend("删除组件[" + Exl.SelectedRows(0).Cells(1).Value.ToString + "]")
        fData.RemoveAt(Exl.SelectedRows(0).Cells(0).Value)
        ReDim vData(vData.Length - 2)
        For i = 0 To vData.Length - 1
            vData(i) = fData(i)
        Next
        Check_Status(False, True)

        Save_Data()
    End Sub
End Class