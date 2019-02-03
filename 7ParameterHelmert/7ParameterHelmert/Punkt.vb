Public Class Punkt
    'Punktattribute
    Dim PunktNr As String
    Dim X_Koordinate, Y_Koordinate, Z_Koordinate As Double

    'Constructor
    Public Sub New(PunktNr As String, X_Koordinate As String, Y_Koordinate As String, Z_Koordinate As String)
        _PunktNr = PunktNr
        _X_Koordinate = X_Koordinate
        _Y_Koordinate = Y_Koordinate
        _Z_Koordinate = Z_Koordinate

    End Sub
    Public Sub New()

    End Sub
    'Getter und Setter

    Public Property _PunktNr As String
        Get
            Return PunktNr
        End Get
        Set(value As String)
            PunktNr = value
        End Set
    End Property

    Public Property _X_Koordinate As String
        Get
            Return X_Koordinate
        End Get
        Set(value As String)
            If IsNumeric(value) Then
                value = value.Replace(".", ",")
                X_Koordinate = CDbl(value)
            Else
                Throw New Exception
                MsgBox("X-Koordinate ist felherhaft")
            End If
        End Set
    End Property

    Public Property _Y_Koordinate As String
        Get
            Return Y_Koordinate
        End Get
        Set(value As String)
            If IsNumeric(value) Then
                value = value.Replace(".", ",")
                Y_Koordinate = CDbl(value)
            Else
                Throw New Exception
                MsgBox("Y-Koordinate ist felherhaft")
            End If
        End Set
    End Property

    Public Property _Z_Koordinate As String
        Get
            Return Z_Koordinate
        End Get
        Set(value As String)
            If IsNumeric(value) Then
                value = value.Replace(".", ",")
                Z_Koordinate = CDbl(value)
            Else
                Throw New Exception
                MsgBox("Z-Koordinate ist felherhaft")
            End If
        End Set
    End Property

    Public Overrides Function toString() As String
        Dim ergebnis As String = _PunktNr.ToString + "-" + _X_Koordinate.ToString + "-" + _Y_Koordinate.ToString + "-" + _Z_Koordinate.ToString
        Return ergebnis
    End Function

End Class
