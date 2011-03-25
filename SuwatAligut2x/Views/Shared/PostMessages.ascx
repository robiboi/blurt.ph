<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<SuwatAligut2x.Models.PostsModels>>" %>

    <%
        foreach (SuwatAligut2x.Models.PostsModels post in Model)
        {
    %>
    <div class="PostPreview">
        <div class="statcontainer">
            <div class="Views"><img src="http://www.gravatar.com/avatar/<%=post.Gravatar %>?s=128&d=identicon&r=PG" alt="" style="width: 50px;" /></div>
            <div class="Votes"><%=post.NumberOfVotes %> <span>Votes</span></div>
        </div>
        <div class="Summary">
            <div class="MessageContent"><a href="<%=Url.Content(string.Format("~/users/post/{0}/{1}", post.PostUserId, post.MessageId)) %>"><%=post.Message %></a></div>
            <div class="UserInfo">Posted by: <a href="<%=Url.Content("~/users/post/" + post.PostUserId) %>"><%=post.DisplayName %></a></div>
        </div>
    </div>
    <%
        }
    %>