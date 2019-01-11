<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class vList
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(vList))
        Me.Exl = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.设置ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.添加组件ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.删除组件ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Log = New System.Windows.Forms.TextBox()
        Me.ID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MyName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Status = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ReadWrite = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.IFEO = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Info = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ReadWrite_Action = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.IFEO_Action = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.File_Path = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.Exl, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Exl
        '
        Me.Exl.AllowUserToAddRows = False
        Me.Exl.AllowUserToDeleteRows = False
        Me.Exl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Exl.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Exl.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ID, Me.MyName, Me.Status, Me.ReadWrite, Me.IFEO, Me.Info, Me.ReadWrite_Action, Me.IFEO_Action, Me.File_Path})
        Me.Exl.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Exl.Location = New System.Drawing.Point(12, 12)
        Me.Exl.MultiSelect = False
        Me.Exl.Name = "Exl"
        Me.Exl.RowHeadersVisible = False
        Me.Exl.RowTemplate.Height = 23
        Me.Exl.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.Exl.Size = New System.Drawing.Size(568, 249)
        Me.Exl.TabIndex = 0
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.设置ToolStripMenuItem, Me.添加组件ToolStripMenuItem, Me.删除组件ToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(119, 70)
        '
        '设置ToolStripMenuItem
        '
        Me.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem"
        Me.设置ToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.设置ToolStripMenuItem.Text = "设置"
        '
        '添加组件ToolStripMenuItem
        '
        Me.添加组件ToolStripMenuItem.Name = "添加组件ToolStripMenuItem"
        Me.添加组件ToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.添加组件ToolStripMenuItem.Text = "添加组件"
        '
        '删除组件ToolStripMenuItem
        '
        Me.删除组件ToolStripMenuItem.Name = "删除组件ToolStripMenuItem"
        Me.删除组件ToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.删除组件ToolStripMenuItem.Text = "删除组件"
        '
        'Log
        '
        Me.Log.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Log.Location = New System.Drawing.Point(12, 267)
        Me.Log.Multiline = True
        Me.Log.Name = "Log"
        Me.Log.ReadOnly = True
        Me.Log.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.Log.Size = New System.Drawing.Size(568, 94)
        Me.Log.TabIndex = 1
        Me.Log.WordWrap = False
        '
        'ID
        '
        Me.ID.HeaderText = "ID"
        Me.ID.Name = "ID"
        Me.ID.ReadOnly = True
        Me.ID.Visible = False
        '
        'MyName
        '
        Me.MyName.HeaderText = "组件名"
        Me.MyName.Name = "MyName"
        Me.MyName.ReadOnly = True
        '
        'Status
        '
        Me.Status.HeaderText = "组件状态"
        Me.Status.Name = "Status"
        Me.Status.ReadOnly = True
        '
        'ReadWrite
        '
        Me.ReadWrite.HeaderText = "文件读写"
        Me.ReadWrite.Name = "ReadWrite"
        Me.ReadWrite.ReadOnly = True
        '
        'IFEO
        '
        Me.IFEO.HeaderText = "IFEO"
        Me.IFEO.Name = "IFEO"
        Me.IFEO.ReadOnly = True
        '
        'Info
        '
        Me.Info.HeaderText = "组件描述"
        Me.Info.Name = "Info"
        Me.Info.ReadOnly = True
        '
        'ReadWrite_Action
        '
        Me.ReadWrite_Action.HeaderText = "读写禁用/恢复"
        Me.ReadWrite_Action.Name = "ReadWrite_Action"
        Me.ReadWrite_Action.ReadOnly = True
        '
        'IFEO_Action
        '
        Me.IFEO_Action.HeaderText = "IFEO禁用/恢复"
        Me.IFEO_Action.Name = "IFEO_Action"
        Me.IFEO_Action.ReadOnly = True
        '
        'File_Path
        '
        Me.File_Path.HeaderText = "组件路径"
        Me.File_Path.Name = "File_Path"
        Me.File_Path.ReadOnly = True
        '
        'vList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(592, 373)
        Me.Controls.Add(Me.Log)
        Me.Controls.Add(Me.Exl)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "vList"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "组件清单"
        CType(Me.Exl, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Exl As System.Windows.Forms.DataGridView
    Friend WithEvents Log As System.Windows.Forms.TextBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents 设置ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 添加组件ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 删除组件ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MyName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Status As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ReadWrite As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents IFEO As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Info As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ReadWrite_Action As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents IFEO_Action As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents File_Path As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
