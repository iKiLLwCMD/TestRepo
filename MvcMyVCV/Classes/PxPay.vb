'****************************************************************************** 
'* Name : PxPay.cs 
'* Description : Classes used interact with the PxPay interface using C# .Net 3.5 
'* Copyright : Direct Payment Solutions 2009(c) 
'* Date : 2009-05-06 
'* Version : 1.0 
'* Author : Thomas Treadwell 
'****************************************************************************** 

Imports Microsoft.VisualBasic
Imports System.Net
Imports System.IO
Imports System.Xml

Imports System.Reflection

Namespace PaymentExpress
    Namespace PxPay
        ''' <summary> 
        ''' Main class for submitting transactions via PxPay using static methods 
        ''' </summary> 
        Public Class PxPay
            Private _WebServiceUrl As String = ConfigurationManager.AppSettings("PaymentExpress.PxPay")
            Private _PxPayUserId As String
            Private _PxPayKey As String

            ''' <summary> 
            ''' 
            ''' </summary> 
            ''' <param name="PxPayUserId"></param> 
            ''' <param name="PxPayKey"></param> 
            Public Sub New(ByVal PxPayUserId As String, ByVal PxPayKey As String)
                _PxPayUserId = PxPayUserId
                _PxPayKey = PxPayKey
            End Sub

            ''' <summary> 
            ''' 
            ''' </summary> 
            ''' <param name="result"></param> 
            ''' <returns></returns> 
            Public Function ProcessResponse(ByVal result As String) As ResponseOutput
                Dim myResult As New ResponseOutput(SubmitXml(ProcessResponseXml(result)))
                Return myResult
            End Function

            ''' <summary> 
            ''' 
            ''' </summary> 
            ''' <param name="input"></param> 
            ''' <returns></returns> 
            Public Function GenerateRequest(ByVal input As RequestInput) As RequestOutput
                Dim result As New RequestOutput(SubmitXml(GenerateRequestXml(input)))
                Return result
            End Function

            Private Function SubmitXml(ByVal InputXml As String) As String
                Dim webReq As HttpWebRequest = DirectCast(WebRequest.Create(_WebServiceUrl), HttpWebRequest)
                webReq.Method = "POST"

                Dim reqBytes As Byte()

                reqBytes = System.Text.Encoding.UTF8.GetBytes(InputXml)
                webReq.ContentType = "application/x-www-form-urlencoded"
                webReq.ContentLength = reqBytes.Length
                webReq.Timeout = 5000
                Dim requestStream As Stream = webReq.GetRequestStream()
                requestStream.Write(reqBytes, 0, reqBytes.Length)
                requestStream.Close()

                Dim webResponse As HttpWebResponse = DirectCast(webReq.GetResponse(), HttpWebResponse)
                Using sr As New StreamReader(webResponse.GetResponseStream(), System.Text.Encoding.ASCII)
                    Return sr.ReadToEnd()
                End Using
            End Function

            ''' <summary> 
            ''' Generates the XML required for a GenerateRequest call 
            ''' </summary> 
            ''' <param name="input"></param> 
            ''' <returns></returns> 
            Private Function GenerateRequestXml(ByVal input As RequestInput) As String

                Dim sw As New StringWriter()

                Dim settings As New XmlWriterSettings()
                settings.Indent = True
                settings.NewLineOnAttributes = False
                settings.OmitXmlDeclaration = True

                Using writer As XmlWriter = XmlWriter.Create(sw, settings)
                    writer.WriteStartDocument()
                    writer.WriteStartElement("GenerateRequest")
                    writer.WriteElementString("PxPayUserId", _PxPayUserId)
                    writer.WriteElementString("PxPayKey", _PxPayKey)

                    Dim properties As PropertyInfo() = input.[GetType]().GetProperties()

                    For Each prop As PropertyInfo In properties
                        If prop.CanWrite Then
                            Dim val As String = DirectCast(prop.GetValue(input, Nothing), String)

                            If val IsNot Nothing OrElse val <> String.Empty Then

                                writer.WriteElementString(prop.Name, val)
                            End If
                        End If
                    Next
                    writer.WriteEndElement()
                    writer.WriteEndDocument()
                    writer.Flush()
                End Using

                Return sw.ToString()
            End Function

            ''' <summary> 
            ''' Generates the XML required for a ProcessResponse call 
            ''' </summary> 
            ''' <param name="result"></param> 
            ''' <returns></returns> 
            Private Function ProcessResponseXml(ByVal result As String) As String

                Dim sw As New StringWriter()

                Dim settings As New XmlWriterSettings()
                settings.Indent = True
                settings.NewLineOnAttributes = False
                settings.OmitXmlDeclaration = True

                Using writer As XmlWriter = XmlWriter.Create(sw, settings)
                    writer.WriteStartDocument()
                    writer.WriteStartElement("ProcessResponse")
                    writer.WriteElementString("PxPayUserId", _PxPayUserId)
                    writer.WriteElementString("PxPayKey", _PxPayKey)
                    writer.WriteElementString("Response", result)
                    writer.WriteEndElement()
                    writer.WriteEndDocument()
                    writer.Flush()
                End Using

                Return sw.ToString()
            End Function

        End Class


        ''' <summary> 
        ''' Class containing properties describing transaction details 
        ''' </summary> 
        Public Class RequestInput
            Private _AmountInput As String
            Private _BillingId As String
            Private _CurrencyInput As String
            Private _DpsBillingId As String
            Private _DpsTxnRef As String
            Private _EmailAddress As String
            Private _EnableAddBillCard As String
            Private _MerchantReference As String
            Private _TxnData1 As String
            Private _TxnData2 As String
            Private _TxnData3 As String
            Private _TxnType As String
            Private _TxnId As String
            Private _UrlFail As String
            Private _UrlSuccess As String
            Private _Opt As String


            Public Sub New()
            End Sub


            Public Property AmountInput() As String
                Get
                    Return _AmountInput
                End Get
                Set(ByVal value As String)
                    _AmountInput = value
                End Set
            End Property

            Public Property BillingId() As String
                Get
                    Return _BillingId
                End Get
                Set(ByVal value As String)
                    _BillingId = value
                End Set
            End Property

            Public Property CurrencyInput() As String
                Get
                    Return _CurrencyInput
                End Get
                Set(ByVal value As String)
                    _CurrencyInput = value
                End Set
            End Property

            Public Property DpsBillingId() As String
                Get
                    Return _DpsBillingId
                End Get
                Set(ByVal value As String)
                    _DpsBillingId = value
                End Set
            End Property

            Public Property DpsTxnRef() As String
                Get
                    Return _DpsTxnRef
                End Get
                Set(ByVal value As String)
                    _DpsTxnRef = value
                End Set
            End Property

            Public Property EmailAddress() As String
                Get
                    Return _EmailAddress
                End Get
                Set(ByVal value As String)
                    _EmailAddress = value
                End Set
            End Property

            Public Property EnableAddBillCard() As String
                Get
                    Return _EnableAddBillCard
                End Get
                Set(ByVal value As String)
                    _EnableAddBillCard = value
                End Set
            End Property

            Public Property MerchantReference() As String
                Get
                    Return _MerchantReference
                End Get
                Set(ByVal value As String)
                    _MerchantReference = value
                End Set
            End Property

            Public Property TxnData1() As String
                Get
                    Return _TxnData1
                End Get
                Set(ByVal value As String)
                    _TxnData1 = value
                End Set
            End Property

            Public Property TxnData2() As String
                Get
                    Return _TxnData2
                End Get
                Set(ByVal value As String)
                    _TxnData2 = value
                End Set
            End Property

            Public Property TxnData3() As String
                Get
                    Return _TxnData3
                End Get
                Set(ByVal value As String)
                    _TxnData3 = value
                End Set
            End Property

            Public Property TxnType() As String
                Get
                    Return _TxnType
                End Get
                Set(ByVal value As String)
                    _TxnType = value
                End Set
            End Property

            Public Property TxnId() As String
                Get
                    Return _TxnId
                End Get
                Set(ByVal value As String)
                    _TxnId = value
                End Set
            End Property

            Public Property UrlFail() As String
                Get
                    Return _UrlFail
                End Get
                Set(ByVal value As String)
                    _UrlFail = value
                End Set
            End Property

            Public Property UrlSuccess() As String
                Get
                    Return _UrlSuccess
                End Get
                Set(ByVal value As String)
                    _UrlSuccess = value
                End Set
            End Property

            Public Property Opt() As String
                Get
                    Return _Opt
                End Get
                Set(ByVal value As String)
                    _Opt = value
                End Set
            End Property

            ' If there are any additional input parameters simply add a new read/write property 

        End Class

        ''' <summary> 
        ''' Class containing properties describing the output of the request 
        ''' </summary> 
        Public Class RequestOutput

            Public Sub New(ByVal Xml As String)
                _Xml = Xml
                SetProperty()
            End Sub

            Private _valid As String
            Private _URI As String

            Private _Xml As String

            Public Property valid() As String
                Get
                    Return _valid
                End Get
                Set(ByVal value As String)
                    _valid = value
                End Set
            End Property

            Public Property URI() As String
                Get
                    Return _URI
                End Get
                Set(ByVal value As String)
                    _URI = value
                End Set
            End Property

            Public ReadOnly Property Url() As String
                Get
                    Return _URI.Replace("&amp;", "&")
                End Get
            End Property


            Private Sub SetProperty()

                Dim reader As XmlReader = XmlReader.Create(New StringReader(_Xml))

                While reader.Read()
                    Dim prop As PropertyInfo
                    If reader.NodeType = XmlNodeType.Element Then
                        prop = Me.[GetType]().GetProperty(reader.Name)
                        If prop IsNot Nothing Then
                            Me.[GetType]().GetProperty(reader.Name).SetValue(Me, reader.ReadString(), System.Reflection.BindingFlags.[Default], Nothing, Nothing, Nothing)
                        End If
                        If reader.HasAttributes Then

                            For count As Integer = 0 To reader.AttributeCount - 1
                                'Read the current attribute 
                                reader.MoveToAttribute(count)
                                prop = Me.[GetType]().GetProperty(reader.Name)
                                If prop IsNot Nothing Then
                                    Me.[GetType]().GetProperty(reader.Name).SetValue(Me, reader.Value, System.Reflection.BindingFlags.[Default], Nothing, Nothing, Nothing)
                                End If
                            Next
                        End If
                    End If

                End While
            End Sub
        End Class

        ''' <summary> 
        ''' Class containing properties describing the outcome of the transaction 
        ''' </summary> 
        Public Class ResponseOutput

            Public Sub New(ByVal Xml As String)
                _Xml = Xml
                SetProperty()
            End Sub

            Private _valid As String
            Private _AmountSettlement As String
            Private _AuthCode As String
            Private _CardName As String
            Private _CardNumber As String
            Private _DateExpiry As String
            Private _DpsTxnRef As String
            Private _Success As String
            Private _ResponseText As String
            Private _DpsBillingId As String
            Private _CardHolderName As String
            Private _CurrencySettlement As String
            Private _TxnData1 As String
            Private _TxnData2 As String
            Private _TxnData3 As String
            Private _TxnType As String
            Private _CurrencyInput As String
            Private _MerchantReference As String
            Private _ClientInfo As String
            Private _TxnId As String
            Private _EmailAddress As String
            Private _BillingId As String
            Private _TxnMac As String

            Private _Xml As String

            Public Property valid() As String
                Get
                    Return _valid
                End Get
                Set(ByVal value As String)
                    _valid = value
                End Set
            End Property

            Public Property AmountSettlement() As String
                Get
                    Return _AmountSettlement
                End Get
                Set(ByVal value As String)
                    _AmountSettlement = value
                End Set
            End Property

            Public Property AuthCode() As String
                Get
                    Return _AuthCode
                End Get
                Set(ByVal value As String)
                    _AuthCode = value
                End Set
            End Property

            Public Property CardName() As String
                Get
                    Return _CardName
                End Get
                Set(ByVal value As String)
                    _CardName = value
                End Set
            End Property

            Public Property CardNumber() As String
                Get
                    Return _CardNumber
                End Get
                Set(ByVal value As String)
                    _CardNumber = value
                End Set
            End Property

            Public Property DateExpiry() As String
                Get
                    Return _DateExpiry
                End Get
                Set(ByVal value As String)
                    _DateExpiry = value
                End Set
            End Property

            Public Property DpsTxnRef() As String
                Get
                    Return _DpsTxnRef
                End Get
                Set(ByVal value As String)
                    _DpsTxnRef = value
                End Set
            End Property

            Public Property Success() As String
                Get
                    Return _Success
                End Get
                Set(ByVal value As String)
                    _Success = value
                End Set
            End Property

            Public Property ResponseText() As String
                Get
                    Return _ResponseText
                End Get
                Set(ByVal value As String)
                    _ResponseText = value
                End Set
            End Property

            Public Property DpsBillingId() As String
                Get
                    Return _DpsBillingId
                End Get
                Set(ByVal value As String)
                    _DpsBillingId = value
                End Set
            End Property

            Public Property CardHolderName() As String
                Get
                    Return _CardHolderName
                End Get
                Set(ByVal value As String)
                    _CardHolderName = value
                End Set
            End Property

            Public Property CurrencySettlement() As String
                Get
                    Return _CurrencySettlement
                End Get
                Set(ByVal value As String)
                    _CurrencySettlement = value
                End Set
            End Property

            Public Property TxnData1() As String
                Get
                    Return _TxnData1
                End Get
                Set(ByVal value As String)
                    _TxnData1 = value
                End Set
            End Property

            Public Property TxnData2() As String
                Get
                    Return _TxnData2
                End Get
                Set(ByVal value As String)
                    _TxnData2 = value
                End Set
            End Property

            Public Property TxnData3() As String
                Get
                    Return _TxnData3
                End Get
                Set(ByVal value As String)
                    _TxnData3 = value
                End Set
            End Property

            Public Property TxnType() As String
                Get
                    Return _TxnType
                End Get
                Set(ByVal value As String)
                    _TxnType = value
                End Set
            End Property

            Public Property CurrencyInput() As String
                Get
                    Return _CurrencyInput
                End Get
                Set(ByVal value As String)
                    _CurrencyInput = value
                End Set
            End Property


            Public Property MerchantReference() As String
                Get
                    Return _MerchantReference
                End Get
                Set(ByVal value As String)
                    _MerchantReference = value
                End Set
            End Property

            Public Property ClientInfo() As String
                Get
                    Return _ClientInfo
                End Get
                Set(ByVal value As String)
                    _ClientInfo = value
                End Set
            End Property

            Public Property TxnId() As String
                Get
                    Return _TxnId
                End Get
                Set(ByVal value As String)
                    _TxnId = value
                End Set
            End Property

            Public Property EmailAddress() As String
                Get
                    Return _EmailAddress
                End Get
                Set(ByVal value As String)
                    _EmailAddress = value
                End Set
            End Property

            Public Property BillingId() As String
                Get
                    Return _BillingId
                End Get
                Set(ByVal value As String)
                    _BillingId = value
                End Set
            End Property

            Public Property TxnMac() As String
                Get
                    Return _TxnMac
                End Get
                Set(ByVal value As String)
                    _TxnMac = value
                End Set
            End Property

            ' If there are any additional elements or attributes added to the output XML simply add a property of the same name. 

            Private Sub SetProperty()

                Dim reader As XmlReader = XmlReader.Create(New StringReader(_Xml))

                While reader.Read()
                    Dim prop As PropertyInfo
                    If reader.NodeType = XmlNodeType.Element Then
                        prop = Me.[GetType]().GetProperty(reader.Name)
                        If prop IsNot Nothing Then
                            Me.[GetType]().GetProperty(reader.Name).SetValue(Me, reader.ReadString(), System.Reflection.BindingFlags.[Default], Nothing, Nothing, Nothing)
                        End If
                        If reader.HasAttributes Then

                            For count As Integer = 0 To reader.AttributeCount - 1
                                'Read the current attribute 
                                reader.MoveToAttribute(count)
                                prop = Me.[GetType]().GetProperty(reader.Name)
                                If prop IsNot Nothing Then
                                    Me.[GetType]().GetProperty(reader.Name).SetValue(Me, reader.Value, System.Reflection.BindingFlags.[Default], Nothing, Nothing, Nothing)
                                End If
                            Next
                        End If
                    End If

                End While
            End Sub


        End Class


    End Namespace
End Namespace

