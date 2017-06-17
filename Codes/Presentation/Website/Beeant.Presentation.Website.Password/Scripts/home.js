var Password = function (config) {
    this.SendTimes = null;
    this.ValidateInfos = null;
    this.NameInput = $("#txtName");
    this.CodeInput = $("#txtCode");
    this.CodeImage = $("#imgCode");
    this.CodeButton = $("#hfCode");
    this.SubmitButton = $("#btnSubmit");
    this.ValidateValueInput = $("#txtValidateValue");
    this.SendValidateValueButton = $("#btnSendValidateValue");
    this.ValidateTypeDropdown = $("#ddlValidateType");
    this.ValidateTypeLable = $("#lblValidateType");
    this.PasswordInput = $("#txtPassword");
    this.SurePasswordInput = $("#txtSurePassword");
    this.PassStrength = $("#divPassStrength");
    this.Base = new Winner.ClassBase();
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
};
Password.prototype = {
    Initialize: function () {
        this.InitName();
        this.InitCode();
        this.InitValidateValue();
        this.InitPassword();
        this.InitValidator();
        this.InitSumbit();
    },
    FocusValue: function (ctrl) {
        if ($(ctrl).attr("DefaultValue") == undefined) {
            $(ctrl).attr("DefaultValue", ctrl.value);
        }
        if (ctrl.value == $(ctrl).attr("DefaultValue")) {
            ctrl.value = "";
        }
    },
    BlurValue: function (ctrl) {
        if (ctrl.value == "") {
            ctrl.value = $(ctrl).attr("DefaultValue");
        }
    },
    //账户
    InitName: function () {
        var self = this;
        this.NameInput.blur(function () {
            self.BlurValue(this);
            this.className = "text";
            $(this).parent().parent().attr("class", "");
        });
        this.NameInput.focus(function () {
            self.FocusValue(this);
            this.className = "text select";
            $(this).parent().parent().attr("class", "");
        });
    },
    //验证码
    InitCode: function () {
        var self = this;
        this.CodeInput.blur(function () {
            self.BlurValue(this);
            this.className = "code";
        });
        this.CodeInput.focus(function () {
            self.FocusValue(this);
            this.className = "code select";
        });
        this.CodeButton.click(function () {
            var date = new Date();
            self.CodeImage.attr("src", "/Home/Code?vesion=" + date);
        });
    },
    //验证码
    InitValidateValue: function () {
        var self = this;
        this.SendValidateValueButton.click(function () {
            if (self.SendValidateValueButton.attr("disabled") == "disabled")
                return;
            var times =0;
            var func = function () {
                if (times <= 0) {
                    self.SendValidateValueButton.removeAttr("disabled");
                    self.SendValidateValueButton.val("重新发送");
                    return;
                }
                self.SendValidateValueButton.val(times + "秒后重新发送");
                times = times - 1;
                setTimeout(func, 1000);
            };
            $.ajax({
                type: "Post",
                url: "/Home/SendCode",
                data: { ValidateType: self.ValidateTypeDropdown.val(), Name: $("input[name='Name']").val() },
                async: false,
                dataType: "json",
                success: function (data) {
                    if (data.Status) {
                        self.SendValidateValueButton.attr("disabled", "disabled");
                        times = parseInt(data.Message);
                        func();
                    } else {
                        alert(data.Message);
                    }
                },
                error: function () {
                    alert("发送失败，请重新发送");
                }
            });
        });
        this.ValidateTypeDropdown.change(function () {
            self.ValidateTypeLable.html($(this).find("option:selected").attr("ShowValue"));
        });
    },
    //密码
    InitPassword: function () {
        var self = this;
        this.PasswordInput.keyup(function () {
            if (this.value == "") {
                self.PassStrength.attr("class", "passstrength");
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
            self.PassStrength.attr("class", className);
        });
        this.PasswordInput.focus(function () {
            this.type = "password";
            self.FocusValue(this);
        });
        this.SurePasswordInput.focus(function () {
            this.type = "password";
            self.FocusValue(this);
        });
        this.PasswordInput.blur(function () {
            if (this.value == "" || this.value == $(this).attr("DefaultValue"))
                this.type = "text";
            self.BlurValue(this);
        });
        this.SurePasswordInput.blur(function () {
            if (this.value == "" || this.value == $(this).attr("DefaultValue"))
                this.type = "text";
            self.BlurValue(this);
        });

    },
    //初始化验证
    InitValidator: function () {
        if (this.PasswordInput.length == 0)
            return;
        this.Validator = new Winner.Validator({ PropertyName: "name", StyleFile: "", IsShowMessage: false });
        this.Validator.Initialize();
        this.Validator.InitializeControl(this.ValidateInfos);
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
    //提交
    InitSumbit: function () {
        var self = this;
        self.SubmitButton.click(function () {
            if (self.NameInput.length > 0 && (self.NameInput.val() == "" || self.NameInput.attr("DefaultValue") == undefined || self.NameInput.val() == self.NameInput.attr("DefaultValue"))) {
                alert("请输入账户名");
                self.NameInput[0].select();
                return false;
            }
            if (self.CodeInput.length > 0 && (self.CodeInput.val() == "" || self.CodeInput.attr("DefaultValue") == undefined || self.CodeInput.val() == self.CodeInput.attr("DefaultValue"))) {
                alert("请输入验证码");
                self.CodeInput[0].select();
                return false;
            }
            if (self.ValidateValueInput.length > 0 && self.ValidateValueInput.val() == "") {
                alert("请输入验证码");
                self.ValidateValueInput[0].select();
                return false;
            }
            if (self.Validator != null) {
                return self.Validator.ValidateSubmit();
            }
            return true;
        });
    }
};
