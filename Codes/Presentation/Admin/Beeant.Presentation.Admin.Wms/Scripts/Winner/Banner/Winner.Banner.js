Winner.Banner = function (id, config) {
    this.Base = new Winner.ClassBase();
    this.Container = $("#" + id);
    this.PropertyName = "Banner";
    this.IsAuto = true;
    this.Times = 5000;
    this.Index = 0;
    this.IsPlay = false;
    this.IsShowButton = false;
    this.ShowButton = true;
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
}
Winner.Banner.prototype = {
    Initialize: function () {//初始化
        if (this.Container == null) {
            return;
        }
        this.LoadControl();
        this.BindEvent();
        if (this.IsAuto) {
            this.StartAutoPlay();
        }
    },
    LoadControl: function () {//加载控件
        var self = this;
        this.Elements = this.Container.find("*[" + this.PropertyName + "='element']");
        this.PreviousButton = this.Container.find("*[" + this.PropertyName + "='previous']");
        this.NextButton = this.Container.find("*[" + this.PropertyName + "='next']");
        this.Views = this.Container.find("*[" + this.PropertyName + "='view']");
        this.Elements.each(function (index, sender) {
            if (index == self.Index)
                return;
            $(sender).css("left", (0 - $(sender).width()) + "px");
        });
        this.SetView();
    },
    BindEvent: function () {//绑定事件
        this.BindContainerEvent();
        this.BindButtonEvent();
        this.BindElementsEvent();
    },
    BindContainerEvent: function () {//绑定容器事件
        if (this.IsShowButton)
            return;
        var self = this;
        var overfunc = function () {
            self.PreviousButton.stop(false, true);
            self.NextButton.stop(false, true);
            self.PreviousButton.animate({ left: 0 }, { duration: 500 });
            self.NextButton.animate({ right: 0 }, { duration: 500 });
        }
        var outfunc = function () {
            self.PreviousButton.stop(false, true);
            self.NextButton.stop(false, true);
            self.PreviousButton.animate({ left: 0 - self.PreviousButton.width() }, { duration: 500 });
            self.NextButton.animate({ right: 0 - self.NextButton.width() }, { duration: 500 });
        }
        self.PreviousButton.bind("mouseover", function () {
            overfunc();
        }).bind("mouseout", function () {
            outfunc();
        });
        self.NextButton.bind("mouseover", function () {
            overfunc();
        }).bind("mouseout", function () {
            outfunc();
        });
        this.Container.bind("mouseover", function () {
            overfunc();
        }).bind("touchstart", function () {
            overfunc();
        }).bind("touchend", function () {
            outfunc();
        }).bind("mouseout", function () {
            outfunc();
        }).hover(
            function () {
                overfunc();
            }, function () {
                outfunc();
            }
        );
    },
    BindButtonEvent: function () {//绑定按钮
        var self = this;
        this.PreviousButton.click(function () {
            self.Play(true);
        }).mouseover(function () {
            self.StopAutoPlay();
        }).mouseout(function () {
            self.StartAutoPlay();
        }).bind("touchstart", (function () {
            self.StopAutoPlay();
        })).bind("touchend", (function () {
            self.StartAutoPlay();
        }));
        this.NextButton.click(function () {
            self.Play(false);
        }).mouseover(function () {
            self.StopAutoPlay();
        }).mouseout(function () {
            self.StartAutoPlay();
        }).bind("touchstart", (function () {
            self.StopAutoPlay();
        })).bind("touchend", (function () {
            self.StartAutoPlay();
        }));
    },
    BindElementsEvent: function () {//绑定元素
        var self = this;
        this.Elements.mouseover(function () {
            self.StopAutoPlay();
        }).mouseout(function () {
            self.StartAutoPlay();
        }).bind("touchstart", (function () {
            self.StopAutoPlay();
        })).bind("touchend", (function () {
            self.StartAutoPlay();
        })).bind("swipeleft", (function () {
            self.Play(false);
        })).bind("swiperight", (function () {
            self.Play(true);
        }));;
    },
    Play: function (dir) {//
        if (this.IsPlay)
            return;
        var self = this;
        this.IsPlay = true;
        var nextIndex = this.GetNextIndex(dir);
        var element = this.Elements[this.Index];
        var nextElement = this.Elements[nextIndex];
        var func = function () {
            self.Index = nextIndex;
            self.IsPlay = false;
            self.SetView();
        };
        if (dir) {
            $(nextElement).css("left", (0 - $(nextElement).width()) + "px");
            $(element).animate({ left: $(element).width() }, { duration: 500 });
            $(nextElement).animate({ left: 0 }, 500, '', function () {
                func();
            });
        } else {
            $(nextElement).css("left", $(nextElement).width() + "px");
            $(element).animate({ left: -$(element).width() }, { duration: 500 });
            $(nextElement).animate({ left: 0 }, 500, '', function () {
                func();
            });
        }

    },
    SetView: function () {
        this.Views.removeAttr("Select");
        this.Views.removeClass("select");
        if (this.Index >= 0 && this.Index < this.Views.length) {
            $(this.Views[this.Index]).attr("Select", "true");
            $(this.Views[this.Index]).addClass("select");
        }
    },
    GetNextIndex: function (dir) {
        var nextIndex = this.Index;
        if (dir) {
            nextIndex = nextIndex - 1;
            nextIndex = nextIndex < 0 ? this.Elements.length - 1 : nextIndex;
        } else {
            nextIndex = nextIndex + 1;
            nextIndex = nextIndex >= this.Elements.length ? 0 : nextIndex;
        }
        return nextIndex;
    },
    StartAutoPlay: function () {//开启自动播放
        var self = this;
        if (this.Elements.length <= 1)
            return;
        var func = function () {
            self.Play(false);
        }
        this.Timer = setInterval(func, this.Times);
    },
    StopAutoPlay: function () {//停止播放
        clearInterval(this.Timer);
    }

}
