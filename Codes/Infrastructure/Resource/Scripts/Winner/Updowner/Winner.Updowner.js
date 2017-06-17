Winner.Updowner = function (id) {
    this.Container = $("#" + id);
    this.Elements = $(document).find("*[Updowner='Element']");
    this.CurrentIndex = 0;
    this.YStep = 4;
    this.XStep = 1;
    this.XMaxDistance = 20;
    this.YAutoDistance = 20;
    this.MoveDistance = 0;
    this.MoveType = "";
    this.IsStartMusic = true;
    this.IsMove = false;
};
Winner.Updowner.prototype =
{
    Initialize: function () {
        document.ontouchmove = function (e) { e.preventDefault(); };
        this.Container.css("position", "relative");
        this.Container.width($(document).width());
        this.Container.height($(document).height());
        this.Elements.css("position", "absolute");
        this.Elements.css("top", "100%");
        if (this.Elements.length > 0) {
            $(this.Elements[0]).css("top", "0");
        }
        $("*[Updowner='Icon']").css("left", (this.Container.width() - $("#icon").width()) / 2 + "px");
        this.BindEvent();
    },
    BindEvent: function () {
        var self = this;
        this.Container.find("a").bind("touchstart", function () {
            self.IsMove = false;
            window.location.href = this.href;
            return false;
        });
        this.Container.bind("touchstart", function (event) {
            self.OrgPos = self.GetPosition(event);
            self.BeforPos = self.OrgPos;
            return false;
        });
        this.Container.bind("touchmove", function (event) {
            self.IsMove = true;
            self.MovePos = self.GetPosition(event);
            self.MoveDistance = self.MovePos.Y - self.BeforPos.Y;
            var value = self.MoveType;
            if (self.OrgPos.Y - self.MovePos.Y > 0) {
                self.MoveType = "down";
            } else if (self.OrgPos.Y - self.MovePos.Y < 0) {
                self.MoveType = "up";
            }
            if (value != "" && value != self.MoveType) {
                self.MoveType = "";
            }
            self.Move();
            self.BeforPos = self.MovePos;
            return false;
        });
        this.Container.bind("touchend", function () {
            self.FinishMove();
            self.IsMove = false;
            return false;
        });
        this.Container.bind("touchcancel", function () {
            self.FinishMove();
            self.IsMove = false;
            return false;
        });
        $("*[Updowner='Music']").bind("click", function () {
            if (self.IsStartMusic) {
                $(this).removeClass("rotate");
                $(this).find("audio")[0].pause();
                self.IsStartMusic = false;
            } else {
                $(this).addClass("rotate");
                self.IsStartMusic = true;
                $(this).find("audio")[0].play();
            }
            return false;
        });
    },
    GetPosition: function (event) { //得到位置
        return { Y: event.originalEvent.changedTouches[0].pageY, X: event.originalEvent.changedTouches[0].pageX };
    },
    Move: function () { //移动
        if (this.IsMove == false) {
            return;
        }
        if (this.MoveType != "") {
            var nextIndex = this.GetNextIndex();
            var element = this.Elements[this.CurrentIndex];
            var nextElement = this.Elements[nextIndex];
            $(element).css("z-index", 1);
            $(nextElement).css("z-index", 2);
            this.SetElementWidth(element);
            this.SetElementPosition(nextElement);
        } else {
            this.Reset();
        }
        return;
    },
    SetElementWidth: function (element) {//设置宽度
        if ($(element).width() <= this.Container.width()) {
            var step = 0;
            if (this.MoveType == "down") {
                if (this.MoveDistance < 0) {
                    step = 0 - this.XStep;
                } else {
                    step = this.XStep;
                }
            } else {
                if (this.MoveDistance < 0) {
                    step = this.XStep;
                } else {
                    step = 0 - this.XStep;
                }
            }
            if (step < 0 && (this.Container.width() - $(element).width()) / 2 > this.XMaxDistance)
                return;
            $(element).width($(element).width() + step);
            $(element).css("margin-left", (this.Container.width() - $(element).width()) / 2 + "px");
        } else {
            this.ResetCurrentElement();
        }
    },
    SetElementPosition: function (nextElement) {
        var step = 0;
        if (this.MoveType == "down") {
            if (this.MoveDistance < 0) {
                step = 0 - this.YStep;
            } else {
                step = this.YStep;
            }
        } else {
            if (nextElement.offsetTop >= this.Container.height()) {
                nextElement.style.top = 0 - this.Container.height() + "px";
            }
            if (this.MoveDistance > 0) {
                step = this.YStep;
            } else {
                step = 0 - this.YStep;
            }
        }
        nextElement.style.top = nextElement.offsetTop + step + "px";
    },
    ResetCurrentElement: function () {
        var element = this.Elements[this.CurrentIndex];
        $(element).css("width", "100%");
        $(element).css("margin-left", "0px");
    },
    Reset: function () {
        var self = this;
        this.ResetCurrentElement();
        this.Elements.each(function (index, sender) {
            if (self.CurrentIndex == index) {
                $(this).css("top", "0");
            } else {
                $(this).css("top", "100%");
            }
        });
    },
    GetNextIndex: function () {//得到下一个节点
        var nextIndex;
        if (this.MoveType == "down") {
            if (this.CurrentIndex == this.Elements.length - 1)
                nextIndex = 0;
            else
                nextIndex = this.CurrentIndex + 1;
        } else {
            if (this.CurrentIndex == 0)
                nextIndex = this.Elements.length - 1;
            else
                nextIndex = this.CurrentIndex - 1;
        }
        return nextIndex;
    },
    FinishMove: function () {
        if (!this.IsMove)
            return;
        var self = this;
        var nextIndex = this.GetNextIndex();
        var nextElement = $(this.Elements[nextIndex]);
        if (Math.abs(nextElement.position().top) < this.YAutoDistance) {
            this.Reset();
        } else {
            nextElement.animate({ top: 0 }, 500, function () {
                self.ResetCurrentElement();
                self.CurrentIndex = nextIndex;
                self.Reset();
            })
        }


    }
}