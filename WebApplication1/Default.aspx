<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1._Default" UICulture="auto" Culture="auto" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <h2>Thread Culture</h2>
    <%= System.Threading.Thread.CurrentThread.CurrentUICulture.ToString() %>

    <h2>Language</h2>
  
    <%= new Civica.C360.Language.Language(HttpContext.Current).GetCurrentLanguage() %>
    <br />
    
     <%=new Civica.C360.Language.Translator(new Civica.C360.Language.Language(HttpContext.Current), new Civica.C360.Language.FileLanguagePackService()).Translate("ABC") %>
     <asp:Literal runat="server" Text="<%$ Lang:ABC%>" /> 

     <asp:Label ID="Label3" runat="server" Text="<%$ Lang:Welsh Text %>"></asp:Label>
     <p>%%Civica.Lang:MyTest.I am some text that will be different in welsh%%</p>


    <asp:Button ID="Welsh" RunAt="server" Text="Welsh" OnClick="Welsh_Click" />
</asp:Content>
