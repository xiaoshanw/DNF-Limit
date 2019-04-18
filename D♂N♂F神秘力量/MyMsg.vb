Public Class MyMsg
    Public Sub Clear()
        TextBox1.Text = ""
        Application.DoEvents()
    End Sub
    Public Sub Append(ByVal InString As String)
        TextBox1.AppendText(InString + vbCrLf)
        Application.DoEvents()
    End Sub
    Public Sub Add(ByVal InString As String)
        TextBox1.Text += InString
        Application.DoEvents()
    End Sub

End Class
