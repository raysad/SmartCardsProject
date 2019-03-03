<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
		Me.Button1 = New System.Windows.Forms.Button()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.txtCom = New System.Windows.Forms.TextBox()
		Me.txtCard = New System.Windows.Forms.TextBox()
		Me.txtReadAll = New System.Windows.Forms.TextBox()

		Me.Button2 = New System.Windows.Forms.Button()
		Me.ICC = New System.Windows.Forms.TextBox()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.SuspendLayout()
		'
		'Button1
		'
		Me.Button1.Location = New System.Drawing.Point(327, 14)
		Me.Button1.Name = "Button1"
		Me.Button1.Size = New System.Drawing.Size(89, 44)
		Me.Button1.TabIndex = 0
		Me.Button1.Text = "Detect Card"
		Me.Button1.UseVisualStyleBackColor = True
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Location = New System.Drawing.Point(7, 30)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(34, 13)
		Me.Label1.TabIndex = 1
		Me.Label1.Text = "COM:"
		'
		'txtCom
		'
		Me.txtCom.Location = New System.Drawing.Point(44, 27)
		Me.txtCom.Name = "txtCom"
		Me.txtCom.ReadOnly = True
		Me.txtCom.Size = New System.Drawing.Size(88, 20)
		Me.txtCom.TabIndex = 2

		'
		'textReadAll
		Me.txtReadAll.Location = New System.Drawing.Point(60, 27)
		Me.txtReadAll.Name = "txtReadAll"
		Me.txtReadAll.ReadOnly = True
		Me.txtReadAll.Size = New System.Drawing.Size(88, 20)
		Me.txtReadAll.TabIndex = 2
		'
		'
		'txtCard
		'
		Me.txtCard.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.txtCard.Location = New System.Drawing.Point(149, 30)
		Me.txtCard.Name = "txtCard"
		Me.txtCard.ReadOnly = True
		Me.txtCard.Size = New System.Drawing.Size(155, 13)
		Me.txtCard.TabIndex = 3
		'
		'Button2
		'
		Me.Button2.Location = New System.Drawing.Point(327, 64)
		Me.Button2.Name = "Button2"
		Me.Button2.Size = New System.Drawing.Size(89, 44)
		Me.Button2.TabIndex = 4
		Me.Button2.Text = "Read ICC"
		Me.Button2.UseVisualStyleBackColor = True
		'
		'ICC
		'
		Me.ICC.Location = New System.Drawing.Point(44, 76)
		Me.ICC.Name = "ICC"
		Me.ICC.ReadOnly = True
		Me.ICC.Size = New System.Drawing.Size(277, 20)
		Me.ICC.TabIndex = 6
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Location = New System.Drawing.Point(7, 80)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(27, 13)
		Me.Label2.TabIndex = 5
		Me.Label2.Text = "ICC:"
		'
		'Form1
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(428, 134)
		Me.Controls.Add(Me.ICC)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.Button2)
		Me.Controls.Add(Me.txtCard)
		Me.Controls.Add(Me.txtReadAll
				)
		Me.Controls.Add(Me.txtCom)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.Button1)
		Me.Name = "Form1"
		Me.Text = "VbNET using ASKReaderLib.DLL using ASKCSC.DLL"
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtCom As System.Windows.Forms.TextBox
	Friend WithEvents txtCard As System.Windows.Forms.TextBox
	Friend WithEvents txtReadAll As System.Windows.Forms.TextBox
	Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents ICC As System.Windows.Forms.TextBox
	Friend WithEvents Label2 As System.Windows.Forms.Label


End Class
