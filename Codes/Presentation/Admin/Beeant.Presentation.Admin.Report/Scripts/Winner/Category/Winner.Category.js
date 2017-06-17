Winner.Category = function (id, config) {
    this.Base = new Winner.ClassBase();
    this.Container = document.getElementById(id);
    if (this.Container == null) {
        return;
    }
    this.CategoryIdName = "CategoryId";
    this.IsTriggerName = "IsTrigger";
    this.OverClassName = "OverClassName";
    this.OutClassName = "OutClassName";
    this.Padding = { Left: 0, Top: 0 };
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
};
Winner.Category.prototype = {
    Initialize: function () {//加载css样式文件
        this.Container.style.position = "relative";
        this.LoadControl(this.Container.childNodes);
    },
    LoadControl: function (ctrls) {//加载控件
        for (var i = 0; i < ctrls.length; i++) {
            this.LoadControl(ctrls[i].childNodes);
            if (this.Base.GetAttribute(ctrls[i], this.IsTriggerName) == null) {
                continue;
            }
            var content = this.GetContent(ctrls, this.Base.GetAttribute(ctrls[i], this.CategoryIdName));
            this.BindEvent(ctrls[i], content);
        }
    },
    GetContent: function (ctrls, catagoryId) {//得到显示框
        for (var i = 0; i < ctrls.length; i++) {
            if (this.Base.GetAttribute(ctrls[i], this.IsTriggerName) == null && this.Base.GetAttribute(ctrls[i], this.CategoryIdName) == catagoryId) {
                return ctrls[i];
            }
            this.LoadControl(ctrls[i].childNodes);
        }
        return null;
    },
    BindEvent: function (trigger, content) {//绑定事件

        var self = this;
        this.Base.BindEvent(trigger, "mouseover", function () {
            self.Show(trigger, content);
        });
        this.Base.BindEvent(trigger, "mouseout", function () {
            self.Hide(trigger, content);
        });
        if (content == null) return;
        this.Base.BindEvent(content, "mouseover", function () {
            self.Show(trigger, content);
        });
        this.Base.BindEvent(content, "mouseout", function () {
            self.Hide(trigger, content);
        });
    },
    SetContentPosition: function (trigger, content) {
        if (content == null) return;
        content.style.position = "absolute";
        content.style.top = this.Padding.Top + "px";
        content.style.left = this.Base.GetElementLeft(trigger, this.Container) + trigger.clientWidth + this.Padding.Left + "px";
    },
    Show: function (trigger, content) {
        this.Container.style.display = "";
        trigger.style.display = "";
        this.SetContentPosition(trigger, content);
        var outClassName = this.Base.GetAttribute(trigger, this.OutClassName);
        if (outClassName == undefined || outClassName == "") {
            this.Base.SetAttribute(trigger, this.OutClassName, trigger.className);
        }
        trigger.className = this.Base.GetAttribute(trigger, this.OverClassName);
        if (content != null)
            content.style.display = "";
    },
    Hide: function (trigger, content) {
        trigger.className = this.Base.GetAttribute(trigger, this.OutClassName);
        if (content != null)
            content.style.display = "none";
    }
};
