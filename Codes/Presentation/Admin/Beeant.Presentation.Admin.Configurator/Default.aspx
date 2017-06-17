<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Beeant.Presentation.Admin.Configurator.Default" %>
<%@ Import Namespace="Beeant.Basic.Services.WebForm.Extension" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<link rel="shortcut icon" href="<%=this.GetUrl("PresentationAdminHomeUrl") %>/images/favicon.ico" type="image/x-icon">
<head runat="server">
    <title>蜂蚁窝权限系统-登入</title>
 
</head>
<body>
 <link href="<%=this.GetUrl("PresentationAdminHomeUrl")  %>/Styles/login.css" rel="stylesheet" type="text/css" />
    <form id="form1" runat="server">
   <div class="container">
		    <div class="logo"><img src="<%=this.GetUrl("PresentationAdminHomeUrl")%>/Images/Erp_login_head.jpg" alt="蜂蚁窝权限系统" /></div>
        	<div class="body">
            	<dl class="cf">
               	  <dt>用户帐号</dt>
            	    <dd><input type="text"  id="txtUserName" class="input" runat="server" name="txtUserAccountName" /></dd>
                </dl>
                <dl class="cf"> 
               	  <dt>登录密码</dt>
                    <dd><input type="password"  id="txtPassword" class="input" runat="server" name="txtPassword" /></dd>
                 </dl>
                 <dl class="cf" <%=IsShowCode?"":"style='display:none'" %> >
               	  <dt>验证码</dt>
                    <dd style="float: left;display: block;">
                        <input type="text"  class="input" runat="server" id="txtCode" maxlength="4" style="width: 60px;" />
                       
                    </dd>
                    <dd style="float: left;display: block; margin-left: 10px;"> 
                       <a href="javascript:void;"><img id="imgCode" src="Code.aspx?vesion=<%=DateTime.Now.ToString() %>" /></a> 
                    </dd>
                 </dl>
                <dl class="cf">  
                    <dt>&nbsp;</dt>
                	<dd>
                        <asp:Button ID="btnLogin" runat="server" Text="登录"  CssClass="btn" onclick="btnLogin_Click" /> </dd>
                </dl>                
            </div>
       <div class="fillet01"><img alt="" src="<%=this.GetUrl("PresentationAdminHomeUrl") %>/Images/Erp_login_fillet01.jpg"/></div>
       <div class="fillet02"><img alt="" src="<%=this.GetUrl("PresentationAdminHomeUrl") %>/Images/Erp_login_fillet02.jpg"/></div>
	</div>
         <script type="text/javascript">
           $("#imgCode").click(function() {
               this.src = "Code.aspx?vesion=" + new Date();
           });
       </script>
    </form>
</body>
</html>
