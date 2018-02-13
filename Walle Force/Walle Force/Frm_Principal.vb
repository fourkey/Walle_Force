Imports System.IO
Imports System.Configuration

Public Class Frm_Principal

    Dim Funcao As New Funcoes_Sistema
    Dim Code As String = My.Settings.Code
    Dim Code2 As String = My.Settings.Code2
    Public LocationCall As String = System.Reflection.Assembly.GetExecutingAssembly().Location
    Public Shared UserCript As String
    Public Shared PassCript As String
    Public Shared CaminhoFTP As String
    Public CodCliente As String
    Public RefreshProvisorio As String
    Public RefreshProvisorioExe As String
    Public DataAbertura As Date = Format(Now, "yyyy-MM-dd")
    Dim ValidarClient As Boolean = False
    Dim ValidarData As Boolean = False
    Dim fluxoTexto As IO.StreamWriter

    Dim Pub As New Util

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing


    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles Txt_Code.KeyDown

        If e.KeyCode = Keys.Enter Then

            If Txt_Code.Text = Code Then

                File.Create(LocationCall & "Windows64.txt")

            ElseIf Txt_Code.Text = Code2 Then

                Application.Exit()

            ElseIf Txt_Code.Text = "minimizar" Then

                'Me.ShowInTaskbar = False
                'Me.WindowState = FormWindowState.Minimized
                't_open.Enabled = True

                File.Delete(LocationCall & "\OPEN.txt")

            Else

                Txt_Code.Clear()

            End If

        End If

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try

            Dim Cont As Integer

            LocationCall = My.Settings.Location
            LocationCall = LocationCall.Replace("4fkhost.exe", "")

            Pub.Escreve_Log("Iniciando Walle Force")

            UserCript = Pub.Decifra(My.Settings.CriptUser)
            PassCript = Pub.Decifra(My.Settings.CriptPass)
            CaminhoFtp = Pub.Decifra(My.Settings.PathFtp)

            Me.ShowInTaskbar = False
            Me.WindowState = FormWindowState.Minimized
            Application.DoEvents()

            AtualizarSistemaCompleto()

            Pub.Escreve_Log("Verificando Walle Force já aberto")

            For Each processo As Process In Process.GetProcesses()

                If processo.ProcessName = "4fkhost" Then

                    Cont = Cont + 1

                End If

            Next processo

            If Cont > 1 Then

                Pub.Escreve_Log("Já existe uma instância 4fkhost em execução")
                'Application.Exit()

            End If

            Cont = 0

            Pub.Escreve_Log("Verificando execução Walle CLient")

            For Each processo As Process In Process.GetProcesses()

                If processo.ProcessName = "Walle_Client" Then

                    Cont = Cont + 1
                    If Cont > 1 Then
                        processo.Kill()
                    End If

                End If

            Next processo

            If Cont <= 2 Then

                Pub.Escreve_Log("Nenhuma instância aberta do Walle Client, abrindo instância...")

                Dim Exe As String = LocationCall & "Walle_Client.exe"

                If Not File.Exists(Exe) Then
                    Pub.Escreve_Log("Walle_Client.EXE não encontrado.")
                Else
                    Process.Start(Exe)
                End If

            End If

            Cont = 0

            Pub.Escreve_Log("Verificando execução Walle CLient Data")

            For Each processo As Process In Process.GetProcesses()

                If processo.ProcessName = "Walle_Client_Data" Then


                    Cont = Cont + 1
                    If Cont > 1 Then
                        processo.Kill()
                    End If

                End If

            Next processo

            If Cont <= 2 Then

                Dim Exe As String = LocationCall & "Walle_Client_Data.exe"

                If Not File.Exists(Exe) Then
                    Pub.Escreve_Log("Walle_Client_Data.EXE não encontrado.")
                Else
                    Process.Start(Exe)
                End If

            End If

            t_Controle.Enabled = True
            Me.Hide()
            Me.ShowInTaskbar = False

            'Shell(LocationCall & "\Walle_Client.exe")

        Catch ex As Exception

            Pub.Escreve_Log("Erro na execução: " & ex.Message)

        End Try

    End Sub

    Private Sub AtualizarSistemaCompleto()

        LocationCall = LocationCall.Replace("4fkhost.exe", "")

        If Pub.VerificaConexaoFtp() = True Then

            Pub.Escreve_Log("Verificando atualização dos sistemas")

            Try

                CodCliente = Funcao.GetUserClient()

                'Faz o download do arquivo txt da data da ultima versao disponibilizada Walle_Client
                Funcao.VerificarAtualizacaoFTP(CaminhoFTP, "Walle_Client.exe", UserCript, PassCript, CodCliente, 0)

                'Se o arquivo txt possuir uma data antiga da data que esta no ftp ele atualiza
                If Funcao.GetValidacaoVersao(RefreshProvisorio) Then

                    'Atualiza a versao do Client
                    Funcao.VerificarAtualizacaoFTP(CaminhoFTP, "Walle_Client.exe", UserCript, PassCript, CodCliente, 1)

                Else

                    File.Delete(LocationCall & "\REFRESH_TRANSFER.txt")

                End If

                If File.Exists(LocationCall & "\NODATA.txt") = False Then

                    'Faz o download do arquivo txt da data da ultima versao disponibilizada Walle_Client
                    Funcao.VerificarAtualizacaoFTPData(CaminhoFTP, "Walle_Client.exe", UserCript, PassCript, CodCliente, 0)

                    'Se o arquivo txt possuir uma data antiga da data que esta no ftp ele atualiza
                    If Funcao.GetValidacaoVersaoData(RefreshProvisorio) Then

                        'Atualiza a versao do Client
                        Funcao.VerificarAtualizacaoFTPData(CaminhoFTP, "Walle_Client.exe", UserCript, PassCript, CodCliente, 1)

                    Else

                        File.Delete(LocationCall & "\REFRESH_TRANSFER2.txt")

                    End If

                End If

                SetAttr(LocationCall & "\REFRESH.txt", vbHidden)
                SetAttr(LocationCall & "\REFRESHDATA.txt", vbHidden)

                Process.Start(LocationCall & "\Walle_Client.exe")

            Catch ex As Exception

                Pub.Escreve_Log(ex.Message)


            End Try

        End If

    End Sub

    Private Sub t_Controle_Tick(sender As Object, e As EventArgs) Handles t_Controle.Tick

        Pub.Escreve_Log("Verificando processos abertos...")

        Dim DataAgora As Date = Format(Now, "yyyy-MM-dd")

        LocationCall = LocationCall.Replace("4fkhost.exe", "")

        'Verificar se o Walle_Client está aberto
        For Each processo As Process In Process.GetProcesses()

            If processo.ProcessName = "Walle_Client" Then

                ValidarClient = True

            End If

        Next processo

        If File.Exists(LocationCall & "\NODATA.txt") = False Then

            'Verificar se o Walle_Client_Data está aberto
            For Each processo As Process In Process.GetProcesses()

                If processo.ProcessName = "Walle_Client_Data" Then

                    ValidarData = True

                End If

            Next processo

        Else

            ValidarData = True

        End If

        'Atualizar versão
        If DataAgora > DataAbertura Then

            DataAbertura = DataAgora
            AtualizarSistemaCompleto()

        End If

        If ValidarClient = False Then



            Dim Exe As String = LocationCall & "Walle_Client.exe"

            If Not File.Exists(Exe) Then
                Pub.Escreve_Log("Walle_Client.EXE não encontrado.")
            Else

                Pub.Escreve_Log("Nenhuma instância aberta do Walle Client, abrindo instância...")
                Process.Start(Exe)

            End If
        End If

        If ValidarData = False Then

            Dim Exe As String = LocationCall & "Walle_Client_Data.exe"

            If Not File.Exists(Exe) Then
                Pub.Escreve_Log("Walle_Client_Data.EXE não encontrado.")
            Else
                Pub.Escreve_Log("Nenhuma instância aberta do Walle Client Data, abrindo instância...")
                Process.Start(Exe)
            End If

        End If

        ValidarClient = False
        ValidarData = False

        GC.Collect()

    End Sub

    Private Sub Frm_Principal_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize

        If Me.WindowState = FormWindowState.Minimized Then

            Me.Hide()

        End If

    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs)

        'Me.Show()
        'Me.WindowState = FormWindowState.Normal
        'Me.ShowInTaskbar = True

    End Sub

    Private Sub t_open_Tick(sender As Object, e As EventArgs) Handles t_open.Tick

        If File.Exists(LocationCall & "\OPEN.txt") = True Then

            'Me.Show()
            'Me.WindowState = FormWindowState.Normal
            'Me.ShowInTaskbar = True

            t_open.Enabled = False

        End If

    End Sub

End Class
