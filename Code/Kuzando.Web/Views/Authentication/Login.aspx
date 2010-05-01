<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Content/css/openid.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript" src="/Scripts/Lib/openid-jquery.js"></script>
	<script type="text/javascript">
		$(document).ready(function() {
		    openid.init('openid_identifier');
		});
	</script>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Login or Register
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Login or register using OpenID</h2>
    
    
    <div>
    <% using (Html.BeginForm("Authenticate", "Authentication", new {ReturnUrl = Request.QueryString["ReturnUrl"]}))
       { %>
        
            <fieldset>
                <legend>Sign-in or Create New Account</legend>
                    
                <div id="openid_choice">
	    		<p>Please click your account provider:</p>
	    		<div id="openid_btns"></div>
    			</div>
			
                <p>
                    
                    <%= Html.Hidden("action", "verify") %>
                    <%= Html.TextBox("openid_identifier", "http://", new { autofocus = true })%>
                    <%= Html.ValidationMessage("openId")%>
                </p>
                <p>
                    <input type="submit" value="Login" id="openid_submit"/>
                </p>
                
                <noscript>
			    <p>OpenID is service that allows you to log-on to many different websites using a single indentity.
			    Find out <a href="http://openid.net/what/">more about OpenID</a> and <a href="http://openid.net/get/">how to get an OpenID enabled account</a>.</p>
			    </noscript>
            </fieldset>
    <% } %>
    </div>

</asp:Content>
