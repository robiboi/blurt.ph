<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<SuwatAligut2x.Models.PostsModels>>" %>

    <script type="text/javascript">
        $(function() {
            $(".btnVote").hide();
        });
    </script>
    <%
        foreach (SuwatAligut2x.Models.PostsModels post in Model)
        {
    %>
    <div class="PostPreview">
        <div class="statcontainer">
            <div class="Views"><img src="http://www.gravatar.com/avatar/<%=post.Gravatar %>?s=128&d=identicon&r=PG" alt="" style="width: 50px;" /></div>
            <div class="Votes Relative"><a href="#" class="btnVote VoteUp">&nbsp;</a><%=post.NumberOfVotes %> <span>Votes</span><a href="#" class="btnVote VoteDown">&nbsp;</a></div>
        </div>
        <div class="Summary">
            <div class="MessageContent"><a href="<%=Url.Content(string.Format("~/users/post/{0}/{1}", post.PostUserId, post.MessageId)) %>"><%=post.Message %></a></div>
            <div class="UserInfo">Posted by: <a href="<%=Url.Content("~/users/post/" + post.PostUserId) %>"><%=post.DisplayName %></a></div>
        </div>
    </div>
    <%
        }
    %>
    
    <% if (HttpContext.Current.User.Identity.IsAuthenticated)
       { %>
    <script type="text/javascript">
        $(function() {
            $(".btnVote").hide();
            $("div.Votes").hover(
                function() {
                    $(this).find(".btnVote").show();
                },
                function() {
                    $(this).find(".btnVote").hide();
                }
            );
        });
    </script>
    <%} %>