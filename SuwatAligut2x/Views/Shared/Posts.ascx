<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<SuwatAligut2x.Models.PostsModels>>" %>

<div id="posts">
    All Posts <a href="#">Newest</a> | <a href="#">Featured</a> | <a href="#">Votes</a>
    <%if (this.Context.User.Identity.IsAuthenticated)
      { %>
    <div class="Post">
        <%using (Ajax.BeginForm("CreatePost", "Home", new AjaxOptions { UpdateTargetId = "posts" }))
          { %>
        <%=Html.TextBox("NewPost")%>
        <input type="submit" value="Submit" />
        <%} %>
    </div>
    <%} %>
    <%Html.RenderPartial("PostMessages", Model); %>
</div>
<div class="Pager">
    
    
</div>