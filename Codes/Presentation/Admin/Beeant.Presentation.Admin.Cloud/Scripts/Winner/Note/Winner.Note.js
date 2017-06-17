Winner = typeof(Winner)!="undefined" ? Winner : {};
Winner.Note = function (config) {
    this.Base = new Winner.ClassBase();
    this.Html = "<div class='close'><span></span><div class='img' Instance='CloseButton'></div></div>" +
        "<div><iframe Instance='Iframe' class='noteiframe' iframeborder='0'></iframe></div>"+"<div class='padding_btn'></div>";
    this.PropertyName = "Note";
    this.UrlPropertyName = "NoteUrl";
    this.Base = new Winner.ClassBase();
    this.StyleFile = "/scripts/Winner/Note/Styles/Style.css";
    this.PageBox = [];
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
};
Winner.Note.prototype = {
    Initialize: function () {
        this.Base.LoadCssFile(this.StyleFile);
        this.Container = document.createElement('div');
        this.Container.className = "note";
        this.Container.style.position = "absolute";
        this.Container.innerHTML = this.Html;
        this.Container.style.display = "none";
        this.Base.LoadInstances(this, this.Container);
        this.BindClostButtonEvent();
        document.body.appendChild(this.Container);
        this.LoadControl(document.childNodes);
    },
    LoadControl: function (ctrls) {//加载控件
        for (var i = 0; i < ctrls.length; i++) {
            this.LoadControl(ctrls[i].childNodes);
            var name = this.Base.GetAttribute(ctrls[i], this.PropertyName);
            if (name == null || name == "") continue;
            this.BindEvent(ctrls[i]);
        }
    },
    BindClostButtonEvent: function () {//绑定控件事件
        var self = this;
        this.Base.BindEvent(this.CloseButton, "click", function () {
            self.Hide();
        });

    },
    Hide: function () {
        this.Container.style.display = "none";
    },
    BindEvent: function (sender) {
        var self = this;
        this.Base.BindEvent(sender, "click", function () {
            self.Display(sender, true);
        });
    },
    Display: function (sender, value) {
        var left = this.Base.GetElementLeft(sender);
        var top = this.Base.GetElementTop(sender);
        this.Container.style.left = 20 + left + "px";
        this.Container.style.top = top + "px";
        this.Iframe.src = this.Base.GetAttribute(sender, this.UrlPropertyName);
        if ($(this.Container).height() > 50)
            $(this.Iframe).css({ height: $(this.Container).height() - 50 });
        this.Container.style.display = value ? "" : "none";
    }
};