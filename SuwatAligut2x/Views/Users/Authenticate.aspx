<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SuwatAligut2x.Models.RegisterOpenId>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Authenticate
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Confirm</h2>
    
    <p>This OpenID is new to me.</p>
    <%using (Html.BeginForm("OpenIdConfirm", "Users"))
      { %>
    <%=Html.HiddenFor(m=>m.ClaimedOpenId)%>
    <%=Html.HiddenFor(m=>m.FriendlyOpenId)%>
    <%=Html.HiddenFor(m=>m.ReturnUrl)%>
    <%=Html.HiddenFor(m=>m.Email) %>
    
    <%=Model.FriendlyOpenId%>
    <input type="submit" value="Confirm and Create New Account" />
    <input type="button" value="Cancel" onclick="window.location.href = '/'" />
    <%} %>
</asp:Content>

