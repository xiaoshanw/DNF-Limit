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
        Dim vSelectedRows As Integer
        If Exl.SelectedRows IsNot Nothing Then
            If Exl.SelectedRows.Count > 0 Then
                vSelectedRows = Exl.SelectedRows(0).Index
            End If
        End If
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

        If Exl.SelectedRows IsNot Nothing Then
            If Exl.SelectedRows.Count > 0 Then
                If vSelectedRows < Exl.Rows.Count Then Exl.Rows(vSelectedRows).Selected = True
            End If
        End If

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

                    If Set_IFEO(Exl.Rows(e.RowIndex).Cells(1).Value.ToString, vEnabled, VCD_Path, MyException) Then
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
                IFEO模式ToolStripMenuItem.Checked = False
                文件读写模式ToolStripMenuItem.Checked = False
                禁用ToolStripMenuItem.Checked = False
                If vData(Exl.SelectedRows(0).Cells(0).Value).Name.ToLower.Trim.EndsWith(".exe") Then
                    IFEO模式ToolStripMenuItem.Enabled = True
                Else
                    IFEO模式ToolStripMenuItem.Enabled = False
                End If
                   
                Select Case vData(Exl.SelectedRows(0).Cells(0).Value).Value
                    Case 0
                        禁用ToolStripMenuItem.Checked = True
                    Case 1
                        文件读写模式ToolStripMenuItem.Checked = True
                    Case 2
                        IFEO模式ToolStripMenuItem.Checked = True
                End Select

            Catch ex As Exception

            End Try
        End If
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

    Private Sub vList_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim DataGridView_Type As Type = Exl.GetType
            Dim pi As Reflection.PropertyInfo = DataGridView_Type.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.NonPublic)
            pi.SetValue(Exl, True, Nothing)
            PAppend("开启表格双缓冲")
        Catch ex As Exception
            PAppend("开启表格双缓冲错误[" + ex.Message + "]")
        End Try
    End Sub

    Private Sub IFEO模式ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IFEO模式ToolStripMenuItem.Click
        Try
            vData(Exl.SelectedRows(0).Cells(0).Value).Value = 2
            PAppend("设置插件[" + vData(Exl.SelectedRows(0).Cells(0).Value).Name + "]默认[IFEO模式]")
            Save_Data()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub 文件读写模式ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 文件读写模式ToolStripMenuItem.Click
        Try
            vData(Exl.SelectedRows(0).Cells(0).Value).Value = 1
            PAppend("设置插件[" + vData(Exl.SelectedRows(0).Cells(0).Value).Name + "]默认[文件禁用模式]")
            Save_Data()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub 禁用ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 禁用ToolStripMenuItem.Click
        Try
            vData(Exl.SelectedRows(0).Cells(0).Value).Value = 0
            PAppend("设置插件[" + vData(Exl.SelectedRows(0).Cells(0).Value).Name + "]默认[禁用]")
            Save_Data()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Exl_Sorted(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Exl.Sorted
        Dim nData = New ArrayList
        For i = 0 To Exl.Rows.Count - 1
            For Each vline In vData
                If vline.Path = Exl.Rows(i).Cells(8).Value Then
                    nData.Add(vline)
                End If
            Next
        Next
        vData = nData.ToArray(GetType(My_Data_Type))
    End Sub
End Class