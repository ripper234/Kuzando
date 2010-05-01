<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Kuzando Home
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Html.Encode(ViewData["Message"]) %></h2>
    <p>
    Kuzando is a simple task management system based on post-its.<br />
    To start using Kuzando, <a href="/Authentication/Login" title="Login or create your account">login or create your account</a>.
    </p>
    <p>Writen by <a href="http://ripper234.com/">Ron Gross</a> for Aya Federman.</p>
    <br />
    <img src="/Content/Images/screenshot_small.png" width="500" height="456" alt="Kuzando Screenshot" title="Screenshot"/>
</asp:Content>
