﻿Imports System.Net
Imports System.IO
Imports Tweet
Imports LinqToTwitter
Imports System.Linq
Imports System.Web

Partial Class _Default
    Inherits System.Web.UI.Page

    'Private auth As SingleUserAuthorizer
    Private auth As ApplicationOnlyAuthorizer
    Private twitCtxt As TwitterContext


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            'LEFT OFF HERE, 09/09/2013: Try Single User Authentication
            'ref: http://linqtotwitter.codeplex.com/wikipage?title=Single%20User%20Authorization&referringTitle=Learning%20to%20use%20OAuth
            '   -> Debug




            'Dim credentials As IOAuthCredentials = New SessionStateCredentials()

            'If (credentials.ConsumerKey Is Nothing Or credentials.ConsumerSecret Is Nothing) Then
            '    credentials.ConsumerKey = ConfigurationManager.AppSettings("TwitterConsumerKey")
            '    credentials.ConsumerSecret = ConfigurationManager.AppSettings("TwitterConsumerSecret")
            'End If

            'Dim authURL As Action(Of String) = AddressOf Response.Redirect
            'auth = New WebAuthorizer()
            'auth = New WebAuthorizer With {.Credentials = credentials, .PerformRedirect = authURL}


            'If Not Page.IsPostBack Then
            '    auth.CompleteAuthorization(Request.Url)
            'End If

            auth = New ApplicationOnlyAuthorizer() With { _
                .Credentials = New InMemoryCredentials() With { _
                .ConsumerKey = ConfigurationManager.AppSettings("TwitterConsumerKey"), _
                .ConsumerSecret = ConfigurationManager.AppSettings("TwitterConsumerSecret") _
                } _
            }





            '    auth = New SingleUserAuthorizer() With { _
            '        .Credentials = New SingleUserInMemoryCredentials() With { _
            '        .ConsumerKey = ConfigurationManager.AppSettings("TwitterConsumerKey"), _
            '        .ConsumerSecret = ConfigurationManager.AppSettings("TwitterConsumerSecret"), _
            '        .TwitterAccessToken = ConfigurationManager.AppSettings("TwitterAccessToken"), _
            '        .TwitterAccessTokenSecret = ConfigurationManager.AppSettings("TwitterAccessTokenSecret") _
            '    } _
            '}







        Catch ex As Exception
            Throw ex
        End Try



    End Sub


    Protected Sub initGo_lBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles initGo_lBtn.Click

        Try
            auth.Authorize()
        Catch ex As Exception
            Throw ex
        End Try



        initSearch_pnl.Visible = False
        result_pnl.Visible = True

        Dim htQuery As String = IIf(initSearch_txt.Text.Contains("#"), initSearch_txt.Text.Trim.Replace("#", "%23"), "%23" & initSearch_txt.Text.Trim)

        Dim twitCtxt As TwitterContext = New TwitterContext(auth)


        Dim srch = (From search In twitCtxt.Search Where search.Type = SearchType.Search And search.Query = initSearch_txt.Text.Trim Select search).SingleOrDefault
        Dim resultsList As Generic.List(Of Status) = srch.Statuses

        results_repeater.DataSource = resultsList
        results_repeater.DataBind()
        'Using twitCtxt = New TwitterContext(auth)

        'End Using


    End Sub



End Class
