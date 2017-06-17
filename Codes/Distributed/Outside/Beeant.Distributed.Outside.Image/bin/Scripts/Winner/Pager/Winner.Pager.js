Winner.Pager = function (id, pageIndex, pageCount, config) {
    this.Base = new Winner.ClassBase();
    this.Container = document.getElementById(id);
    this.PageIndex = pageIndex;
    this.PageCount = pageCount;
    if (this.Container == null) {
        return;
    }
    this.PropertyName = "Pager";
    this.Config = { First: "first", Previous: "previous", Next: "next",
        Last: "last", Link: "link", PageBox: "pagebox", PageButton: "pagebutton"
    };
    this.PageBox = [];
    this.SureFunction = null;
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
};
Winner.Pager.prototype = {
    Initialize: function () { //加载css样式文件
        if (this.Container == null)
            return;
        this.Base.LoadCssFile(this.StyleFile);
        this.LoadControl(this.Container.childNodes);
    },
    LoadControl: function (ctrls) {//加载控件
        for (var i = 0; i < ctrls.length; i++) {
            this.LoadControl(ctrls[i].childNodes);
            if (this.Base.GetAttribute(ctrls[i], this.PropertyName) == null) {
                continue;
            }
            this.SelectControl(ctrls[i]);
        }
    },
    SelectControl: function (ctrl) {//选择加载的控件
        var isEnable = ctrl.attributes != null && ctrl.attributes["disabled"] != null && ctrl.attributes["disabled"].value == "disabled";
        var name = this.Base.GetAttribute(ctrl, this.PropertyName);
        if (this.LoadBox(name, isEnable, ctrl)) {
            return;
        }
        if (this.LoadButton(name, isEnable, ctrl)) {
            return;
        }
        this.LoadLink(name, isEnable, ctrl);
    },
    LoadBox: function (name, isEnable, ctrl) {//加载输入跳转input
        if (name == this.Config.PageBox) {
            this.PageBox = ctrl;
            if (!isEnable) {
                this.BindBoxEvent(this.PageBox);
            }
            return true;
        }
        return false;
    },
    LoadButton: function (name, isEnable, ctrl) { //加载跳转按钮
        if (name == this.Config.PageButton) {
            if (!isEnable) {
                this.BindButtonEvent(ctrl);
            }
            return true;
        }
        return false;
    },
    LoadLink: function (name, isEnable, ctrl) {//加载link分页按钮
        var index = this.GetLinkIndex(ctrl, name);
        if (index == 0) {
            return false;
        }
        if (!isEnable) {
            this.BindLinkEvent(ctrl, index);
        }
        return true;
    },
    GetLinkIndex: function (ctrl, name) {//得到Link索引
        switch (name) {
            case this.Config.First: return 1;
            case this.Config.Previous: return this.PageIndex - 1;
            case this.Config.Next: return this.PageIndex + 1;
            case this.Config.Last: return this.PageCount;
            case this.Config.Link: return parseInt(ctrl.innerHTML);
        }
        return 0;
    },
    CheckChange: function (index) {//检查是否要跳转
        return index > 0 && index <= this.PageCount && this.PageCount != 1;
    },
    BindLinkEvent: function (obj, index) {//添加分页按钮事件
        if (obj == null) {
            return;
        }
        var self = this;
        this.Base.BindEvent(obj, "click", function () {
            return self.CheckChange(index);
        });
    },
    BindBoxEvent: function (obj) {//添加输入框事件，只能输入数值
        if (obj == null) {
            return;
        }
        this.Base.BindEvent(obj, "keyup", function () {
            this.value = this.value.replace(/(^0[0-9]*$)|(^[^1-9]*[^0-9]*$)|([^0-9])/g, '');
        });
    },
    BindButtonEvent: function (obj) {//添加按钮点击事件
        if (obj == null) {
            return;
        }
        var self = this;
        this.Base.BindEvent(obj, "click", function () {
            var rev = self.CheckChange(parseInt(self.PageBox.value));
            if (!rev) {
                self.PageBox.select();
            }
            else if (self.SureFunction != null) {
                self.SureFunction(parseInt(self.PageBox.value));
            }
            return rev;
        });
    }
};
