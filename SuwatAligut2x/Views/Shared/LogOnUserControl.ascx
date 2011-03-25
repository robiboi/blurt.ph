<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SuwatAligut2x.Helpers" %>

<%
    if (Request.IsAuthenticated) {
%>
        Welcome <b><%=Html.LogonUser() %></b>!
        [ <%= Html.ActionLink("Log Off", "LogOff", "Users") %> ]
<%
    }
    else {
%> 
        [ <%= Html.ActionLink("Log On", "LogOn", "Users", new { returnurl = this.Context.Request.Url.AbsolutePath }, null)%> ]
<%
    }
%>
