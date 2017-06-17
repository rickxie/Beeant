Register = function (validateInfos) {
    this.Inputs = [
        { Id: "txtName" },
        { Id: "txtPassword"},
        { Id: "txtSurePassword"},
        { Id: "txtRealName"},
        { Id: "txtEmail"},
        { Id: "txtMobile" },
        { Id: "txtCode"}];
    this.Emails = ["163.com", "126.com", "qq.com", "sina.com",
        "gmail.com", "sohu.com", "vip.163.com", "vip.126.com", "188.com",
        "139.com", "yeah.net"];
    this.Base = new Winner.ClassBase();
    this.ValidateInfos = validateInfos;
};
Register.prototype = {
    Initialize: function () {
        this.InitValidator();
        this.InitInputs();
        this.InitPassword();
        this.IniOther();
        this.InitProtocol();
        this.BindEmals();
    },
    //初始化验证
    InitValidator: function () {
        this.Validator = new Winner.Validator({ StyleFile: "", IsShowMessage: false });
        this.Validator.Initialize();
        for (var i = 0; i < this.ValidateInfos.length; i++) {
            this.ValidateInfos[i].ShowMessageEvent = "focus";
            this.ValidateInfos[i].HideMessageEvent = "blur";
        }
        this.Validator.InitializeControl(this.ValidateInfos);
        this.ResetInvokeValidate();
        this.AddOtherValidations();
    },
    ResetInvokeValidate: function () {//重置验证
        var self = this;
        var func = function (validInfo) {
            if (validInfo == null)
                return false;
            if ($(validInfo.Control).attr("IsValidation") == "false")
                return false;
            var handle = self.Validator.GetValidateHandle(validInfo);
            self.Validator.SetControlValidateStyle(validInfo.Control, handle);
            self.Validator.Message(validInfo, handle);
            return handle == null;
        };
        this.Validator.Validate = function (valid) {
           return  func(valid);
        };
    },
    AddOtherValidations: function () { //重置其它信息
        var self = this;
        var nameInfo = this.Validator.GetValidateInfo($("#txtName")[0]);
        if (nameInfo != null) {
            var func = function () {
                return self.CheckName();
            };
            nameInfo.Handles.push({ Function: func, Message: "该用户已经存在" });
        }
        var emailInfo = this.Validator.GetValidateInfo($("#txtMail")[0]);
        if (emailInfo != null) {
            var func1 = function () {
                return self.CheckEmail();
            };
            emailInfo.Handles.push({ Function: func1, Message: "该邮箱已经被占用" });
        }
        var mobileInfo = this.Validator.GetValidateInfo($("#txtMobile")[0]);
        if (mobileInfo != null) {
            var func2 = function () {
                return self.CheckMobile();
            };
            mobileInfo.Handles.push({ Function: func2, Message: "该手机已经被占用" });
        }
        for (var i = 0; i < this.ValidateInfos.length; i++) {
            if (this.ValidateInfos[i].Name == "Password") {
                var sureInfo = this.Validator.AddControlRegularValidate(
                    { Control: $("#txtSurePassword")[0], Message: "请再次输入密码", IsEventValidate: true, Rules: this.ValidateInfos[i].Rules, ValidateEvent: 'blur' });
                sureInfo.Handles.push({
                    Function: function () {
                        return $("#txtSurePassword").val() == $("#txtPassword").val();
                    },
                    Message: "两次输入密码不一致"
                });
            }
        }
    },
    //初始化控件
    InitInputs: function () {
        for (var i = 0; i < this.Inputs.length; i++) {
            this.BindInputEvent(this.Inputs[i]);
        }
    },
    BindInputEvent: function (input) {
        var self = this;
        $("#" + input.Id).blur(function () {
            self.BlurValue(input);
        });
        $("#" + input.Id).focus(function () {
            self.FocusValue(input);
        });
    },
    FocusValue: function (input) {
        var ctrl = $("#" + input.Id);
        if (ctrl.attr("DefaultValue") == undefined) {
            ctrl.attr("DefaultValue", ctrl.value);
        }
        if (ctrl.val() == ctrl.attr("DefaultValue")) {
            ctrl.val("");
        }
        ctrl.attr("class", ctrl.attr("class").replace(" validctrlerror", "") + " validctrlsucess select");
    },
    BlurValue: function (input) {
        var ctrl = $("#" + input.Id);
        if (ctrl.val() == "") {
            ctrl.val($(ctrl).attr("DefaultValue"));
            ctrl.attr("class", ctrl[0].className.replace(" validctrlerror", "").replace(" validctrlsucess", "").replace(" select", ""));
        }

    },
    //密码
    InitPassword: function () {
        $("#txtPassword").keyup(function () {
            if (this.value == "") {
                $("#divPassStrength").attr("class", "passstrength");
                return;
            }
            var numRg = new RegExp("^\\d+$", "g");
            var enRg = new RegExp("^[a-zA-Z]+$", "g");
            var comRg = new RegExp("^\\w+$", "g");
            var className = "passstrength spass";
            if (numRg.test(this.value) || enRg.test(this.value)) {
                className = "passstrength";
            } else if (comRg.test(this.value)) {
                className = "passstrength mpass";
            }
            $("#divPassStrength").attr("class", className);
        });
       
    },
    //其它
    IniOther: function () {
        var self = this;
        $("#hfCode").click(function () {
            var date = new Date();
            $("#imgCode").attr("src", "/Home/RegisterCode?vesion" + date);
        });
        $("#ckShowOther").click(function () {
            if (this.checked)
                $("#divOther").show();
            else
                $("#divOther").hide();
        });
        $("#btnSubmit").click(function () {
            if ($("#txtCode").val() == "" || $("#txtCode").val() == $("#txtCode").attr("DefaultValue")) {
                alert("请输入验证码");
                return false;
            }
            if (!$("#ckprotocol")[0].checked) {
                alert("请先阅读并同意用户注册协议");
                return false;
            }
            for (var i = 0; i < self.Inputs.length; i++) {
                var ctrl = $("#" + self.Inputs[i].Id);
                if (ctrl.val() == ctrl.attr("DefaultValue"))
                    ctrl.val("");
            }
            var rev = self.Validator.ValidateSubmit();
            if (!rev) {
                for (var j = 0; j < self.Inputs.length; j++) {
                    var cl = $("#" + self.Inputs[j].Id);
                    if (cl.val() == "")
                        cl.val(cl.attr("DefaultValue"));
                }
            }
            return rev;
        });
    },
    //显示Email
    BindEmals: function () {
        var self = this;
        $("#txtEmail").keyup(function (event) {
            self.EmailControl = this;
            self.ShowEmails(this, event);
        });
        $(document).click(function () {
            self.SelectEmail(self.EmailControl, null);
        });
        $("#showEmails").mouseout(function () {
            if (self.EmailControl != null) {
                $(self.EmailControl).removeAttr("IsValidation");
            }
        });
    },
    ShowEmails: function (ctrl, event) {
        if ($(ctrl).val() == "") {
            $("#showEmails").hide();
            return;
        }
        var offset = $(ctrl).position();
        $("#showEmails")[0].style.left = offset.left + "px";
        $("#showEmails")[0].style.top = offset.top + $(ctrl).innerHeight() + 2 + "px";
        $("#showEmails").show();
        switch (event.keyCode) {
            case 13:
                var index = this.GetSelectEmailIndex();
                if (index != -1) {
                    this.SelectEmail(ctrl, $("#showEmails").children()[index]);
                }
                this.Base.CancelEventUp(event);
                break;
            case 38:
                this.MoveEmail(ctrl, -1);
                break;
            case 40:
                this.MoveEmail(ctrl, 1);
                break;
            default:
                this.CreateEmails(ctrl);
                break;
        }
        return;
    },
    MoveEmail: function (ctrl, step) {//移动邮件
        if ($("#showEmails").children().length == 0)
            return;
        var index = this.GetSelectEmailIndex();
        if (index != -1) {
            $("#showEmails").children()[index].className = "";
        }
        index = index + step;
        if (index < 0)
            index = $("#showEmails").children().length - 1;
        if (index >= $("#showEmails").children().length)
            index = 0;
        this.OverEmail(ctrl, $("#showEmails").children()[index], true);
    },
    OverEmail: function (ctrl, email, isSelectText) {
        email.className = "cur";
        $(ctrl).attr("IsValidation", "false");
        if (isSelectText) {
            $(ctrl).val(email.innerHTML);
        }
    },
    GetSelectEmailIndex: function () {//得到选择项
        var index = -1;
        $("#showEmails").children().each(function (i, value) {
            if (value.className == "cur") {
                index = i;
                return;
            }
        });
        return index;
    },
    CreateEmails: function (ctrl) {//创建邮箱
        if ($(ctrl).val() == "")
            return;
        $("#showEmails").html("");
        var self = this;
        var values = $(ctrl).val().split('@');
        $(this.Emails).each(function (index, value) {
            if (values.length > 1 && value.indexOf(values[1], 0) == -1) {
                return;
            }
            var li = document.createElement("span");
            $(li).html(values[0] + "@" + value);
            $(li).mouseover(function () {
                self.OverEmail(ctrl, this, false);
            });
            $(li).mouseout(function () {
                this.className = "";
            });
            $(li).click(function () {
                self.SelectEmail(ctrl, li);
            });
            $("#showEmails").append(li);
        });
    },
    SelectEmail: function (ctrl, email) {
        if (email != null) {
            $(ctrl).focus();
            $(ctrl).val(email.innerHTML);
        }
        $("#showEmails").hide();
        $(ctrl).removeAttr("IsValidation");
    },
    //协议
    InitProtocol: function () {
        $("#hfProtocol").click(function () {
            $("#divProtocol").show();
        });
        $("#closeProtocol").click(function () {
            $("#divProtocol").hide();
        });
    },
    //检查
    CheckName: function () {
        if ($("#txtName").val() == "" || $("#txtName").val() == $("#txtName").attr("DefaultValue"))
            return true;
        var rev = true;
        $.ajax({
            type: "GET",
            url: "/Home/CheckAccountIdentity?name=Name&Number=" + $("#txtName").val(),
            async: false,
            dataType: "text",
            success: function (data) {
                if (data != "True") {
                    rev = false;
                }

            }
        });
        return rev;
    },
    CheckEmail: function () {
        if ($("#txtEmail").val() == "" || $("#txtEmail").val() == $("#txtEmail").attr("DefaultValue"))
            return true;
        var rev = true;
        $.ajax({
            type: "GET",
            url: "/Home/CheckAccountIdentity?name=Email&Number=" + $("#txtEmail").val(),
            async: false,
            dataType: "text",
            success: function (data) {
                if (data != "True") {
                    rev = false;
                }

            }
        });
        return rev;
    },
    CheckMobile: function () {
        if ($("#txtMobile").val() == "" || $("#txtMobile").val() == $("#txtMobile").attr("DefaultValue"))
            return true;
        var rev = true;
        $.ajax({
            type: "GET",
            url: "/Home/CheckAccountIdentity?name=Mobile&Number=" + $("#txtMobile").val(),
            async: false,
            dataType: "text",
            success: function (data) {
                if (data != "True") {
                    rev = false;
                }

            }
        });
        return rev;
    }

};
