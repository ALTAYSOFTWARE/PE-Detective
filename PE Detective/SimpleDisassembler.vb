Imports System.Text

Public Class AdvancedDisassembler
    Private Shared ReadOnly Reg32 As String() = {"EAX", "ECX", "EDX", "EBX", "ESP", "EBP", "ESI", "EDI"}

    Public Shared Function Disassemble(bytes As Byte(), baseAddress As Long) As String
        Dim sb As New StringBuilder()
        Dim i As Integer = 0

        ' Başlık
        sb.AppendLine($"{"Address".PadRight(10)} | {"Hex Dump".PadRight(20)} | {"Command".PadRight(30)}")
        sb.AppendLine(New String("-"c, 70))

        While i < bytes.Length
            Try
                Dim b As Byte = bytes(i)
                Dim currentOffset As Long = baseAddress + i
                Dim addrStr As String = currentOffset.ToString("X8")
                Dim cmd As String = "???"
                Dim size As Integer = 1
                Dim operand As String = ""

                ' --- GÜVENLİ SINIR KONTROLÜ ---
                ' Eğer kalan byte sayısı işlem için yetersizse döngüyü kır
                If i >= bytes.Length Then Exit While

                Select Case b
                    ' --- 1 Byte Komutlar ---
                    Case &H90 : cmd = "NOP"
                    Case &H60 : cmd = "PUSHAD"
                    Case &H61 : cmd = "POPAD"
                    Case &HC3 : cmd = "RET"
                    Case &HC9 : cmd = "LEAVE"
                    Case &HCC : cmd = "INT 3"

                    ' --- INC / DEC Register ---
                    Case &H40 To &H47 : cmd = "INC " & Reg32(b - &H40)
                    Case &H48 To &H4F : cmd = "DEC " & Reg32(b - &H48)

                    ' --- PUSH / POP Register ---
                    Case &H50 To &H57 : cmd = "PUSH " & Reg32(b - &H50)
                    Case &H58 To &H5F : cmd = "POP " & Reg32(b - &H58)

                    ' --- PUSH Imm32 (5 Byte) ---
                    Case &H68
                        If i + 4 < bytes.Length Then
                            Dim val = BitConverter.ToUInt32(bytes, i + 1)
                            cmd = "PUSH"
                            operand = "0x" & val.ToString("X")
                            size = 5
                        End If

                    ' --- PUSH Imm8 (2 Byte) ---
                    Case &H6A
                        If i + 1 < bytes.Length Then
                            cmd = "PUSH"
                            operand = "0x" & bytes(i + 1).ToString("X2")
                            size = 2
                        End If

                    ' --- MOV Register, Imm32 (5 Byte) ---
                    Case &HB8 To &HBF
                        If i + 4 < bytes.Length Then
                            Dim val = BitConverter.ToUInt32(bytes, i + 1)
                            cmd = "MOV " & Reg32(b - &HB8) & ","
                            operand = "0x" & val.ToString("X")
                            size = 5
                        End If

                    ' --- XOR ---
                    Case &H31, &H33
                        If i + 1 < bytes.Length Then
                            cmd = "XOR"
                            operand = "REG, REG/MEM"
                            size = 2
                            If bytes(i + 1) = &HC0 Then operand = "EAX, EAX"
                        End If

                    ' --- CALL Rel32 (5 Byte) ---
                    Case &HE8
                        If i + 4 < bytes.Length Then
                            Dim offset = BitConverter.ToInt32(bytes, i + 1)
                            Dim dest = currentOffset + 5 + offset
                            cmd = "CALL"
                            operand = "0x" & dest.ToString("X8")
                            size = 5
                        End If

                    ' --- JMP Rel32 (5 Byte) ---
                    Case &HE9
                        If i + 4 < bytes.Length Then
                            Dim offset = BitConverter.ToInt32(bytes, i + 1)
                            Dim dest = currentOffset + 5 + offset
                            cmd = "JMP"
                            operand = "0x" & dest.ToString("X8")
                            size = 5
                        End If

                    ' --- JMP Short (2 Byte) ---
                    Case &HEB
                        If i + 1 < bytes.Length Then
                            ' HATA DÜZELTME: CType yerine Matematiksel Dönüşüm
                            Dim rawOffset As Integer = bytes(i + 1)
                            Dim offset As Integer = If(rawOffset > 127, rawOffset - 256, rawOffset)

                            Dim dest = currentOffset + 2 + offset
                            cmd = "JMP SHORT"
                            operand = "0x" & dest.ToString("X8")
                            size = 2
                        End If

                    ' --- CONDITIONAL JUMPS (2 Byte) ---
                    Case &H74 : ParseSafeJcc(i, bytes, currentOffset, "JE", cmd, operand, size)
                    Case &H75 : ParseSafeJcc(i, bytes, currentOffset, "JNE", cmd, operand, size)
                    Case &H72 : ParseSafeJcc(i, bytes, currentOffset, "JB", cmd, operand, size)
                    Case &H73 : ParseSafeJcc(i, bytes, currentOffset, "JNB", cmd, operand, size)
                    Case &H7F : ParseSafeJcc(i, bytes, currentOffset, "JG", cmd, operand, size)

                    ' --- MOV EBP, ESP ---
                    Case &H8B
                        If i + 1 < bytes.Length Then
                            If bytes(i + 1) = &HEC Then
                                cmd = "MOV EBP, ESP"
                            Else
                                cmd = "MOV REG, MEM"
                            End If
                            size = 2
                        End If

                    Case Else
                        cmd = "DB"
                        operand = b.ToString("X2")
                End Select

                ' --- Çıktı Formatlama ---
                Dim hexStr As String = ""
                ' Döngü taşmasını önlemek için Math.Min kullanıyoruz
                Dim printSize As Integer = Math.Min(size, bytes.Length - i)

                For k As Integer = 0 To printSize - 1
                    hexStr &= bytes(i + k).ToString("X2") & " "
                Next

                sb.AppendLine($"{addrStr} | {hexStr.PadRight(20)} | {cmd} {operand}")
                i += size

            Catch ex As Exception
                ' Tek bir satırda hata olsa bile devam et, sadece o satırı ??? yap
                sb.AppendLine($"Error at offset {i}: {ex.Message}")
                i += 1
            End Try
        End While

        Return sb.ToString()
    End Function

    ' Güvenli JCC (Koşullu Zıplama) Hesaplayıcı
    Private Shared Sub ParseSafeJcc(idx As Integer, bytes As Byte(), currentAddr As Long, name As String, ByRef cmd As String, ByRef op As String, ByRef size As Integer)
        If idx + 1 < bytes.Length Then
            ' HATA DÜZELTME: SByte taşmasını önlemek için manuel hesaplama
            Dim rawOffset As Integer = bytes(idx + 1)
            Dim offset As Integer = If(rawOffset > 127, rawOffset - 256, rawOffset)

            Dim dest = currentAddr + 2 + offset
            cmd = name
            op = "0x" & dest.ToString("X8")
            size = 2
        End If
    End Sub
End Class