<%@ Page Title="Example - Include" Language="C#" MasterPageFile="~/MasterPages/Fragment.master" AutoEventWireup="true"
   CodeBehind="Include.aspx.cs" Inherits="WebApplication.Example.Include" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <asp:Literal runat="server" ID="LitTransformationResult" />
</asp:Content>
