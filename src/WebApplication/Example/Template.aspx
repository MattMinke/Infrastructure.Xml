<%@ Page Title="Example - Include" Language="C#" MasterPageFile="~/MasterPages/Fragment.master" AutoEventWireup="true"
   CodeBehind="Template.aspx.cs" Inherits="WebApplication.Example.Template" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <asp:Literal runat="server" ID="LitTransformationResult" />
</asp:Content>
