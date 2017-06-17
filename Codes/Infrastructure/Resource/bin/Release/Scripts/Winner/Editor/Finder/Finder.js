Winner = typeof(Winner)!="undefined" ? Winner : {};
Winner.Editor = typeof (Winner.Editor) != "undefined" ? Winner.Editor : {};
Winner.Editor.Finder = function (id, config) {
    this.Base = new Winner.ClassBase();
    this.Container = document.getElementById(id);
    if (this.Container == null) {
        return;
    }
    this.StyleFile = "/scripts/Winner/Editor/Finder/Styles/Style.css";
    this.PropertyName = "Finder";
    this.UrlPropertyName = "Url";
    this.Config = { Element: "Element", RightMenu: "RightMenu", Select: "Select", Browse: "Browse",
        UploadSwitch: "UploadSwitch",  UploaderContent: "UploaderContent"
    };
    this.SelectFile = null; //选择的对象
    this.RightMenu = null; //右键菜单
    this.UploaderContent = null; //上传控件容器
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
};
Winner.Editor.Finder.prototype =
{
    Initialize: function () {//初始化
        this.Base.LoadCssFile(this.StyleFile);
        this.Container.className = "finder";
        this.LoadControl(this.Container.childNodes);
        this.BindDocumentEvent();
    },
    LoadControl: function (ctrls) {//加载控件
        for (var i = 0; i < ctrls.length; i++) {
            this.LoadControl(ctrls[i].childNodes);
            this.SelectControl(ctrls[i]);
        }
    },
    SelectControl: function (ctrl) {//选择加载的控件
        var name = this.Base.GetAttribute(ctrl, this.PropertyName);
        switch (name) {
            case this.Config.Element: this.BindElementEvent(ctrl); break;
            case this.Config.RightMenu: this.SetRightMenu(ctrl); break;
            case this.Config.Select: this.BindSelectEvent(ctrl); break;
            case this.Config.Browse: this.BindBrowseEvent(ctrl); break;
            case this.Config.UploadSwitch: this.BindUploadSwitchEvent(ctrl); break;
            case this.Config.UploaderContent: this.SetUploaderContent(ctrl); break;
        }
    },
    SetUploaderContent: function (ctrl) {//设置上传控件
        this.UploaderContent = ctrl;
        this.UploaderContent.style.display = "none";
    },
    SetRightMenu: function (ctrl) {//设置右键菜单
        this.RightMenu = ctrl;
        this.RightMenu.style.display = "none";
    },
    BindUploadSwitchEvent: function (ctrl) {//关闭上传控件事件
        var self = this;
        this.Base.BindEvent(ctrl, "click", function () {
            if (self.UploaderContent != null) {
                self.UploaderContent.style.display = self.UploaderContent.style.display == 'none' ? "" : "none";
            }
        });
    },
    BindSelectEvent: function (ctrl) {//选择事件
        var self = this;
        this.Base.BindEvent(ctrl, "click", function () {
            self.Select();
        });
    },
    BindBrowseEvent: function (ctrl) {//浏览事件
        var self = this;
        this.Base.BindEvent(ctrl, "click", function () {
            self.Browse();
        });
    },
    BindDocumentEvent: function () {//绑定document事件
        this.BindDocumentContextmenuEvent();
        this.BindDocumentClickEvent();
    },
    BindDocumentContextmenuEvent: function () {//绑定document右键事件
        var self = this;
        document.oncontextmenu = function (event) {
            self.CancelSelectFile();
            self.HideRightMenu(event);
            return false;
        };
    },
    BindDocumentClickEvent: function () {//绑定document右键事件
        var self = this;
        this.Base.BindEvent(document, "click", function (event) {
            self.CancelSelectFile();
            self.HideRightMenu(event);
            return false;
        });
    },
    BindElementEvent: function (element) {//绑定图片容器事件
        this.BindElementOverEvent(element);
        this.BindElementClickEvent(element);
        this.BindElementContextmenuEvent(element);
        this.BindElementDblclickEvent(element);
        this.BindElementOutEvent(element);
    },
    BindElementOverEvent: function (element) {//绑定图片容器鼠标移过事件
        var self = this;
        this.Base.BindEvent(element, "mouseover", function (event) {
            if (element.className != "select") {
                element.className = "over";
            }
            return self.Base.CancelEventUp(event);
        });
    },
    BindElementOutEvent: function (element) {//绑定图片容器鼠标移出事件
        var self = this;
        this.Base.BindEvent(element, "mouseout", function (event) {
            if (element.className != "select") {
                element.className = "out";
            }
            return self.Base.CancelEventUp(event);
        });
    },
    BindElementClickEvent: function (element) {//绑定图片容器鼠标点击事件
        var self = this;
        this.Base.BindEvent(element, "click", function (event) {
            return self.SetSelectFile(element, event);
        });
    },
    BindElementContextmenuEvent: function (element) {//绑定图片容器鼠标点击事件
        var self = this;
        element.oncontextmenu = function (event) {
            var rev = self.SetSelectFile(element, event);
            self.ShowRightMenu(event);
            return rev;
        };
    },
    BindElementDblclickEvent: function (element) {//双击事件
        var self = this;
        this.Base.BindEvent(element, "dblclick", function (event) {
            self.SetSelectFile(element, event);
            self.Select();
        });
    },
    SetSelectFile: function (obj, event) {//设置选择文件
        this.CancelSelectFile();
        this.SelectFile = obj;
        obj.className = "select";
        return this.Base.CancelEventUp(event);
    },
    CancelSelectFile: function () {//取消选择中
        if (this.SelectFile != null) {
            this.SelectFile.className = "out";
            this.SelectFile = null;
        }
    },
    ShowRightMenu: function (event) {//显示右键菜单
        if (this.RightMenu == null || this.SelectFile == null)
            return;
        var position = this.GetMousePosition(event);
        this.RightMenu.style.position = "absolute";
        this.RightMenu.style.top = position.Y + "px";
        this.RightMenu.style.left = position.X + "px";
        this.RightMenu.style.display = "";
    },
    HideRightMenu: function () {//隐藏右键菜单
        if (this.RightMenu == null)
            return;
        this.RightMenu.style.display = "none";
    },
    GetEvent: function (event) {//得到事件源
        event = window.event ? window.event : event;
        return event;
    },
    Select: function () {//选择文件
        if (this.SelectFile != null) {
            var url = this.Base.GetAttribute(this.SelectFile, this.UrlPropertyName);
            eval(this.GetUrlParms()["function"].replace("{0}", url).replace("{1}", ""));
            window.close();
        }
    },
    Browse: function () {//预览
        if (this.SelectFile != null) {
            var url = this.Base.GetAttribute(this.SelectFile, this.UrlPropertyName);
            window.open(url);
        }
    },
    GetMousePosition: function (event) {//得到鼠标位置
        var position = { X: [], Y: [] };
        event = this.GetEvent(event);
        if (window.event) {
            position.X = parseInt(event.clientX) + parseInt(document.documentElement.scrollLeft) - parseInt(document.body.clientLeft) + 2;
            position.Y = parseInt(event.clientY) + parseInt(document.documentElement.scrollTop) + parseInt(document.body.clientTop) - 2;
        }
        else {
            position.X = parseInt(event.pageX) + 2;
            position.Y = parseInt(event.pageY) - 2;
        }
        return position;
    },
    GetUrlParms: function () {//得到URL参数
        var args = new Object();
        var pairs = location.search.substring(1).split("&"); //在逗号处断开
        for (var i = 0; i < pairs.length; i++) {
            var pos = pairs[i].indexOf('='); //查找name=value
            if (pos == -1) continue; //如果没有找到就跳过
            var argname = pairs[i].substring(0, pos); //提取name
            var value = pairs[i].substring(pos + 1); //提取value
            args[argname] = unescape(value); //存为属性
        }
        return args;
    }
};
       