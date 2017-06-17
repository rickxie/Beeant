Winner = typeof (Winner) != "undefined" ? Winner : {};
Winner.Slider = function (container, content, config) { //{BeginTime:null,NowTime:null,Invterval:1000,DayId:"",HourId:"",MinuteId:"",SecondId:"",BeginFunction:null,RunFunction:null,EndFuntion:null}
    this.Base = new Winner.ClassBase();
    this.Container = $(container);
    this.Content = $(content);
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
    this.IsMove = false;
};
Winner.Slider.prototype = {
    Initialize: function () {//初始化
        this.BindEvent();
    },
    BindEvent: function () {
        var self = this;
        this.StartX = 0;
        this.Container.bind("swipeleft", function (event) {
            var dis = event.swipestart.coords[0] - event.swipestop.coords[0];
            self.MoveContent(0 - dis * 6, true);
        }).bind("swiperight", function (event) {
            var dis = event.swipestart.coords[0] - event.swipestop.coords[0];
            self.MoveContent(0 - dis * 6, true);
        }).bind("touchstart", function (event) {
            self.StartX = event.originalEvent.touches[0].pageX;
        }).bind("touchmove", function (event) {
            var endX = event.originalEvent.touches[0].pageX;
            self.MoveContent(endX - self.StartX);
            self.StartX = endX;
        });
        $(document).find("*[Slider='LeftButton']").click(function() {
            self.MoveContent(parseInt(self.Container.width() / 2));
        });
        $(document).find("*[Slider='RightButton']").click(function () {
            self.MoveContent(0-parseInt(self.Container.width() / 2));
        });
    },
    MoveContent: function(dis, isAnimate) {
        if (this.IsMove)
            return;
        var self = this;
        this.IsMove = true;
        dis = this.Content.position().left + dis;
        if (dis > 0)
            dis = 0;
        if (dis < this.Container.width() - this.Content.width())
            dis = this.Container.width() - this.Content.width();
        if (isAnimate) {
            this.Content.animate({ left: dis }, 300);
            setTimeout(function () { self.IsMove = false }, 300);
        } else {
            this.Content.animate({ left: dis }, 0);
            this.IsMove = false;
        }
    }
};