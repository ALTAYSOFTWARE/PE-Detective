<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tabScanner = New System.Windows.Forms.TabPage()
        Me.lblDatabase = New System.Windows.Forms.Label()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnScan = New System.Windows.Forms.Button()
        Me.grpResult = New System.Windows.Forms.GroupBox()
        Me.txtResult = New System.Windows.Forms.TextBox()
        Me.grpDetails = New System.Windows.Forms.GroupBox()
        Me.txtOverlay = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtSubsystem = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtFileSize = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtOffset = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtEP = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.grpFile = New System.Windows.Forms.GroupBox()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.txtFilePath = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tabDisasm = New System.Windows.Forms.TabPage()
        Me.rtbDisasm = New System.Windows.Forms.RichTextBox()
        Me.tabSections = New System.Windows.Forms.TabPage()
        Me.lvSections = New System.Windows.Forms.ListView()
        Me.colName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colVSize = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colRSize = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colEntropy = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colStatus = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tabStrings = New System.Windows.Forms.TabPage()
        Me.rtbStrings = New System.Windows.Forms.RichTextBox()
        Me.tabHex = New System.Windows.Forms.TabPage()
        Me.rtbHex = New System.Windows.Forms.RichTextBox()
        Me.TabControl1.SuspendLayout()
        Me.tabScanner.SuspendLayout()
        Me.grpResult.SuspendLayout()
        Me.grpDetails.SuspendLayout()
        Me.grpFile.SuspendLayout()
        Me.tabDisasm.SuspendLayout()
        Me.tabSections.SuspendLayout()
        Me.tabStrings.SuspendLayout()
        Me.tabHex.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tabScanner)
        Me.TabControl1.Controls.Add(Me.tabDisasm)
        Me.TabControl1.Controls.Add(Me.tabSections)
        Me.TabControl1.Controls.Add(Me.tabStrings)
        Me.TabControl1.Controls.Add(Me.tabHex)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(550, 450)
        Me.TabControl1.TabIndex = 0
        '
        'tabScanner
        '
        Me.tabScanner.Controls.Add(Me.lblDatabase)
        Me.tabScanner.Controls.Add(Me.btnExit)
        Me.tabScanner.Controls.Add(Me.btnScan)
        Me.tabScanner.Controls.Add(Me.grpResult)
        Me.tabScanner.Controls.Add(Me.grpDetails)
        Me.tabScanner.Controls.Add(Me.grpFile)
        Me.tabScanner.Location = New System.Drawing.Point(4, 26)
        Me.tabScanner.Name = "tabScanner"
        Me.tabScanner.Padding = New System.Windows.Forms.Padding(3)
        Me.tabScanner.Size = New System.Drawing.Size(542, 420)
        Me.tabScanner.TabIndex = 0
        Me.tabScanner.Text = "Scanner"
        Me.tabScanner.UseVisualStyleBackColor = True
        '
        'lblDatabase
        '
        Me.lblDatabase.AutoSize = True
        Me.lblDatabase.ForeColor = System.Drawing.Color.Gray
        Me.lblDatabase.Location = New System.Drawing.Point(8, 390)
        Me.lblDatabase.Name = "lblDatabase"
        Me.lblDatabase.Size = New System.Drawing.Size(69, 17)
        Me.lblDatabase.TabIndex = 11
        Me.lblDatabase.Text = "DB Status"
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(453, 365)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(75, 43)
        Me.btnExit.TabIndex = 10
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnScan
        '
        Me.btnScan.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
        Me.btnScan.Location = New System.Drawing.Point(372, 365)
        Me.btnScan.Name = "btnScan"
        Me.btnScan.Size = New System.Drawing.Size(75, 43)
        Me.btnScan.TabIndex = 9
        Me.btnScan.Text = "Scan"
        Me.btnScan.UseVisualStyleBackColor = True
        '
        'grpResult
        '
        Me.grpResult.Controls.Add(Me.txtResult)
        Me.grpResult.Location = New System.Drawing.Point(8, 237)
        Me.grpResult.Name = "grpResult"
        Me.grpResult.Size = New System.Drawing.Size(526, 122)
        Me.grpResult.TabIndex = 8
        Me.grpResult.TabStop = False
        Me.grpResult.Text = "Scan Result"
        '
        'txtResult
        '
        Me.txtResult.BackColor = System.Drawing.Color.White
        Me.txtResult.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtResult.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtResult.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
        Me.txtResult.ForeColor = System.Drawing.Color.Blue
        Me.txtResult.Location = New System.Drawing.Point(3, 20)
        Me.txtResult.Multiline = True
        Me.txtResult.Name = "txtResult"
        Me.txtResult.ReadOnly = True
        Me.txtResult.Size = New System.Drawing.Size(520, 99)
        Me.txtResult.TabIndex = 0
        Me.txtResult.Text = "Ready to scan..."
        Me.txtResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'grpDetails
        '
        Me.grpDetails.Controls.Add(Me.txtOverlay)
        Me.grpDetails.Controls.Add(Me.Label6)
        Me.grpDetails.Controls.Add(Me.txtSubsystem)
        Me.grpDetails.Controls.Add(Me.Label5)
        Me.grpDetails.Controls.Add(Me.txtFileSize)
        Me.grpDetails.Controls.Add(Me.Label4)
        Me.grpDetails.Controls.Add(Me.txtOffset)
        Me.grpDetails.Controls.Add(Me.Label3)
        Me.grpDetails.Controls.Add(Me.txtEP)
        Me.grpDetails.Controls.Add(Me.Label2)
        Me.grpDetails.Location = New System.Drawing.Point(8, 80)
        Me.grpDetails.Name = "grpDetails"
        Me.grpDetails.Size = New System.Drawing.Size(526, 140)
        Me.grpDetails.TabIndex = 7
        Me.grpDetails.TabStop = False
        Me.grpDetails.Text = "PE Details"
        '
        'txtOverlay
        '
        Me.txtOverlay.BackColor = System.Drawing.Color.White
        Me.txtOverlay.Location = New System.Drawing.Point(312, 75)
        Me.txtOverlay.Name = "txtOverlay"
        Me.txtOverlay.ReadOnly = True
        Me.txtOverlay.Size = New System.Drawing.Size(122, 24)
        Me.txtOverlay.TabIndex = 9
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(245, 78)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(64, 17)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "Overlay :"
        '
        'txtSubsystem
        '
        Me.txtSubsystem.BackColor = System.Drawing.Color.White
        Me.txtSubsystem.Location = New System.Drawing.Point(312, 49)
        Me.txtSubsystem.Name = "txtSubsystem"
        Me.txtSubsystem.ReadOnly = True
        Me.txtSubsystem.Size = New System.Drawing.Size(122, 24)
        Me.txtSubsystem.TabIndex = 7
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(234, 52)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(85, 17)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Subsystem :"
        '
        'txtFileSize
        '
        Me.txtFileSize.BackColor = System.Drawing.Color.White
        Me.txtFileSize.Location = New System.Drawing.Point(312, 23)
        Me.txtFileSize.Name = "txtFileSize"
        Me.txtFileSize.ReadOnly = True
        Me.txtFileSize.Size = New System.Drawing.Size(122, 24)
        Me.txtFileSize.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(245, 26)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(62, 17)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "File Size :"
        '
        'txtOffset
        '
        Me.txtOffset.BackColor = System.Drawing.Color.White
        Me.txtOffset.Location = New System.Drawing.Point(85, 49)
        Me.txtOffset.Name = "txtOffset"
        Me.txtOffset.ReadOnly = True
        Me.txtOffset.Size = New System.Drawing.Size(120, 24)
        Me.txtOffset.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 52)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(75, 17)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "File Offset :"
        '
        'txtEP
        '
        Me.txtEP.BackColor = System.Drawing.Color.White
        Me.txtEP.Location = New System.Drawing.Point(85, 23)
        Me.txtEP.Name = "txtEP"
        Me.txtEP.ReadOnly = True
        Me.txtEP.Size = New System.Drawing.Size(120, 24)
        Me.txtEP.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 26)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(86, 17)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Entry Point :"
        '
        'grpFile
        '
        Me.grpFile.Controls.Add(Me.btnBrowse)
        Me.grpFile.Controls.Add(Me.txtFilePath)
        Me.grpFile.Controls.Add(Me.Label1)
        Me.grpFile.Location = New System.Drawing.Point(8, 6)
        Me.grpFile.Name = "grpFile"
        Me.grpFile.Size = New System.Drawing.Size(526, 56)
        Me.grpFile.TabIndex = 6
        Me.grpFile.TabStop = False
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(485, 19)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(35, 23)
        Me.btnBrowse.TabIndex = 2
        Me.btnBrowse.Text = "..."
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'txtFilePath
        '
        Me.txtFilePath.BackColor = System.Drawing.Color.WhiteSmoke
        Me.txtFilePath.Location = New System.Drawing.Point(44, 21)
        Me.txtFilePath.Name = "txtFilePath"
        Me.txtFilePath.ReadOnly = True
        Me.txtFilePath.Size = New System.Drawing.Size(435, 24)
        Me.txtFilePath.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(35, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "File :"
        '
        'tabDisasm
        '
        Me.tabDisasm.Controls.Add(Me.rtbDisasm)
        Me.tabDisasm.Location = New System.Drawing.Point(4, 26)
        Me.tabDisasm.Name = "tabDisasm"
        Me.tabDisasm.Padding = New System.Windows.Forms.Padding(3)
        Me.tabDisasm.Size = New System.Drawing.Size(542, 420)
        Me.tabDisasm.TabIndex = 1
        Me.tabDisasm.Text = "Disassembler"
        Me.tabDisasm.UseVisualStyleBackColor = True
        '
        'rtbDisasm
        '
        Me.rtbDisasm.BackColor = System.Drawing.Color.White
        Me.rtbDisasm.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtbDisasm.Font = New System.Drawing.Font("Consolas", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
        Me.rtbDisasm.Location = New System.Drawing.Point(3, 3)
        Me.rtbDisasm.Name = "rtbDisasm"
        Me.rtbDisasm.ReadOnly = True
        Me.rtbDisasm.Size = New System.Drawing.Size(536, 414)
        Me.rtbDisasm.TabIndex = 0
        Me.rtbDisasm.Text = ""
        '
        'tabSections
        '
        Me.tabSections.Controls.Add(Me.lvSections)
        Me.tabSections.Location = New System.Drawing.Point(4, 26)
        Me.tabSections.Name = "tabSections"
        Me.tabSections.Size = New System.Drawing.Size(542, 420)
        Me.tabSections.TabIndex = 2
        Me.tabSections.Text = "Sections & Entropy"
        Me.tabSections.UseVisualStyleBackColor = True
        '
        'lvSections
        '
        Me.lvSections.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colName, Me.colVSize, Me.colRSize, Me.colEntropy, Me.colStatus})
        Me.lvSections.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvSections.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
        Me.lvSections.FullRowSelect = True
        Me.lvSections.GridLines = True
        Me.lvSections.HideSelection = False
        Me.lvSections.Location = New System.Drawing.Point(0, 0)
        Me.lvSections.Name = "lvSections"
        Me.lvSections.Size = New System.Drawing.Size(542, 420)
        Me.lvSections.TabIndex = 0
        Me.lvSections.UseCompatibleStateImageBehavior = False
        Me.lvSections.View = System.Windows.Forms.View.Details
        '
        'colName
        '
        Me.colName.Text = "Name"
        Me.colName.Width = 80
        '
        'colVSize
        '
        Me.colVSize.Text = "V. Size"
        Me.colVSize.Width = 80
        '
        'colRSize
        '
        Me.colRSize.Text = "Raw Size"
        Me.colRSize.Width = 80
        '
        'colEntropy
        '
        Me.colEntropy.Text = "Entropy"
        Me.colEntropy.Width = 70
        '
        'colStatus
        '
        Me.colStatus.Text = "Status"
        Me.colStatus.Width = 120
        '
        'tabStrings
        '
        Me.tabStrings.Controls.Add(Me.rtbStrings)
        Me.tabStrings.Location = New System.Drawing.Point(4, 26)
        Me.tabStrings.Name = "tabStrings"
        Me.tabStrings.Size = New System.Drawing.Size(542, 420)
        Me.tabStrings.TabIndex = 3
        Me.tabStrings.Text = "Strings"
        Me.tabStrings.UseVisualStyleBackColor = True
        '
        'rtbStrings
        '
        Me.rtbStrings.BackColor = System.Drawing.Color.White
        Me.rtbStrings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtbStrings.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
        Me.rtbStrings.Location = New System.Drawing.Point(0, 0)
        Me.rtbStrings.Name = "rtbStrings"
        Me.rtbStrings.ReadOnly = True
        Me.rtbStrings.Size = New System.Drawing.Size(542, 420)
        Me.rtbStrings.TabIndex = 0
        Me.rtbStrings.Text = ""
        '
        'tabHex
        '
        Me.tabHex.Controls.Add(Me.rtbHex)
        Me.tabHex.Location = New System.Drawing.Point(4, 26)
        Me.tabHex.Name = "tabHex"
        Me.tabHex.Size = New System.Drawing.Size(542, 420)
        Me.tabHex.TabIndex = 4
        Me.tabHex.Text = "Hex View"
        Me.tabHex.UseVisualStyleBackColor = True
        '
        'rtbHex
        '
        Me.rtbHex.BackColor = System.Drawing.Color.White
        Me.rtbHex.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtbHex.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
        Me.rtbHex.Location = New System.Drawing.Point(0, 0)
        Me.rtbHex.Name = "rtbHex"
        Me.rtbHex.ReadOnly = True
        Me.rtbHex.Size = New System.Drawing.Size(542, 420)
        Me.rtbHex.TabIndex = 0
        Me.rtbHex.Text = ""
        '
        'Form1
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(550, 450)
        Me.Controls.Add(Me.TabControl1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Advanced Packer Detector v3.0 (Hex Edition)"
        Me.TabControl1.ResumeLayout(False)
        Me.tabScanner.ResumeLayout(False)
        Me.tabScanner.PerformLayout()
        Me.grpResult.ResumeLayout(False)
        Me.grpResult.PerformLayout()
        Me.grpDetails.ResumeLayout(False)
        Me.grpDetails.PerformLayout()
        Me.grpFile.ResumeLayout(False)
        Me.grpFile.PerformLayout()
        Me.tabDisasm.ResumeLayout(False)
        Me.tabSections.ResumeLayout(False)
        Me.tabStrings.ResumeLayout(False)
        Me.tabHex.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabScanner As System.Windows.Forms.TabPage
    Friend WithEvents tabDisasm As System.Windows.Forms.TabPage
    Friend WithEvents grpFile As System.Windows.Forms.GroupBox
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents txtFilePath As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents grpDetails As System.Windows.Forms.GroupBox
    Friend WithEvents txtSubsystem As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtFileSize As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtOffset As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtEP As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents grpResult As System.Windows.Forms.GroupBox
    Friend WithEvents txtResult As System.Windows.Forms.TextBox
    Friend WithEvents btnScan As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents lblDatabase As System.Windows.Forms.Label
    Friend WithEvents rtbDisasm As System.Windows.Forms.RichTextBox
    Friend WithEvents tabSections As System.Windows.Forms.TabPage
    Friend WithEvents tabStrings As System.Windows.Forms.TabPage
    Friend WithEvents lvSections As System.Windows.Forms.ListView
    Friend WithEvents colName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colVSize As System.Windows.Forms.ColumnHeader
    Friend WithEvents colRSize As System.Windows.Forms.ColumnHeader
    Friend WithEvents colEntropy As System.Windows.Forms.ColumnHeader
    Friend WithEvents colStatus As System.Windows.Forms.ColumnHeader
    Friend WithEvents rtbStrings As System.Windows.Forms.RichTextBox
    Friend WithEvents txtOverlay As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents tabHex As System.Windows.Forms.TabPage
    Friend WithEvents rtbHex As System.Windows.Forms.RichTextBox
End Class