<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FolderList.ascx.cs" Inherits="Beeant.Presentation.Admin.Editor.Editor.FolderList" %>

    <asp:Repeater ID="Repeater1" runat="server">
        <ItemTemplate>
           <li><a href='<%#GetUrl(Eval("Id")) %>' value='<%#Eval("Id") %>'><%#Eval("Name") %></a></li>
        </ItemTemplate>
    </asp:Repeater>

