Register = function (validateInfos) {
    this.Inputs = [
        { Id: "txtMobile" },
        { Id: "txtPassword" },
        { Id: "txtSurePassword" },
        { Id: "txtMobile" },
        { Id: "txtCode" },
        { Id: "txtMobileCode" }];
    this.Base = new Winner.ClassBase();
    this.ValidateInfos = validateInfos;
};
Register.prototype = {
    Initialize: function () {
        this.InitValidator();
        this.InitInputs();
        this.InitPassword();
        this.InitOther();
        this.InitProtocol();
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
            return func(valid);
        };
    },
    AddOtherValidations: function () { //重置其它信息
        var self = this;
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
        $("#txtPassword").focus(function () {
            this.type = "password";
        });
        $("#txtPassword").blur(function () {
            if (this.value == "" || this.value == $(this).attr("DefaultValue"))
                this.type = "text";
        });
        $("#txtSurePassword").focus(function () {
            this.type = "password";
        });
        $("#txtSurePassword").blur(function () {
            if (this.value == "" || this.value == $(this).attr("DefaultValue"))
                this.type = "text";
        });
    },
    //其它
    InitOther: function () {
        var self = this;
        $("#hfCode").click(function () {
            var date = new Date();
            $("#imgCode").attr("src", "/Home/Code?vesion=" + date);
        });
        $("#btnMobileCode").click(function() {
            self.SendMobileCode();
        });
        $("#btnSubmit").click(function () {
            if ($("#txtMobileCode").val() == "" || $("#txtMobileCode").val() == $("#txtMobileCode").attr("DefaultValue")) {
                $("#txtMobileCode")[0].select();
                alert("请输入您收到的手机验证码");
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
    //协议
    InitProtocol: function () {
        $("#hfProtocol").click(function () {
            $("#divProtocol").css("top",$("dl").height()+30+"px");
            $("#divProtocol").show();
        });
        $("#closeProtocol").click(function () {
            $("#divProtocol").hide();
        });
    },
    //检查
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
    },
    //检查
    SendMobileCode: function () {
        if ($("#btnMobileCode").attr("disabled") == "disabled") {
            return;
        }
        if ($("#txtMobile").val() == "" || $("#txtMobile").val() == $("#txtMobile").attr("DefaultValue")) {
            alert("请输入手机号码");
            $("#txtMobile")[0].select();
            return;
        }
        if ($("#txtMobile").attr("class").indexOf("validctrlerror") > -1) {
            alert("请输入正确的手机号码");
            $("#txtMobile")[0].select();
            return;
        }
        if ($("#txtCode").val() == "" || $("#txtCode").val() == $("#txtCode").attr("DefaultValue")) {
            alert("请输入验证码");
            $("#txtCode")[0].select();
            return ;
        }
        var times = 0;
        var func = function () {
            if (times == 0) {
                $("#btnMobileCode").removeAttr("disabled");
                $("#btnMobileCode").val("获取手机验证码");
                return;
            }
            $("#btnMobileCode").val(times+"秒后重写获取验证码");
            times = times - 1;
            setTimeout(func, 1000);
        }
        $.ajax({
            type: "GET",
            url: "/Home/SendMobileCode?mobile=" + $("#txtMobile").val() + "&code=" + $("#txtCode").val(),
            async: false,
            dataType: "json",
            success: function (data) {
                var date = new Date();
                $("#imgCode").attr("src", "/Home/Code?vesion=" + date);
                if (!data.status) {
                    alert(data.message);
                } else {
                    times = parseInt(data.message);
                    $("#btnMobileCode").attr("disabled", "disabled");
                    func();
                }

            },
            error:function() {
                var date = new Date();
                $("#imgCode").attr("src", "/Home/Code?vesion=" + date);
            }
        });
    }

};
