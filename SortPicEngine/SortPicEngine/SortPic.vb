Imports System.IO
Imports System.Drawing



Public Class SortPic


    Public Function id_str(ByVal n As Integer, ByVal prelenth As Integer) As String
        Dim str_n = n.ToString
        Dim str_prelenth = prelenth.ToString
        If Len(str_n) > str_prelenth Then
            Debug.WriteLine("目前序号数字长度大于预期长度")
            MsgBox("文件数量位数 > 序号长度" + vbCrLf + Len(str_n).ToString + ">" + str_prelenth)
            Return False

        End If
        Do While Len(str_n) < str_prelenth
            str_n = "0" + str_n
            If Len(str_n) = str_prelenth Then
                Return str_n
            End If
        Loop
        Return ""
    End Function

    Public Function find_max(ByVal array1 As Array) As Integer
        Dim v_max = 0
        If array1.Length > 0 Then
            For Each vaule In array1
                If vaule > v_max Then
                    v_max = vaule
                End If
            Next
            Debug.WriteLine(v_max)
            Return v_max
        End If
        Debug.WriteLine(0)
        Return 0
    End Function

    Public Function find_error()

        If TextBox3.Text = "" Then
            MsgBox("空路径，撒比")
            Return 0
        End If

        Dim dDir As New DirectoryInfo(TextBox3.Text)
        If Not dDir.Exists Then
            MsgBox("没找到这个路径，撒比")
            Return 0
        End If

        Dim s As New DirectoryInfo(TextBox3.Text)
        Dim files As FileInfo() = s.GetFiles("*.png")
        Dim count = CType(TextBox2.Text, Integer)
        If Len(files.Length.ToString) > count Then
            MsgBox("文件数量位数 > 序号长度" + vbCrLf + Len(files.Length.ToString).ToString + ">" + count.ToString)
            Return 0
        End If
        MsgBox("OK!")
        Return 1

    End Function




    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim s As New DirectoryInfo(TextBox3.Text)
        Dim files As FileInfo() = s.GetFiles("*.png")
        'Debug.WriteLine("shabi")

        Dim n = 1
        Dim prelenth = TextBox2.Text
        If prelenth = "" Then
            prelenth = "3"
        End If

        Dim str1 = ""
        For Each f As FileInfo In files
            Dim bmp As New Bitmap(f.FullName)

            Debug.WriteLine("PIC_" + id_str(n, prelenth) + ":" + "Width: " & bmp.Width.ToString() + " > Height: " & bmp.Height.ToString())
            n += 1
            str1 = str1 + "PIC_" + id_str(n, prelenth) + ":" + "Width: " & bmp.Width.ToString() + " > Height: " & bmp.Height.ToString() + vbCrLf

        Next
        TextBox1.Text = str1
    End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        find_error()

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim s As New DirectoryInfo(TextBox3.Text)
        Dim files As FileInfo() = s.GetFiles("*.png")

        Dim n = 1
        Dim prelenth = TextBox2.Text
        If prelenth = "" Then
            prelenth = "3"
        End If
        Dim f_name_no_extension() As Array
        '返回文件名不包含扩展名

        Dim str1 = ""
        For Each f As FileInfo In files
            Dim f_name = f.Name.Replace(f.Extension, "")
            'f_name_no_extension(1) = 1
            Debug.WriteLine(f_name)
            Dim bmp As New Bitmap(f.FullName)
            'Debug.WriteLine("Width: " & bmp.Width.ToString() + " > Height: " & bmp.Height.ToString())

            str1 = str1 + f_name + vbCrLf
        Next
        TextBox1.Text = str1
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If find_error() = 0 Then
            Debug.WriteLine("break")
            Return
        End If

        Dim s As New DirectoryInfo(TextBox3.Text)
        Dim files As FileInfo() = s.GetFiles("*.png")
        Dim l_files = files.Length
        Debug.WriteLine(l_files.GetType)
        Dim f_name(files.Length) As String
        Dim f_width(files.Length) As Integer
        Dim f_height(files.Length) As Integer
        Dim f_extension(files.Length) As String
        For i = 0 To files.Length - 1
            Using bmp As New Bitmap(files(i).FullName)
                f_name(i) = files(i).Name '.Replace(files(i).Extension, "")
                f_width(i) = bmp.Width
                f_height(i) = bmp.Height
                f_extension(i) = files(i).Extension
            End Using

        Next
        Dim f_sort(files.Length) As Integer
        Dim f_width2 = f_width
        Dim f_height2 = f_height
        Dim width_max As Integer = find_max(f_width2)
        Dim height_max As Integer = find_max(f_height2)
        Dim index = 0

        If RadioButton1.Checked Then 'kuan
            Do While width_max > 0
                For i = 0 To files.Length - 1
                    If f_width2(i) = width_max Then
                        f_width2(i) = 0
                        f_sort(index) = i
                        index += 1
                    End If
                Next
                width_max = find_max(f_width2)
            Loop
        ElseIf RadioButton2.Checked Then 'gao
            Do While height_max > 0
                For i = 0 To files.Length - 1
                    If f_height2(i) = height_max Then
                        f_height2(i) = 0
                        f_sort(index) = i
                        index += 1
                    End If
                Next
                height_max = find_max(f_height2)
            Loop
        End If


        Dim str1 = ""
        Dim str2 = ""
        For j = 0 To f_sort.Length - 2
            str1 = str1 + f_sort(j).ToString + " "

            Dim oldname = files(f_sort(j)).FullName
            Dim newname = id_str(j + 1, CType(TextBox2.Text, Integer)) + "_" + f_name(f_sort(j)) 'files(f_sort(j)).DirectoryName + "\" + 
            Debug.WriteLine(oldname + " " + newname)
            str2 = str2 + oldname + " --> " + newname + vbCrLf
            If CheckBox1.Checked = False Then
                My.Computer.FileSystem.RenameFile(oldname, newname)
            End If
        Next



        Debug.WriteLine(str1)
        TextBox1.Text = str2




    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        FolderBrowserDialog1.ShowDialog()
        TextBox3.Text = FolderBrowserDialog1.SelectedPath

    End Sub
End Class


