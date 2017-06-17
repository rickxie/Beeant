Winner = typeof (Winner) != "undefined" ? Winner : {};
Winner.ClassBase = function () { };
Winner.ClassBase.prototype =
{
    GetHtmlHead: function () { //得到Html的Head
        return document.getElementsByTagName("head").item(0);
    },
    LoadCssFile: function (src, container, loadFunction) { //加载样式文件
        var obj = document.createElement("link");
        obj.rel = "stylesheet";
        obj.type = "text/css";
        obj.href = src;
        if (src != null && src != "") {
            container = container == undefined ? this.GetHtmlHead() : container;
            container.appendChild(obj);
            if (loadFunction != undefined) {
                this.BindEvent(obj, "load", function () {
                    loadFunction();
                });
            }
        }
        return obj;
    },
    LoadScriptFile: function (src, container, loadFunction) { //加载脚本文件
        var obj = document.createElement("script");
        obj.type = "text/javascript";
        obj.src = src;
        if (src != null && src != "") {
            container = container == undefined ? this.GetHtmlHead() : container;
            container.appendChild(obj);
            if (loadFunction != undefined) {
                this.BindEvent(obj, "load", function () {
                    loadFunction();
                });
            }
        }
        return obj;
    },
    BindEvent: function (obj, eventName, func) { //添加事件
        if (obj.attachEvent) {
            obj.attachEvent("on" + eventName, func);
        } else if (obj.addEventListener) {
            obj.addEventListener(eventName, func, false);
        }
    },
    RemoveEvent: function (obj, eventName, func) { //取消绑定
        if (obj.detachEvent) {
            obj.detachEvent('on' + eventName, func);
        } else if (obj.removeEventListener) {
            obj.removeEventListener(eventName, func, false);
        }
    },
    GetAttribute: function (obj, name) { //得到属性值
        if (obj.attributes != null && obj.attributes[name] != null) {
            return obj.attributes[name].value;
        }
        return null;
    },
    SetAttribute: function (obj, name, value) { //设置属性值
        if (obj.setAttribute != null) {
            obj.setAttribute(name, value);
            return true;
        }
        return false;
    },
    RemoveAttribute: function (obj, name) { //移除属性值
        if (obj.setAttribute != null) {
            obj.removeAttribute(name);
            return true;
        }
        return false;
    },
    CancelEventUp: function (event) { //取消事件冒泡
        event = window.event ? window.event : event;
        if (document.all) {
            event.returnValue = false;
            event.cancelBubble = true;
        } else {
            event.preventDefault();
            event.stopPropagation();
        }
        return false;
    },
    GetElementTop: function (obj, endObj) { //显示Y坐标
        var top = obj.offsetTop;
        var style = this.GetElementStyle(obj.offsetParent);
        if (obj.offsetParent != null && (endObj == undefined || obj.offsetParent != endObj)
            && (style == null || style.position != "absolute")) {
            top += this.GetElementTop(obj.offsetParent, endObj);
        }
        return top;
    },
    GetElementLeft: function (obj, endObj) { //显示X坐标
        var left = obj.offsetLeft;
        var style = this.GetElementStyle(obj.offsetParent);
        if (obj.offsetParent != null && (endObj == undefined || obj.offsetParent != endObj)
            && (style == null || style.position != "absolute")) {
            left += this.GetElementLeft(obj.offsetParent, endObj);
        }
        return left;
    },
    GetElementStyle: function (obj) { //得到元素样式
        if (obj == null) return null;
        if (obj.currentStyle)
            return obj.currentStyle;
        else if (document.defaultView && document.defaultView.getComputedStyle)
            return document.defaultView.getComputedStyle(obj);
        return null;
    },
    LoadConfig: function (sender, config) { //加载自定义配置
        if (config == undefined)
            return;
        for (var con in config) {
            if (con != "undefined") {
                eval("sender." + con.toString() + " = config[con]");
            }
        }
    },
    LoadInstances: function (sender, ctrl, name) { //加载实例
        if (sender == null || ctrl == null)
            return;
        name = name == undefined ? "Instance" : name;
        for (var i = 0; i < ctrl.childNodes.length; i++) {
            var instance = this.GetAttribute(ctrl.childNodes[i], name);
            if (instance != undefined && instance != "") {
                eval("sender." + instance + "=ctrl.childNodes[i]");
                this.RemoveAttribute(ctrl.childNodes[i], name);
            }
            this.LoadInstances(sender, ctrl.childNodes[i], name);
        }
    },
    Deserialize: function (value) { //解析Json
        if (value == null || value == "")
            return new Array();
        try {
            return eval(value);
        } catch (e) {

        }
        return new Array();
    },
    Serialize: function (obj) { //序列化json
        var value = [];
        var rev = "";
        if (Object.prototype.toString.apply(obj) === '[object Array]') {
            for (var i = 0; i < obj.length; i++)
                value.push(this.Serialize(obj[i]));
            rev = '[' + value.join(',') + ']';
        } else if (Object.prototype.toString.apply(obj) === '[object Date]') {
            rev = "new Date(" + obj.getTime() + ")";
        } else if (Object.prototype.toString.apply(obj) === '[object RegExp]' || Object.prototype.toString.apply(obj) === '[object Function]') {
            rev = obj.toString();
        } else if (Object.prototype.toString.apply(obj) === '[object Object]') {
            for (var name in obj) {
                var val = typeof (obj[name]) == 'string' ? '"' + obj[name] + '"' :
                    (typeof (obj[name]) === 'object' ? this.Serialize(obj[name]) : obj[name]);
                value.push(name + ':' + val);
            }
            rev = '{' + value.join(',') + '}';
        }
        return rev;
    },
    GetFileSize: function (file) {//得到文件大小
        try {
            var filesize = [];
            for (var i = 0; i < file.files.length; i++) {
                filesize.push(file.files[i].size);
            }
            return filesize;
        } catch (e) {
            return null;
        }
    },
    Clone: function (obj) {  //克隆
        function func() { }
        func.prototype = obj;
        var o = new func();
        for (var a in o) {
            if (typeof o[a] == "object") {
                o[a] = this.Clone(o[a]);
            }
        }
        return o;
    },
    ReplaceAll: function (source, oldString, newString) {//替换所有
        var reg = new RegExp("\\" + oldString, "g");
        return source.replace(reg, newString);
    },
    GetDto: function (content) {
        var result = (result == undefined ? {} : result);
        $(content).find("input,select,textarea").each(function () {
            if (this.type == "radio" || this.type == "checkbox")
                return;
            if ($(this).attr("disabled") == "disabled")
                return null;
            var name = $(this).attr("name");
            if (name != undefined) {
                var value = $(this).val() == $(this).attr("ShowValue") || $(this).val() == "" ? "" : $(this).val();
                eval("result." + name + "=value;");
            }
        });
        $(content).find("input[type=radio]").each(function () {
            if ($(this).attr("disabled") == "disabled")
                return null;
            var name = $(this).attr("name");
            if (name != undefined) {
                var value = $(content).find('input:radio[name=' + name + ']:checked').val();
                eval("result." + name + "=value;");
            }
        });
        $(content).find("input[type=checkbox]").each(function () {
            if ($(this).attr("disabled") == "disabled")
                return null;
            var name = $(this).attr("name");
            if (name != undefined) {
                var vals = [];
                $(content).find('input:checkbox[name=' + name + ']:checked').each(function (index, sender) {
                    vals.push(this.value);
                });
                var value = vals.join(",");
                eval("result." + name + "=value;");
            }
        });
        return result;
    },
    SetDto: function (content, data) {
        var setVal = function (content, con) {
            if (ctrl.attr("ShowValue") != undefined && (data[con] == null || data[con] == "")) {
                ctrl.val(ctrl.attr("ShowValue"));
            } else {
                ctrl.val(data[con]);
            }
        }
        var setRadio = function (content, con) {
            $(content).find('input:radio[name=' + con + ']').each(function () {
                this.checked = this.value == data[con];
            });
        }
        var setCheckBox = function (content, con) {
            $(content).find('input:checkbox[name=' + con + ']').each(function () {
                this.checked = data[con]!=null && (data[con].toString().indexOf(this.value) > -1);
            });
        }
        for (var con in data) {
            var ctrl = $("*[name=" + con + "]");
            if (ctrl.length == 0)
                continue;
            if (ctrl.attr("type") == "radio")
                setRadio(content, con);
            else if (ctrl.attr("type") == "checkbox")
                setCheckBox(content, con);
            else
                setVal(content, con);
        }

    }
};
var Wcb = new Winner.ClassBase();
