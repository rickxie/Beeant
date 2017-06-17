Winner = typeof(Winner)!="undefined" ? Winner : {};
Winner.LineChart = function (containerId, config) {
    this.Base = new Winner.ClassBase();
    this.FlashName = "flash" + containerId;
    this.Container = document.getElementById(containerId);
    if (this.Container == null) return;
    this.RootPath = "/scripts/Winner/LineChart/";
    this.FlashPath = this.RootPath + "Flashs/DynamicLineChart.swf";
    this.FlashParams = {
//        Maximum: null,
//        Minimum: null,
//        Interval: null,
//        PageSize: 10,
//        HorizontalTitle: null,
//        HorizontalField: null,
//        VerticalTitle: null,
//        Url: "",
//        Lines: [
//           { XField: "data", YFiled: "val1", DisplayName: "",ToolTip:'',Color:25154,Alpha:1,Weight:2 }
//        ]
    };
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
    this.Html = "<object id=" + this.FlashName + " classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\"" +
        "codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0\"" +
        " width=\"100%\" height=\"100%\" id=\"game\" align=\"middle\">" +
        "<param name=\"allowScriptAccess\" value=\"sameDomain\" />" +
        "<param name=\"allowFullScreen\" value=\"false\" />" +
        "<param name=\"movie\" value=\"" + this.FlashPath + "\"/>" +
        "<param name=\"quality\" value=\"high\"/>" +
        "<param name=\"wmode\" value=\"transparent\"/>" +
        "<param name=\"bgcolor\" value=\"#ffffff\"/>" +
        "<embed src=\"" + this.FlashPath + "\" quality=\"high\" bgcolor=\"#ffffff\" name=\"" + this.FlashName + "\" " +
        "wmode=\"Opaque\" width=\"100%\" height=\"100%\"  align=\"middle\" " +
        "allowScriptAccess=\"sameDomain\" allowFullScreen=\"false\" type=\"application/x-shockwave-flash\"" +
        "pluginspage=\"http://www.macromedia.com/go/getflashplayer\"/></object>";

};
Winner.LineChart.prototype =
{
    Initialize: function () { //加载css样式文件
        this.Base.LoadCssFile(this.RootPath + "Styles/Style.css");
        this.Container.className = "linechart";
        this.Container.innerHTML = this.Html;
        this.Base.LoadInstances(this, this.Container);
        this.InitializeFlash();
    },
    InitializeFlash: function () {//初始化flash
        var self = this;
        var func = function () {
            self.SetFlash();
            if (self.Flash == null || self.Flash.LoadData == null) setTimeout(func, 500);
            else self.CompleteLoad();
        };
        func();
    },
    SetFlash: function () {//得到falsh
        var obj = document[this.FlashName];
        if (obj == null)
            obj = document.getElementById(this.FlashName);
        this.Flash = obj;
    },
    CompleteLoad: function () {//加载完成
        this.SetFlashParams();
        this.AddLine();
        this.LoadData();
    },
    SetFlashParams: function () {//绑定falsh
        if (this.FlashParams.Maximum != null) this.Flash.SetMaximum(this.FlashParams.Maximum);
        if (this.FlashParams.Minimum != null) this.Flash.SetMinimum(this.FlashParams.Minimum);
        if (this.FlashParams.Interval != null) this.Flash.SetInterval(this.FlashParams.Interval);
        if (this.FlashParams.PageSize != null) this.Flash.SetPageSize(this.FlashParams.PageSize);
        if (this.FlashParams.HorizontalTitle != null) this.Flash.SetHorizontalTitle(this.FlashParams.HorizontalTitle);
        if (this.FlashParams.HorizontalField != null) this.Flash.SetHorizontalField(this.FlashParams.HorizontalField);
        if (this.FlashParams.VerticalTitle != null) this.Flash.SetVerticalTitle(this.FlashParams.VerticalTitle);
    },
    AddLine: function () {//添加线
        if (this.FlashParams.Lines == null) return;
        for (var i = 0; i < this.FlashParams.Lines.length; i++) {
            this.Flash.AddLine(this.FlashParams.Lines[i]);
        }
    },
    LoadData: function () {//加载数据
        if (this.FlashParams.Url == null) return;
        this.Flash.LoadData(this.FlashParams.Url);
    }
};
 
 
  