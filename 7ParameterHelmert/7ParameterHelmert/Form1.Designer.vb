<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.b_quellsystem = New System.Windows.Forms.Button()
        Me.b_zielsystem = New System.Windows.Forms.Button()
        Me.tb_quellsystem = New System.Windows.Forms.TextBox()
        Me.tb_zielsystem = New System.Windows.Forms.TextBox()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.b_Transformation = New System.Windows.Forms.Button()
        Me.B_export = New System.Windows.Forms.Button()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.B_residuen = New System.Windows.Forms.Button()
        Me.B_reset = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'b_quellsystem
        '
        Me.b_quellsystem.Location = New System.Drawing.Point(39, 30)
        Me.b_quellsystem.Name = "b_quellsystem"
        Me.b_quellsystem.Size = New System.Drawing.Size(143, 53)
        Me.b_quellsystem.TabIndex = 0
        Me.b_quellsystem.Text = "Messwerte im Quellsystem auswählen"
        Me.b_quellsystem.UseVisualStyleBackColor = True
        '
        'b_zielsystem
        '
        Me.b_zielsystem.Location = New System.Drawing.Point(39, 98)
        Me.b_zielsystem.Name = "b_zielsystem"
        Me.b_zielsystem.Size = New System.Drawing.Size(143, 51)
        Me.b_zielsystem.TabIndex = 1
        Me.b_zielsystem.Text = "Koordinaten der Passpunkte im Zielsystem auswählen"
        Me.b_zielsystem.UseVisualStyleBackColor = True
        '
        'tb_quellsystem
        '
        Me.tb_quellsystem.Enabled = False
        Me.tb_quellsystem.Location = New System.Drawing.Point(229, 47)
        Me.tb_quellsystem.Name = "tb_quellsystem"
        Me.tb_quellsystem.Size = New System.Drawing.Size(380, 20)
        Me.tb_quellsystem.TabIndex = 2
        '
        'tb_zielsystem
        '
        Me.tb_zielsystem.Enabled = False
        Me.tb_zielsystem.Location = New System.Drawing.Point(229, 114)
        Me.tb_zielsystem.Name = "tb_zielsystem"
        Me.tb_zielsystem.Size = New System.Drawing.Size(380, 20)
        Me.tb_zielsystem.TabIndex = 3
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'b_Transformation
        '
        Me.b_Transformation.Enabled = False
        Me.b_Transformation.Location = New System.Drawing.Point(39, 188)
        Me.b_Transformation.Name = "b_Transformation"
        Me.b_Transformation.Size = New System.Drawing.Size(143, 51)
        Me.b_Transformation.TabIndex = 4
        Me.b_Transformation.Text = "Transformation berechnen"
        Me.b_Transformation.UseVisualStyleBackColor = True
        '
        'B_export
        '
        Me.B_export.Enabled = False
        Me.B_export.Location = New System.Drawing.Point(358, 188)
        Me.B_export.Name = "B_export"
        Me.B_export.Size = New System.Drawing.Size(119, 51)
        Me.B_export.TabIndex = 5
        Me.B_export.Text = "Transformierte Neupunte exportieren"
        Me.B_export.UseVisualStyleBackColor = True
        '
        'B_residuen
        '
        Me.B_residuen.Enabled = False
        Me.B_residuen.Location = New System.Drawing.Point(229, 188)
        Me.B_residuen.Name = "B_residuen"
        Me.B_residuen.Size = New System.Drawing.Size(104, 51)
        Me.B_residuen.TabIndex = 6
        Me.B_residuen.Text = "Residuen anzeigen"
        Me.B_residuen.UseVisualStyleBackColor = True
        '
        'B_reset
        '
        Me.B_reset.Enabled = False
        Me.B_reset.Location = New System.Drawing.Point(501, 188)
        Me.B_reset.Name = "B_reset"
        Me.B_reset.Size = New System.Drawing.Size(108, 51)
        Me.B_reset.TabIndex = 7
        Me.B_reset.Text = "Neue Transformation berechnen"
        Me.B_reset.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(621, 263)
        Me.Controls.Add(Me.B_reset)
        Me.Controls.Add(Me.B_residuen)
        Me.Controls.Add(Me.B_export)
        Me.Controls.Add(Me.b_Transformation)
        Me.Controls.Add(Me.tb_zielsystem)
        Me.Controls.Add(Me.tb_quellsystem)
        Me.Controls.Add(Me.b_zielsystem)
        Me.Controls.Add(Me.b_quellsystem)
        Me.Name = "Form1"
        Me.Text = "7 Parameter Helmerttransformation"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents b_quellsystem As Button
    Friend WithEvents b_zielsystem As Button
    Friend WithEvents tb_quellsystem As TextBox
    Friend WithEvents tb_zielsystem As TextBox
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents b_Transformation As Button
    Friend WithEvents B_export As Button
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
    Friend WithEvents B_residuen As Button
    Friend WithEvents B_reset As Button
End Class
