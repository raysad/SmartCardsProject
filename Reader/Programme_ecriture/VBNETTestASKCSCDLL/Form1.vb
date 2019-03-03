Public Class Form1

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim SearchExtender As AskReaderLib.CSC.sCARD_SearchExtTag
        Dim Status As Integer
        Dim ATR(200) As Byte
        Dim lgATR As Integer
        lgATR = 200
        Dim Com As Integer
        Dim SearchMask As Integer

        txtCom.Text = ""
		txtCard.Text = ""
		txtReadAll.Text = ""
		Try
            AskReaderLib.CSC.SearchCSC()
            ' user can also use line below to speed up coupler connection
            ' AskReaderLib.CSC.Open ("COM2");

            ' Define type of card to be detected: number of occurence for each loop
            SearchExtender.CONT = 0
            SearchExtender.ISOB = 2
            SearchExtender.ISOA = 2
            SearchExtender.TICK = 1
			SearchExtender.INNO = 2
			SearchExtender.MIFARE = 0
            SearchExtender.MV4k = 0
            SearchExtender.MV5k = 0
            SearchExtender.MONO = 0

			' Define type of card to be detected
			SearchMask = AskReaderLib.CSC.SEARCH_MASK_ISOB Or AskReaderLib.CSC.SEARCH_MASK_ISOA

			Status = AskReaderLib.CSC.SearchCardExt(SearchExtender, SearchMask, 1, 20, Com, lgATR, ATR)

            If (Status <> AskReaderLib.CSC.RCSC_Ok) Then
                txtCom.Text = "Error :" & Status
            Else
                txtCom.Text = Hex$(Com)
            End If

			If (Com = 2) Then
				txtCard.Text = "ISO14443A-4 no Calypso "



			ElseIf (Com = 4) Then
				txtCard.Text = "ISOB14443B-4 Calypso"
			ElseIf (Com = 8) Then
				txtCard.Text = "ISOB14443A-3"
			ElseIf (Com = 9) Then
				txtCard.Text = "ISOB14443B-4 Calypso"
			ElseIf (Com = 12) Then
				txtCard.Text = "ISO14443A-4 Calypso"
			ElseIf (Com = &H6F) Then
				txtCard.Text = "Card not found"
			Else
				txtCard.Text = ""
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim ApduCom As Byte() = New Byte(4) {CByte(&H94), CByte(&HB2), CByte(&H1), CByte(&H14), CByte(&H1D)}
        Dim ApduResp(31) As Byte
        Dim lenApduResp As Integer
        lenApduResp = 100
        Dim StatusAPDU As Integer

        ICC.Text = ""
        Try
			StatusAPDU = AskReaderLib.CSC.CSC_ISOCommand(ApduCom, 5, ApduResp, lenApduResp)



		Catch ex As Exception
        End Try
        If (StatusAPDU = AskReaderLib.CSC.RCSC_Ok) And (lenApduResp <> 0) Then
            ICC.Text = AskReaderLib.CSC.ToStringN(ApduResp)
        End If
    End Sub
    Private Sub Form1_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        AskReaderLib.CSC.Close()
    End Sub

	Private Sub ICC_TextChanged(sender As Object, e As EventArgs) Handles ICC.TextChanged

	End Sub
End Class
