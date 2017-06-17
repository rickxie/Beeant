function intiPassword(validateInfos) {
    var defaultValueFunc = function (input) {
        function focusValue(input) {
            var ctrl = $(input);
            ctrl.addClass("nor");
            if (ctrl.attr("DefaultValue") == undefined) {
                ctrl.attr("DefaultValue", ctrl.value);
            }
            if (ctrl.val() == ctrl.attr("DefaultValue")) {
                ctrl.val("");
            }
        }

        function blurValue(input) {
            var ctrl = $(input);
            if (ctrl.val() == "") {
                ctrl.val($(ctrl).attr("DefaultValue"));
                ctrl.attr("class", ctrl[0].className.replace(" validctrlerror", "").replace(" validctrlsucess", "").replace(" select", ""));
                ctrl.removeClass("nor");
            } else {
                ctrl.addClass("nor");
            }
        }
        $(input).blur(function () {
            blurValue(input);
        });
        $(input).focus(function () {
            focusValue(input);
        });
    }
    $(document).find("input[DefaultValue]").each(function (index, sender) {
        defaultValueFunc(this);
    });

    function initPassword() {
        $("#txtNewPassword").keyup(function () {
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
        $("#txtNewPassword").focus(function () {
            this.type = "password";
        });
        $("#txtNewPassword").blur(function () {
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
        $("#txtOldPassword").focus(function () {
            this.type = "password";
        });

        $("#txtOldPassword").blur(function () {
            if (this.value == "" || this.value == $(this).attr("DefaultValue"))
                this.type = "text";
        });
    }

    //初始化验证
    function initValidator() {
        var self = this;
        this.Validator = new Winner.Validator({ PropertyName: "ValidateName", StyleFile: "", IsShowMessage: false });
        this.Validator.Initialize();
        this.Validator.InitializeControl(validateInfos);
        for (var i = 0; i < validateInfos.length; i++) {
            if (validateInfos[i].Name == "Password") {
                var sureInfo = this.Validator.AddControlRegularValidate(
                    { Control: $("#txtSurePassword")[0], Message: "请再次输入密码", IsEventValidate: true, Rules: validateInfos[i].Rules, ValidateEvent: 'blur' });
                sureInfo.Handles.push({
                    Function: function () {
                        return $("#txtSurePassword").val() == $("#txtNewPassword").val();
                    },
                    Message: "两次输入密码不一致"
                });
            }
        }
        $("#btnSubmit").click(function () {
            if ($("#txtOldPassword").val() == "") {
                alert("输入原始密码");
                return false;
            }
            if (self.Validator != null) {
                return self.Validator.ValidateSubmit();
            }
            return true;
        });
    }

    initPassword();
    initValidator();
}