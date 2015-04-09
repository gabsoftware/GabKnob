﻿'Imports System
'Imports System.Collections
Imports System.ComponentModel
'Imports System.Drawing
'Imports System.Windows.Forms
Imports System.Drawing.Drawing2D
Imports System.Threading


Namespace GabSoftware.WinControls

    ''' <summary>
    ''' GabKnob is a customizable knob control.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class GabKnob
        Private _knobDiameter As Integer
        Private _knobBorderColor As Color
        Private _knobBorderSize As Integer
        Private _knobBorderType As eKnobBorderType
        Private _knobSize As Size
        Private _knobAngle As Integer
        Private _knobAngleMin As Integer
        Private _knobAngleMax As Integer
        Private _knobValue As Integer
        Private _knobValueMin As Integer
        Private _knobValueMax As Integer
        Private _knobColor1 As Color
        Private _knobColor2 As Color
        Private _knobColor3 As Color
        Private _knob3ColorsMode As Boolean
        Private _knobGradientCenterOffset As Point
        Private _knobMiddleColorDistance As Single
        Private _knobFillType As eKnobFillType
        Private _knobMoveType As eKnobMoveType
        Private _knobMoveInverse As eKnobMoveDirection
        Private _knobBrush As Brush
        Private _knobPositionBrush As Brush
        Private _knobPositionWidth As Integer
        Private _knobPositionHeight As Integer
        Private _knobPositionDistance As Integer
        Private _knobPositionColor As Color
        Private _knobPositionColorClicked As Color
        Private _knobPositionType As eKnobPositionType
        Private _knobDisplayValue As Boolean
        Private _knobDisplayValueType As eKnobDisplayValueType
        Private _knobDisplayTextColor As Color
        Private _knobDisplayTextSuffix As String
        Private _knobDisplayFont As Font
        Private _knobDisplayPosition As Point
        Private _knobDisplayBrush As Brush
        Private _isdown As Boolean
        Private _older As Point
        Private _b_knob_gradient As PathGradientBrush
        Private _b_knob_solid As SolidBrush
        Private _g_knob_gradient_path As GraphicsPath
        Private _r_knobBorder As Rectangle
        Private _r_knobPosition As Rectangle
        Private _isloaded As Boolean
        Private _thread As Thread
        Private _test As Integer
        Private _valueChanging As Boolean
        Private _knobSensivity As Single
        Private _knobInitialSensivity As Single
        Private _knobSensivityMultiplier As Single
        Private _knobSensivityDivisor As Single

        Public Event KnobValueChanged(ByVal KnobValue As Integer)



#Region " Constructeur "
        ''' <summary>
        ''' Constructeur de la classe ; instancie un nouveau GabKnob
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

            InitializeComponent()

            'spécifie que l'on dessine soit-même
            SetStyle(ControlStyles.ResizeRedraw, True)
            SetStyle(ControlStyles.UserPaint, True)
            SetStyle(ControlStyles.SupportsTransparentBackColor, True)
            SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            SetStyle(ControlStyles.Selectable, True)
            SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            SetStyle(ControlStyles.UserMouse, True)


            'misc
            Me._isdown = False
            Me._isloaded = False
            Me._valueChanging = False

            'initialisation du knob
            Me.KnobFillType = eKnobFillType.Gradient
            Me.KnobColor1 = Color.LightSteelBlue
            Me.KnobColor2 = Color.SteelBlue
            Me.KnobColor3 = Color.DarkSlateBlue
            Me.Knob3ColorsMode = True
            Me.KnobMiddleColorDistance = 0.4F
            Me.KnobGradientCenterOffset = New Point(-25, -25)
            Me._knobAngleMin = 45
            Me.KnobAngleMin = 45
            Me._knobAngleMax = 315
            Me.KnobAngleMax = 315
            Me.KnobAngle = 45
            Me.KnobValueMin = 0
            Me.KnobValueMax = 128
            Me.KnobMoveType = eKnobMoveType.Both
            Me.KnobMoveDirection = eKnobMoveDirection.Normal
            Me.KnobSize = New Size(100, 100)
            Me.KnobSensivity = 1.0
            Me.KnobSensivityDivisor = 4.0
            Me.KnobSensivityMultiplier = 2.0

            'bordure
            Me.KnobBorderType = eKnobBorderType.Solid
            Me.KnobBorderSize = 4
            Me.KnobBorderColor = Color.Black

            'affichage de la valeur
            Me.KnobDisplayValue = True
            Me.KnobDisplayFont = New Font(Me.Font.Name, 14.25, FontStyle.Regular, GraphicsUnit.Point)
            Me.KnobDisplayTextColor = Me.ForeColor
            Me.KnobDisplayTextSuffix = "%"
            Me.KnobDisplayValueType = eKnobDisplayValueType.When_clicked
            Me.KnobDisplayPosition = New Point(-15, -10)

            'initialisation du témoin de position
            Me.KnobPositionType = eKnobPositionType.Circle
            Me.KnobPositionWidth = 10
            Me.KnobPositionHeight = 10
            Me.KnobPositionDistance = 8
            Me.KnobPositionColor = Color.MidnightBlue
            Me.KnobPositionColorClicked = Color.LightSteelBlue

            'initialisation divers
            Me.BackColor = Color.Beige
            Me.Width = 200
            Me.Height = 200

        End Sub

#End Region

#Region " Enums "
        ''' <summary>
        ''' Type du knob
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum eKnobFillType

            Solid = 1
            Gradient = 2
            Picture = 3

        End Enum

        ''' <summary>
        ''' Type de la bordure
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum eKnobBorderType

            Solid = 1
            None = 2

        End Enum

        ''' <summary>
        ''' Type du témoin de position
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum eKnobPositionType
            Rectangle = 1
            Circle = 2
        End Enum

        ''' <summary>
        ''' Type du mouvement
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum eKnobMoveType

            Horizontal = 1
            Vertical = 2
            Both = 3

        End Enum

        ''' <summary>
        ''' Sens normal ou inversé du mouvement
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum eKnobMoveDirection

            Normal = 1
            Reversed = -1

        End Enum

        ''' <summary>
        ''' Condition d'affichage de la valeur retournée
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum eKnobDisplayValueType

            Always = 1
            When_clicked = 2
            When_not_clicked = 3

        End Enum

#End Region

#Region " Propriétés "
        ''' <summary>
        ''' Propriété de test (ne fait rien)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property Test() As Integer
            Get
                Return _test
            End Get
            Set(ByVal value As Integer)
                _test = value
            End Set
        End Property

        <Browsable(True)> Public Property KnobGradientCenterOffset() As Point
            Get
                Return _knobGradientCenterOffset
            End Get
            Set(ByVal value As Point)
                _knobGradientCenterOffset = value
                If _isloaded Then
                    Me.SetupSolidBrush()
                    Me.SetupGradientBrush()
                End If
                Refresh()
            End Set
        End Property

        ''' <summary>
        ''' The sensivity of the knob when in vertical or horizontal mode
        ''' </summary>
        ''' <value>A single value in the interval ]0 ; 10] </value>
        ''' <returns></returns>
        ''' <remarks>Below 1.0 reduce sensivity, more than 1.0 augment it.</remarks>
        <Browsable(True)> Public Property KnobSensivity() As Single
            Get
                Return _knobSensivity
            End Get
            Set(ByVal value As Single)
                If value <= 10.0 And value > 0 Then
                    _knobSensivity = value
                    _knobInitialSensivity = value
                End If

            End Set
        End Property

        ''' <summary>
        ''' Multiply the sensivity by this coefficient when the multiplier key is pressed while moving the knob (default is SHIFT key)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobSensivityMultiplier() As Single
            Get
                Return _knobSensivityMultiplier
            End Get
            Set(ByVal value As Single)
                _knobSensivityMultiplier = value
            End Set
        End Property

        ''' <summary>
        ''' Divide the sensivity by this coefficient when the multiplier key is pressed while moving the knob (default is CTRL key)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobSensivityDivisor() As Single
            Get
                Return _knobSensivityDivisor
            End Get
            Set(ByVal value As Single)
                _knobSensivityDivisor = value
            End Set
        End Property

        ''' <summary>
        ''' Diamètre du knob
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>non utilisé actuellement</remarks>
        <Browsable(False)> Public Property KnobDiameter() As Integer
            Get
                Return _knobDiameter
            End Get
            Set(ByVal value As Integer)
                _knobDiameter = value
                Refresh()
            End Set
        End Property

        ''' <summary>
        ''' Type de knob ( couleur pleine ou dégradé )
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobFillType() As eKnobFillType
            Get
                Return _knobFillType
            End Get
            Set(ByVal value As eKnobFillType)
                _knobFillType = value
                Refresh()
            End Set
        End Property

        ''' <summary>
        ''' Définit la manière de changer la valeur du knob à la souris
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobMoveType() As eKnobMoveType
            Get
                Return _knobMoveType
            End Get
            Set(ByVal value As eKnobMoveType)
                _knobMoveType = value
            End Set
        End Property

        ''' <summary>
        ''' Inverse ou non le sens du bouton par rapport au mouvement de la souris
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobMoveDirection() As eKnobMoveDirection
            Get
                Return _knobMoveInverse
            End Get
            Set(ByVal value As eKnobMoveDirection)
                _knobMoveInverse = value
            End Set
        End Property

        ''' <summary>
        ''' Type de la bordure
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobBorderType() As eKnobBorderType
            Get
                Return _knobBorderType
            End Get
            Set(ByVal value As eKnobBorderType)
                _knobBorderType = value
                SetupMisc()
                SetupSolidBrush()
                SetupGradientBrush()
                Refresh()
            End Set
        End Property

        ''' <summary>
        ''' Taille de la bordure
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobBorderSize() As Integer
            Get
                Return _knobBorderSize
            End Get
            Set(ByVal value As Integer)
                If value = (value \ 2) * 2 Or value = 1 Then
                    _knobBorderSize = value
                    Refresh()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Taille du knob (valeur paires seulement)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobSize() As Size
            Get
                Return _knobSize
            End Get
            Set(ByVal value As Size)
                If (value.Width = (value.Width \ 2) * 2) And (value.Height = (value.Height \ 2) * 2) Then
                    _knobSize = value
                    Refresh()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Couleur de la bordure
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobBorderColor() As Color
            Get
                Return _knobBorderColor
            End Get
            Set(ByVal value As Color)
                _knobBorderColor = value
                Refresh()
            End Set
        End Property

        ''' <summary>
        ''' Couleur principale en mode Couleur pleine, Couleur du centre en mode dégradé
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobColor1() As Color
            Get
                Return _knobColor1
            End Get
            Set(ByVal value As Color)
                _knobColor1 = value
                If _isloaded Then
                    Me.SetupSolidBrush()
                    Me.SetupGradientBrush()
                End If
                Refresh()
            End Set
        End Property

        ''' <summary>
        ''' Couleur autour du centre en mode dégradé
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobColor2() As Color
            Get
                Return _knobColor2
            End Get
            Set(ByVal value As Color)
                _knobColor2 = value
                If _isloaded Then
                    Me.SetupSolidBrush()
                    Me.SetupGradientBrush()
                End If
                Refresh()
            End Set
        End Property

        ''' <summary>
        ''' 3ème couleur du dégradé en mode 3 couleurs
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobColor3() As Color
            Get
                Return _knobColor3
            End Get
            Set(ByVal value As Color)
                _knobColor3 = value
                If _isloaded Then
                    Me.SetupSolidBrush()
                    Me.SetupGradientBrush()
                End If
                Refresh()
            End Set
        End Property

        ''' <summary>
        ''' Specify if the knob should use 3 colors instead of 2 for its gradient
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property Knob3ColorsMode() As Boolean
            Get
                Return _knob3ColorsMode
            End Get
            Set(ByVal value As Boolean)
                _knob3ColorsMode = value
                If _isloaded Then
                    Me.SetupSolidBrush()
                    Me.SetupGradientBrush()
                End If
                Refresh()
            End Set
        End Property

        ''' <summary>
        ''' A single value between 0.0F and 1.0F which specify the relative position of the middle color of the gradient in 3 colors mode, between the center and the border of the gradient.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobMiddleColorDistance() As Single
            Get
                Return _knobMiddleColorDistance
            End Get
            Set(ByVal value As Single)
                If value > 0.0F And value < 1.0F Then
                    _knobMiddleColorDistance = value
                    If _isloaded Then
                        Me.SetupSolidBrush()
                        Me.SetupGradientBrush()
                    End If
                    Refresh()
                End If

            End Set
        End Property

        ''' <summary>
        ''' Angle du knob en degrés(de 0 à 360)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobAngle() As Integer
            Get
                Return _knobAngle
            End Get
            Set(ByVal value As Integer)
                If value >= _knobAngleMin And value <= _knobAngleMax Then
                    _knobAngle = value
                    If Not _valueChanging Then

                        Me.KnobValue = ((_knobAngle - _knobAngleMin) / (_knobAngleMax - _knobAngleMin)) * (_knobValueMax - _knobValueMin) + _knobValueMin

                    End If
                    Refresh()
                    'Else
                    'Return
                End If
            End Set
        End Property

        ''' <summary>
        ''' Angle minimum du knob en degrés
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobAngleMin() As Integer
            Get
                Return _knobAngleMin
            End Get
            Set(ByVal value As Integer)
                If value >= 0 And value < _knobAngleMax Then
                    _knobAngleMin = value
                    Refresh()
                Else
                    Return
                End If
            End Set
        End Property

        ''' <summary>
        ''' Angle maximum du knob en degrés
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobAngleMax() As Integer
            Get
                Return _knobAngleMax
            End Get
            Set(ByVal value As Integer)
                If value > _knobAngleMin And value <= 360 Then
                    _knobAngleMax = value
                    Refresh()
                Else
                    Return
                End If
            End Set
        End Property

        ''' <summary>
        ''' Valeur de retour du knob ( on ne peut actuellement pas définir l'angle à l'aide de cette valeur)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Property KnobValue() As Integer
            Get
                Return _knobValue
            End Get
            Set(ByVal value As Integer)
                _knobValue = value
                RaiseEvent KnobValueChanged(_knobValue)
            End Set
        End Property

        ''' <summary>
        ''' Définit ou retourne la valeur du knob (methode publique)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property Value() As Integer
            Get
                Return _knobValue
            End Get
            Set(ByVal value As Integer)
                _valueChanging = True
                KnobValue = value
                KnobAngle = ((value - _knobValueMin) / (_knobValueMax - _knobValueMin)) * (_knobAngleMax - _knobAngleMin) + _knobAngleMin
                _valueChanging = False

            End Set
        End Property

        ''' <summary>
        ''' Valeur minimum retournée par le knob
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobValueMin() As Integer
            Get
                Return _knobValueMin
            End Get
            Set(ByVal value As Integer)
                _knobValueMin = value
            End Set
        End Property

        ''' <summary>
        ''' Valeur maximale retournée par le knob
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobValueMax() As Integer
            Get
                Return _knobValueMax
            End Get
            Set(ByVal value As Integer)
                _knobValueMax = value
            End Set
        End Property

        ''' <summary>
        ''' Affiche la valeur du knob
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobDisplayValue() As Boolean
            Get
                Return _knobDisplayValue
            End Get
            Set(ByVal value As Boolean)
                _knobDisplayValue = value
                Refresh()
            End Set
        End Property

        ''' <summary>
        ''' Change la couleur du texte de la valeur
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobDisplayTextColor() As Color
            Get
                Return _knobDisplayTextColor
            End Get
            Set(ByVal value As Color)
                _knobDisplayTextColor = value
                Refresh()
            End Set
        End Property

        ''' <summary>
        ''' Ajoute ce texte à la valeur affichée
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobDisplayTextSuffix() As String
            Get
                Return _knobDisplayTextSuffix
            End Get
            Set(ByVal value As String)
                _knobDisplayTextSuffix = value
                Refresh()
            End Set
        End Property

        ''' <summary>
        ''' Police utilisée pour l'affichage de la valeur
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobDisplayFont() As Font
            Get
                Return _knobDisplayFont
            End Get
            Set(ByVal value As Font)
                _knobDisplayFont = value
                Refresh()
            End Set
        End Property

        ''' <summary>
        ''' La position par rapport au centre du knob
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobDisplayPosition() As Point
            Get
                Return _knobDisplayPosition
            End Get
            Set(ByVal value As Point)
                _knobDisplayPosition = value
                Refresh()
            End Set
        End Property

        ''' <summary>
        ''' Définit la condition d'affichage de la valeur
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobDisplayValueType() As eKnobDisplayValueType
            Get
                Return _knobDisplayValueType
            End Get
            Set(ByVal value As eKnobDisplayValueType)
                _knobDisplayValueType = value
            End Set
        End Property

        ''' <summary>
        ''' Le brush utilisé pour dessiner la valeur sur le bouton
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(False)> Public Property KnobDisplayBrush() As Brush
            Get
                Return _knobDisplayBrush
            End Get
            Set(ByVal value As Brush)
                _knobDisplayBrush = value
            End Set
        End Property

        ''' <summary>
        ''' Brush utilisé pour dessiner l'interieur du knob
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(False)> Public Property KnobBrush() As Brush
            Get
                Return _knobBrush
            End Get
            Set(ByVal value As Brush)
                _knobBrush = value
            End Set
        End Property

        ''' <summary>
        ''' Brush utilisé pour dessiner le témoin de position du knob
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(False)> Public Property KnobPositionBrush() As Brush
            Get
                Return _knobPositionBrush
            End Get
            Set(ByVal value As Brush)
                _knobPositionBrush = value
            End Set
        End Property

        ''' <summary>
        ''' Largeur du témoin de position
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobPositionWidth() As Integer
            Get
                Return _knobPositionWidth
            End Get
            Set(ByVal value As Integer)
                _knobPositionWidth = value
                Refresh()
            End Set
        End Property

        ''' <summary>
        ''' Hauteur du témoin de position
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobPositionHeight() As Integer
            Get
                Return _knobPositionHeight
            End Get
            Set(ByVal value As Integer)
                _knobPositionHeight = value
                Refresh()
            End Set
        End Property

        ''' <summary>
        ''' Distance du témoin de position par rapport à la bordure du knob
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobPositionDistance() As Integer
            Get
                Return _knobPositionDistance
            End Get
            Set(ByVal value As Integer)
                If value < Height \ 2 Then
                    _knobPositionDistance = value
                    SetupMisc()
                    Refresh()
                Else
                    Return
                End If
            End Set
        End Property

        ''' <summary>
        ''' Couleur du témoin de position
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobPositionColor() As Color
            Get
                Return _knobPositionColor
            End Get
            Set(ByVal value As Color)
                _knobPositionColor = value
                Refresh()
            End Set
        End Property

        ''' <summary>
        ''' Couleur du témoin de position
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobPositionColorClicked() As Color
            Get
                Return _knobPositionColorClicked
            End Get
            Set(ByVal value As Color)
                _knobPositionColorClicked = value
                Refresh()
            End Set
        End Property

        ''' <summary>
        ''' Type du témoin de position (pour l'instant rectangle ou ellipse)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(True)> Public Property KnobPositionType() As eKnobPositionType
            Get
                Return _knobPositionType
            End Get
            Set(ByVal value As eKnobPositionType)
                _knobPositionType = value
                Refresh()
            End Set
        End Property

#End Region

#Region " Méthodes privées "
        'mesure d'angle entre 2 points
        Private Function LineAngle(ByVal P1 As Point, ByVal P2 As Point) As Double

            Return LineAngle(P1.X, P1.Y, P2.X, P2.Y)

        End Function
        Private Function LineAngle(ByVal P1x As Integer, ByVal P1y As Integer, ByVal P2x As Integer, ByVal P2y As Integer) As Double
            Dim angle As Double
            angle = Math.Atan2(P1y - P2y, P1x - P2x) * (180 / Math.PI) - 90

            If angle < 0 Then angle = angle + 360

            Return angle

        End Function

        Private Sub SetupMisc()
            _r_knobPosition = New Rectangle((-_knobPositionWidth / 2) - 0.5, ((-Height + _knobBorderSize) / 2) + _knobPositionDistance, _knobPositionWidth, _knobPositionHeight)
            _r_knobBorder = New Rectangle(((-Width + _knobBorderSize) / 2) + 1, ((-Height + _knobBorderSize) / 2) + 1, Width - _knobBorderSize - 2, Height - _knobBorderSize - 2)

        End Sub

        Private Sub SetupSolidBrush()
            _b_knob_solid = New SolidBrush(_knobColor1)
        End Sub

        Private Sub SetupGradientBrush()
            _g_knob_gradient_path = New GraphicsPath()
            _g_knob_gradient_path.AddEllipse(_r_knobBorder)

            _b_knob_gradient = New PathGradientBrush(_g_knob_gradient_path)
            _b_knob_gradient.WrapMode = WrapMode.Clamp

            If _knob3ColorsMode = True Then
                Dim b_knob_color_blend As New ColorBlend(3)
                b_knob_color_blend.Colors = New Color() {_knobColor3, _knobColor2, _knobColor1}
                b_knob_color_blend.Positions = New Single() {0.0F, _knobMiddleColorDistance, 1.0F}
                _b_knob_gradient.InterpolationColors = b_knob_color_blend
            Else
                Dim b_knob_color_blend As New ColorBlend(2)
                b_knob_color_blend.Colors = New Color() {_knobColor2, _knobColor1}
                b_knob_color_blend.Positions = New Single() {0.0F, 1.0F}
                _b_knob_gradient.InterpolationColors = b_knob_color_blend
            End If


            _b_knob_gradient.CenterPoint = _knobGradientCenterOffset

        End Sub

        Private Sub SetupValeurBrush()
            _knobDisplayBrush = New SolidBrush(Me.KnobDisplayTextColor)
        End Sub

        Private Sub ThreadedPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
            'redessine dans un thread

            Dim g As Graphics = e.Graphics

            g.SmoothingMode = SmoothingMode.AntiAlias


            'mets le centre du bouton au milieu
            g.TranslateTransform(Width / 2 - 0.5, Height / 2 - 0.5)


            'imobile
            g.FillEllipse(_knobBrush, _r_knobBorder) 'intérieur


            If _knobBorderType = eKnobBorderType.Solid And _knobBorderSize <> 0 Then
                g.DrawEllipse(New Pen(_knobBorderColor, _knobBorderSize), _r_knobBorder) 'anneau
            End If


            'dessine la valeur du knob
            If Me.KnobDisplayValue = True Then
                Select Case Me.KnobDisplayValueType
                    Case eKnobDisplayValueType.Always
                        g.DrawString(_knobValue & " " & Me.KnobDisplayTextSuffix, Me.KnobDisplayFont, Me.KnobDisplayBrush, Me.KnobDisplayPosition) '0 - 15, 0 - 10

                    Case eKnobDisplayValueType.When_clicked
                        If _isdown Then
                            g.DrawString(_knobValue & " " & Me.KnobDisplayTextSuffix, Me.KnobDisplayFont, Me.KnobDisplayBrush, Me.KnobDisplayPosition) '0 - 15, 0 - 10
                        End If

                    Case eKnobDisplayValueType.When_not_clicked
                        If Not _isdown Then
                            g.DrawString(_knobValue & " " & Me.KnobDisplayTextSuffix, Me.KnobDisplayFont, Me.KnobDisplayBrush, Me.KnobDisplayPosition) '0 - 15, 0 - 10
                        End If

                End Select
            End If


            'effectue une rotation du plan 
            g.RotateTransform(_knobAngle + 180)

            'mouvement
            If Me.KnobPositionType = eKnobPositionType.Rectangle Then
                g.FillRectangle(_knobPositionBrush, _r_knobPosition) 'témoin de position rectangulaire
            Else
                g.FillEllipse(_knobPositionBrush, _r_knobPosition) 'témoin de position rond
            End If

        End Sub

#End Region

#Region " Méthodes publiques "


        Public Overrides Sub Refresh()
            'raffraichi l'affichage
            If _isloaded = False Then
                Return
            End If

            If Me.ClientRectangle.Width = 0 Then
                Return
            End If

            'knob de couleur unie
            If _knobFillType = eKnobFillType.Solid Then
                _knobBrush = _b_knob_solid
            End If

            'knob en dégradé
            If _knobFillType = eKnobFillType.Gradient Then
                _knobBrush = _b_knob_gradient
            End If

            'témoin de position rectangulaire
            _knobPositionBrush = New SolidBrush(IIf(_isdown, _knobPositionColorClicked, _knobPositionColor))

            MyBase.Refresh()

        End Sub

        Private Sub GabKnobSource_KnobValueChanged(ByVal KnobValue As Integer) Handles Me.KnobValueChanged
            Application.DoEvents() 'ca bouffe plein de cpu, faudrait trouver autre chose
        End Sub

        Private Sub Knob_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
            'redessine
            If (Not _thread Is Nothing) Then
                If (_thread.IsAlive) Then
                    _thread.Abort()
                End If
            End If


            _thread = New Thread(AddressOf ThreadedPaint)
            _thread.IsBackground = True
            _thread.Start(e)
            _thread.Join()

        End Sub

#End Region

#Region " Base Events "
        Private Sub Knob_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            _isloaded = True

            Me.SetupMisc()
            Me.SetupGradientBrush()
            Me.SetupSolidBrush()
            Me.SetupValeurBrush()

            Refresh()
        End Sub

        Private Sub GabKnobSource_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
            If Width = (Width / 2) * 2 Then
                Height = Width
                _knobSize = Me.Size
                Me.SetupMisc()
                Me.SetupSolidBrush()
                Me.SetupGradientBrush()
                'Me.SetupValeurBrush()
                Refresh()
            End If

        End Sub

        Private Sub GabKnobSource_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown

            _isdown = True
            _older = New Point(e.X, e.Y)
            GabKnobSource_MouseMove(sender, e)

        End Sub

        Private Sub GabKnobSource_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp

            _isdown = False
            Refresh()

        End Sub

        Private Sub GabKnobSource_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
            If e.Button = Windows.Forms.MouseButtons.None Then Exit Sub
            'MsgBox("salut")
            If (_isdown = True) Then
                Dim pt As Point
                pt = New Point(e.X, e.Y)
                Select Case _knobMoveType

                    Case eKnobMoveType.Vertical
                        'déplacement vertical
                        If pt.Y > _older.Y Then
                            'vers le bas
                            Me.KnobAngle -= (pt.Y - _older.Y) * _knobSensivity * _knobMoveInverse
                        Else
                            If pt.Y < _older.Y Then
                                'vers le haut
                                Me.KnobAngle += (_older.Y - pt.Y) * _knobSensivity * _knobMoveInverse
                            End If
                        End If

                    Case eKnobMoveType.Horizontal
                        'déplacement horizontal
                        If pt.X > _older.X Then
                            'vers la droite
                            Me.KnobAngle += (pt.X - _older.X) * _knobSensivity * _knobMoveInverse
                        Else
                            If pt.X < _older.X Then
                                'vers la gauche
                                Me.KnobAngle -= (_older.X - pt.X) * _knobSensivity * _knobMoveInverse
                            End If
                        End If

                    Case eKnobMoveType.Both
                        'déplacement vertical et horizontal
                        Me.KnobAngle = Me.LineAngle(pt, New Point(Width \ 2, Height \ 2))

                End Select

                _older = pt
            End If
            Thread.Sleep(10) 'the knob magically become less cpu-hungry

        End Sub

        Private Sub GabKnobSource_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
            If (e.Delta < 0) Then
                'augmente l'angle
                Me.KnobAngle += 10
            End If
            If (e.Delta > 0) Then
                'reduit l'angle
                Me.KnobAngle -= 10
            End If
        End Sub

        ''' <summary>
        ''' Modify the sensivity of the knob when pressing the CTRL or SHIFT keys
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub GabKnobSource_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
            Select Case e.KeyCode
                Case Keys.ControlKey
                    _knobSensivity = _knobInitialSensivity / _knobSensivityDivisor
                Case Keys.ShiftKey
                    _knobSensivity = _knobInitialSensivity * _knobSensivityMultiplier
            End Select
        End Sub

        ''' <summary>
        ''' Restore the sensivity of the knob when releasing the CTRL or SHIFT keys
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub GabKnobSource_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
            Select Case e.KeyCode
                Case Keys.ControlKey
                    _knobSensivity = _knobInitialSensivity
                Case Keys.ShiftKey
                    _knobSensivity = _knobInitialSensivity
            End Select
        End Sub



#End Region


    End Class

End Namespace