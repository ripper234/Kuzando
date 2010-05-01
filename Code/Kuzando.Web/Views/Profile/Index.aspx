<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Kuzando.Common.Web.ItemModel<Kuzando.Web.Models.Profile, Kuzando.Model.Entities.DB.User>>" %>
<%@ Import Namespace="MvcContrib.UI.Grid" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">    
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Kuzando - Profile
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div>
    <h1>Edit your profile</h1>
</div>
       <br />
       
       <% using (Html.BeginForm("Edit", "Profile")) {%>     	
       
       <%= Html.EditorFor(x => x.Item)%>
       
       <input type="submit" value="Update"/>
       
       <%} %>
       
     	
</asp:Content>