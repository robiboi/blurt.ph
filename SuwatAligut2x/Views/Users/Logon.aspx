<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Logon
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%=Url.Content("~/Content/openid.css") %>" type="text/css" rel="Stylesheet" />
    <script src="<%=Url.Content("~/Scripts/openid-jquery.js") %>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            openid.init('openid_identifier');
        });
	</script>
	<style>
	    #openid-url-input { margin-bottom:30px; margin-left:30px; }
	</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<span><%=ViewData["Message"] %></span>
	<p>You must log in before entering the Members Area: </p>
	<form action="Authenticate?ReturnUrl=<%=HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]) %>" method="post" id="openid_form">
		<input type="hidden" name="action" value="verify" />
		<input type="hidden" name="oauth_version" id="oauth_version" />
		<input type="hidden" name="oauth_serveer" id="oauth_server" />
		<fieldset>
			<legend>Sign-in or Create New Account</legend>
			<div id="openid_choice">

				<p>Please click your account provider:</p>
				<div id="openid_btns"></div>
			</div>
			<div id="openid_input_area">
				
			</div>
			<noscript>
				<p>OpenID is service that allows you to log-on to many different websites using a single indentity.
				Find out <a href="http://openid.net/what/">more about OpenID</a> and <a href="http://openid.net/get/">how to get an OpenID enabled account</a>.</p>
			</noscript>
		</fieldset>
		<p>Or, you can manually enter your OpenID</p>
        <table id="openid-url-input">
            <tr>         
            <td class="vt large">

                <input id="openid_identifier" name="openid_identifier" class="openid-identifier" style="height:28px; width:500px;" type="text" tabindex="100" />
            </td>
            <td class="vt large">
                <input id="submit-button" style="margin-left:5px; height:36px;" type="submit" value="Log in" tabindex="101" />
            </td>
            </tr>                                
        </table>     
	</form>


	<script type="text/javascript">
	document.getElementById("openid_identifier").focus();
	</script>

</asp:Content>
