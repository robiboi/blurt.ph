<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Welcome!</h2>
    <p>
        Twitter ala TSTP
    </p>
    <p>
        No need for registration.  If you already have an OpenID account you can login directly and post what ever in your mind.
        If you don't have an OpenID account, you can use your Google or Yahoo account.
    </p>
    <%Html.RenderAction("Posts", new { page = 0, size = 10 }); %>
</asp:Content>
