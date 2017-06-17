Winner = typeof (Winner) != "undefined" ? Winner : {};
Winner.Timer = function (config) { //{BeginTime:null,NowTime:null,Invterval:1000,DayId:"",HourId:"",MinuteId:"",SecondId:"",BeginFunction:null,RunFunction:null,EndFuntion:null}
    this.Base = new Winner.ClassBase();
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
};
Winner.Timer.prototype = {
    Initialize: function () {//初始化
        if (this.Invterval != undefined) {
            this.Invterval = 1000;
        }
        this.Step = this.Invterval / 1000;
        if (this.BeginTime != null) {
            this.BeginTime = this.FormatTime(this.BeginTime);
        }
        else {
            this.BeginTime = new Date().getTime();
        }
        if (this.NowTime != null) {
            this.NowTime = this.FormatTime(this.NowTime);
        }
        else {
            this.NowTime = new Date().getTime();
        }
        this.EndTime = this.FormatTime(this.EndTime);
        this.Timer = { Day: 0, Hour: 0, Minute: 0, Sencond: 0, BeginSenconds: (this.BeginTime-this.NowTime) / 1000, CurrentSenconds: (this.EndTime - this.NowTime) / 1000, TotalSenconds: (this.EndTime - this.BeginTime) / 1000 };
        this.Calculate();
    },
    FormatTime: function (time) {
        var ps = time.split(" ");
        var pd = ps[0].split("-");
        var pt = ps.length > 1 ? ps[1].split(":") : [0, 0, 0];
        var d = new Date(pd[0], pd[1] - 1, pd[2], pt[0], pt[1], pt[2]);
        return d.getTime();
    },
    Calculate: function () {
        var self = this;
        var func = function () {
            if (self.Timer.BeginSenconds > 0) {
                self.Timer.BeginSenconds = self.Timer.BeginSenconds - self.Step;
                if (self.Timer.BeginFunction != undefined && self.BeginFunction != null) {
                    self.BeginFunction(self);
                }
                return;
            }
            self.Timer.CurrentSenconds = self.Timer.CurrentSenconds - self.Step;
            if (self.Timer.CurrentSenconds > 0) {
                self.Timer.Day = Math.floor(self.Timer.CurrentSenconds / (3600 * 24));
                if (self.DayId != undefined || self.DayId == "") {
                    self.Timer.Hour = Math.floor(self.Timer.CurrentSenconds % (3600 * 24) / 3600);
                }
                else {
                    self.Timer.Hour = Math.floor(self.Timer.CurrentSenconds / 3600);
                }
                self.Timer.Minute = Math.floor(self.Timer.CurrentSenconds % 3600 / 60);
                self.Timer.Sencond = Math.floor(self.Timer.CurrentSenconds % 60);
                if (self.DayId != undefined || self.DayId == "") {
                    document.getElementById(self.DayId).innerHTML = self.Timer.Day;
                }
                if (self.HourId != undefined || self.HourId == "") {
                    document.getElementById(self.HourId).innerHTML = self.Timer.Hour;
                }
                if (self.MinuteId != undefined || self.MinuteId == "") {
                    document.getElementById(self.MinuteId).innerHTML = self.Timer.Minute;
                }
                if (self.SecondId != undefined || self.SecondId == "") {
                    document.getElementById(self.SecondId).innerHTML = self.Timer.Sencond;
                }
                if (self.RunFunction != undefined && self.RunFunction != null) {
                    self.RunFunction(self);
                }
                setTimeout(func, self.Invterval);
            } else {
                if (self.EndFunction != null) {
                    self.EndFunction(self);
                }
            }
        };
        func();

    }
};