Imports System.Xml

Public Class SignatureItem
    Public Property Name As String
    Public Property Pattern As String
    Public Property Extension As String ' Örn: AMD64, I386

    ' Deseni byte ve maske dizisine çeviren yardımcı fonksiyon
    Public Function IsMatch(buffer As Byte()) As Boolean
        ' Hex stringi temizle (boşlukları sil)
        Dim cleanPattern As String = Pattern.Replace(" ", "").ToUpper()

        ' Desen uzunluğu buffer'dan büyükse eşleşmez
        If (cleanPattern.Length / 2) > buffer.Length Then Return False

        For i As Integer = 0 To (cleanPattern.Length / 2) - 1
            Dim hexByte As String = cleanPattern.Substring(i * 2, 2)

            ' "??" ise bu baytı atla (wildcard)
            If hexByte = "??" Then Continue For

            ' Tek soru işareti kontrolü (örn: 4?) - Nadir ama destekleyelim
            If hexByte.Contains("?") Then
                ' Karmaşık maskeleme gerekir, şimdilik ?? tam bayt desteği verelim.
                Continue For
            End If

            ' Bayt karşılaştırması
            Dim byteVal As Byte = Convert.ToByte(hexByte, 16)
            If buffer(i) <> byteVal Then Return False
        Next

        Return True
    End Function
End Class