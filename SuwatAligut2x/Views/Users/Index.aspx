<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SuwatAligut2x.Models.UsersModels>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Index</h2>

    <% using (Html.BeginForm()) {%>
        <%= Html.ValidationSummary(true) %>
        
        <fieldset>
            <legend>Fields</legend>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.UserId) %>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.UserId) %>
                <%= Html.ValidationMessageFor(model => model.UserId) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.DisplayName) %>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.DisplayName) %>
                <%= Html.ValidationMessageFor(model => model.DisplayName) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.FullName) %>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.FullName) %>
                <%= Html.ValidationMessageFor(model => model.FullName) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.Gravatar) %>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.Gravatar) %>
                <%= Html.ValidationMessageFor(model => model.Gravatar) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.Location) %>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.Location) %>
                <%= Html.ValidationMessageFor(model => model.Location) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.Age) %>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.Age) %>
                <%= Html.ValidationMessageFor(model => model.Age) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.BirthDate) %>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.BirthDate, String.Format("{0:g}", Model.BirthDate)) %>
                <%= Html.ValidationMessageFor(model => model.BirthDate) %>
            </div>
            
            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%= Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

