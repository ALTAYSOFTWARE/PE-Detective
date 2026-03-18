Imports System.IO
Imports System.Text

Public Class AdvancedPEAnalyzer

    Public Structure SectionInfo
        Dim Name As String
        Dim VirtualSize As UInteger
        Dim VirtualAddress As UInteger
        Dim RawSize As UInteger
        Dim RawAddress As UInteger
        Dim Entropy As Double
        Dim Characteristics As UInteger
    End Structure

    ' --- SHANNON ENTROPİ HESAPLAYICI ---
    Public Shared Function CalculateEntropy(data As Byte()) As Double
        Dim frequencies(255) As Integer
        For Each b As Byte In data
            frequencies(b) += 1
        Next

        Dim entropy As Double = 0
        Dim totalBytes As Integer = data.Length

        For Each count As Integer In frequencies
            If count > 0 Then
                Dim probability As Double = count / totalBytes
                entropy -= probability * Math.Log(probability, 2)
            End If
        Next

        Return entropy
    End Function

    ' --- BÖLÜMLERİ VE ENTROPİLERİNİ OKU ---
    Public Shared Function GetSections(filePath As String) As List(Of SectionInfo)
        Dim sections As New List(Of SectionInfo)

        Using fs As New FileStream(filePath, FileMode.Open, FileAccess.Read)
            Using br As New BinaryReader(fs)
                ' DOS Header
                If br.ReadUInt16() <> &H5A4D Then Return sections
                fs.Seek(&H3C, SeekOrigin.Begin)
                Dim peOffset As Integer = br.ReadInt32()

                ' PE Header
                fs.Seek(peOffset, SeekOrigin.Begin)
                If br.ReadUInt32() <> &H4550 Then Return sections

                ' File Header
                Dim machine As UShort = br.ReadUInt16()
                Dim numberOfSections As UShort = br.ReadUInt16()
                fs.Seek(12, SeekOrigin.Current)
                Dim sizeOfOptionalHeader As UShort = br.ReadUInt16()
                fs.Seek(2, SeekOrigin.Current) ' Characteristics

                ' Optional Header'ı atla (Section Table hemen ondan sonra gelir)
                fs.Seek(sizeOfOptionalHeader, SeekOrigin.Current)

                ' --- BÖLÜM TABLOSUNU OKU ---
                For i As Integer = 1 To numberOfSections
                    Dim sec As New SectionInfo()

                    ' İsim (8 Byte, null terminated)
                    Dim nameBytes As Byte() = br.ReadBytes(8)
                    sec.Name = Encoding.ASCII.GetString(nameBytes).Trim(vbNullChar)

                    sec.VirtualSize = br.ReadUInt32()
                    sec.VirtualAddress = br.ReadUInt32()
                    sec.RawSize = br.ReadUInt32()
                    sec.RawAddress = br.ReadUInt32()

                    fs.Seek(12, SeekOrigin.Current) ' Relocations, LineNumbers vs atla
                    sec.Characteristics = br.ReadUInt32()

                    ' ENTROPİ HESAPLA
                    ' Mevcut pozisyonu kaydet
                    Dim currentPos As Long = fs.Position

                    ' Eğer RawSize > 0 ise o bölümün içeriğini oku
                    If sec.RawSize > 0 AndAlso sec.RawAddress > 0 Then
                        Try
                            fs.Seek(sec.RawAddress, SeekOrigin.Begin)
                            Dim buffer As Byte() = br.ReadBytes(CInt(sec.RawSize))
                            sec.Entropy = CalculateEntropy(buffer)
                        Catch
                            sec.Entropy = 0
                        End Try
                    End If

                    ' Kaldığımız yere geri dön
                    fs.Seek(currentPos, SeekOrigin.Begin)

                    sections.Add(sec)
                Next
            End Using
        End Using

        Return sections
    End Function

    ' --- STRING ÇIKARICI ---
    Public Shared Function ExtractStrings(filePath As String, minLength As Integer) As String
        Dim sb As New StringBuilder()
        Dim buffer As Byte() = File.ReadAllBytes(filePath)
        Dim asciiCount As Integer = 0
        Dim currentStr As New StringBuilder()

        For Each b As Byte In buffer
            ' Yazdırılabilir karakterler (ASCII 32-126)
            If b >= 32 AndAlso b <= 126 Then
                currentStr.Append(Chr(b))
            Else
                If currentStr.Length >= minLength Then
                    sb.AppendLine(currentStr.ToString())
                End If
                currentStr.Clear()
            End If
        Next

        ' Son kalanı da ekle
        If currentStr.Length >= minLength Then sb.AppendLine(currentStr.ToString())

        Return sb.ToString()
    End Function
    ' --- OVERLAY HESAPLAYICI ---
    Public Shared Function GetOverlaySize(filePath As String) As Long
        Dim maxPointer As Long = 0
        Dim fileSize As Long = New FileInfo(filePath).Length

        Using fs As New FileStream(filePath, FileMode.Open, FileAccess.Read)
            Using br As New BinaryReader(fs)
                ' DOS & PE Header Kontrolleri
                If br.ReadUInt16() <> &H5A4D Then Return 0
                fs.Seek(&H3C, SeekOrigin.Begin)
                Dim peOffset As Integer = br.ReadInt32()
                fs.Seek(peOffset, SeekOrigin.Begin)
                If br.ReadUInt32() <> &H4550 Then Return 0

                ' File Header
                Dim machine As UShort = br.ReadUInt16()
                Dim numberOfSections As UShort = br.ReadUInt16()
                fs.Seek(12, SeekOrigin.Current)
                Dim sizeOfOptionalHeader As UShort = br.ReadUInt16()
                fs.Seek(2, SeekOrigin.Current)

                ' Section Table'a git
                fs.Seek(sizeOfOptionalHeader, SeekOrigin.Current)

                ' Tüm bölümleri gez ve en son biten bölümü bul
                For i As Integer = 1 To numberOfSections
                    fs.Seek(8 + 4 + 4, SeekOrigin.Current) ' Name, VirtualSize, VirtualAddress atla
                    Dim sizeOfRawData As UInt32 = br.ReadUInt32()
                    Dim pointerToRawData As UInt32 = br.ReadUInt32()
                    fs.Seek(16, SeekOrigin.Current) ' Diğerleri atla

                    Dim endOfSection As Long = pointerToRawData + sizeOfRawData
                    If endOfSection > maxPointer Then
                        maxPointer = endOfSection
                    End If
                Next
            End Using
        End Using

        ' Eğer dosya boyutu, son bölümden büyükse fark Overlay'dir
        If fileSize > maxPointer Then
            Return fileSize - maxPointer
        Else
            Return 0
        End If
    End Function
End Class