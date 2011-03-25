<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SuwatAligut2x.Models.PostsModels>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Post
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Post</h2>

    <div class="PostPreview">
        <div class="statcontainer">
            <div class="Views"><img src="http://www.gravatar.com/avatar/<%=Model.Gravatar %>?s=128&d=identicon&r=PG" alt="" style="width: 50px;" /></div>
            <div class="Votes"><%=Model.NumberOfVotes %></div>
        </div>
        <div class="Message">
            <div class="MessageContent"><%=Model.Message %></div>
        </div>
        <div class="CommentsContainer">
            
            <%Html.RenderPartial("Comments", Model.Comments); %>

        </div>
        <div class="clear"></div>
    </div>
</asp:Content>
