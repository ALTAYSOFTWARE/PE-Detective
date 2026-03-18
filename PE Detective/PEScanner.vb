Imports System.IO
Imports System.Text

Public Class PEScanner
    ' RVA (Relative Virtual Address) adresini Dosya Ofsetine çevirir
    Public Shared Function GetEntryPointOffset(filePath As String) As Long
        Using fs As New FileStream(filePath, FileMode.Open, FileAccess.Read)
            Using br As New BinaryReader(fs)
                ' 1. DOS Header: MZ kontrolü ve e_lfanew
                If br.ReadUInt16() <> &H5A4D Then Return -1 ' MZ değil
                fs.Seek(&H3C, SeekOrigin.Begin) ' e_lfanew ofseti
                Dim peHeaderOffset As Integer = br.ReadInt32()

                ' 2. PE Header: Signature kontrolü
                fs.Seek(peHeaderOffset, SeekOrigin.Begin)
                If br.ReadUInt32() <> &H4550 Then Return -1 ' PE\0\0 değil

                ' 3. File Header
                Dim machine As UInt16 = br.ReadUInt16()
                Dim numberOfSections As UInt16 = br.ReadUInt16()
                fs.Seek(12, SeekOrigin.Current) ' TimeDateStamp, SymbolTable vs atla
                Dim sizeOfOptionalHeader As UInt16 = br.ReadUInt16()
                Dim characteristics As UInt16 = br.ReadUInt16()

                ' 4. Optional Header (Standart Alanlar)
                Dim magic As UInt16 = br.ReadUInt16()
                Dim entryPointRVA As UInt32 = 0

                ' 32-bit (0x10B) veya 64-bit (0x20B) kontrolü
                If magic = &H10B Then
                    fs.Seek(14, SeekOrigin.Current) ' CodeSize, InitDataSize vs atla
                    entryPointRVA = br.ReadUInt32()
                ElseIf magic = &H20B Then
                    fs.Seek(14, SeekOrigin.Current)
                    entryPointRVA = br.ReadUInt32()
                Else
                    Return -1 ' Bilinmeyen format
                End If

                ' Eğer EntryPoint 0 ise (örn: DLL'ler bazen)
                If entryPointRVA = 0 Then Return 0

                ' 5. Section Headers: EntryPoint hangi bölümde?
                ' Optional Header'ın sonuna git
                fs.Seek(peHeaderOffset + 4 + 20 + sizeOfOptionalHeader, SeekOrigin.Begin)

                For i As Integer = 1 To numberOfSections
                    Dim sectionName As Byte() = br.ReadBytes(8)
                    Dim virtualSize As UInt32 = br.ReadUInt32()
                    Dim virtualAddress As UInt32 = br.ReadUInt32()
                    Dim sizeOfRawData As UInt32 = br.ReadUInt32()
                    Dim pointerToRawData As UInt32 = br.ReadUInt32()
                    fs.Seek(16, SeekOrigin.Current) ' Relocations, LineNumbers, Characteristics vs atla

                    ' EntryPointRVA bu bölümün içinde mi?
                    If entryPointRVA >= virtualAddress AndAlso entryPointRVA < (virtualAddress + virtualSize) Then
                        ' Formül: DosyaOfseti = EP_RVA - SanalAdres + RawAdres
                        Return (entryPointRVA - virtualAddress) + pointerToRawData
                    End If
                Next
            End Using
        End Using
        Return -1
    End Function
End Class