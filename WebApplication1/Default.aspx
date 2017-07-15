<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1._Default" UICulture="auto" Culture="auto" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    
    <%= System.Threading.Thread.CurrentThread.CurrentUICulture.ToString() %>

    <br />
    
     <%=new Civica.C360.Language.Translator(new Civica.C360.Language.Language(HttpContext.Current), new Civica.C360.Language.FileLanguagePackService()).Translate("","ABC") %>
     <asp:Literal runat="server" Text="<%$ Lang:ABC%>" /> 

   <asp:Label ID="Label2" Runat="server" Text="<%$ Lang:any old shite %>"></asp:Label>
   <asp:Label ID="Label3" runat="server" Text="<%$ Lang:Welsh Text %>"></asp:Label>
    <p>QWERTY</p>


</asp:Content>
