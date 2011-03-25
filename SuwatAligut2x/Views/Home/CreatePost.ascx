<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SuwatAligut2x.Models.PostsModels>" %>

<div class="Post">
    <%using (Html.BeginForm("CreatePost", "Home"))
      { %>
    <%=Html.TextBoxFor(m => m.Message)%>
    <input type="submit" value="Submit" />
    <%} %>
</div>