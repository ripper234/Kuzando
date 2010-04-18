<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Kuzando.Common.Web.ItemModel<Kuzando.Persistence.Repositories.TasksForDateRange, Kuzando.Model.Entities.DB.User>>" %>
<%@ Import Namespace="MvcContrib.UI.Grid" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="/Scripts/GridRenderer.js"></script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Recent Questions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h1>Your tasks for <%= Html.Encode(Model.Item.Range.From.ToShortDateString()) %> - <%= Html.Encode(Model.Item.Range.To.ToShortDateString())%></h1>
       <br />
       <%= Html.Grid(Model.Item.Tasks).Columns(column => {
     		column.For(x => x.Id).Named("Person ID");
     		column.For(x => x.Title);
     		column.For(x => x.DueDate).Format("{0:d}");
     	})
        .Attributes(style => "width:100%")
     	.Empty("There are no people.")
     	.RowStart(row => "<tr foo='bar'>") %>
     	
     	<br />
     	
     	<table width="100%" class="tasksgrid">
     	    <tr>
     	        <% foreach (var day in Enum.GetValues(typeof(DayOfWeek))) {%>
                 <th class='taskcell'><%= day %></th>
     	        <%
                 }%>
     	    </tr>
     	    <% for (int row = 0; row < 4; ++row)
             {%>
     	    <%= "<tr" + ((row % 2 == 0) ? " class='even'" : "") + ">" %>
     	    <%
                 foreach (var day in Enum.GetValues(typeof (DayOfWeek)))
                 {%>
     	    <td class='taskcell' ></td>
     	    <%}%>
     	    </tr>
     	    <%
             }%>
     	</table>
     	
     	<form id="jsParams" action="">
     	<%= Html.HiddenFor(x => x.Item.Range.From, new {id = "fromDate"})%>
     	<%= Html.HiddenFor(x => x.Item.Range.To, new {id = "toDate"})%>
     	</form>
</asp:Content>