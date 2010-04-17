<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kuzando.Common.Web.ModelBase<Kuzando.Model.Entities.DB.User>>" %>
<%
    if (Model != null && Model.LoggedInUser != null) {
%>
        Welcome <b><%= Html.Encode(Model.LoggedInUser.Name) %></b>!
        [ <%= Html.ActionLink("Logout", "Logout", "Authentication", new { returnUrl = ViewContext.HttpContext.Request.Url.PathAndQuery }, null)%> ]
<%
    }
    else {
%> 
        [ <%= Html.ActionLink("Login", "Login", "Authentication", new { returnUrl = ViewContext.HttpContext.Request.Url.PathAndQuery }, null)%> ]
<%
    }
%>
