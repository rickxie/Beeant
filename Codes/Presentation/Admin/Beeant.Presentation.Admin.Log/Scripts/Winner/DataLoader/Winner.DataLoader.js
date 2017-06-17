Winner = typeof(Winner)!="undefined" ? Winner : {};
Winner.DataLoader = function (config) {
    this.Config = config; //[{Triggers:[{Sender:null,Event:"",Function:null}],Loading:{Content:"Trigger|Content",Type:"Append|Replace""},Url:"",Paramters:null,Content:null,RequestType:"OneTime|Repeat",ShowType:"Append|Replace",Group:"",DataType:"text|json",IsExecute:false,BeginLoadFunction:null,BeginShowFunction:null,ErrorLoadFunction:null,EndShowFunction:func}]
};
Winner.DataLoader.prototype =
{
    Initialize: function () {//加载css样式文件
        if (this.Config == null)
            return;
        for (var i = 0; i < this.Config.length; i++) {
            for (var k = 0; k < this.Config[i].Triggers.length; k++) {
                this.BindEvent(this.Config[i].Triggers[k].Sender, this.Config[i].Triggers[k].Event, this.Config[i].Triggers[k].Function,this.Config[i].Triggers[k].Loading, this.Config[i]);
            }
        }
    },
    BindEvent: function (sender, event,func,loading, info) {
        var self = this;
        $(sender).bind(event, function () {
            var isLoad = true;
            if (func != null) {
                isLoad = func(info,event);
            }
            if (!isLoad)
                return;
            if (loading != undefined) {
                info.Loading = loading;
            }
            self.HideContens(info);
            self.LoadData(sender, info);
        });
        if (info.IsExecute) {
            self.HideContens(info);
            self.LoadData(sender, info);
        }
    },
    HideContens: function (info) {
        for (var i = 0; i < this.Config.length; i++) {
            if (this.Config[i].Content == info.Content)
                continue;
            if (this.Config[i].Group == undefined || this.Config[i].Group == "" || this.Config[i].Group != info.Group)
                continue;
            $(this.Config[i].Content).hide();
        }
    },
    GetLoadingHtml: function () {
        var html = "<div class='loading'>"
                 + "<img src='/Scripts/Winner/DataLoader/images/loading.gif' style='width:50px;height:50px;'  />"
                 + "</div>";
        return html;
    },
    ShowLoading: function (loading) {
        var html = this.GetLoadingHtml();
        loading.Loader = $(html);
        if (loading.Type == "Replace") {
            var width = loading.Container.width();
            var left = loading.Container.position().left;
            var height = $(window).height();
            var top = loading.Container.position().top;
            if (width > $(window).width()) {
                width = $(window).width();
                left = 0;
            }
            if (height > $(window).height()) {
                height = $(window).height();
                top = 0;
            }
            loading.Loader.css("left", left + "px")
                .css("width", width + "px")
                .css("height", height + "px")
                    .css("top", top + "px");
            loading.Container.html("");
        }
        loading.Container.append(loading.Loader);
    },
    HideLoading:function(loading) {
        if (loading.TempContainer != null) {
            $(loading.TempContainer.html()).insertBefore($(loading.TempContainer));
            loading.TempContainer.remove();
            loading.TempContainer = null;
        }
        loading.Loader.remove();
    },
    CheckLoad: function (sender, info) {
        if (info.BeginLoadFunction != undefined && info.BeginLoadFunction != null) {
            info.BeginLoadFunction(sender, info);
        }
        if (info.IsLoad || info.IsFullLoadComplate) {
            return true;
        }
        return false;
    },
    SetLoad: function (sender, info, data,isBegin) {
        if (isBegin) {
            info.IsLoad = true;
            return;
        }
        info.IsLoad = false;
        if (info.RequestType != "Repeat" || info.RequestType == "Repeat" && (data == null || data == "" || data.length==0)) {
            info.IsFullLoadComplate = true;
        }
    },
    LoadData: function (sender, info) {
        if (info.Loading.Container == undefined) {
            info.Loading.Container = info.Loading.Content == "Trigger" ? $(sender) : $(info.Content);
        }
        if (this.CheckLoad(sender, info)) {
            return;
        }
        info.Loading.Container.show();
        this.SetLoad(sender, info, null, true);
        var html = info.Loading.Container.html();
        $(info.Content).show();
        this.ShowLoading(info.Loading);
        var type = info.DataType == undefined ? "text" : info.DataType;
        var self = this;
        $.ajax({
            type: "Get",
            url: info.Url,
            async: true,
            data: info.Paramters,
            dataType: type,
            success: function (data) {
                if (info.BeginShowFunction != undefined && info.BeginShowFunction != null) {
                    info.BeginShowFunction(sender, info, data);
                }
                self.HideLoading(info.Loading);
                self.SetLoad(sender, info, data, false);
                if (type == "text") {
                    self.ShowText(info, $(info.Content), data, $(info.Content).html());
                } else if (type == "json") {
                    self.ShowJson(info, $(info.Content), data, $(info.Content).html());
                }
                if (info.EndShowFunction != undefined && info.EndShowFunction != null) {
                    info.EndShowFunction(sender, info, data);
                }
            },
            error: function (ex) {
                self.SetLoad(sender, info, ex, false);
                info.Loading.Container.html(html);
                if (info.ErrorLoadFunction != undefined && info.ErrorLoadFunction != null) {
                    info.ErrorLoadFunction(sender, info);
                }
            }
        });
    },
    ShowText: function (info, content, data, html) {
        if (info.ShowType == "Append") {
            content.append(data);
        }
        else if (info.ShowType == "Replace") {
            content.html(data);
        }
    },
    ShowJson: function (info, content, data, html) {
        var jsonhtml = this.GetJsonHtml("", data, html);
        if (info.ShowType == "Append") {
            content.append(jsonhtml);
        }
        else if (info.ShowType == "Replace") {
            content.html(jsonhtml);
        }
        content.find("*[DataLoaderArray]").removeAttr("DataLoaderArray");
    },
    GetJsonHtml: function (name, data, html) {//得到HTML
        if (Object.prototype.toString.apply(data) === '[object Array]') {
            var arrayHtmls = $("<div>" + html + "</div>").find("*[DataLoaderArray='@" + name + "']");
            for (var i = 0; i < arrayHtmls.length; i++) {
                var replaceHtml = "";
                var arrayHtml = arrayHtmls[i].outerHTML;
                if (data != null && data.length > 0) {
                    for (var j = 0; j < data.length; j++) {
                        replaceHtml += this.GetJsonHtml(name, data[j], arrayHtml);
                    }
                }
                html = html.replace(arrayHtml, replaceHtml);
            }
        } else if (Object.prototype.toString.apply(data) === '[object Object]') {
            for (var d in data) {
                var nextName = name == "" ? d : name + "." + d;
                var replaceName = "@" + nextName;
                if (typeof (data[d]) === 'object' && data[d] == null) {
                    html = this.ReplaceAll(html, replaceName, data[d]);
                }
                else if ((typeof (data[d]) === 'object' || Object.prototype.toString.apply(data[d]) === '[object Array]')) {
                    html = this.GetJsonHtml(nextName, data[d] == null ? [] : data[d], html);
                } else {
                    html = this.ReplaceAll(html, replaceName, data[d]);
                }
            }
        }
        return html;
    },
    ReplaceAll: function (source, oldString, newString) {//替换所有
        newString = newString == null ? "" : newString;
        var reg = new RegExp("\\" + oldString, "g");
        return source.replace(reg, newString);
    }
};