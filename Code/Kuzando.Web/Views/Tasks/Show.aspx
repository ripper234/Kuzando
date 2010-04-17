<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Kuzando.Common.Web.ItemModel<Kuzando.Persistence.Repositories.TasksForDateRange, Kuzando.Model.Entities.DB.User>>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Your tasks></title>
</head>
<body>
<h1>Your tasks for <%= Html.Encode(Model.Item.Range.From.ToShortDateString()) %> - <%= Html.Encode(Model.Item.Range.To.ToShortDateString())%></h1>
    <ul>
    <% foreach (var item in Model.Item.Tasks)
       {%>
        <li><%= item.Title %></li>
    <%
       }%>
       </ul>
</body>
</html>
