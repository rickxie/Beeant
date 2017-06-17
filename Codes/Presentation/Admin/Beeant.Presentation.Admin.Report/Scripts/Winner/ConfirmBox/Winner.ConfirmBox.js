Winner.ConfirmBox = function (config) {
    this.Base = new Winner.ClassBase();
    this.PropertyName = "ConfirmBox";
    this.SuccessHanldePropertyName = "ConfirmBoxSuccessHanlde";
    this.FailureHanldePropertyName = "ConfirmBoxFailureHanlde";
    this.ComfirmValidatePropertyName = "ComfirmValidate";
    this.MessagePropertyName = "ConfirmMessage";
    this.CheckBoxsMessagePropertyName = "ComfirmCheckBoxMessage";
    this.DropdownListMessagePropertyName = "ComfirmDropdownListMessage";
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
};
Winner.ConfirmBox.prototype = {
    Initialize: function () {//加载css样式文件
        this.LoadControl(document.childNodes);
    },
    LoadControl: function (ctrls) {//加载控件
        for (var i = 0; i < ctrls.length; i++) {
            this.LoadControl(ctrls[i].childNodes);
            var name = this.Base.GetAttribute(ctrls[i], this.PropertyName);
            if (name == null || name == "") continue;
            this.BindComfirmEvent(ctrls[i]);
        }
    },
    BindComfirmEvent: function (ctrl) {//绑定事件
        var self = this;
        ctrl.onclick = function () {
            var rev = self.Validate(ctrl);
            var successHanlde = self.Base.GetAttribute(ctrl, self.SuccessHanldePropertyName);
            var failureHanlde = self.Base.GetAttribute(ctrl, self.FailureHanldePropertyName);
            if (rev && successHanlde != null && successHanlde != "")
                eval("window." + successHanlde + "()");
            else if (failureHanlde != null && failureHanlde != "") {
                eval("window." + failureHanlde + "()");
            }
            return rev;
        };
    },
    GetComfirmInfo: function (ctrl) {//得到验证信息
        var name = this.Base.GetAttribute(ctrl, this.PropertyName);
        var info = { Control: ctrl, Message: this.Base.GetAttribute(ctrl, this.MessagePropertyName),
            CheckBoxs: this.GetComfirmCheckBoxs(name),
            CheckBoxsMessage: this.Base.GetAttribute(ctrl, this.CheckBoxsMessagePropertyName),
            Dropdownlist: this.GetDropdownlist(name)
        };
        return info;
    },
    GetComfirmCheckBoxs: function (name) {//得到验证checkbox
        var ctrls = document.getElementsByTagName('input');
        if (ctrls == null || ctrls.length == 0) return null;
        var rev = [];
        for (var i = 0; i < ctrls.length; i++) {
            if (ctrls[i].type == "checkbox" && this.IsComfirmControl(ctrls[i], name)) {
                rev.push(ctrls[i]);
            }
        }
        return rev;
    },
    GetDropdownlist: function (name) {//得到验证Dropdownlist
        var ctrls = document.getElementsByTagName('select');
        if (ctrls == null || ctrls.length == 0) return null;
        var rev = [];
        for (var i = 0; i < ctrls.length; i++) {
            if (this.IsComfirmControl(ctrls[i], name)) {
                rev.push({ Control: ctrls[i], Message: this.Base.GetAttribute(ctrls[i], this.DropdownListMessagePropertyName) });
            }
        }
        return rev;
    },
    IsComfirmControl: function (ctrl, name) {//是否为验证的控件
        var ckName = this.Base.GetAttribute(ctrl, this.ComfirmValidatePropertyName);
        if (ckName == null) return false;
        var ckNames = ckName.split(',');
        for (var i = 0; i < ckNames.length; i++) {
            if (ckNames[i] == name)
                return true;
        }
        return false;
    },
    Validate: function (ctrl) {//验证
        var rev = true;
        var info = this.GetComfirmInfo(ctrl);
        if (info.Message != null && info.Message != "")
            rev = confirm(info.Message);
        return rev && this.ValidateDropdownlist(info) && this.ValidateCheckBoxs(info);
    },
    ValidateCheckBoxs: function (info) {//验证多选框
        if (info.CheckBoxs == null || info.CheckBoxs.length == 0) return true;
        for (var i = 0; i < info.CheckBoxs.length; i++) {
            if (info.CheckBoxs[i].checked) return true;
        }
        alert(info.CheckBoxsMessage);
        return false;
    },
    ValidateDropdownlist: function (info) {//验证下拉框
        if (info.Dropdownlist == null || info.Dropdownlist.length == 0) return true;
        for (var i = 0; i < info.Dropdownlist.length; i++) {
            if (info.Dropdownlist[i].Control.value == "") {
                alert(info.Dropdownlist[i].Message);
                return false;
            }
        }
        return true;
    }
};
