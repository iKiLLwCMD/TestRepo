Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.IO
Imports System.Diagnostics
Imports System.Configuration
Imports System.Text.RegularExpressions


Public Class Converter
#Region "Properties"
    Private _ffExe As String
    Public Property ffExe() As String
        Get
            Return _ffExe
        End Get
        Set(value As String)
            _ffExe = value
        End Set
    End Property

    Private _WorkingPath As String
    Public Property WorkingPath() As String
        Get
            Return _WorkingPath
        End Get
        Set(value As String)
            _WorkingPath = value
        End Set
    End Property

#End Region

#Region "Constructors"
    Public Sub New()
        Initialize()
    End Sub
    Public Sub New(ffmpegExePath As String)
        _ffExe = ffmpegExePath
        Initialize()
    End Sub
#End Region

#Region "Initialization"
    Private Sub Initialize()
        'first make sure we have a value for the ffexe file setting
        If String.IsNullOrEmpty(_ffExe) Then
            Dim o As Object = ConfigurationManager.AppSettings("ffmpeg:ExeLocation")
            If o Is Nothing Then
                Throw New Exception("Could not find the location of the ffmpeg exe file.  The path for ffmpeg.exe " & "can be passed in via a constructor of the ffmpeg class (this class) or by setting in the app.config or web.config file.  " & "in the appsettings section, the correct property name is: ffmpeg:ExeLocation")
            Else
                If String.IsNullOrEmpty(o.ToString()) Then
                    Throw New Exception("No value was found in the app setting for ffmpeg:ExeLocation")
                End If
                _ffExe = o.ToString()
            End If
        Else

        End If

        'Now see if ffmpeg.exe exists
        Dim workingpath As String = GetWorkingFile()
        If String.IsNullOrEmpty(workingpath) Then
            'ffmpeg doesn't exist at the location stated.
            Throw New Exception("Could not find a copy of ffmpeg.exe")
        End If
        _ffExe = workingpath

        'now see if we have a temporary place to work
        If String.IsNullOrEmpty(_WorkingPath) Then
            Dim o As Object = ConfigurationManager.AppSettings("ffmpeg:WorkingPath")
            If o IsNot Nothing Then
                _WorkingPath = o.ToString()
            Else
                _WorkingPath = String.Empty
            End If
        End If
    End Sub

    Private Function GetWorkingFile() As String
        'try the stated directory
        If File.Exists(_ffExe) Then
            Return _ffExe
        End If

        'oops, that didn't work, try the base directory
        If File.Exists(Path.GetFileName(_ffExe)) Then
            Return Path.GetFileName(_ffExe)
        End If

        'well, now we are really unlucky, let's just return null
        Return Nothing
    End Function
#End Region

#Region "Get the File without creating a file lock"
    Public Shared Function LoadImageFromFile(fileName As String) As System.Drawing.Image
        Dim theImage As System.Drawing.Image = Nothing
        Using fileStream As New FileStream(fileName, FileMode.Open, FileAccess.Read)
            Dim img As Byte()
            img = New Byte(fileStream.Length - 1) {}
            fileStream.Read(img, 0, img.Length)
            fileStream.Close()
            theImage = System.Drawing.Image.FromStream(New MemoryStream(img))
            img = Nothing
        End Using
        GC.Collect()
        Return theImage
    End Function

    Public Shared Function LoadMemoryStreamFromFile(fileName As String) As MemoryStream
        Dim ms As MemoryStream = Nothing
        Using fileStream As New FileStream(fileName, FileMode.Open, FileAccess.Read)
            Dim fil As Byte()
            fil = New Byte(fileStream.Length - 1) {}
            fileStream.Read(fil, 0, fil.Length)
            fileStream.Close()
            ms = New MemoryStream(fil)
        End Using
        GC.Collect()
        Return ms
    End Function
#End Region

#Region "Run the process"
    Private Function RunProcess(Parameters As String) As String
        'create a process info
        Dim oInfo As New ProcessStartInfo(Me._ffExe, Parameters)
        oInfo.UseShellExecute = False
        oInfo.CreateNoWindow = True
        oInfo.RedirectStandardOutput = True
        oInfo.RedirectStandardError = True

        'Create the output and streamreader to get the output
        Dim output As String = Nothing
        Dim srOutput As StreamReader = Nothing

        'try the process
        Try
            'run the process
            Dim proc As Process = System.Diagnostics.Process.Start(oInfo)

            proc.WaitForExit()

            'get the output
            srOutput = proc.StandardError

            'now put it in a string
            output = srOutput.ReadToEnd()

            proc.Close()
        Catch generatedExceptionName As Exception
            output = String.Empty
        Finally
            'now, if we succeded, close out the streamreader
            If srOutput IsNot Nothing Then
                srOutput.Close()
                srOutput.Dispose()
            End If
        End Try
        Return output
    End Function
#End Region

#Region "GetVideoInfo"
    Public Function GetVideoInfo(inputFile As MemoryStream, Filename As String) As VideoFile
        Dim tempfile As String = Path.Combine(Me.WorkingPath, System.Guid.NewGuid().ToString() & Path.GetExtension(Filename))
        Dim fs As FileStream = File.Create(tempfile)
        inputFile.WriteTo(fs)
        fs.Flush()
        fs.Close()
        GC.Collect()

        Dim vf As VideoFile = Nothing
        Try
            vf = New VideoFile(tempfile)
        Catch ex As Exception
            Throw ex
        End Try

        GetVideoInfo(vf)

        Try
            File.Delete(tempfile)

        Catch generatedExceptionName As Exception
        End Try

        Return vf
    End Function
    Public Function GetVideoInfo(inputPath As String) As VideoFile
        Dim vf As VideoFile = Nothing
        Try
            vf = New VideoFile(inputPath)
        Catch ex As Exception
            Throw ex
        End Try
        GetVideoInfo(vf)
        Return vf
    End Function
    Public Sub GetVideoInfo(input As VideoFile)
        'set up the parameters for video info
        Dim Params As String = String.Format("-i {0}", input.Path)
        Dim output As String = RunProcess(Params)
        input.RawInfo = output

        'get duration
        Dim re As New Regex("[D|d]uration:.((\d|:|\.)*)")
        Dim m As Match = re.Match(input.RawInfo)

        If m.Success Then
            Dim duration As String = m.Groups(1).Value
            Dim timepieces As String() = duration.Split(New Char() {":"c, "."c})
            If timepieces.Length = 4 Then
                input.Duration = New TimeSpan(0, Convert.ToInt16(timepieces(0)), Convert.ToInt16(timepieces(1)), Convert.ToInt16(timepieces(2)), Convert.ToInt16(timepieces(3)))
            End If
        End If

        'get audio bit rate
        re = New Regex("[B|b]itrate:.((\d|:)*)")
        m = re.Match(input.RawInfo)
        Dim kb As Double = 0.0
        If m.Success Then
            [Double].TryParse(m.Groups(1).Value, kb)
        End If
        input.BitRate = kb

        'get the audio format
        re = New Regex("[A|a]udio:.*")
        m = re.Match(input.RawInfo)
        If m.Success Then
            input.AudioFormat = m.Value
        End If

        'get the video format
        re = New Regex("[V|v]ideo:.*")
        m = re.Match(input.RawInfo)
        If m.Success Then
            input.VideoFormat = m.Value
        End If

        'get the video format
        re = New Regex("(\d{2,3})x(\d{2,3})")
        m = re.Match(input.RawInfo)
        If m.Success Then
            Dim width As Integer = 0
            Dim height As Integer = 0
            Integer.TryParse(m.Groups(1).Value, width)
            Integer.TryParse(m.Groups(2).Value, height)
            input.Width = width
            input.Height = height
        End If
        input.infoGathered = True
    End Sub
#End Region

#Region "Convert to WMV"
    Public Function ConvertToWMV(inputFile As MemoryStream, Filename As String) As OutputPackage
        Dim tempfile As String = Path.Combine(Me.WorkingPath, System.Guid.NewGuid().ToString() & Path.GetExtension(Filename))
        Dim fs As FileStream = File.Create(tempfile)
        inputFile.WriteTo(fs)
        fs.Flush()
        fs.Close()
        GC.Collect()

        Dim vf As VideoFile = Nothing
        Try
            vf = New VideoFile(tempfile)
        Catch ex As Exception
            Throw ex
        End Try

        Dim oo As OutputPackage = ConvertToWMV(vf)

        Try
            File.Delete(tempfile)

        Catch generatedExceptionName As Exception
        End Try

        Return oo
    End Function
    Public Function ConvertToWMV(inputPath As String) As OutputPackage
        Dim vf As VideoFile = Nothing
        Try
            vf = New VideoFile(inputPath)
        Catch ex As Exception
            Throw ex
        End Try

        Dim oo As OutputPackage = ConvertToWMV(vf)
        Return oo
    End Function
    Public Function ConvertToWMV(input As VideoFile) As OutputPackage
        If Not input.infoGathered Then
            GetVideoInfo(input)
        End If
        Dim ou As New OutputPackage()

        'set up the parameters for getting a previewimage
        Dim filename As String = System.Guid.NewGuid().ToString() & ".jpg"
        'Dim secs As Integer

        ''divide the duration in 3 to get a preview image in the middle of the clip
        ''instead of a black image from the beginning.
        'secs = CInt(Math.Truncate(Math.Round(TimeSpan.FromTicks(input.Duration.Ticks \ 3).TotalSeconds, 0)))

        'Dim finalpath As String = Path.Combine(Me.WorkingPath, filename)
        'Dim Params As String = String.Format("-i {0} {1} -vcodec mjpeg -ss {2} -vframes 1 -an -f rawvideo", input.Path, finalpath, secs)
        'Dim output As String = RunProcess(Params)

        'ou.RawOutput = output

        'If File.Exists(finalpath) Then
        '    ou.PreviewImage = LoadImageFromFile(finalpath)
        '    Try
        '        File.Delete(finalpath)
        '    Catch generatedExceptionName As Exception
        '    End Try
        'Else
        '    'try running again at frame 1 to get something
        '    Params = String.Format("-i {0} {1} -vcodec mjpeg -ss {2} -vframes 1 -an -f rawvideo", input.Path, finalpath, 1)
        '    output = RunProcess(Params)

        '    ou.RawOutput = output

        '    If File.Exists(finalpath) Then
        '        ou.PreviewImage = LoadImageFromFile(finalpath)
        '        Try
        '            File.Delete(finalpath)
        '        Catch generatedExceptionName As Exception
        '        End Try
        '    End If
        'End If

        Dim finalpath As String = input.Path.ToString.Replace("flv", "wmv")
        'Dim Params As String = String.Format("-i {0} -y -ar 22050 -ab 64 -f flv {1}", input.Path, finalpath)
        Dim Params As String = String.Format("-y -i {0} -vcodec wmv2 -acodec wmav2 -b:v 1000k -b:a 160k -r 25 {1}", input.Path, finalpath)
        Dim output As String = RunProcess(Params)

        If File.Exists(finalpath) Then
            ou.VideoStream = LoadMemoryStreamFromFile(finalpath)
            Try
                File.Delete(finalpath)
            Catch generatedExceptionName As Exception
            End Try
        End If
        Return ou
    End Function
#End Region
End Class

Public Class VideoFile
#Region "Properties"
    Private _Path As String
    Public Property Path() As String
        Get
            Return _Path
        End Get
        Set(value As String)
            _Path = value
        End Set
    End Property

    Public Property Duration() As TimeSpan
        Get
            Return m_Duration
        End Get
        Set(value As TimeSpan)
            m_Duration = value
        End Set
    End Property
    Private m_Duration As TimeSpan
    Public Property BitRate() As Double
        Get
            Return m_BitRate
        End Get
        Set(value As Double)
            m_BitRate = value
        End Set
    End Property
    Private m_BitRate As Double
    Public Property AudioFormat() As String
        Get
            Return m_AudioFormat
        End Get
        Set(value As String)
            m_AudioFormat = value
        End Set
    End Property
    Private m_AudioFormat As String
    Public Property VideoFormat() As String
        Get
            Return m_VideoFormat
        End Get
        Set(value As String)
            m_VideoFormat = value
        End Set
    End Property
    Private m_VideoFormat As String
    Public Property Height() As Integer
        Get
            Return m_Height
        End Get
        Set(value As Integer)
            m_Height = value
        End Set
    End Property
    Private m_Height As Integer
    Public Property Width() As Integer
        Get
            Return m_Width
        End Get
        Set(value As Integer)
            m_Width = value
        End Set
    End Property
    Private m_Width As Integer
    Public Property RawInfo() As String
        Get
            Return m_RawInfo
        End Get
        Set(value As String)
            m_RawInfo = value
        End Set
    End Property
    Private m_RawInfo As String
    Public Property infoGathered() As Boolean
        Get
            Return m_infoGathered
        End Get
        Set(value As Boolean)
            m_infoGathered = value
        End Set
    End Property
    Private m_infoGathered As Boolean
#End Region

#Region "Constructors"
    Public Sub New(path As String)
        _Path = path
        Initialize()
    End Sub
#End Region

#Region "Initialization"
    Private Sub Initialize()
        Me.infoGathered = False
        'first make sure we have a value for the video file setting
        If String.IsNullOrEmpty(_Path) Then
            Throw New Exception("Could not find the location of the video file")
        End If

        'Now see if the video file exists
        If Not File.Exists(_Path) Then
            Throw New Exception("The video file " & _Path & " does not exist.")
        End If
    End Sub
#End Region
End Class

Public Class OutputPackage
    Public Property VideoStream() As MemoryStream
        Get
            Return m_VideoStream
        End Get
        Set(value As MemoryStream)
            m_VideoStream = value
        End Set
    End Property
    Private m_VideoStream As MemoryStream
    Public Property PreviewImage() As System.Drawing.Image
        Get
            Return m_PreviewImage
        End Get
        Set(value As System.Drawing.Image)
            m_PreviewImage = value
        End Set
    End Property
    Private m_PreviewImage As System.Drawing.Image
    Public Property RawOutput() As String
        Get
            Return m_RawOutput
        End Get
        Set(value As String)
            m_RawOutput = value
        End Set
    End Property
    Private m_RawOutput As String
    Public Property Success() As Boolean
        Get
            Return m_Success
        End Get
        Set(value As Boolean)
            m_Success = value
        End Set
    End Property
    Private m_Success As Boolean
End Class

