<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Kuzando.Common.Web.ItemModel<Kuzando.Persistence.Repositories.TasksForDateRange, Kuzando.Model.Entities.DB.User>>" %>
<%@ Import Namespace="MvcContrib.UI.Grid" %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {

    }
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="/Scripts/GridRenderer.js"></script>
    <script type="text/javascript" src="/Scripts/Lib/jquery-ui-1.8.custom.min.js"></script>
    <script type="text/javascript" src="/Scripts/Lib/jquery.jeditable.mini.js"></script>
    <script type="text/javascript" src="/Scripts/Lib/showdown.js"></script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Kuzando - Tasks Made Easy
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="edit">Your tasks for <%= Html.Encode(Model.Item.Range.From.ToShortDateString()) %> - <%= Html.Encode(Model.Item.Range.To.ToShortDateString())%></h1>
    
    <table id="action-icons">
    <tbody>
        <tr>
            <td><img src="/Content/images/new_sticky.png" width="64" height="64" title="Create a new sticky" alt="" id="newsticky"/></td>
            <td><img src="/Content/images/trashbin.png" width="60" height="60" title="Trash it" alt="" id="trash"/></td>
        </tr>
        
        <tr>
            <td>
            
            <a href="<%= Url.Action("Show", "Tasks", new { from = Model.Item.Range.From.AddDays(-7),
                                                            to = Model.Item.Range.To.AddDays(-7)}) %>">
                <img src="/Content/images/left_arrow.png" title="Previous Week" alt="" width="36" height="36"/>
            </a>
            </td>
            
            <td>
            <a href="<%= Url.Action("Show", "Tasks", new { from = Model.Item.Range.From.AddDays(7),
                                                            to = Model.Item.Range.To.AddDays(7)}) %>">
                <img src="/Content/images/right_arrow.png" title="Next Week" alt="" width="36" height="36"/>
            </a>
            </td>
        </tr>
        </tbody>
    </table>
    
 	<table width="100%" class="tasksgrid">
 	<tbody>
 	    <tr>
 	        <% foreach (var day in Enum.GetValues(typeof(DayOfWeek))) {%>
             <th class='taskcell'>
             <% if (Model.Item.Range.From.AddDays((int)day).Date == DateTime.Now.Date) {%>
                 <div class="today">
                 <% } %>
    <%= Html.Encode(day) %> 
    <% if (DateTime.Now.Subtract(Model.Item.Range.From).Days == (int)day) {%>
                 </div>
                 <% } %>
                 </th>
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
    </tbody>
 	</table>
     	
 	<form id="jsParams" action="">
 	<%= Html.HiddenFor(x => x.Item.Range.From, new {id = "fromDate"})%>
 	<%= Html.HiddenFor(x => x.Item.Range.To, new {id = "toDate"})%>
 	</form>
</asp:Content>