<%@ Page Title="Math" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true"
   CodeBehind="Math.aspx.cs" Inherits="WebApplication.Math" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h3 style="font-weight:900;">Data Source:
    <asp:DropDownList runat="server" ID="ddlData" AutoPostBack="true">
        <asp:ListItem Text="Math 1" Value="1" />
        <asp:ListItem Text="Math 2" Value="2" />
        <asp:ListItem Text="Math 3" Value="3" />
    </asp:DropDownList>
  </h3>

    <asp:Literal runat="server" ID="LitTransformationResult" />
</asp:Content>

<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
  
</asp:Content>

