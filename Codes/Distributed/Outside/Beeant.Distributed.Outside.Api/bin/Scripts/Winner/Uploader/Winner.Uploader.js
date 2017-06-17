Winner.Uploader = function (id, config) {
    this.Base = new Winner.ClassBase();
    this.Container = document.getElementById(id);
    if (this.Container == null) {
        return;
    }
    this.PropertyName = "Uploader";
    this.ResourcePath = "/scripts/Winner/Uploader/";
    this.StyleFile = this.ResourcePath + "Styles/Style.css";
    this.Extensions = [{ Name: ".jpg.gif.png.bmp.jpeg", Path: null },
    { Name: ".doc.docx", Path: this.ResourcePath + "img/doc.gif" }];
    this.FileButton = null;
    this.ViewContainer = null;
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
};
Winner.Uploader.prototype = {
    Initialize: function () {//加载css样式文件
        this.Base.LoadCssFile(this.StyleFile);
        this.Container.className = "uploader";
        this.LoadControl(this.Container.childNodes);
        this.BindEvent();
    },
    LoadControl: function (ctrls) {//创建控件
        for (var i = 0; i < ctrls.length; i++) {
            this.LoadControl(ctrls[i].childNodes);
            var name = this.Base.GetAttribute(ctrls[i], this.PropertyName);
            if (name == null)
                continue;
            if (name == "File") {
                ctrls[i].className = "file";
                this.FileButton = ctrls[i];
            }
            else if (name == "View") {
                this.ViewContainer = ctrls[i];
            }
            else if (name == "Value") {
                this.FileValue = ctrls[i];
            }
        }
    },
    BindEvent: function () {//绑定事件
        if (this.FileButton == null)
            return;
        var self = this;
        this.Base.BindEvent(this.FileButton, "change", function () {
            self.ShowView();
            self.SetValue();
        });
    },
    SetValue: function () {
        if (this.FileValue != undefined && this.FileValue != null) {
            var self = this;
            var reader = new FileReader();
            reader.onload = function (e) {
                self.FileValue.value = e.target.result;
            }
            reader.readAsDataURL(this.FileButton.files[0]);
        }
    },
    ShowView: function () { //展示图形
        if (this.ViewContainer == null)
            return;
        this.ViewContainer.innerHTML = "";
        if (this.FileButton.files != null) {
            for (var i = 0; i < this.FileButton.files.length; i++) {
                this.AddView(this.FileButton.files[i]);
            }
        } else {
            this.AddView(this.FileButton);
        }

    },
    AddView: function (file) {//设置图形
        var view = this.CreateView();
        var extension = this.GetExtension(file.value == undefined ? file.name : file.value);
        if (extension == null) {
            this.SetView(view, this.ResourcePath + "Images/none.png");
            return;
        }
        this.SelectViewType(file, view);
    },
    CreateView: function () { //创建视图
        var view = document.createElement("div");
        var img = document.createElement('img');
        img.alt = "";
        img.src = "";
        view.appendChild(img);
        this.ViewContainer.appendChild(view);
        return view;
    },
    GetExtension: function (value) {//得到扩展信息
        var extarr = value.split('.');
        var ext = extarr[extarr.length - 1].toLowerCase();
        for (var i = 0; i < this.Extensions.length; i++) {
            if (this.Extensions[i].Name.indexOf(ext) > -1) {
                return this.Extensions[i];
            }
        }
        return null;
    },
    SetView: function (view, value) { //得到初始化路径
        var imgs = view.getElementsByTagName('img');
        if (imgs != null && imgs.length > 0 && imgs[0].src != "")
            imgs[0].src = value;
    },
    SelectViewType: function (file, view) {//判断浏览器选择设置显示图片方式
        switch (this.GetBrowserVision()) {
            case "Other": this.SetOtherView(file, view); break;
            case "IE6": this.SetSimpleView(file, view); break;
            case "IE": this.SetFilterView(file, view); break;
        }
    },
    SetOtherView: function (file, view) {//火狐下路径
        try {
            this.SetView(view, window.URL.createObjectURL(file));
        }
        catch (ex) { }
    },
    SetSimpleView: function (file, view) {//设置IE6下的路径
        try {
            this.SetView(view, this.FileButton.value);
        }
        catch (ex) { }
    },
    SetFilterView: function (file, view) {//滤镜路径
        try {
            view.innerHTML = "";
            var path = this.GetFilterPath(file);
            view.style.display = "block";
            view.style.width = "100%";
            view.style.height = "100%";
            view.style.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod='scale',src=\"" + path + "\");";
        }
        catch (ex) { }
    },
    GetFilterPath: function () {//得到滤镜路径
        this.FileButton.select();
        this.FileButton.blur();
        try {
            return document.selection.createRange().text;
        } finally { document.selection.empty(); }
    },
    GetBrowserVision: function () { //判断浏览器类型
        if (document.all) {
            if (navigator.userAgent.indexOf("MSIE 6.0") > 0) return "IE6";
            if (navigator.userAgent.indexOf("MSIE 7.0") > 0 || navigator.userAgent.indexOf("MSIE 8.0") > 0
                || navigator.userAgent.indexOf("MSIE 9.0") > 0) return "IE";
            return "Other";
        }
        return "Other";
    }
};
