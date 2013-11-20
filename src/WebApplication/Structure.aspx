<%@ Page Title="XSLT Structure" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true"
   CodeBehind="Structure.aspx.cs" Inherits="WebApplication.Structure" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link href="/Styles/docco.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
  

    <asp:Literal runat="server" ID="LitTransformationResult" />
</asp:Content>

<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
    
</asp:Content>

