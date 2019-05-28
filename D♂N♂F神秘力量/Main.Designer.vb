<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Button15 = New System.Windows.Forms.Button()
        Me.Button14 = New System.Windows.Forms.Button()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.GamePath = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Button17 = New System.Windows.Forms.Button()
        Me.Button16 = New System.Windows.Forms.Button()
        Me.Button13 = New System.Windows.Forms.Button()
        Me.Button12 = New System.Windows.Forms.Button()
        Me.Button11 = New System.Windows.Forms.Button()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.Button9 = New System.Windows.Forms.Button()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.还原ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.关闭ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AutoKill_GameLoader = New System.Windows.Forms.Timer(Me.components)
        Me.AutoKill_Kill = New System.Windows.Forms.Timer(Me.components)
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.配置文件ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.打开目录ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.重置配置ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.交流赞助ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.交流群421483534ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.赞助ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.Button15)
        Me.GroupBox1.Controls.Add(Me.Button14)
        Me.GroupBox1.Controls.Add(Me.Button6)
        Me.GroupBox1.Controls.Add(Me.Button5)
        Me.GroupBox1.Controls.Add(Me.Button3)
        Me.GroupBox1.Controls.Add(Me.Button2)
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Controls.Add(Me.Button4)
        Me.GroupBox1.Controls.Add(Me.GamePath)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 27)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(677, 179)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "TX全家桶"
        '
        'Button15
        '
        Me.Button15.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button15.Font = New System.Drawing.Font("宋体", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Button15.ForeColor = System.Drawing.Color.Red
        Me.Button15.Location = New System.Drawing.Point(549, 76)
        Me.Button15.Name = "Button15"
        Me.Button15.Size = New System.Drawing.Size(122, 23)
        Me.Button15.TabIndex = 11
        Me.Button15.Text = "暴力禁用模式"
        Me.ToolTip1.SetToolTip(Me.Button15, "禁用所有组件，相关功能不可用")
        Me.Button15.UseVisualStyleBackColor = True
        '
        'Button14
        '
        Me.Button14.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button14.Location = New System.Drawing.Point(515, 18)
        Me.Button14.Name = "Button14"
        Me.Button14.Size = New System.Drawing.Size(75, 23)
        Me.Button14.TabIndex = 10
        Me.Button14.Text = "自动查找"
        Me.ToolTip1.SetToolTip(Me.Button14, "自动查找游戏路径")
        Me.Button14.UseVisualStyleBackColor = True
        '
        'Button6
        '
        Me.Button6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button6.Location = New System.Drawing.Point(6, 134)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(665, 23)
        Me.Button6.TabIndex = 9
        Me.Button6.Text = "帮助"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button5.Location = New System.Drawing.Point(340, 105)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(331, 23)
        Me.Button5.TabIndex = 8
        Me.Button5.Text = "手动模式"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button3.Font = New System.Drawing.Font("宋体", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Button3.ForeColor = System.Drawing.Color.Green
        Me.Button3.Location = New System.Drawing.Point(6, 105)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(328, 23)
        Me.Button3.TabIndex = 7
        Me.Button3.Text = "一键恢复"
        Me.ToolTip1.SetToolTip(Me.Button3, "恢复清单中所有组件")
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button2.Font = New System.Drawing.Font("宋体", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Button2.ForeColor = System.Drawing.Color.Green
        Me.Button2.Location = New System.Drawing.Point(6, 76)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(537, 23)
        Me.Button2.TabIndex = 6
        Me.Button2.Text = "一键禁用(全家桶组件)"
        Me.ToolTip1.SetToolTip(Me.Button2, "标准禁用，保留连发、换装，适合绝大多数玩家")
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(6, 47)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(665, 23)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "检测组件状态"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button4.Location = New System.Drawing.Point(596, 18)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(75, 23)
        Me.Button4.TabIndex = 5
        Me.Button4.Text = "手动选择"
        Me.ToolTip1.SetToolTip(Me.Button4, "手动选择游戏路径")
        Me.Button4.UseVisualStyleBackColor = True
        '
        'GamePath
        '
        Me.GamePath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GamePath.Location = New System.Drawing.Point(65, 20)
        Me.GamePath.Name = "GamePath"
        Me.GamePath.Size = New System.Drawing.Size(444, 21)
        Me.GamePath.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 12)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "游戏路径"
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.Button17)
        Me.GroupBox2.Controls.Add(Me.Button16)
        Me.GroupBox2.Controls.Add(Me.Button13)
        Me.GroupBox2.Controls.Add(Me.Button12)
        Me.GroupBox2.Controls.Add(Me.Button11)
        Me.GroupBox2.Controls.Add(Me.Button10)
        Me.GroupBox2.Controls.Add(Me.Button9)
        Me.GroupBox2.Controls.Add(Me.Button8)
        Me.GroupBox2.Controls.Add(Me.Button7)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 212)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(677, 201)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "附加功能(绿色常用、红色危险、黑色普通，鼠标停留显示简介)"
        '
        'Button17
        '
        Me.Button17.Font = New System.Drawing.Font("宋体", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Button17.ForeColor = System.Drawing.Color.Green
        Me.Button17.Location = New System.Drawing.Point(6, 78)
        Me.Button17.Name = "Button17"
        Me.Button17.Size = New System.Drawing.Size(328, 23)
        Me.Button17.TabIndex = 8
        Me.Button17.Text = "获取游戏全文件访问权"
        Me.ToolTip1.SetToolTip(Me.Button17, "解决更新失败、文件无法访问的问题")
        Me.Button17.UseVisualStyleBackColor = True
        '
        'Button16
        '
        Me.Button16.Location = New System.Drawing.Point(340, 20)
        Me.Button16.Name = "Button16"
        Me.Button16.Size = New System.Drawing.Size(331, 23)
        Me.Button16.TabIndex = 7
        Me.Button16.Text = "重置游戏配置文件"
        Me.ToolTip1.SetToolTip(Me.Button16, "解决卡频道、黑屏")
        Me.Button16.UseVisualStyleBackColor = True
        '
        'Button13
        '
        Me.Button13.Font = New System.Drawing.Font("宋体", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Button13.ForeColor = System.Drawing.Color.Green
        Me.Button13.Location = New System.Drawing.Point(6, 20)
        Me.Button13.Name = "Button13"
        Me.Button13.Size = New System.Drawing.Size(328, 23)
        Me.Button13.TabIndex = 6
        Me.Button13.Text = "后台模式"
        Me.ToolTip1.SetToolTip(Me.Button13, "自动关闭TenioDL、GameLoader、TesService等残余启动器 ")
        Me.Button13.UseVisualStyleBackColor = True
        '
        'Button12
        '
        Me.Button12.Location = New System.Drawing.Point(339, 49)
        Me.Button12.Name = "Button12"
        Me.Button12.Size = New System.Drawing.Size(332, 23)
        Me.Button12.TabIndex = 5
        Me.Button12.Text = "禁用TGuardSvc服务"
        Me.ToolTip1.SetToolTip(Me.Button12, "解决CPU、磁盘占用")
        Me.Button12.UseVisualStyleBackColor = True
        '
        'Button11
        '
        Me.Button11.Location = New System.Drawing.Point(340, 78)
        Me.Button11.Name = "Button11"
        Me.Button11.Size = New System.Drawing.Size(331, 23)
        Me.Button11.TabIndex = 4
        Me.Button11.Text = "Win10蓝屏解决方案"
        Me.Button11.UseVisualStyleBackColor = True
        '
        'Button10
        '
        Me.Button10.ForeColor = System.Drawing.Color.Red
        Me.Button10.Location = New System.Drawing.Point(6, 136)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(328, 23)
        Me.Button10.TabIndex = 3
        Me.Button10.Text = "删除自动下载的可执行组件(TX管家等)"
        Me.ToolTip1.SetToolTip(Me.Button10, "精简客户端")
        Me.Button10.UseVisualStyleBackColor = True
        '
        'Button9
        '
        Me.Button9.ForeColor = System.Drawing.Color.Red
        Me.Button9.Location = New System.Drawing.Point(6, 165)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(328, 23)
        Me.Button9.TabIndex = 2
        Me.Button9.Text = "删除DNF更新残留安装包"
        Me.ToolTip1.SetToolTip(Me.Button9, "精简客户端")
        Me.Button9.UseVisualStyleBackColor = True
        '
        'Button8
        '
        Me.Button8.Font = New System.Drawing.Font("宋体", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Button8.ForeColor = System.Drawing.Color.Green
        Me.Button8.Location = New System.Drawing.Point(6, 49)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(328, 23)
        Me.Button8.TabIndex = 1
        Me.Button8.Text = "执行chkdsk磁盘检查"
        Me.ToolTip1.SetToolTip(Me.Button8, "解决游戏不定时顿卡")
        Me.Button8.UseVisualStyleBackColor = True
        '
        'Button7
        '
        Me.Button7.ForeColor = System.Drawing.Color.Red
        Me.Button7.Location = New System.Drawing.Point(6, 107)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(328, 23)
        Me.Button7.TabIndex = 0
        Me.Button7.Text = "禁用/恢复Intel CPU 幽灵与熔断补丁"
        Me.ToolTip1.SetToolTip(Me.Button7, "提高帧率")
        Me.Button7.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "D♂N♂F神秘力量"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.还原ToolStripMenuItem, Me.关闭ToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(95, 48)
        '
        '还原ToolStripMenuItem
        '
        Me.还原ToolStripMenuItem.Name = "还原ToolStripMenuItem"
        Me.还原ToolStripMenuItem.Size = New System.Drawing.Size(94, 22)
        Me.还原ToolStripMenuItem.Text = "还原"
        '
        '关闭ToolStripMenuItem
        '
        Me.关闭ToolStripMenuItem.Name = "关闭ToolStripMenuItem"
        Me.关闭ToolStripMenuItem.Size = New System.Drawing.Size(94, 22)
        Me.关闭ToolStripMenuItem.Text = "关闭"
        '
        'AutoKill_GameLoader
        '
        Me.AutoKill_GameLoader.Interval = 5000
        '
        'AutoKill_Kill
        '
        Me.AutoKill_Kill.Interval = 10000
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.配置文件ToolStripMenuItem, Me.交流赞助ToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(701, 24)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        '配置文件ToolStripMenuItem
        '
        Me.配置文件ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.打开目录ToolStripMenuItem, Me.重置配置ToolStripMenuItem})
        Me.配置文件ToolStripMenuItem.Name = "配置文件ToolStripMenuItem"
        Me.配置文件ToolStripMenuItem.Size = New System.Drawing.Size(65, 20)
        Me.配置文件ToolStripMenuItem.Text = "配置文件"
        '
        '打开目录ToolStripMenuItem
        '
        Me.打开目录ToolStripMenuItem.Name = "打开目录ToolStripMenuItem"
        Me.打开目录ToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.打开目录ToolStripMenuItem.Text = "打开目录"
        '
        '重置配置ToolStripMenuItem
        '
        Me.重置配置ToolStripMenuItem.Name = "重置配置ToolStripMenuItem"
        Me.重置配置ToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.重置配置ToolStripMenuItem.Text = "重置配置"
        '
        '交流赞助ToolStripMenuItem
        '
        Me.交流赞助ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.交流群421483534ToolStripMenuItem, Me.赞助ToolStripMenuItem})
        Me.交流赞助ToolStripMenuItem.Name = "交流赞助ToolStripMenuItem"
        Me.交流赞助ToolStripMenuItem.Size = New System.Drawing.Size(77, 20)
        Me.交流赞助ToolStripMenuItem.Text = "交流♂赞助"
        '
        '交流群421483534ToolStripMenuItem
        '
        Me.交流群421483534ToolStripMenuItem.Name = "交流群421483534ToolStripMenuItem"
        Me.交流群421483534ToolStripMenuItem.Size = New System.Drawing.Size(280, 22)
        Me.交流群421483534ToolStripMenuItem.Text = "QQ群♂421483534（入群备注神秘力量）"
        '
        '赞助ToolStripMenuItem
        '
        Me.赞助ToolStripMenuItem.Name = "赞助ToolStripMenuItem"
        Me.赞助ToolStripMenuItem.Size = New System.Drawing.Size(280, 22)
        Me.赞助ToolStripMenuItem.Text = "赞助の二♂维♂码"
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(701, 425)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MinimumSize = New System.Drawing.Size(600, 400)
        Me.Name = "Main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents GamePath As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents Button10 As System.Windows.Forms.Button
    Friend WithEvents Button9 As System.Windows.Forms.Button
    Friend WithEvents Button11 As System.Windows.Forms.Button
    Friend WithEvents Button12 As System.Windows.Forms.Button
    Friend WithEvents Button13 As System.Windows.Forms.Button
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents AutoKill_GameLoader As System.Windows.Forms.Timer
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents 还原ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 关闭ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AutoKill_Kill As System.Windows.Forms.Timer
    Friend WithEvents Button14 As System.Windows.Forms.Button
    Friend WithEvents Button15 As System.Windows.Forms.Button
    Friend WithEvents Button16 As System.Windows.Forms.Button
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents 配置文件ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 打开目录ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 重置配置ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Button17 As System.Windows.Forms.Button
    Friend WithEvents 交流赞助ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 交流群421483534ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 赞助ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip

End Class
