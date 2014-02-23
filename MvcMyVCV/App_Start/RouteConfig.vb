﻿Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Http
Imports System.Web.Mvc
Imports System.Web.Routing

Public Class RouteConfig
    Public Shared Sub RegisterRoutes(ByVal routes As RouteCollection)
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}")


        routes.MapHttpRoute( _
            name:="DefaultApi", _
            routeTemplate:="api/{controller}/{id}", _
            defaults:=New With {.id = RouteParameter.Optional} _
        )

        routes.MapRoute( _
            name:="ShortList", _
            url:="ShortList/{id}", _
            defaults:=New With {.controller = "Home", .action = "ShortList", .id = UrlParameter.Optional} _
        )

        routes.MapRoute( _
            name:="ViewVCV", _
            url:="ViewVCV/{id}", _
            defaults:=New With {.controller = "Home", .action = "ViewVCV", .id = UrlParameter.Optional} _
        )

        routes.MapRoute( _
            name:="Default", _
            url:="{controller}/{action}/{id}", _
            defaults:=New With {.controller = "Home", .action = "Index", .id = UrlParameter.Optional} _
        )

        
    End Sub
End Class