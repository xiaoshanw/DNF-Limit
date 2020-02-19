Public Class vLimit

#Region "sys"

    Dim SC_MANAGER_CREATE_SERVICE = 2
    Dim SERVICE_START = 16
    Dim SERVICE_KERNEL_DRIVER = 1
    Dim SERVICE_DEMAND_START = 3
    Dim SERVICE_ERROR_IGNORE = 0
    <System.Runtime.InteropServices.DllImport("advapi32.dll", SetLastError:=True)> _
    Private Shared Function OpenSCManager(ByVal machineName As String, ByVal databaseName As String, ByVal dwAccess As UInteger) As IntPtr
    End Function
    <System.Runtime.InteropServices.DllImport("advapi32.dll", SetLastError:=True)> _
    Private Shared Function CreateService(ByVal hSCManager As IntPtr, ByVal lpServiceName As String, ByVal lpDisplayName As String, ByVal dwDesiredAccess As Integer, ByVal dwServiceType As Integer, ByVal dwStartType As Integer, ByVal dwErrorControl As Integer, ByVal lpBinaryPathName As String, ByVal lpLoadOrderGroup As String, ByVal lpdwTagId As Integer, ByVal lpDependencies As Integer, ByVal lpServiceStartName As Integer, ByVal lpPassword As Integer) As IntPtr
    End Function
    <System.Runtime.InteropServices.DllImport("advapi32.dll", SetLastError:=True)> _
    Private Shared Function OpenService(ByVal hSCManager As IntPtr, ByVal lpServiceName As String, ByVal dwDesiredAccess As UInteger) As IntPtr
    End Function
    <System.Runtime.InteropServices.DllImport("advapi32.dll", SetLastError:=True)> _
    Private Shared Function CloseServiceHandle(ByVal serviceHandle As IntPtr) As Boolean
    End Function
    <System.Runtime.InteropServices.DllImport("advapi32.dll", SetLastError:=True)> _
    Private Shared Function StartService(ByVal hService As IntPtr, ByVal dwNumServiceArgs As Integer, ByVal lpServiceArgVectors As String()) As Boolean
    End Function
    <System.Runtime.InteropServices.DllImport("advapi32.dll", SetLastError:=True)> _
    Public Shared Function ControlService(ByVal hService As IntPtr, ByVal dwControl As SERVICE_CONTROL, ByRef lpServiceStatus As SERVICE_STATUS) As Boolean
    End Function
    <System.Runtime.InteropServices.DllImport("kernel32.dll")> _
    Public Shared Function GetLastError() As UInteger
    End Function
    Public Enum SERVICE_CONTROL As Integer
        [STOP] = &H1
        PAUSE = &H2
        CONTINUE_fix = &H3
        INTERROGATE = &H4
        SHUTDOWN = &H5
        PARAMCHANGE = &H6
        NETBINDADD = &H7
        NETBINDREMOVE = &H8
        NETBINDENABLE = &H9
        NETBINDDISABLE = &HA
        DEVICEEVENT = &HB
        HARDWAREPROFILECHANGE = &HC
        POWEREVENT = &HD
        SESSIONCHANGE = &HE
    End Enum
    Public Enum SERVICE_STATE As Integer
        SERVICE_STOPPED = &H1
        SERVICE_START_PENDING = &H2
        SERVICE_STOP_PENDING = &H3
        SERVICE_RUNNING = &H4
        SERVICE_CONTINUE_PENDING = &H5
        SERVICE_PAUSE_PENDING = &H6
        SERVICE_PAUSED = &H7
    End Enum
    Public Structure SERVICE_STATUS
        Dim dwServiceType As Int32
        Dim dwCurrentState As Int32
        Dim dwControlsAccepted As Int32
        Dim dwWin32ExitCode As Int32
        Dim dwServiceSpecificExitCode As Int32
        Dim dwCheckPoint As Int32
        Dim dwWaitHint As Int32
    End Structure
    Public Enum SERVICE_ACCEPT As Integer
        [STOP] = &H1
        PAUSE_CONTINUE = &H2
        SHUTDOWN = &H4
        PARAMCHANGE = &H8
        NETBINDCHANGE = &H10
        HARDWAREPROFILECHANGE = &H20
        POWEREVENT = &H40
        SESSIONCHANGE = &H80
    End Enum
#End Region
    Public sys_path = (Path_Info + "\vLimit-d.sys").Replace("\\", "\")
    Dim sys_ini = ("C:\Users\Public\Documents\vLimit-d\")
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If Not Check_File() Then
            IO.File.WriteAllBytes(sys_path, My.Resources.vLimit_d)
        End If
        If Not Check_Device() Then
            SYS_Install()
        End If
        Button1_Click(Me, e)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Check_File() Then
            Label2.Text = "正常"
            Label2.ForeColor = Color.Green
        Else
            Label2.Text = "不存在"
            Label2.ForeColor = Color.Red
        End If
        If Check_Device() Then
            Label5.Text = "已注册"
            Label5.ForeColor = Color.Green
        Else
            Label5.Text = "未注册"
            Label5.ForeColor = Color.Red
        End If
        If Check_Status() Then
            Label6.Text = "已加载"
            Label6.ForeColor = Color.Green
        Else
            Label6.Text = "未加载"
            Label6.ForeColor = Color.Red
        End If
    End Sub
    Private Function Check_File() As Boolean
        Return IO.File.Exists(sys_path)
    End Function
    Private Function Check_Device() As Boolean
        Dim svc() = ServiceProcess.ServiceController.GetDevices
        For Each vline In svc
            If vline.DisplayName.ToLower = "vlimit-d" Then
                Return True
            End If
        Next
        Return False
    End Function
    Private Function Check_Status() As Boolean
        Dim svc() = ServiceProcess.ServiceController.GetDevices
        For Each vline In svc
            If vline.DisplayName.ToLower = "vlimit-d" Then
                If vline.Status = ServiceProcess.ServiceControllerStatus.Running Then Return True
            End If
        Next
        Return False
    End Function

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If Check_Status() = True Then Exit Sub
        If Check_Device() = False Then
            MsgBox("驱动程序未注册")
        Else
            SYS_Run()
            Button1_Click(Me, e)
        End If

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If Check_Status() Then
            SYS_Stop()
            Button1_Click(Me, e)
        Else
            MsgBox("驱动未加载")
        End If
    End Sub
    Private Sub SYS_Install()
        '打开服务管理器
        Dim hSCManager = OpenSCManager(Nothing, Nothing, SC_MANAGER_CREATE_SERVICE)
        '注册驱动
        CloseServiceHandle(CreateService(hSCManager, "vLimit-d", "vLimit-d", SERVICE_START, SERVICE_KERNEL_DRIVER, SERVICE_DEMAND_START, SERVICE_ERROR_IGNORE, sys_path, 0, 0, 0, 0, 0))
        CloseServiceHandle(hSCManager)
    End Sub
    Private Sub SYS_Run()
        Dim svc() = ServiceProcess.ServiceController.GetDevices
        For Each vline In svc
            If vline.DisplayName.ToLower = "vlimit-d" Then
                vline.Start() : Exit For
            End If
        Next
        Dim regist As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine
        Dim svcreg As Microsoft.Win32.RegistryKey = regist.OpenSubKey("SYSTEM\CurrentControlSet\Services\vLimit-d", True)
        svcreg.SetValue("Start", 2, Microsoft.Win32.RegistryValueKind.DWord)
        svcreg.Flush()
    End Sub
    Private Sub SYS_Stop()
        Dim svc() = ServiceProcess.ServiceController.GetDevices
        For Each vline In svc
            If vline.DisplayName.ToLower = "vlimit-d" Then
                vline.Stop() : Exit For
            End If
        Next
        Dim regist As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine
        Dim svcreg As Microsoft.Win32.RegistryKey = regist.OpenSubKey("SYSTEM\CurrentControlSet\Services\vLimit-d", True)
        svcreg.SetValue("Start", 3, Microsoft.Win32.RegistryValueKind.DWord)
        svcreg.Flush()
    End Sub
    Public Function SYS_Uninstall() As String
        Dim str = IO.Path.GetTempFileName
        Dim ServiceInstallerObj = New ServiceProcess.ServiceInstaller
        Dim Context = New System.Configuration.Install.InstallContext(str, Nothing)
        ServiceInstallerObj.Context = Context
        ServiceInstallerObj.ServiceName = "vLimit-d"
        ServiceInstallerObj.Uninstall(Nothing)
        SYS_Uninstall = IO.File.ReadAllText(str)
        IO.File.Delete(str)

    End Function

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        If Check_Device() Then
            MsgBox(SYS_Uninstall)
            IO.File.Delete(sys_path)
            Button1_Click(Me, e)
        End If
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        If Not Check_File() Then IO.File.WriteAllBytes(sys_path, My.Resources.vLimit_d)
        If Not Check_Device() Then SYS_Install()
        If Not Check_Status() Then SYS_Run()
        Button1_Click(Me, e)
    End Sub

    Private Sub vLimit_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Button1_Click(Me, e)
        Try
            ListBox1.Items.Clear()
            For Each vline In IO.Directory.GetFiles(sys_ini)
                ListBox1.Items.Add(SYS_STRING_DECODE(vline.Replace(sys_ini, "")))
            Next
        Catch ex As Exception

        End Try
        
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        Button5_Click(Me, e)
    End Sub

    Private Sub 添加ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 添加ToolStripMenuItem.Click
        Dim fop = New OpenFileDialog
        fop.Filter = "可执行文件|*.exe"
        If fop.ShowDialog = Windows.Forms.DialogResult.OK Then
            SYS_ADD(fop.FileName.ToLower)
            ReloadDriver()
        End If
    End Sub
    Private Sub SYS_ADD(ByVal vpath As String)
        If Not ListBox1.Items.Contains(vpath) Then
            ListBox1.Items.Add(vpath)
            IO.Directory.CreateDirectory(sys_ini)
            IO.File.Create(sys_ini + SYS_STRING_ENCODE(vpath)).Close()
        End If
    End Sub
    Private Sub SYS_DEL(ByVal vstr As String)
        IO.File.Delete(sys_ini + SYS_STRING_ENCODE(vstr))
        ListBox1.Items.Remove(vstr)
    End Sub

    Private Function SYS_STRING_ENCODE(ByVal vString As String) As String
        Return vString.Replace(":", "#").Replace("\", "$")
    End Function
    Private Function SYS_STRING_DECODE(ByVal vString As String) As String
        Return vString.Replace("#", ":").Replace("$", "\")
    End Function

    Private Sub 删除ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 删除ToolStripMenuItem.Click
        If ListBox1.SelectedIndices Is Nothing Then Exit Sub
        Dim vstr = New ArrayList
        For Each vline In ListBox1.SelectedIndices
            vstr.Add(ListBox1.Items(vline).ToString)
        Next
        For Each vline As String In vstr
            SYS_DEL(vline)
            ReloadDriver()
        Next
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        Dim strA() As String
        For Each vline In IO.File.ReadAllLines(INI_Path)
            Try
                strA = vline.Split("|")
                If strA.Length > 3 Then
                    vline = (Main.GamePath.Text + strA(2)).Replace("\\", "\")
                    If IO.Path.GetExtension(vline).ToLower = ".exe" Then

                        If strA(1) <> "0" And ListBox1.Items.Contains(vline) = False Then SYS_ADD(vline.ToLower)
                    End If
                End If

            Catch ex As Exception

            End Try

        Next
        ReloadDriver()
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        vMSG.TextBox1.Text = My.Resources.vLimit_d说明
        vMSG.TextBox1.Select(0, 0)
        vMSG.Show()
    End Sub
    Private Sub ReloadDriver()
        If Label6.Text = "已加载" Then
            Button4_Click(Me, Nothing)
            Button3_Click(Me, Nothing)
        End If
    End Sub
End Class