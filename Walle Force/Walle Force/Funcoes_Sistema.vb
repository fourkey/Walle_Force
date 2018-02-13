Imports System.Text
Imports System.IO
Imports System.Security.Cryptography
Imports System.Net


Public Class Funcoes_Sistema

    'Criptografia
    Dim textoCifrado As Byte()
    Dim sal() As Byte = {&H0, &H1, &H2, &H3, &H4, &H5, &H6, &H5, &H4, &H3, &H2, &H1, &H0}
    Dim Pub As New Util

    Public Sub VerificarAtualizacaoFTP(ByVal Server As String, ByVal Arquivo As String,
                                     ByVal usuarioftp As String, ByVal senhaftp As String, ByVal ClienteCod As String, ByVal Tipo As Integer)

        Dim linhatexto As String = ""

        If Tipo = 0 Then

            linhatexto = "REFRESH.txt"

        Else

            linhatexto = "Walle_Client.exe"

        End If

        Dim Local As String = Frm_Principal.LocationCall

        Local = Local.Replace("4fkhost.exe", "")

        Dim FtpClient As FtpWebRequest = FtpWebRequest.Create(Frm_Principal.CaminhoFTP & "/Walle/Client/" & ClienteCod & "/Refresh/" & linhatexto)
        FtpClient.Credentials = New NetworkCredential(usuarioftp, senhaftp)
        FtpClient.Method = WebRequestMethods.Ftp.DownloadFile
        FtpClient.UsePassive = False


        If Tipo = 0 Then

            Frm_Principal.RefreshProvisorio = "\REFRESH_TRANSFER.txt"

            'Get the response to the Ftp request and the associated stream
            Using response As System.Net.FtpWebResponse = CType(FtpClient.GetResponse, System.Net.FtpWebResponse)
                Using responseStream As IO.Stream = response.GetResponseStream
                    'loop to read & write to file
                    Using fs As New IO.FileStream(Local & Frm_Principal.RefreshProvisorio, IO.FileMode.Create)
                        Dim buffer(2047) As Byte
                        Dim read As Integer = 0
                        Do
                            read = responseStream.Read(buffer, 0, buffer.Length)
                            fs.Write(buffer, 0, read)
                        Loop Until read = 0 'see Note(1)
                        responseStream.Close()
                        fs.Flush()
                        fs.Close()
                    End Using
                    responseStream.Close()
                End Using
                response.Close()
            End Using

        Else

            Dim RefreshProvisorioExe As String = "Walle_Client_" & Format(Now, "yyyy-MM-dd HH-mm-ss") & ".exe"
            Dim NovaCont As String = Frm_Principal.RefreshProvisorio

            'Get the response to the Ftp request and the associated stream
            Using response As System.Net.FtpWebResponse = CType(FtpClient.GetResponse, System.Net.FtpWebResponse)
                Using responseStream As IO.Stream = response.GetResponseStream
                    'loop to read & write to file
                    Using fs As New IO.FileStream(Local & "\Refresh\" & RefreshProvisorioExe, IO.FileMode.Create)
                        Dim buffer(2047) As Byte
                        Dim read As Integer = 0
                        Do
                            read = responseStream.Read(buffer, 0, buffer.Length)
                            fs.Write(buffer, 0, read)
                        Loop Until read = 0 'see Note(1)
                        responseStream.Close()
                        fs.Flush()
                        fs.Close()
                    End Using
                    responseStream.Close()
                End Using
                response.Close()
            End Using

            Dim fluxoTexto As IO.StreamWriter

            If IO.File.Exists(Local & "\Windows64.txt") = False Then
                fluxoTexto = New IO.StreamWriter(Local & "\Windows64.txt", True)
                fluxoTexto.Close()
            End If

            For Each processo As Process In Process.GetProcesses()

                If processo.ProcessName = "Walle_Client" Then

                    Try
                        processo.Kill()
                    Catch ex As Exception

                    End Try


                End If

            Next processo

            File.Delete(Local & "Windows64.txt")
            System.Threading.Thread.Sleep(2000)


            Try

                File.Delete(Local & "Walle_Client.exe")
                System.Threading.Thread.Sleep(5000)

            Catch ex As Exception

                Try

                    System.Threading.Thread.Sleep(5000)
                    File.Delete(Local & "Walle_Client.exe")
                    System.Threading.Thread.Sleep(5000)

                Catch ex2 As Exception

                    Try

                        System.Threading.Thread.Sleep(5000)
                        File.Delete(Local & "Walle_Client.exe")
                        System.Threading.Thread.Sleep(5000)

                    Catch ex3 As Exception

                        System.Threading.Thread.Sleep(5000)
                        File.Delete(Local & "Walle_Client.exe")
                        System.Threading.Thread.Sleep(5000)

                    End Try

                End Try

            End Try

            File.Copy(Local & "\Refresh\" & RefreshProvisorioExe, Local & "Walle_Client.exe")
            System.Threading.Thread.Sleep(5000)

            File.Delete(Local & "\REFRESH.txt")

            File.Copy(Local & NovaCont, Local & "\REFRESH.TXT")

            File.Delete(Local & "\Refresh\REFRESH_TRANSFER.txt")
            File.Delete(Local & "\REFRESH_TRANSFER.txt")
            System.Threading.Thread.Sleep(1000)

            File.Delete(Local & "\Refresh\" & RefreshProvisorioExe)
            System.Threading.Thread.Sleep(3000)

        End If

    End Sub

    Public Sub VerificarAtualizacaoFTPData(ByVal Server As String, ByVal Arquivo As String,
                                     ByVal usuarioftp As String, ByVal senhaftp As String, ByVal ClienteCod As String, ByVal Tipo As Integer)

        Dim linhatexto As String = ""

        If Tipo = 0 Then

            linhatexto = "REFRESHDATA.txt"

        Else

            linhatexto = "Walle_Client_Data.exe"

        End If

        Dim Local As String = Frm_Principal.LocationCall

        Local = Local.Replace("4fkhost.exe", "")

        Dim FtpClient As FtpWebRequest = FtpWebRequest.Create(Frm_Principal.CaminhoFTP & "/Walle/Client/" & ClienteCod & "/Refresh/" & linhatexto)
        FtpClient.Credentials = New NetworkCredential(usuarioftp, senhaftp)
        FtpClient.Method = WebRequestMethods.Ftp.DownloadFile
        FtpClient.UsePassive = False


        If Tipo = 0 Then

            Frm_Principal.RefreshProvisorio = "\REFRESH_TRANSFER2.txt"

            'Get the response to the Ftp request and the associated stream
            Using response As System.Net.FtpWebResponse = CType(FtpClient.GetResponse, System.Net.FtpWebResponse)
                Using responseStream As IO.Stream = response.GetResponseStream
                    'loop to read & write to file
                    Using fs As New IO.FileStream(Local & Frm_Principal.RefreshProvisorio, IO.FileMode.Create)
                        Dim buffer(2047) As Byte
                        Dim read As Integer = 0
                        Do
                            read = responseStream.Read(buffer, 0, buffer.Length)
                            fs.Write(buffer, 0, read)
                        Loop Until read = 0 'see Note(1)
                        responseStream.Close()
                        fs.Flush()
                        fs.Close()
                    End Using
                    responseStream.Close()
                End Using
                response.Close()
            End Using

        Else

            Dim RefreshProvisorioExe As String = "Walle_Client_Data" & Format(Now, "yyyy-MM-dd HH-mm-ss") & ".exe"

            'Get the response to the Ftp request and the associated stream
            Using response As System.Net.FtpWebResponse = CType(FtpClient.GetResponse, System.Net.FtpWebResponse)
                Using responseStream As IO.Stream = response.GetResponseStream
                    'loop to read & write to file
                    Using fs As New IO.FileStream(Local & "\Refresh\" & RefreshProvisorioExe, IO.FileMode.Create)
                        Dim buffer(2047) As Byte
                        Dim read As Integer = 0
                        Do
                            read = responseStream.Read(buffer, 0, buffer.Length)
                            fs.Write(buffer, 0, read)
                        Loop Until read = 0 'see Note(1)
                        responseStream.Close()
                        fs.Flush()
                        fs.Close()
                    End Using
                    responseStream.Close()
                End Using
                response.Close()
            End Using

            Dim fluxoTexto As IO.StreamWriter

            If IO.File.Exists(Local & "\Windows64.txt") = False Then
                fluxoTexto = New IO.StreamWriter(Local & "\Windows64.txt", True)
                fluxoTexto.Close()
            End If

            For Each processo As Process In Process.GetProcesses()

                If processo.ProcessName = "Walle_Client_Data" Then
                    Try
                        processo.Kill()
                    Catch ex As Exception

                    End Try


                End If

            Next processo

            File.Delete(Local & "Windows64.txt")
            System.Threading.Thread.Sleep(2000)

            Try

                File.Delete(Local & "Walle_Client_Data.exe")

            Catch ex As Exception



            End Try

            File.Copy(Local & "\Refresh\" & RefreshProvisorioExe, Local & "Walle_Client_Data.exe")
            System.Threading.Thread.Sleep(2000)

            File.Delete(Local & "\Refresh\" & RefreshProvisorioExe)
            System.Threading.Thread.Sleep(2000)

            File.Delete(Local & "\REFRESHDATA.txt")
            System.Threading.Thread.Sleep(2000)

            File.Copy(Local & "REFRESH_TRANSFER2.txt", Local & "\REFRESHDATA.txt")
            System.Threading.Thread.Sleep(2000)

            File.Delete(Local & "REFRESH_TRANSFER2.txt")

        End If

    End Sub

    Public Function GetUserClient() As String

        Dim chave As New Rfc2898DeriveBytes(Pub.Decifra(My.Settings.KeyGetUser), sal)
        Dim algoritmo = New RijndaelManaged()
        Dim fluxoTexto As IO.StreamReader
        Dim linhaTexto As String
        Dim Cliente As String = ""
        Dim USERCLIENT As String = My.Settings.Location & "USERCLIENT.txt"

        USERCLIENT = USERCLIENT.Replace("4fkhost.exe", "")

        If IO.File.Exists(USERCLIENT) Then

            fluxoTexto = New IO.StreamReader(USERCLIENT)
            linhaTexto = fluxoTexto.ReadLine

            fluxoTexto.Close()

            textoCifrado = Convert.FromBase64String(linhaTexto)

            algoritmo.Key = chave.GetBytes(16)
            algoritmo.IV = chave.GetBytes(16)

            Using StreamFonte = New MemoryStream(textoCifrado)

                Using StreamDestino As New MemoryStream()

                    Using crypto As New CryptoStream(StreamFonte, algoritmo.CreateDecryptor(), CryptoStreamMode.Read)

                        moveBytes(crypto, StreamDestino)

                        Dim bytesDescriptografados() As Byte = StreamDestino.ToArray()
                        Dim mensagemDescriptografada = New UnicodeEncoding().GetString(bytesDescriptografados)

                        Cliente = mensagemDescriptografada

                    End Using

                End Using

            End Using

        Else

            MsgBox("Walle: Atenção alguns arquivos foram movidos ou modificados indevidamente, por favor entre em contato com o fornecedor.", vbExclamation)
            Application.Exit()

        End If

        Return Cliente

    End Function

    Private Sub moveBytes(ByVal fonte As Stream, ByVal destino As Stream)
        Dim bytes(2048) As Byte
        Dim contador = fonte.Read(bytes, 0, bytes.Length - 1)
        While (0 <> contador)
            destino.Write(bytes, 0, contador)
            contador = fonte.Read(bytes, 0, bytes.Length - 1)
        End While
    End Sub

    Public Function GetValidacaoVersao(ByVal CaminhoNovo As String)

        Dim fluxoTexto As IO.StreamReader
        Dim Local As String = Frm_Principal.LocationCall
        Dim Data1 As Date
        Dim Data2 As Date

        Local = Local.Replace("4fkhost.exe", "")

        If IO.File.Exists(Local & CaminhoNovo) Then

            fluxoTexto = New IO.StreamReader(Local & CaminhoNovo)
            Data1 = fluxoTexto.ReadLine

            fluxoTexto.Close()

        End If

        If IO.File.Exists(Local & "\REFRESH.txt") Then

            fluxoTexto = New IO.StreamReader(Local & "\REFRESH.txt")
            Data2 = fluxoTexto.ReadLine

            fluxoTexto.Close()

        End If

        If Data1 > Data2 Then

            Return True

        Else

            Return False

        End If

    End Function

    Public Function GetValidacaoVersaoData(ByVal CaminhoNovo As String)

        Dim fluxoTexto As IO.StreamReader
        Dim Local As String = Frm_Principal.LocationCall
        Dim Data1 As Date
        Dim Data2 As Date

        Local = Local.Replace("4fkhost.exe", "")

        If IO.File.Exists(Local & CaminhoNovo) Then

            fluxoTexto = New IO.StreamReader(Local & CaminhoNovo)
            Data1 = fluxoTexto.ReadLine

            fluxoTexto.Close()

        End If

        If IO.File.Exists(Local & "\REFRESHDATA.txt") Then

            fluxoTexto = New IO.StreamReader(Local & "\REFRESHDATA.txt")
            Data2 = fluxoTexto.ReadLine

            fluxoTexto.Close()

        End If

        If Data1 > Data2 Then

            Return True

        Else

            Return False

        End If

    End Function

End Class
