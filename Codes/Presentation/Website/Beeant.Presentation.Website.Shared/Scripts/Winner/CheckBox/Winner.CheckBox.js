Winner.CheckBox = function (id, config) {
    this.Base = new Winner.ClassBase();
    this.Container = document.getElementById(id);
    if (this.Container == null) {
        return;
    }
    this.AllCheckPropertyName = "AllCheckName";
    this.SubCheckPropertyName = "SubCheckName";
    this.StyleFile = "/scripts/Winner/CheckBox/Styles/Style.css";
    if (config != undefined ) {
        this.Base.LoadConfig(this, config);
    }
};
Winner.CheckBox.prototype = {
    Initialize: function () {//加载css样式文件
        if (this.Container == null) {
            return;
        }
        this.Base.LoadCssFile(this.StyleFile);
        this.LoadControl(this.Container);
    },
    LoadControl: function (container) {//加载控件
        var boxs = this.GetCheckBoxs(container);
        if (boxs == null || boxs.length == 0)
            return;
        for (var i = 0; i < boxs.length; i++) {
            this.BindCheckBoxEvent(boxs[i], boxs);
        }
    },
    GetCheckBoxs: function (container) {//得到所有checkbox控件
        var ctrls = container.getElementsByTagName('input');
        if (ctrls == null || ctrls.length == 0)
            return null;
        var rev = [];
        for (var i = 0; i < ctrls.length; i++) {
            if (ctrls[i].type == "checkbox") {
                rev.push(ctrls[i]);
            }
        }
        return rev;
    },
    BindCheckBoxEvent: function (ctrl, boxs) {//绑定点击事件
        var self = this;
        this.Base.BindEvent(ctrl, "click", function () {
            self.SetSubCheckBoxs(ctrl, boxs);
            self.SetAllCheckBox(ctrl, boxs);
            self.AfterClick();
        });
    },
    SetSubCheckBoxs: function (ctrl, boxs) {//设置子控件
        var name = this.Base.GetAttribute(ctrl, this.AllCheckPropertyName);
        if (name == null)
            return;
        for (var i = 0; i < boxs.length; i++) {
            if (this.IsSubCheckBox(boxs[i], name)) {
                boxs[i].checked = ctrl.checked;
            }
        }
    },
    IsSubCheckBox: function (ctrl, name) {//是否为全选下的控件
        var ckName = this.Base.GetAttribute(ctrl, this.SubCheckPropertyName);
        if (ckName == null || name == this.Base.GetAttribute(ctrl, this.AllCheckPropertyName))
            return false;
        var ckNames = ckName.split(',');
        for (var i = 0; i < ckNames.length; i++) {
            if (ckNames[i] == name)
                return true;
        }
        return false;
    },
    SetAllCheckBox: function (ctrl, boxs) {//设置总控件
        var ckName = this.Base.GetAttribute(ctrl, this.SubCheckPropertyName);
        if (ckName == null)
            return;
        var ckNames = ckName.split(',');
        var rev = this.SetAllCheckBoxByNames(ckNames, boxs);
        if (rev != null && rev.length > 0) {
            for (var i = 0; i < rev.length; i++) {
                this.SetAllCheckBox(rev[i], boxs);
            }
        }
    },
    SetAllCheckBoxByNames: function (ckNames, boxs) {//设置所有名称为ckNames的全选按钮
        var rev = [];
        for (var i = 0; i < ckNames.length; i++) {
            var t = this.GetAllCheckBoxByName(ckNames[i], boxs);
            if (t != null) {
                this.SetAllCheckBoxInfo(t, boxs, ckNames[i]);
                rev.push(t);
            }
        }
        return rev;
    },
    SetAllCheckBoxInfo: function (ctrl, boxs, name) {//设置全选按钮
        var subInfo = this.GetSubCheckBoxInfo(boxs, name);
        ctrl.checked = subInfo.CheckCount > 0 ? true : false;
        ctrl.className = ctrl.checked == false || subInfo.SubCount == subInfo.CheckCount ? "checkall" : "checkpart";
    },
    GetSubCheckBoxInfo: function (boxs, name) {//得到子控件选中的数量
        var subCount = 0, checkCount = 0;
        for (var i = 0; i < boxs.length; i++) {
            if (this.IsSubCheckBox(boxs[i], name)) {
                subCount++;
                if (boxs[i].checked) {
                    checkCount++;
                }
            }
        }
        return { CheckCount: checkCount, SubCount: subCount };
    },
    GetAllCheckBoxByName: function (name, boxs) {//根据名称得到全选按钮
        for (var i = 0; i < boxs.length; i++) {
            if (name == this.Base.GetAttribute(boxs[i], this.AllCheckPropertyName))
                return boxs[i];
        }
        return null;
    },
    AfterClick:function() {
        
    }

};
