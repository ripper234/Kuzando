<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Kuzando.Common.Web.ItemModel<Kuzando.Persistence.Repositories.TasksForDateRange, Kuzando.Model.Entities.DB.User>>" %>
<%@ Import Namespace="Kuzando.Model.Entities.DB"%>
<%@ Import Namespace="Kuzando.Web.Helpers" %>
<%@ Import Namespace="Kuzando.Common" %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {

    }
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="/Scripts/GridRenderer.js"></script>
    <script type="text/javascript" src="/Scripts/Lib/jquery-ui-1.8.1.custom.min.js"></script>
    <script type="text/javascript" src="/Scripts/Lib/jquery.jeditable.mini.js"></script>
    <script type="text/javascript" src="/Scripts/Lib/showdown.js"></script>
    <script type="text/javascript" src="/Scripts/Lib/jquery-jtemplates.js"></script>
    <script type="text/javascript" src="/Scripts/Lib/jquery.rule.js"></script>
    
    <link type="text/css" href="/Scripts/Lib/jquery-ui/cupertino/jquery-ui-1.8.1.custom.css" rel="Stylesheet" />	

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Kuzando - Tasks Made Easy
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="float:left">
    <h1>Your tasks for 
    <%= Html.Encode(DateFormatter.Format(Model.Item.Range.From)) %> - <%= Html.Encode(DateFormatter.Format(Model.Item.Range.To))%>
    
    </h1>
    
    </div>
    
    <div style="float:right">
    <table id="action-icons">
    <tbody>
        <tr>
            <td><%= "<input id='datepicker' type='hidden' value='" + Html.Encode(DateFormatter.Format(Model.Item.Range.From)) + "'/>"%></td>
            <td style="text-align:left">
            <table><tr>
            <td>Hide<br />Done?</td>
            <td>
            <fieldset>
            <%= "<input type='checkbox' title='Show or hide Done tasks' id='hide-done' " + (((Model.LoggedInUser.SettingsFlags & UserSettings.HideDone) == UserSettings.HideDone) ? "Checked" : "") + "/>"%>
            </fieldset>
            </td>
            </tr></table>
            </td>
            
            <td><img src="/Content/images/new_sticky.png" width="64" height="64" title="Create a new sticky" alt="" id="newsticky"/></td>
            <td><img src="/Content/images/trashbin.png" width="60" height="60" title="Trash it" alt="" id="trash"/></td>
        </tr>
        
        <tr>
            <td></td>
            <td id="prevWeek">
                <a href="<%= Url.Action("ShowWeek", "Tasks", new { from = Model.Item.Range.From.AddDays(-7).GetDaysSince1970()}) %>">
                <img src="/Content/images/left_arrow.png" title="Previous Week" alt="" width="36" height="36"/>
            </a>
            </td>
            
            <td id="nextWeek">
                <a href="<%= Url.Action("ShowWeek", "Tasks", new { from = Model.Item.Range.From.AddDays(7).GetDaysSince1970()}) %>">
                <img src="/Content/images/right_arrow.png" title="Next Week" alt="" width="36" height="36"/>
            </a>
            </td>
        </tr>
        </tbody>
    </table>
    </div>
    <br style="clear:both"/>
 	<table width="100%" class="tasksgrid">
 	<tbody>
 	    <tr>
 	        <% foreach (var day in Enum.GetValues(typeof(DayOfWeek))) {%>
             <th class='headercell'>
             <div>
             <%= Html.Encode(day +  " (" + Model.Item.Range.From.AddDays((int)day).Day + ")") %> 
             </div>
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
 	<fieldset>
 	<%= "<input id='fromDate' name='Item.Range.From' type='hidden' value='"
        + Model.Item.Range.From.GetDaysSince1970()
 	    + "' />" %>
 	</fieldset>
 	</form>
 	
 	<div class="sticky-template" style="display:none">
 	<table class="task-table">
    <tbody>
    <tr>
        <td class="sticky-icons">
        <div class="checked-cell"></div>
        <img class="warn" src="/Content/Images/warning.png" alt="overdue" height="20" width="20" title="This task is overdue!"/>
        </td>
        <td>
        <div class="edit"></div>
        </td>
    </tr>
    <tr>
        <td class="warn">
        
        </td>
    </tr>
    </tbody>
    </table>
    </div>
</asp:Content>