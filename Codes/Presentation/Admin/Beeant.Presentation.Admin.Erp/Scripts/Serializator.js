var Serializator = function (config) {
    this.Base = new Winner.ClassBase();
    this.Note = new Winner.Note();
    this.Note.Initialize();
    $(this.Note.Container).css("background", "#fff");
    $(this.Note.Container).css("width", "1200px");
    $(this.Note.Container).css("height", "600px");
    $(this.Note.Iframe).css("height", "600px");
    $(this.Note.Container).css("top", "120px");
    this.Base = new Winner.ClassBase();
    this.Html = "";
    this.Serializes = [];
    this.ResetFunction = null;
    this.Parent =window.parent==undefined?null: window.parent.window.Serial;
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
};
Serializator.prototype = {
    Initialize: function () { //加载css样式文件
        this.BindEvent();
        this.LoadData();
    },
    LoadData: function () {
        for (var i = 0; i < this.Serializes.length; i++) {
            var valueId = this.Serializes[i].ValueId;
            var valueInput = $("#" + valueId);
            var values = this.Base.Deserialize(valueInput.val());
            for (var k = 0; k < values.length; k++) {
                this.SetHtml($("#" + this.Serializes[i].Id), this.Serializes[i].Html, values[k]);
            }
        }
    },
    GetHtml: function (serializes, id) {
        for (var i = 0; i < serializes.length; i++) {
            if (serializes[i].Id == id) {
                return serializes[i].Html;
            }
        }
    },
    BindEvent: function () {//绑定事件
        var self = this;
        $(document).find("input[SerializeSelect='sure']").click(function () {
            self.Select($(this).attr("ContainerId"));
        });
        $(document).find("input[SerializeSelect='cancel']").click(function () {
            self.Parent.Note.Hide();
        });
    },
    Select: function (containerId) {//选择
        var valueId = this.GetQueryString("SerializeValueId");
        var serializeContainer = window.parent.$("#" + this.GetQueryString("SerializeContainerId"));
        var valueInput = window.parent.$("#" + valueId);
        var datas = this.GetDatas(containerId);
        var values = this.Base.Deserialize(valueInput.val());
        var html = this.GetHtml(this.Parent.Serializes, this.GetQueryString("SerializeContainerId"));
        for (var k = 0; k < datas.length; k++) {
            var rev = true;
            for (var i = 0; i < values.length; i++) {
                if (values[i].Id == datas[k].Id) {
                    rev = false;
                    break;
                }
            }
            if (rev) {
                values.push(datas[k]);
                this.SetHtml(serializeContainer, html, datas[k]);
            }
        }
        var value = this.Base.Serialize(values);
        valueInput.val(value);
        this.Parent.Note.Hide();
        if (this.Parent.ResetFunction != null) {
            this.Parent.ResetFunction(valueId);
        }
    },
    SetHtml: function (serializeContainer, html, value) {//设置HTML
        if (html == "" || html == undefined) {
            this.BindRemove(serializeContainer);
            return;
        }
        for (var name in value) {
            html = this.Base.ReplaceAll(html, "@" + name.toString(), value[name]);
        }
        serializeContainer.append(html);
        this.BindRemove(serializeContainer);
    },
    BindRemove: function (serializeContainer) {
        var self = this;
        serializeContainer.find("a[SerializeRemove]").click(function () {
            var serializator = self.Parent == null ? self : self.Parent;
            serializator.Remove($(this).attr("SerializeValueId"), $(this).attr("SerializeId"), this);
        });
        serializeContainer.find("input[SerializeName]").blur(function () {
            var serializator = self.Parent == null ? self : self.Parent;
            serializator.Update($(this).attr("SerializeValueId"), $(this).attr("SerializeName"), $(this).attr("SerializeId"), this);
        });
    },
    GetDatas: function (containerId) {//得到数据
        var value = [];
        $("#" + containerId).find("input[type='checkbox']:checked").each(function (index, sender) {
            var ctrls = $(sender).parent().parent().find("input[SerializeName]");
            var val = [];
            ctrls.each(function (i, sen) {
                val.push($(sen).attr("SerializeName") + ":'" + $(sen).val() + "'");
            });
            if (ctrls.length > 0) {
                value.push("{" + val.join(",") + "}");
            }
        });
        return this.Base.Deserialize("[" + this.Base.ReplaceAll(value.join(","), "\n", "") + "]");
    },
    Remove: function (valueId, id, sender) {//移除
        var valueInput = $("#" + valueId);
        var values = this.Base.Deserialize(valueInput.val());
        var newValues = [];
        for (var i = 0; i < values.length; i++) {
            if (values[i].Id == id)
                continue;
            newValues.push(values[i]);
        }
        var value = this.Base.Serialize(newValues);
        valueInput.val(value);
        $(sender).parent().parent().remove();
        if (this.ResetFunction != null) {
            this.ResetFunction(valueId);
        }
    },
    Update: function (valueId, name, id, sender) {
        var valueInput = $("#" + valueId);
        var values = this.Base.Deserialize(valueInput.val());
        for (var i = 0; i < values.length; i++) {
            if (values[i].Id == id) {
                eval("values[i]." + name + "='" + $(sender).val() + "';");
            }
        }
        var value = this.Base.Serialize(values);
        valueInput.val(value);
    },
    GetQueryString: function (name) {//得到URL参数
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return r[2]; return null;
    }
};
 