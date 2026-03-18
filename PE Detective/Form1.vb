Imports System.IO
Imports System.Xml
Imports System.Text
Imports System.Reflection
Imports System.Resources

Public Class Form1
    ' İmzaları tutacak listemiz
    Dim signatures As New List(Of SignatureItem)
    ' --- KISAYOL (.LNK) ÇÖZÜCÜ FONKSİYON ---
    ' Windows Script Host kullanarak kısayolun gittiği gerçek adresi bulur
    Private Function ResolveShortcut(lnkPath As String) As String
        Try
            ' Geç bağlama (Late Binding) ile WScript.Shell nesnesi oluştur
            ' Bu sayede projeye ekstra referans eklemene gerek kalmaz.
            Dim shell As Object = CreateObject("WScript.Shell")
            Dim shortcut As Object = shell.CreateShortcut(lnkPath)

            Dim targetPath As String = shortcut.TargetPath

            ' Eğer hedef dosya gerçekten varsa o yolu döndür
            If File.Exists(targetPath) Then
                Return targetPath
            Else
                Return lnkPath ' Hedef bulunamazsa yine de eski yolu döndür
            End If
        Catch ex As Exception
            ' Bir hata olursa (örn: okuma izni yoksa) orijinal yolu döndür
            Return lnkPath
        End Try
    End Function
    ' --- SÜRÜKLE BIRAK (DRAG & DROP) OLAYLARI ---

    ' 1. Dosya üzerine gelince imleci değiştir
    Private Sub Form1_DragEnter(sender As Object, e As DragEventArgs) Handles MyBase.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    ' 2. Dosya bırakılınca çalışacak kod
    Private Sub Form1_DragDrop(sender As Object, e As DragEventArgs) Handles MyBase.DragDrop
        Dim files() As String = CType(e.Data.GetData(DataFormats.FileDrop), String())

        If files.Length > 0 Then
            Dim droppedFile As String = files(0) ' İlk dosyayı al

            ' EĞER DOSYA BİR KISAYOLSA (.LNK)
            If Path.GetExtension(droppedFile).ToLower() = ".lnk" Then
                ' Kısayolun hedefini bul ve droppedFile değişkenini güncelle
                droppedFile = ResolveShortcut(droppedFile)
            End If

            ' Yeni yolu arayüze yaz
            txtFilePath.Text = droppedFile

            ' Tüm analiz fonksiyonlarını sırasıyla çalıştır
            ' (Mevcut fonksiyonların burada çağrılıyor)
            AnalyzePE(droppedFile)          ' PE Başlıklarını oku
            ScanSignatures(droppedFile)     ' Packer Taraması yap
            ShowDisassembly(droppedFile)    ' Disassembler'ı güncelle
            AnalyzeExtraFeatures(droppedFile) ' Bölümler, Entropi ve Overlay hesapla
        End If
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Program açılınca kaynaklardaki XML'leri belleğe yükle
        LoadSignaturesFromResources()
    End Sub

    ' --- KAYNAKLARDAN (RESOURCES) OKUMA FONKSİYONU ---
    ' --- GÜNCELLENMİŞ KAYNAK OKUMA (BYTE ARRAY VE STRING DESTEKLİ) ---
    Sub LoadSignaturesFromResources()
        Dim count As Integer = 0
        Dim rm As ResourceManager = My.Resources.ResourceManager

        ' Kaynak setini al
        Dim resourceSet = rm.GetResourceSet(System.Globalization.CultureInfo.CurrentUICulture, True, True)

        For Each entry As DictionaryEntry In resourceSet
            Dim resourceName As String = entry.Key.ToString()
            Dim resourceValue As Object = entry.Value
            Dim xmlContent As String = ""

            ' 1. Durum: Kaynak metin (String) olarak kayıtlıysa
            If TypeOf resourceValue Is String Then
                xmlContent = resourceValue.ToString()

                ' 2. Durum: Kaynak dosya (Byte Array) olarak kayıtlıysa (Genellikle XML dosyaları böyle gelir)
            ElseIf TypeOf resourceValue Is Byte() Then
                xmlContent = System.Text.Encoding.UTF8.GetString(CType(resourceValue, Byte()))
            End If

            ' Eğer içerik XML formatındaysa işle
            If Not String.IsNullOrEmpty(xmlContent) AndAlso xmlContent.Trim().StartsWith("<?xml") Then
                Try
                    Dim doc As New XmlDocument()
                    doc.LoadXml(xmlContent) ' String'den XML yükle

                    Dim root As XmlNode = doc.DocumentElement

                    ' ENTRY etiketlerini tara
                    For Each node As XmlNode In root.SelectNodes("ENTRY")
                        Dim name As String = node.SelectSingleNode("NAME")?.InnerText

                        ' EntryPoint veya EntirePE verisini al
                        Dim pattern As String = ""
                        Dim epNode = node.SelectSingleNode("ENTRYPOINT")
                        Dim entireNode = node.SelectSingleNode("ENTIREPE")

                        If epNode IsNot Nothing AndAlso Not String.IsNullOrEmpty(epNode.InnerText.Trim()) Then
                            pattern = epNode.InnerText
                        ElseIf entireNode IsNot Nothing AndAlso Not String.IsNullOrEmpty(entireNode.InnerText.Trim()) Then
                            pattern = entireNode.InnerText
                        End If

                        If Not String.IsNullOrEmpty(pattern) AndAlso Not String.IsNullOrEmpty(name) Then
                            signatures.Add(New SignatureItem With {
                                .Name = name,
                                .Pattern = pattern,
                                .Extension = resourceName
                            })
                            count += 1
                        End If
                    Next
                Catch ex As Exception
                    Debug.WriteLine("Hata (XML): " & resourceName & " - " & ex.Message)
                End Try
            End If
        Next

        lblDatabase.Text = "Database: " & count & " signatures loaded."
    End Sub

    ' --- BUTON VE ARAYÜZ OLAYLARI ---
    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Dim ofd As New OpenFileDialog()
        ofd.Filter = "Executable Files|*.exe;*.dll;*.ocx|All Files|*.*"
        If ofd.ShowDialog() = DialogResult.OK Then
            txtFilePath.Text = ofd.FileName
            AnalyzePE(ofd.FileName)
            ' Dosya seçilince otomatik tarama yapmasın, kullanıcı Scan tuşuna bassın (isteğe bağlı)
            ScanSignatures(ofd.FileName)
            'txtResult.Text = "Ready to scan."
            'txtResult.ForeColor = Color.Black
        End If
    End Sub

    Private Sub btnScan_Click(sender As Object, e As EventArgs) Handles btnScan.Click
        If File.Exists(txtFilePath.Text) Then
            ScanSignatures(txtFilePath.Text)
        Else
            MessageBox.Show("Please select a file first.")
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Application.Exit()
    End Sub

    ' --- TARAMA MANTIĞI ---
    Sub ScanSignatures(filePath As String)
        txtResult.Text = "Scanning..."
        txtResult.ForeColor = Color.Gray
        Application.DoEvents()

        Try
            ' 1. Önce Entry Point Ofsetini Bul
            Dim epOffset As Long = PEScanner.GetEntryPointOffset(filePath)

            ' EP bulunamazsa -1 döner ama bazı dosyalarda (örn .NET native olmayan) yine de tarama yapmak isteyebiliriz.
            ' Şimdilik sadece EP varsa devam edelim.
            If epOffset <= 0 Then
                txtResult.Text = "Invalid PE or Unknown Format"
                txtResult.ForeColor = Color.Red
                Exit Sub
            End If

            ' 2. Dosyayı Oku (Geniş bir buffer alalım, Phoenix imzası uzun olabilir)
            Dim buffer(4096) As Byte
            Using fs As New FileStream(filePath, FileMode.Open, FileAccess.Read)
                fs.Seek(epOffset, SeekOrigin.Begin)
                Dim readBytes = fs.Read(buffer, 0, buffer.Length)
                ' Eğer dosya sonuna geldiysek buffer'ın kalanını sıfırla veya boyutu küçült
                ReDim Preserve buffer(readBytes - 1)
            End Using

            ' 3. Karşılaştır
            Dim found As Boolean = False

            For Each sig In signatures
                If sig.IsMatch(buffer) Then
                    txtResult.Text = sig.Name & " [" & sig.Extension & "]"
                    txtResult.ForeColor = Color.Red
                    found = True

                    Exit For ' İlk bulduğunda dur
                End If

            Next

            If Not found Then
                txtResult.Text = "Nothing found / Unknown Packer"
                txtResult.ForeColor = Color.Green
            End If
            ShowDisassembly(txtFilePath.Text)
            AnalyzeExtraFeatures(filePath)

        Catch ex As Exception
            txtResult.Text = "Error: " & ex.Message
        End Try
    End Sub

    ' --- PE BİLGİLERİNİ GÖSTEREN KISIM ---
    Sub AnalyzePE(filePath As String)
        Try
            Dim fileInfo As New FileInfo(filePath)
            txtFileSize.Text = (fileInfo.Length / 1024).ToString("0.00") & " KB"

            Using fs As New FileStream(filePath, FileMode.Open, FileAccess.Read)
                Using br As New BinaryReader(fs)
                    ' DOS Header kontrolü
                    If br.ReadUInt16 <> &H5A4D Then Exit Sub
                    fs.Seek(&H3C, SeekOrigin.Begin)
                    Dim peOffset As Integer = br.ReadInt32()

                    ' PE Header
                    fs.Seek(peOffset, SeekOrigin.Begin)
                    If br.ReadUInt32 <> &H4550 Then Exit Sub

                    ' File Header
                    fs.Seek(20, SeekOrigin.Current) ' Machine(2)+Sections(2)+Time(4)+SymTable(4)+SymNum(4)+OptHeaderSize(2)+Char(2)

                    ' Optional Header
                    Dim magic As UInt16 = br.ReadUInt16()
                    Dim epRVA As UInt32 = 0
                    Dim subSys As UInt16 = 0

                    If magic = &H10B Then ' 32-bit PE
                        fs.Seek(14, SeekOrigin.Current) ' Code/Data sizes...
                        epRVA = br.ReadUInt32()
                        fs.Seek(40, SeekOrigin.Current) ' ImageBase, SectionAlign, FileAlign vb...
                        subSys = br.ReadUInt16() ' Subsystem offseti 32 bit için yaklaşık burasıdır
                    ElseIf magic = &H20B Then ' 64-bit PE
                        fs.Seek(14, SeekOrigin.Current)
                        epRVA = br.ReadUInt32()
                        fs.Seek(40, SeekOrigin.Current)
                        subSys = br.ReadUInt16()
                    End If

                    txtEP.Text = epRVA.ToString("X8")
                    txtSubsystem.Text = If(subSys = 2, "GUI", If(subSys = 3, "Console", "Unknown"))

                    ' Dosya üzerindeki gerçek ofseti PEScanner sınıfından alıyoruz
                    Dim realOffset As Long = PEScanner.GetEntryPointOffset(filePath)
                    txtOffset.Text = realOffset.ToString("X8")
                End Using
            End Using
            ' --- OVERLAY HESAPLAMA ---
            Dim overlaySize As Long = AdvancedPEAnalyzer.GetOverlaySize(filePath)

            If overlaySize > 0 Then
                txtOverlay.Text = (overlaySize / 1024).ToString("0.00") & " KB"
                txtOverlay.ForeColor = Color.Red ' Dikkat çeksin diye kırmızı
                txtOverlay.BackColor = Color.MistyRose
            Else
                txtOverlay.Text = "Not Found"
                txtOverlay.ForeColor = Color.Green
                txtOverlay.BackColor = Color.White
            End If
        Catch ex As Exception
            txtEP.Text = "Error"
        End Try
    End Sub
    ' --- DISASSEMBLER ÇAĞRISI ---
    ' --- GÜNCELLENMİŞ DISASSEMBLER ÇAĞRISI ---
    Sub ShowDisassembly(filePath As String)
        rtbDisasm.Text = ""
        Try
            Dim epOffset As Long = PEScanner.GetEntryPointOffset(filePath)
            If epOffset <= 0 Then
                rtbDisasm.Text = "Entry Point bulunamadı."
                Exit Sub
            End If

            ' Phoenix gibi packerlar için en az 256 byte okuyalım
            Dim buffer(256) As Byte
            Using fs As New FileStream(filePath, FileMode.Open, FileAccess.Read)
                fs.Seek(epOffset, SeekOrigin.Begin)
                fs.Read(buffer, 0, buffer.Length)
            End Using

            ' Base Address hesapla (Default 0x400000 varsayalım, PE Headerdan almak daha doğru olur)
            Dim baseAddr As Long = &H400000
            If Not String.IsNullOrEmpty(txtEP.Text) Then
                ' EP RVA'yı baz alarak tahmini bir başlangıç yapıyoruz
                ' Gerçek ImageBase okumak için PE analizi derinleştirilmeli ama bu görsel için yeterli.
                Try : baseAddr += Convert.ToInt64(txtEP.Text, 16) : Catch : End Try
            End If

            ' Disassembler çalıştır
            rtbDisasm.Text = AdvancedDisassembler.Disassemble(buffer, baseAddr)

            ' PROFESYONEL RENKLENDİRME YAP
            ApplySyntaxHighlighting()

        Catch ex As Exception
            rtbDisasm.Text = "Disassembly Error: " & ex.Message
        End Try
    End Sub

    Sub ApplySyntaxHighlighting()
        ' Titreşimi önle
        rtbDisasm.Visible = False

        ' Tüm metni varsayılan renk yap
        rtbDisasm.SelectAll()
        rtbDisasm.SelectionColor = Color.Black
        rtbDisasm.SelectionFont = New Font("Consolas", 10, FontStyle.Regular)

        ' Kelime boyama fonksiyonu
        ColorizeWord("PUSH", Color.Teal)
        ColorizeWord("POP", Color.Teal)
        ColorizeWord("PUSHAD", Color.Purple, True) ' Packer imzası
        ColorizeWord("POPAD", Color.Purple, True)
        ColorizeWord("CALL", Color.Blue, True)
        ColorizeWord("JMP", Color.Red, True)
        ColorizeWord("JE", Color.Red)
        ColorizeWord("JNE", Color.Red)
        ColorizeWord("XOR", Color.DarkOrange)
        ColorizeWord("RET", Color.DarkRed)
        ColorizeWord("NOP", Color.Gray)

        rtbDisasm.DeselectAll()
        rtbDisasm.Visible = True
    End Sub

    Sub ColorizeWord(word As String, color As Color, Optional isBold As Boolean = False)
        Dim index As Integer = 0
        While index < rtbDisasm.TextLength
            index = rtbDisasm.Find(word, index, RichTextBoxFinds.WholeWord)
            If index = -1 Then Exit While

            rtbDisasm.SelectionStart = index
            rtbDisasm.SelectionLength = word.Length
            rtbDisasm.SelectionColor = color
            If isBold Then rtbDisasm.SelectionFont = New Font(rtbDisasm.Font, FontStyle.Bold)

            index += word.Length
        End While
    End Sub

    ' Basit Sözdizimi Renklendirme
    Sub HighlightSyntax()
        Dim keywords() As String = {"CALL", "JMP", "RET", "PUSH", "POP", "MOV"}
        For Each word In keywords
            Dim index As Integer = 0
            While index < rtbDisasm.TextLength
                index = rtbDisasm.Find(word, index, RichTextBoxFinds.None)
                If index = -1 Then Exit While
                rtbDisasm.SelectionStart = index
                rtbDisasm.SelectionLength = word.Length
                rtbDisasm.SelectionColor = Color.Blue
                rtbDisasm.SelectionFont = New Font(rtbDisasm.Font, FontStyle.Bold)
                index += word.Length
            End While
        Next
    End Sub
    ' --- YENİ ÖZELLİKLERİ ÇALIŞTIR ---
    Sub AnalyzeExtraFeatures(filePath As String)
        ' 1. BÖLÜMLERİ LİSTELE
        lvSections.Items.Clear() ' ListView ismi lvSections olsun
        lvSections.View = View.Details
        ' Sütunları Form Load'da eklemen daha iyi olur ama burada örnek olsun:
        If lvSections.Columns.Count = 0 Then
            lvSections.Columns.Add("Name", 80)
            lvSections.Columns.Add("V. Size", 80)
            lvSections.Columns.Add("R. Size", 80)
            lvSections.Columns.Add("Entropy", 60)
            lvSections.Columns.Add("Status", 100)
        End If

        Dim sections = AdvancedPEAnalyzer.GetSections(filePath)
        For Each sec In sections
            Dim item As New ListViewItem(sec.Name)
            item.SubItems.Add(sec.VirtualSize.ToString("X"))
            item.SubItems.Add(sec.RawSize.ToString("X"))
            item.SubItems.Add(sec.Entropy.ToString("0.00"))

            ' Entropi Yorumu
            If sec.Entropy > 7.0 Then
                item.SubItems.Add("Packed/Encrypted")
                item.BackColor = Color.MistyRose ' Şüpheliyse kırmızımsı yap
            ElseIf sec.Entropy > 6.5 Then
                item.SubItems.Add("Suspicious")
                item.BackColor = Color.LightYellow
            Else
                item.SubItems.Add("Normal")
            End If

            lvSections.Items.Add(item)
        Next

        ' 2. STRINGLERİ ÇIKAR
        rtbStrings.Text = ""
        rtbStrings.Text = "Extracting..."
        Application.DoEvents()
        ' En az 5 karakter uzunluğundaki yazıları çıkar
        rtbStrings.Text = AdvancedPEAnalyzer.ExtractStrings(filePath, 5)
        ShowHexDump(txtFilePath.Text)
    End Sub
    ' --- HEX VIEWER FONKSİYONU ---
    Sub ShowHexDump(filePath As String)
        rtbHex.Text = "Loading Hex View..."
        Application.DoEvents()

        Try
            Dim sb As New StringBuilder()

            ' Büyük dosyaları açarken program donmasın diye sadece ilk 16KB (16384 byte) okuyoruz.
            ' İstersen bu limiti artırabilirsin ama RichTextBox çok veriyle yavaşlar.
            Dim previewSize As Integer = 16384
            Dim buffer() As Byte

            Using fs As New FileStream(filePath, FileMode.Open, FileAccess.Read)
                ' Dosya boyutuna göre okuma miktarını ayarla
                Dim readSize As Integer = Math.Min(CInt(fs.Length), previewSize)
                ReDim buffer(readSize - 1)
                fs.Read(buffer, 0, readSize)
            End Using

            ' --- HEX FORMATLAMA DÖNGÜSÜ ---
            ' Örnek Çıktı: 00000000  4D 5A 90 00 ...  MZ..
            For i As Integer = 0 To buffer.Length - 1 Step 16

                ' 1. Adres Sütunu (Offset)
                sb.Append(i.ToString("X8") & "  ")

                ' 2. Hex Sütunu (16 Byte)
                For j As Integer = 0 To 15
                    If i + j < buffer.Length Then
                        sb.Append(buffer(i + j).ToString("X2") & " ")
                    Else
                        sb.Append("   ") ' Boşluk doldur
                    End If
                    ' 8. byte'tan sonra bir ayraç daha koy (okunabilirlik için)
                    If j = 7 Then sb.Append(" ")
                Next

                sb.Append("  ")

                ' 3. ASCII Sütunu
                For j As Integer = 0 To 15
                    If i + j < buffer.Length Then
                        Dim b As Byte = buffer(i + j)
                        ' Yazdırılabilir ASCII karakterleri (32-126 arası) göster, diğerlerine nokta koy
                        If b >= 32 AndAlso b <= 126 Then
                            sb.Append(Chr(b))
                        Else
                            sb.Append(".")
                        End If
                    End If
                Next

                sb.AppendLine() ' Satır sonu
            Next

            If buffer.Length < New FileInfo(filePath).Length Then
                sb.AppendLine(vbCrLf & "... (Preview limited to first 16KB for performance) ...")
            End If

            rtbHex.Text = sb.ToString()

        Catch ex As Exception
            rtbHex.Text = "Hex View Error: " & ex.Message
        End Try
    End Sub
End Class