Imports GabSoftware.WinControls


Public Class Form1

    Private wm_mousemove_count As Integer = 0

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked = True Then
            GabKnob1.KnobFillType = GabSoftware.WinControls.GabKnob.eKnobFillType.Solid
        End If
    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        If RadioButton2.Checked = True Then
            GabKnob1.KnobFillType = GabSoftware.WinControls.GabKnob.eKnobFillType.Gradient
            GabKnob1.Knob3ColorsMode = False
        End If
    End Sub

    Private Sub RadioButton3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton3.CheckedChanged
        If RadioButton3.Checked = True Then
            GabKnob1.KnobPositionType = GabSoftware.WinControls.GabKnob.eKnobPositionType.Rectangle
        End If
    End Sub

    Private Sub RadioButton4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton4.CheckedChanged
        If RadioButton4.Checked = True Then
            GabKnob1.KnobPositionType = GabSoftware.WinControls.GabKnob.eKnobPositionType.Circle
        End If

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim dr As DialogResult = ColorDialog1.ShowDialog
        If dr = Windows.Forms.DialogResult.OK Then
            GabKnob1.KnobColor1 = ColorDialog1.Color
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim dr As DialogResult = ColorDialog1.ShowDialog
        If dr = Windows.Forms.DialogResult.OK Then
            GabKnob1.KnobColor2 = ColorDialog1.Color
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim dr As DialogResult = ColorDialog1.ShowDialog
        If dr = Windows.Forms.DialogResult.OK Then
            GabKnob1.KnobBorderColor = ColorDialog1.Color
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim dr As DialogResult = ColorDialog1.ShowDialog
        If dr = Windows.Forms.DialogResult.OK Then
            GabKnob1.KnobPositionColor = ColorDialog1.Color
        End If
    End Sub

    Private Sub NumericUpDown2_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown2.ValueChanged
        GabKnob1.KnobPositionDistance = NumericUpDown2.Value
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.NumericUpDown1.Minimum = Me.GabKnob1.KnobAngleMin
        Me.NumericUpDown1.Maximum = Me.GabKnob1.KnobAngleMax
        Me.NumericUpDown1.Value = Me.GabKnob1.KnobAngle


        Me.GabKnob1.Value = 80

    End Sub

    Private Sub GabKnob1_KnobValueChanged(ByVal KnobValue As Integer) Handles GabKnob1.KnobValueChanged
        Me.Label3.Text = GabKnob1.Value

    End Sub

    Private Sub GabKnob1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GabKnob1.MouseDown
        Me.NumericUpDown1.Value = GabKnob1.KnobAngle
    End Sub

    Private Sub RadioButton7_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton7.CheckedChanged
        If RadioButton7.Checked = True Then
            GabKnob1.KnobMoveType = GabSoftware.WinControls.GabKnob.eKnobMoveType.Both
        End If
    End Sub

    Private Sub RadioButton5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton5.CheckedChanged
        If RadioButton5.Checked = True Then
            GabKnob1.KnobMoveType = GabSoftware.WinControls.GabKnob.eKnobMoveType.Vertical
        End If
    End Sub

    Private Sub RadioButton6_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton6.CheckedChanged
        If RadioButton6.Checked = True Then
            GabKnob1.KnobMoveType = GabSoftware.WinControls.GabKnob.eKnobMoveType.Horizontal
        End If
    End Sub

    Private Sub RadioButton8_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton8.CheckedChanged
        If RadioButton8.Checked = True Then
            GabKnob1.KnobFillType = GabSoftware.WinControls.GabKnob.eKnobFillType.Gradient
            GabKnob1.Knob3ColorsMode = True
        End If
    End Sub

    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
        Me.GabKnob1.KnobMiddleColorDistance = TrackBar1.Value / 100
    End Sub

    Private Sub NumericUpDown3_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown3.ValueChanged
        GabKnob1.KnobGradientCenterOffset = New Point(NumericUpDown3.Value, NumericUpDown4.Value)
    End Sub

    Private Sub NumericUpDown4_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown4.ValueChanged
        GabKnob1.KnobGradientCenterOffset = New Point(NumericUpDown3.Value, NumericUpDown4.Value)
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim dr As DialogResult = ColorDialog1.ShowDialog
        If dr = Windows.Forms.DialogResult.OK Then
            GabKnob1.KnobColor3 = ColorDialog1.Color
        End If
    End Sub

    'Private Sub GabKnob1_NewMsg(ByVal m As System.Windows.Forms.Message) Handles GabKnob1.NewMsg
    '    Label6.Text = m.ToString
    '    If m.Msg = &H200 Then
    '        wm_mousemove_count += 1
    '        Label7.Text = "WM_MOUSEMOVE : " & wm_mousemove_count
    '    End If
    'End Sub
End Class
