<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<SuwatAligut2x.Models.CommentsModels>>" %>

    <div id="CommentsContainer">
        <% foreach (SuwatAligut2x.Models.CommentsModels comments in Model) { %>
        <div class="Comment">
            <span class="Comment"><%=comments.Comment %></span><span class="CommentBy"> - <%=comments.CommentorsDisplayName %> <%=comments.CommentDate.ToShortDateString() %></span>
        </div>
        <% } %>
        <% if (this.Context.User.Identity.IsAuthenticated)
           { %>
        <span class="AddComment Button">Add Comment</span>
        <div class="CommentBox">
            <% using (Ajax.BeginForm("AddComment", "Users", new AjaxOptions { UpdateTargetId = "CommentsContainer" }))
               { %>
            <%=Html.TextBox("NewComment")%>
            <%=Html.Hidden("MessageId", this.ViewContext.RouteData.Values["msgid"])%>
            <input type="submit" name="Submit" value="Add Comment" />
            <% } %>
        </div>
        <%} %>
    </div>