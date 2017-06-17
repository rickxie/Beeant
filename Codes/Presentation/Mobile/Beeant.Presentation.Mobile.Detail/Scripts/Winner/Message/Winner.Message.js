Winner = typeof (Winner) != "undefined" ? Winner : {};
Winner.Message = function (config) {
    this.Base = new Winner.ClassBase();
    this.RootPath = "/scripts/Winner/Message/";
    this.Html = "<div class='title'><h1>@Title</h1>"
        + "<small Instance='CloseButton'>×</small></div><div class='detail'>@Detail</div>";
    this.IsRun = true;
    this.MaxCount = 100;
    this.InfosHandles = new Array();// [{ Name："",Func:  function(){ return [{ Title: "", Detail: "" }];},CloseFunc:function(info){ }, InvokeTime: 5000}];
    this.Infos = [];
     if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
};
Winner.Message.prototype = {

    Initialize: function () {
        this.Base.LoadCssFile(this.RootPath + "Styles/Style.css");
        this.Container = document.createElement("div");
        this.Container.style.display = "none";
        this.Container.className = 'messbox';
        this.Base.LoadInstances(this, this.Container);
        document.body.appendChild(this.Container);
    },
    BindEvent: function (infoHandle,info) {
        var self = this;
        this.Base.BindEvent(this.CloseButton, "click", function () {
            self.Close(infoHandle, info);
        });
    },
    Close: function(infoHandle, info) { //关闭
        infoHandle.IsShowTitle = false;
        if (infoHandle.CloseFunc != null)
            infoHandle.CloseFunc(infoHandle,info);
        this.Next(infoHandle);
    },
    Start: function() {
        this.IsRun = true;
        if (this.InfosHandles != null && this.InfosHandles.length > 0) {
            for (var i = 0; i < this.InfosHandles.length; i++) {
                this.AddInfos(this.InfosHandles[i]);
            }
        }
    },
    AddInfos: function(infoHandle) {
        var self = this;
        var func = function() {
            if (self.GetCookie(infoHandle.Name) == null) {
                var ancyFunc = function (infos) {
                    if (infos != null && infos.length > 0) {
                        for (var j = 0; j < infos.length && self.Infos.length < self.MaxCount; j++) {
                            self.Infos.push(infos[j]);
                        }
                        self.SetCookie(infoHandle.Name, "", infoHandle.InvokeTime);
                    }
                    if (self.Container.style.display == "none")
                        self.Next(infoHandle);
                };
                infoHandle.Func(ancyFunc);
            }
            if (self.IsRun) {
                setTimeout(func, infoHandle.InvokeTime);
            }
        };
        setTimeout(func, infoHandle.InvokeTime);
    },
    Stop: function () {
        this.IsRun = false;
        this.Container.style.display = "none";
    },
    Show: function (infoHandle,info) {
        this.Container.style.display = "";
        this.Container.innerHTML = this.Html.replace("@Title", info.Title).replace("@Detail", info.Detail);
        this.Base.LoadInstances(this, this.Container);
        this.BindEvent(infoHandle,info);
        infoHandle.IsShowTitle = true;
        this.ShowTitle(infoHandle);
         if (infoHandle.ShowFunc != null)
            infoHandle.ShowFunc(infoHandle,info);
    },
    ShowTitle: function(infoHandle) {//显示标题
        var myTitle = document.title;
        var record = 0;
        function titleBlink() {
            record++;
            if (record == 3) { 
                record = 1;
            }
            if (record == 1) {
                document.title =myTitle;
            }
            if (record == 2) {
                document.title =infoHandle.Message + myTitle;
            }
            if (infoHandle.IsShowTitle)
                setTimeout(titleBlink, 500); //调节时间，单位毫秒。
            else {
                 document.title =  myTitle;
            }
        }
        titleBlink();
    },
    Next: function (infoHandle) {
        var info=this.Infos.pop();
        if (info!=null) {
           this.Show(infoHandle,info);
        } else {
            this.Container.style.display = "none";
        }
    },
    SetCookie: function (name, value, time) {
        var exp = new Date();
        exp.setTime(exp.getTime() + time);
        document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString()+";path=/";
    },
    GetCookie: function (name) {
        var arr = document.cookie.match(new RegExp("(^|)" + name + "=([^;]*)(;|$)"));
        if (arr != null)
            return unescape(arr[2]);
        return null;
    }
 
};
