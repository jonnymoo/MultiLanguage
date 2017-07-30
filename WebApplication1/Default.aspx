<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1._Default" UICulture="auto" Culture="auto" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Multilanguage test page</h1>
    <p>
        Put the DLLs from Language module in the /bin directory.<br />
        Put the modules entry in the web.config<br />
        Put language files in the App_GlobalResources <br />
        Good to go!
    </p>

    <h2>Thread Culture</h2>
    <%= System.Threading.Thread.CurrentThread.CurrentUICulture.ToString() %>

    <h2>Language</h2>
  
    <%= new Civica.C360.Language.Language(HttpContext.Current).CurrentLanguage %>
    <br />
    
    <h2>Example of how to swap in text</h2>
    <p>%%Civica.Lang:MyTest.I am some text that will be different in welsh%%</p>

    <h2>Example of how to swap in a link to change language</h2>

%%Civica.Lang:Language.SetLanguage%%


</asp:Content>
