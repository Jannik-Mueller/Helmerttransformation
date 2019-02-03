Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Math
Imports System.Collections.Generic
Imports System.Linq
Imports Extreme.Mathematics
Imports Extreme.Mathematics.LinearAlgebra



Public Class Form1

    Dim MesswerteEingang As New List(Of Punkt)
    Dim PasspunkteEingang As New List(Of Punkt)
    Dim Passpunkteunreduziert As New List(Of Punkt)
    Dim messwerteunreduziert As New List(Of Punkt)
    Dim Neupunkte As New List(Of Punkt)
    Dim schwerpunktPasspunkte As New Punkt()
    Dim schwerpunktmesswerte As New Punkt()
    Dim messwerteschwerpunktreduziert As New List(Of Punkt)
    Dim passpunkteschwerpunktreduzuert As New List(Of Punkt)
    Dim messwerteunreduziertohneneupunkte As New List(Of Punkt)



    Private Sub b_quellsystem_Click(sender As Object, e As EventArgs) Handles b_quellsystem.Click
        tb_quellsystem.Text = einlesen(MesswerteEingang)
        'Prüfung, ob die Pfadauswahl der beiden Dateien ok ist 
        If istEingangok() = True Then
            If tb_zielsystem.Text <> "" Then
                b_Transformation.Enabled = True
            End If

        Else

                MsgBox("Die dateien Dürfen nicht identisch sein")
            b_Transformation.Enabled = False
        End If
        'https://www.extremeoptimization.com/Documentation/Vector-And-Matrix/Matrices/Solving-Least-Squares-Problems.aspx



    End Sub

    Private Function istEingangok() As Boolean
        ' Funktion zur Überprüfung des Pfades, damit die Gegebenen Dateien unterschiedlich sind
        If tb_quellsystem.Text = tb_zielsystem.Text Then
            Return False
        Else Return True
        End If

    End Function

    Private Function einlesen(Werte As List(Of Punkt)) As String  ' Einlesefunktion
        Dim code As DialogResult
        Dim openfiledialog1 As New OpenFileDialog
        openfiledialog1.Filter = "XML|*.xml"
        openfiledialog1.CheckFileExists = True
        code = openfiledialog1.ShowDialog()
        Dim Pfad As String = openfiledialog1.FileName()
        Try
            Dim xmldoc As XmlDocument
            Dim nodelist As XmlNodeList
            Dim node As XmlNode
            Dim count As Integer = 0

            xmldoc = New XmlDocument
            xmldoc.Load(openfiledialog1.FileName())
            nodelist = xmldoc.SelectNodes("/Koordinaten/Punkt")

            'Punktzuordnung in die Aufgerufene Liste
            For Each node In nodelist
                Dim PunktNr As String = node.ChildNodes.Item(0).InnerText
                Dim X_Koordinate As String = node.ChildNodes.Item(1).InnerText
                Dim Y_Koordinate As String = node.ChildNodes.Item(2).InnerText
                Dim Z_Koordinate As String = node.ChildNodes.Item(3).InnerText
                count += 1

                Werte.Add(New Punkt(PunktNr, X_Koordinate, Y_Koordinate, Z_Koordinate))
            Next

            MsgBox("Es wurden " + count.ToString + " Punkte eingelesen")
            Return Pfad
        Catch ex As Exception
            MsgBox("Fehler in der XML-Datei")
            Return ""
        End Try

    End Function

    Private Sub b_zielsystem_Click(sender As Object, e As EventArgs) Handles b_zielsystem.Click
        'Prüfung, ob die Pfadauswahl der beiden Dateien ok ist 
        tb_zielsystem.Text = einlesen(PasspunkteEingang)
        If istEingangok() = True Then
            If tb_quellsystem.Text <> "" Then
                b_Transformation.Enabled = True
            End If

        Else
            MsgBox("Die dateien Dürfen nicht identisch sein")
            b_Transformation.Enabled = False

        End If

    End Sub

    Private Function identischePunkte() As String()
        ' Funktion zur bestimmung Identischer Punkte in den Dateien anhand ihrer Punktnummer, ermittlung der Punkte welche mit den später errechneten Parameter in das Zielsystem überführt werden müssen
        Dim count As Integer = 0
        Dim neupunktcount As Integer = 0
        Dim gleichePunte As String = ""
        Dim ergebnis(1) As String

        For Each punkt In PasspunkteEingang
            For Each messwert In MesswerteEingang
                If punkt._PunktNr = messwert._PunktNr Then
                    count += 1
                    gleichePunte = gleichePunte + vbNewLine + messwert._PunktNr

                    'Füllen einler liste, welche nur die unreduzierten punkte enthält, mit Messwerten welche die gleich Punktnummern haben wie die Passpunkte
                    'Diese werden nachher für die Ausgleichung benötigt
                    messwerteunreduziertohneneupunkte.Add(messwert)
                End If
            Next


        Next
        ergebnis(0) = count
        ergebnis(1) = gleichePunte
        MsgBox("Es wurden " + count.ToString + " Passpunkte für beide Systeme gefunden. Diese sind:" + vbNewLine + gleichePunte)
        Return ergebnis
    End Function

    Private Sub b_residuen_Click(sender As Object, e As EventArgs) Handles b_Transformation.Click
        If tb_quellsystem.Text = "" Or tb_zielsystem.Text = "" Then
            MsgBox("Es müssen 2 Dateipfade angegeben werden")
            Return
        End If
        If PasspunkteEingang.Count > MesswerteEingang.Count Then
            MsgBox("Die Anzahl der Passpunkte im Zielsystem ist größer der Messpunkte im Quellsystem." + vbNewLine + "Eventuell wurden die Dateien vertauscht.")
            Return
        End If
        If identischePunkte(0) < 3 Then
            MsgBox("Es werden mindestens 3 Passpunkte für die 3D-Transformation benötitgt")
            Return
        End If

        'Berechnung der Schwerpunkte in beiden Systemen
        schwerpunkte()
        'Berechnung der Schwerpunktreduzierten Koordinaten der für die Transformation benötigten Koordinaten des Quellsystems
        schwerpunktreduzierung(schwerpunktmesswerte, messwerteunreduziert, messwerteschwerpunktreduziert)
        'Berechnung der SChwerpunktreduzierten Koordinaten der für die Transformation benötigten Koordinaten des Zielsystems
        schwerpunktreduzierung(schwerpunktPasspunkte, Passpunkteunreduziert, passpunkteschwerpunktreduzuert)

        ' Sortieren der Passpunkte und Messpunkte um die gleiche Reihenfolge in den Listen nach der Punktnummer zu haben 
        messwerteschwerpunktreduziert = messwerteschwerpunktreduziert.OrderBy(Function(messwerteschwerpunktreduziert) messwerteschwerpunktreduziert._PunktNr).ToList
        passpunkteschwerpunktreduzuert = passpunkteschwerpunktreduzuert.OrderBy(Function(passpunkteschwerpunktreduzuert) passpunkteschwerpunktreduzuert._PunktNr).ToList

        'Aufruf der Funktion um die Transformationsparameter zu berechnen+ speichern der Transformationsparameter in einem Array
        Dim parameter As Double() = funktionalesModell(messwerteschwerpunktreduziert, passpunkteschwerpunktreduzuert, schwerpunktmesswerte, schwerpunktPasspunkte, messwerteunreduziertohneneupunkte)

        'Berechnen der Neupunktkoordinaten anhand der eingangskoordinaten und der Transformationsparameter
        MsgBox("Es wurden die Koordinaten von " + neupunkteberechnen(MesswerteEingang, parameter, Neupunkte).ToString + " Punkten transformiert")

        'Enabeln/Disabel von Buttons
        B_export.Enabled = True
        B_residuen.Enabled = True
        b_quellsystem.Enabled = False
        b_Transformation.Enabled = False
        b_zielsystem.Enabled = False
        B_reset.Enabled = True
    End Sub
    Function residuenanzeigen(passpunkte As List(Of Punkt), neupunkte As List(Of Punkt)) As String
        'Die residuen werden für jeden Neupunkt berechnet, welcher die gleiche Punktnummer hat wie einer der Passpunkte
        'Die Differenz zwischen Passpunkt und Neupunkt (soll-ist) wird in einem string gespeichert und als return Wert von der Funktion abgegeben
        Dim rückgabe As String = "Die Residuen betragen :" + vbNewLine
        For Each passpunkt In passpunkte
            For Each neupunkt In neupunkte
                If passpunkt._PunktNr = neupunkt._PunktNr Then
                    Dim x As Double = passpunkt._X_Koordinate - neupunkt._X_Koordinate
                    Dim y As Double = passpunkt._Y_Koordinate - neupunkt._Y_Koordinate
                    Dim z As Double = passpunkt._Z_Koordinate - neupunkt._Z_Koordinate

                    rückgabe += "PunktNr: " + passpunkt._PunktNr.ToString + ", ΔX: " + x.ToString("0.000") + "m" + ", ΔY: " + y.ToString("0.000") + "m" + ", ΔZ: " + z.ToString("0.000") + "m" + vbNewLine
                End If
            Next
        Next
        Return rückgabe
    End Function
    Function neupunkteberechnen(messwerte As List(Of Punkt), parameter As Double(), neupunkte As List(Of Punkt)) As Integer

        ' Befüllen der Rotationsmatrix aus dem Parameterarray
        Dim mR = Matrix.Create(Of Double)(3, 3)
        mR(0, 0) = parameter(3)
        mR(0, 1) = parameter(6) / 180 * Math.PI
        mR(0, 2) = -parameter(5) / 180 * Math.PI
        mR(1, 0) = -parameter(6) / 180 * Math.PI
        mR(1, 1) = parameter(3)
        mR(1, 2) = parameter(4) / 180 * Math.PI
        mR(2, 0) = parameter(5) / 180 * Math.PI
        mR(2, 1) = -parameter(4) / 180 * Math.PI
        mR(2, 2) = parameter(3)

        'Befüllen des Translationsvektors aus dem Parameterarray
        Dim translation = Matrix.Create(Of Double)(3, 1)
        translation(0, 0) = parameter(0)
        translation(1, 0) = parameter(1)
        translation(2, 0) = parameter(2)

        ' Deklarieren von Hilfsvariablen
        Dim count As Integer = 0
        Dim punktvektor = Matrix.Create(Of Double)(3, 1)
        Dim neupunktvektor = Matrix.Create(Of Double)(3, 1)

        'Koordinatentransformation für jeden Punkt
        For Each punkt In messwerte
            punktvektor(0, 0) = punkt._X_Koordinate
            punktvektor(1, 0) = punkt._Y_Koordinate
            punktvektor(2, 0) = punkt._Z_Koordinate

            neupunktvektor = translation + Matrix.Multiply(mR, punktvektor)

            'Speichern der transformierten Koordinaten in der Liste: Neuepunkte
            neupunkte.Add(New Punkt(punkt._PunktNr, neupunktvektor(0, 0).ToString("0.000"), neupunktvektor(1, 0).ToString("0.000"), neupunktvektor(2, 0).ToString("0.000")))
            count += 1

        Next
        Return count

    End Function

    Function funktionalesModell(messwerte As List(Of Punkt), passpunkte As List(Of Punkt), schwerpunktmesswerte As Punkt, schwerpunktpasspunkte As Punkt, Messwerteeingang As List(Of Punkt)) As Double()
        ' Aufstellen der A- MAtrix
        Dim A = Matrix.Create(Of Double)(passpunkte.Count * 3, 4)
        Dim zähler As Integer = 0
        For i As Integer = 0 To passpunkte.Count * 3 - 1

            A(i, 0) = messwerte(zähler)._X_Koordinate
            A(i, 1) = 0
            A(i, 2) = -messwerte(zähler)._Z_Koordinate
            A(i, 3) = messwerte(zähler)._Y_Koordinate

            A(i + 1, 0) = messwerte(zähler)._Y_Koordinate
            A(i + 1, 1) = messwerte(zähler)._Z_Koordinate
            A(i + 1, 2) = 0
            A(i + 1, 3) = -messwerte(zähler)._X_Koordinate

            A(i + 2, 0) = messwerte(zähler)._Z_Koordinate
            A(i + 2, 1) = -messwerte(zähler)._Y_Koordinate
            A(i + 2, 2) = messwerte(zähler)._X_Koordinate
            A(i + 2, 3) = 0

            i += 2
            zähler += 1

        Next

        'Variablen deklarieren, welche nach dem Iterationsprozess noch benötigt werden
        Dim translationen = Matrix.Create(Of Double)(3, 1)
        Dim s0 As Double = 0
        Dim s_alpha As Double = 0
        Dim s_beta As Double = 0
        Dim s_gamma As Double = 0
        Dim s_m As Double = 0
        Dim m As Double
        Dim alpha As Double
        Dim beta As Double
        Dim gamma As Double
        'Iterationsparameter deklarieren
        Dim iterationsanzahl As Integer = 0
        Dim iterationsgrenze As Double = 3

        'Ausgleichung bis die Genauigkeit s0 die iterationsgrenze untersteigt oder bis eine gewisse anzahl an Iterationen durchlaufen ist um unendlichschleifen zu verhindern
        Do Until iterationsgrenze < 0.001 Or iterationsanzahl > 24

            zähler = 0
            ' Berechnung der gekürzten beobachtungen l
            Dim l = Matrix.Create(Of Double)(passpunkte.Count * 3, 1)
            For i As Integer = 0 To passpunkte.Count * 3 - 1
                l(i, 0) = -messwerte(zähler)._X_Koordinate + passpunkte(zähler)._X_Koordinate
                i += 1
                l(i, 0) = -messwerte(zähler)._Y_Koordinate + passpunkte(zähler)._Y_Koordinate
                i += 1
                l(i, 0) = -messwerte(zähler)._Z_Koordinate + passpunkte(zähler)._Z_Koordinate
                zähler += 1
            Next

            ' A Matrix Transponieren
            Dim AT = Matrix.Create(Of Double)(1, 1)
            AT = A.Transpose
            'Q Matrix aufstellen Q =(A*A^T)^-1
            Dim Q = (Matrix.Multiply(AT, A)).GetInverse()

            'delta X = Q*A^T*l
            Dim deltax = Matrix.Create(Of Double)(1, 1)
            deltax = Matrix.Multiply(Matrix.Multiply(Q, AT), l)

            ' Speicherung des Maßstabs und den Rotationen(in Grad)
            m = 1 + deltax(0, 0)
            alpha = deltax(1, 0) / Math.PI * 180
            beta = deltax(2, 0) / Math.PI * 180
            gamma = deltax(3, 0) / Math.PI * 180


            ' Bestimmung der Rotationsmatrix
            Dim mR = Matrix.Create(Of Double)(3, 3)
            mR(0, 0) = 1 * m
            mR(0, 1) = deltax(3, 0) * m
            mR(0, 2) = -deltax(2, 0) * m
            mR(1, 0) = -deltax(3, 0) * m
            mR(1, 1) = 1 * m
            mR(1, 2) = deltax(1, 0) * m
            mR(2, 0) = deltax(2, 0) * m
            mR(2, 1) = -deltax(1, 0) * m
            mR(2, 2) = 1 * m


            'Berechnung der Verbesserungsmatrix
            Dim v = Matrix.Create(Of Double)(1, 1)
            v = Matrix.Multiply(A, deltax) - l
            zähler = 0

            ' Bestimmung X^
            Dim XD = Matrix.Create(Of Double)(passpunkte.Count * 3, 1)
            For i As Integer = 1 To passpunkte.Count - 1
                XD(i, 0) = messwerte(zähler)._X_Koordinate + v(i, 0)
                i += 1
                XD(i, 0) = messwerte(zähler)._Y_Koordinate + v(i, 0)
                i += 1
                XD(i, 0) = messwerte(zähler)._Z_Koordinate + v(i, 0)
                zähler += 1
            Next

            'Bestimmug l^
            Dim lD = l + v
            'Summe der Verbesserungen zum Quadrat
            Dim vv = Matrix.Create(Of Double)(1, 1)
            vv = Matrix.Multiply(v.Transpose, v)
            'Berstimmung s0
            s0 = Math.Sqrt(vv(0, 0) / (passpunkte.Count * 3 - 7))
            'Standardabweichung Maßstab
            s_m = Q(0, 0) * Math.Sqrt(s0)
            'Standardabweichung Alpha
            s_alpha = Q(1, 1) * Math.Sqrt(s0) / Math.PI * 180 * 3600
            'Standardabweichung Alpha
            s_beta = Q(2, 2) * Math.Sqrt(s0) / Math.PI * 180 * 3600
            'Standardabweichung Alpha
            s_gamma = Q(3, 3) * Math.Sqrt(s0) / Math.PI * 180 * 3600


            ' Berechnung der Translationsparameter mit hilfe der Schwerpunktkoordinaten und der Rotationsmatrix
            Dim schwerpunktsoll = Matrix.Create(Of Double)(3, 1)
            schwerpunktsoll(0, 0) = schwerpunktpasspunkte._X_Koordinate
            schwerpunktsoll(1, 0) = schwerpunktpasspunkte._Y_Koordinate
            schwerpunktsoll(2, 0) = schwerpunktpasspunkte._Z_Koordinate

            Dim schwerpunktist = Matrix.Create(Of Double)(3, 1)
            schwerpunktist(0, 0) = schwerpunktmesswerte._X_Koordinate
            schwerpunktist(1, 0) = schwerpunktmesswerte._Y_Koordinate
            schwerpunktist(2, 0) = schwerpunktmesswerte._Z_Koordinate


            translationen = schwerpunktsoll - Matrix.Multiply(mR, schwerpunktist)

            'definition eines "vorläufigen" Rückgabearrays um die "neuen" Beobachtungen mit den Transformationsparametern zu rechnen
            Dim rückgabe1 As Double() = {0, 0, 0, 0, 0, 0, 0}
            rückgabe1(0) = translationen(0, 0)       'Verschiebung in X-Richtung
            rückgabe1(1) = translationen(1, 0)       'Verschiebung in Y-Richtung
            rückgabe1(2) = translationen(2, 0)       'Verschiebung in Z-Richtung

            rückgabe1(3) = m                         'Maßstabsfaktor
            rückgabe1(4) = alpha                     'Alpha in Grad
            rückgabe1(5) = beta                      'Beta in Grad
            rückgabe1(6) = gamma                     'Gamma in Grad

            'Neue Passpunkte durch Transformation berechnen

            passpunkte.Clear()
            Dim x As Integer = neupunkteberechnen(Messwerteeingang, rückgabe1, passpunkte)
            Dim passpunkte1 As New List(Of Punkt)


            For Each punkt In messwerte
                passpunkte1.Add(punkt)
            Next
            'Die Passpunkte müssen nun wieder reduziert werden, damit diese am anfang der schleife wieder genutzt werden können um die gekürzen beobachtungen zu berechnen
            passpunkte.Clear()
            schwerpunktreduzierung(schwerpunktpasspunkte, passpunkte1, passpunkte)
            passpunkte1.Clear()
            'Iterationsparameter aktualisieren
            iterationsanzahl += 1
            iterationsgrenze = s0
        Loop

        '---------------------------------------------
        ' Benutzerinformation
        MsgBox("Die Ermittlung der Translationsparameter erfolgte mittels einer Ausgleichung mit " + iterationsanzahl.ToString + " Iterationen.")
        Dim ausgabe As String = "Die Translationen betragen:" + vbNewLine + "X-Richtung:  " + translationen(0, 0).ToString("0.0000") + vbNewLine + "Y-Richtung:  " + translationen(1, 0).ToString("0.0000") + vbNewLine + "Z-Richtung:  " + translationen(2, 0).ToString("0.0000")
        MsgBox(ausgabe)
        Dim ausgabe2 As String = "Der Maßstab beträgt: " + m.ToString("0.00000") + " mit mit einer Stdabw. von: " + s_m.ToString("0.00000") + vbNewLine + "Winkel alpha(Grad): " + alpha.ToString("0.00000") + " mit einer Stdabw. von : " + s_alpha.ToString("0.00000") + "''" + vbNewLine + "Winkel Beta(Grad): " + beta.ToString("0.00000") + " mit einer Stdabw. von : " + s_beta.ToString("0.00000") + "''" + vbNewLine + "Winkel Gamma(Grad): " + gamma.ToString("0.00000") + " mit einerStdabw. von : " + s_gamma.ToString("0.00000") + "''"
        MsgBox(ausgabe2)
        '---------------------------------------------
        ' Speichern der ermittelten Transformationsparameter in einem Rückgabe Array
        Dim rückgabe As Double() = {0, 0, 0, 0, 0, 0, 0}
        rückgabe(0) = translationen(0, 0)       'Verschiebung in X-Richtung
        rückgabe(1) = translationen(1, 0)       'Verschiebung in Y-Richtung
        rückgabe(2) = translationen(2, 0)       'Verschiebung in Z-Richtung

        rückgabe(3) = m                         'Maßstabsfaktor
        rückgabe(4) = alpha                     'Alpha in Grad
        rückgabe(5) = beta                      'Beta in Grad
        rückgabe(6) = gamma                     'Gamma in Grad
        passpunkte.Clear()

        Return rückgabe


    End Function

    Sub schwerpunkte()   'Schwerpunkberechnung
        Dim pxs As Double
        Dim pys As Double
        Dim pzs As Double
        Dim mxs As Double
        Dim mys As Double
        Dim mzs As Double
        Dim count As Integer = 0

        'für jeden punkt, welcher in beiden Listen die gleiche nummer hat wird jeweils für die Messwerte und die Passpunkte eine gesamtsumme hochgezählt. 
        'Der counter zählt die anzahl der Punkte mit der gleichen Nummer
        For Each punkt In PasspunkteEingang
            For Each messwert In MesswerteEingang
                If punkt._PunktNr = messwert._PunktNr Then
                    Passpunkteunreduziert.Add(punkt)
                    messwerteunreduziert.Add(messwert)
                    pxs = pxs + CDbl(punkt._X_Koordinate)
                    pys = pys + CDbl(punkt._Y_Koordinate)
                    pzs = pzs + CDbl(punkt._Z_Koordinate)
                    mxs = mxs + CDbl(messwert._X_Koordinate)
                    mys = mys + CDbl(messwert._Y_Koordinate)
                    mzs = mzs + CDbl(messwert._Z_Koordinate)
                    count += 1
                End If
            Next

        Next
        'Division jeder gesamtsumme durch die anzahl um den schwerpunkt zu erhalten
        pxs = pxs / count
        pys = pys / count
        pzs = pzs / count
        mxs = mxs / count
        mys = mys / count
        mzs = mzs / count

        'die Koordinaten in einem neuen schwerpunkt speichern
        schwerpunktPasspunkte = New Punkt("Schwerpunkt Passpunkte", pxs, pys, pzs)
        schwerpunktmesswerte = New Punkt("Schwerpunkt Passpunkte aus Messwerten", mxs, mys, mzs)



    End Sub
    Private Sub schwerpunktreduzierung(schwerpunkt As Punkt, punkte As List(Of Punkt), output As List(Of Punkt))
        'Reduzieren jedes punktes einer eingangsliste über einen Punkt und speichern des ergebnisses in einer outputlist
        For Each Punkt In punkte
            Dim x As Double = Punkt._X_Koordinate - schwerpunkt._X_Koordinate
            Dim y As Double = Punkt._Y_Koordinate - schwerpunkt._Y_Koordinate
            Dim z As Double = Punkt._Z_Koordinate - schwerpunkt._Z_Koordinate
            output.Add(New Punkt(Punkt._PunktNr, x, y, z))
        Next
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles B_export.Click
        'Export der Ergebnisspunktkoordinaten in einer XML-Datei
        SaveFileDialog1.Filter = "XML|*.xml"

        'https://www.dotnetperls.com/xmlwriter-vbnet
        'Create xmlwritersettings
        Dim settings As XmlWriterSettings = New XmlWriterSettings
        settings.Indent = True
        Dim count As Integer = 0
        'Create xmlwriter
        If DialogResult.OK = SaveFileDialog1.ShowDialog() Then

            Using writer As XmlWriter = XmlWriter.Create(SaveFileDialog1.FileName(), settings)
                writer.WriteStartDocument()
                writer.WriteStartElement("Koordinaten") 'Öffnen des Wurzelelements

                'Schleife über jede Person in pers
                For Each Punkt In Neupunkte
                    count += 1
                    'Öffnen des Punktlements
                    writer.WriteStartElement("Punkt")
                    'Schreiben der Elemente mit den jeweiligen werten, werden direkt wieder geschlossen
                    writer.WriteElementString("PunktNr", Punkt._PunktNr.ToString)
                    writer.WriteElementString("X-Koordinate", Punkt._X_Koordinate.ToString.Replace(",", "."))
                    writer.WriteElementString("Y-Koordinate", Punkt._Y_Koordinate.ToString.Replace(",", "."))
                    writer.WriteElementString("Z-Koordinate", Punkt._Z_Koordinate.ToString.Replace(",", "."))

                    'Schließen des Punktelements
                    writer.WriteEndElement()
                Next

                ' End document
                'Schließen des Wurzelelements Adressen
                writer.WriteEndElement()
                writer.WriteEndDocument()
                'Ausgabe der gespeicherten Daten
                MessageBox.Show("Es wurden " + count.ToString + " Punkte in der Datei gespeichert")

            End Using




        Else
            MessageBox.Show("Keine Datei ausgewählt")
            Return
        End If
    End Sub

    Private Sub B_residuen_Click_1(sender As Object, e As EventArgs) Handles B_residuen.Click
        'Residuen in MSGBox ausgeben
        MsgBox(residuenanzeigen(PasspunkteEingang, Neupunkte))
    End Sub

    Private Sub B_reset_Click(sender As Object, e As EventArgs) Handles B_reset.Click
        'GUI resetten
        B_residuen.Enabled = False
        B_export.Enabled = False
        b_zielsystem.Enabled = True
        b_quellsystem.Enabled = True
        B_reset.Enabled = False
        tb_quellsystem.Text = ""
        tb_zielsystem.Text = ""
        MesswerteEingang.Clear()
        PasspunkteEingang.Clear()
        Passpunkteunreduziert.Clear()
        messwerteunreduziert.Clear()
        Neupunkte.Clear()
        messwerteschwerpunktreduziert.Clear()
        passpunkteschwerpunktreduzuert.Clear()
        messwerteunreduziertohneneupunkte.Clear()

    End Sub
End Class
