Winner = typeof (Winner) != "undefined" ? Winner : {};
Winner.Album = function (container, content, config) { //{BeginTime:null,NowTime:null,Invterval:1000,DayId:"",HourId:"",MinuteId:"",SecondId:"",BeginFunction:null,RunFunction:null,EndFuntion:null}
    this.Base = new Winner.ClassBase();
    this.Container = $(container);
    this.Content = $(content);
    this.BeginShowFunction = null;
    this.EndShowFunction = null;
    this.EndMoveFunction = null;
    this.ChangeFunction = null;
    this.MoveDirection = "X";
    this.MoveDistance = 20;
    this.IsMove = false;
    this.MinZoomMultiple = 1;
    this.MaxZoomMultiple = 2.5;
    this.Rect = { Top: 40, Left: 0 };
    this.ViewHeight = this.Container.height();
    this.CurrentShowSender = null;
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
};
Winner.Album.prototype = {
    Initialize: function () {//初始化
        this.BindEvent();
    },

    BindEvent: function () {
        var self = this;
        this.Container.find("*[Album='true']").click(function () {
            self.Show(this);
        });
    },
    Append: function (sender) {//添加元素
        var self = this;
        var index = this.Container.find("*[Album='true']").length;
        $(sender).attr("Album", "true").attr("Index", index).click(function () {
            self.Show(this);
        });
    },
    Show: function (sender) {
        var rev = true;
        if (this.BeginShowFunction != null) {
            rev=this.BeginShowFunction(sender);
        }
        if (!rev)
            return;
        this.Content.find("*[Album='true']").css("left", 0 - $(this.Content).width());
        var item = this.GetItem(sender, $(sender).attr("Index"));
        if (item != null) {
            item.css("left", 0);
            var img = item.find("imgAlbum='Image'");
            this.Preview(sender, img);
            this.ShowContent();
            this.Amplify(img);
        } else {
            item = this.Create(sender);
            this.ShowContent();
        }
        this.CurrentShowSender = sender;
        if (this.EndShowFunction != null) {
            this.EndShowFunction(sender, item);
        }
    },

    GetItem: function (sender, index) {
        var bid = this.GetId(sender, index);
        var item = $("#" + bid);
        if (item.length === 0)
            return null;
        return item;
    },
    Preview: function (sender, img) {
        img.css("width", $(sender).width() + "px")
            .css("height", $(sender).width() + "px")
               .css("left", (this.Content.width() - $(sender).width()) / 2 + "px")
               .css("top", (this.Content.height() - $(sender).height()) / 2 + "px");
    },
    Amplify: function (img) {//展示图片
        var radius = $(this.Content).width();
        var width = radius;
        var height = radius;
        var left = ($(this.Content).width() - radius) / 2;
        var top = ($(this.Content).height() - radius) / 2;
        if (img[0].naturalWidth > img[0].naturalHeight) {
            width = radius;
            height = parseInt(img[0].naturalHeight / img[0].naturalWidth * radius);
            top = parseInt(($(this.Content).height() - height) / 2);
        } else if (img[0].naturalWidth < img[0].naturalHeight) {
            height = radius;
            width = parseInt(img[0].naturalWidth / img[0].naturalHeight * radius);
            left = parseInt(($(this.Content).width() - width) / 2);
        }
        img.animate({ width: width, height: height, left: left, top: top }, 300);
    },
    SetSize: function (item, pos) { //调整大小
        item.css("width", this.Content.width() + "px")
             .css("height", this.Content.height() + "px")
           .css("position", "absolute");
        var loading = item.find("*[Album='Loading']");
        loading.css("left", (item.width() - loading.find("img").width()) / 2 + "px")
            .css("top", (item.height() - loading.find("img").height()) / 2 + "px");
        if (pos != undefined) {
            if (pos.Left != undefined) {
                item.css("left", pos.Left + "px");
            }
            if (pos.Top != undefined) {
                item.css("top", pos.Top + "px");
            }
        }
    },
    Create: function (sender, pos) {//创建
        var self = this;
        var html = this.GetHtml(sender);
        var item = this.InsertHtml(sender, html);
        this.SetSize(item, pos);
        var img = item.find("img[Album='Image']");;
        this.Preview(sender, img);
        item.find("*[Album='Loading']").show();
        var src = $(sender).attr("AlbumOriginalUrl");
        var tempImage = document.createElement("img");
        $(tempImage).attr("src", src).bind("load", function () {
            img.attr("src", src);
            item.find("*[Album='Loading']").hide();
            self.Amplify($(img));
        });
        img.bind("click", function () {
            self.HideContent();
            item.find("*[Album='Loading']").hide();
            self.ResetImage(item);
        });
        this.BindMoveEvent(item);
        this.BindZoomEvent(item, img);
        if (this.CreateFunction != null) {
            this.CreateFunction(sender, item);
        }
        return item;
    },
    BindZoomEvent: function (item, img) {
        var self = this;
        item.bind("touchstart", function (event) {
            if (event.originalEvent.touches.length > 1) {
                self.StartTouches = [
                    {
                        pageX: event.originalEvent.touches[0].pageX,
                        pageY: event.originalEvent.touches[0].pageY,
                        screenX: event.originalEvent.touches[0].screenX,
                        screenY: event.originalEvent.touches[0].screenY
                    }, {
                        pageX: event.originalEvent.touches[1].pageX,
                        pageY: event.originalEvent.touches[1].pageY,
                        screenX: event.originalEvent.touches[1].screenX,
                        screenY: event.originalEvent.touches[1].screenY
                    }
                ];
                self.OriginTouches = [
                 {
                     pageX: event.originalEvent.touches[0].pageX,
                     pageY: event.originalEvent.touches[0].pageY,
                     screenX: event.originalEvent.touches[0].screenX,
                     screenY: event.originalEvent.touches[0].screenY
                 }, {
                     pageX: event.originalEvent.touches[1].pageX,
                     pageY: event.originalEvent.touches[1].pageY,
                     screenX: event.originalEvent.touches[1].screenX,
                     screenY: event.originalEvent.touches[1].screenY
                 }
                ];
                self.StartZoom(img);
            }
        }).bind("touchmove", function (event) {
            event.preventDefault();
            if (event.originalEvent.touches.length > 1) {
                self.EndTouches = [
                    {
                        pageX: event.originalEvent.touches[0].pageX,
                        pageY: event.originalEvent.touches[0].pageY
                    }, {
                        pageX: event.originalEvent.touches[1].pageX,
                        pageY: event.originalEvent.touches[1].pageY
                    }
                ];
                self.Zoom(img);
                self.StartTouches = [
                  {
                      pageX: event.originalEvent.touches[0].pageX,
                      pageY: event.originalEvent.touches[0].pageY
                  }, {
                      pageX: event.originalEvent.touches[1].pageX,
                      pageY: event.originalEvent.touches[1].pageY
                  }
                ];
            }
            else if (event.originalEvent.touches.length == 1 && self.IsZoom) {
                self.EndZoom(img);
            }
        }).bind("touchend", function (event) {
            self.EndZoom(img);
        });
    },
    BindMoveEvent: function (item) {
        var self = this;
        item.bind("touchstart", function (event) {
            self.StartTime = new Date();
            self.MoveStartX = event.originalEvent.touches[0].pageX;
            self.MoveStartY = event.originalEvent.touches[0].pageY;
            if (event.originalEvent.touches.length == 1) {
                self.StartMove(this);
            }
        }).bind("touchmove", function (event) {
            event.preventDefault();
            self.MoveEndX = event.originalEvent.touches[0].pageX;
            self.MoveEndY = event.originalEvent.touches[0].pageY;
            if (event.originalEvent.touches.length == 1) {
                if (self.MoveEndY < self.Rect.Top || self.MoveEndX < self.Rect.Left) {
                    self.EndMove(this);
                } else {
                    self.Move(this);
                }

            }
            self.MoveStartX = self.MoveEndX;
            self.MoveStartY = self.MoveEndY;
        }).bind("touchend", function (event) {
            if (event.originalEvent.changedTouches.length == 1) {
                self.EndMove(this);
            }

        });
    },
    ShowContent: function () {
        this.Content.show();
    },
    HideContent: function () {
        this.Content.hide();
    },
    InsertHtml: function (sender, html) {
        var insertObj = null;
        var currentObj = $(html);
        this.Content.find("*[Album='true']").each(function (index, obj) {
            if (parseInt($(this).attr("Index")) > parseInt($(sender).attr("Index"))) {
                insertObj = this;
                return false;
            }
        });
        if (insertObj == null)
            this.Content.append(currentObj);
        else {
            currentObj.insertBefore($(insertObj));
        }
        return currentObj;
    },
    GetId: function (sender, index) {
        if (index == undefined)
            index = $(sender).attr("Index");
        return "item" + index;
    },
    GetHtml: function (sender) {//得到HTML代码
        if (this.IsMove)
            return;
        this.IsMove = true;
        var html = '<div Id="' + this.GetId(sender) + '" Album="true">' +
            '<img src="' + $(sender).attr("src") + '"/></div>';
        this.IsMove = false;
    },
    Move: function (item) {//移动图片
        if (this.IsZoom)
            return;
        if ($(item).is(":animated")) {
            $(item).stop(true, true);
        }
        var img = $(item).find("img[Album='Image']");
        if ($(img).is(":animated")) {
            $(img).stop(true, true);
        }
        var disY = 0;
        var disX = 0;
        if (img.height() > $(window).height()) {
            disY = this.MoveEndY - this.MoveStartY;
        }
        if (img.width() > $(window).width()) {
            disX = this.MoveEndX - this.MoveStartX;
        }
        img.css("top", img.position().top + disY)
        .css("left", img.position().left + disX);
        if (this.MoveDirection == "X") {
            if (img.position().left > 0 || img.position().left < $(window).width() - img.width() || disX == 0) {
                this.MoveItem(item);
            }
        } else {
            if (img.position().top > 0 || img.position().top < $(window).height() - img.height() || disY == 0) {
                this.MoveItem(item);
            }
        }
    },
    MoveItem: function (item) {//移动
        if (this.IsMove)
            return;
        this.IsMove = true;
        var distance = this.MoveDirection == "X" ? this.MoveEndX - this.MoveStartX : this.MoveEndY - this.MoveStartY;
        this.SetNextMoveItem(item);
        if (this.MoveDirection == "X") {
            $(item).css("left", $(item).position().left + distance + "px");
            if (this.NextItem != null) {
                if ($(item).position().left > 0) {
                    this.NextItem.css("left", 0 - $(item).width() + $(item).position().left - this.MoveDistance + "px");
                } else {
                    this.NextItem.css("left", $(item).width() + $(item).position().left + this.MoveDistance + "px");
                }
            }
        } else {
            $(item).css("top", $(item).position().top + distance + "px");
            if (this.NextItem != null) {
                if ($(item).position().top > 0) {
                    this.NextItem.css("top", 0 - $(item).height() + $(item).position().top + this.MoveDistance + "px");
                } else {
                    this.NextItem.css("top", $(item).height() + $(item).position().top + this.MoveDistance + "px");
                }
            }
        }

        this.IsMove = false;
    },
    StartMove: function (item) {

    },
    EndMove: function (item) { //结束移动
        if (this.IsZoom || this.IsMove)
            return;
        var img = $(item).find("img[Album='Image']");
        if (img.attr("OriginWidth") != undefined && img.width() != parseInt(img.attr("OriginWidth"))) {
            if ($(img).is(":animated")) {
                $(img).stop(true, true);
            }
            var info = { width: img.width(), height: img.height(), top: img.position().top, left: img.position().left };
            this.ResetZoom(img, info, true);
            img.animate(info, 300);
        }
        if ($(item).position().left == 0)
            return;
        this.EndTime = new Date();
        var times = this.EndTime.getTime() - this.StartTime.getTime();
        var animateTimes = times <= 80 ? 0 : 300;
        this.StartTime = this.EndTime;
        this.IsMove = true;
        var distance = this.MoveDirection == "X" ? $(item).position().left + $(item).width() - $(window).width() : $(item).position().top + $(item).height() - $(window).height();
        if (this.MoveDirection == "X") {
            var left = 0;
            var width = this.Content.width();
            var nextLeft = distance > 0 ? 0 - width : width;
            if (Math.abs(distance) > width / 5) {
                left = distance > 0 ? width : 0 - width;
                nextLeft = 0;
            }
            if (this.NextItem == null)
                left = 0;

            $(item).animate({ left: left }, animateTimes);
            if (this.NextItem != null)
                this.NextItem.animate({ left: nextLeft }, animateTimes);
            if (this.NextItem != null && nextLeft == 0) {
                this.ResetImage(item);
                this.CurrentShowSender = this.NextSender;
            }

        } else {
            var top = 0;
            var height = this.Content.height();
            var nextTop = distance > 0 ? 0 - height : height;
            if (Math.abs(distance) > height / 5) {
                top = distance > 0 ? height : 0 - height;
                nextTop = 0;
            }
            if (this.NextItem == null)
                top = 0;
            $(item).animate({ left: top }, animateTimes);
            if (this.NextItem != null)
                this.NextItem.animate({ left: nextTop }, animateTimes);
            if (this.NextItem != null && nextTop == 0) {
                this.ResetImage(item);
                this.CurrentShowSender = this.NextSender;
            }
        }
        this.SetWindowScroll(item);
        if (this.ChangeFunction != null) {
            this.ChangeFunction(item);
        }
        this.IsMove = false;
    },
    SetNextMoveItem: function (item) { //得到下一个
        this.NextItem = null;
        if ($(item).position().left == 0)
            return null;
        var index = parseInt($(item).attr("Index"));
        index = $(item).position().left > 0 ? index - 1 : index + 1;
        if (index < 0)
            return null;
        if (index >= this.Container.find("*[Album='true']").length) {
            if (this.EndMoveFunction != null)
                this.EndMoveFunction(item);
            return null;
        }
        var itemSender = this.Container.find("*[Album='true']")[index];
        var nextItem = this.GetItem(itemSender, index);
        if (nextItem == null) {
            nextItem = this.Create(itemSender, { Left: $(item).width() });
        }
        if (this.NextItem != nextItem && this.NextItem != null) {
            this.NextItem.hide();
        }
        this.NextItem = nextItem;
        this.NextSender = itemSender;
        return nextItem;
    },
    SetWindowScroll: function (item) {//设置滚动条
        var elemnt = this.Container.find("*[Album='true']");
        var w = elemnt.width() + parseInt(elemnt.css("margin-left")) + parseInt(elemnt.css("margin-right"));
        var h = elemnt.height() + parseInt(elemnt.css("margin-top")) + parseInt(elemnt.css("margin-bottom"));
        var columnCount = Math.floor(this.Container.width() / w);
        var rowCount = Math.floor(this.ViewHeight / h);
        var index = parseInt($(item).attr("Index"));
        var rowIndex = Math.ceil(index / columnCount);
        if (rowIndex > rowCount)
            $(window).scrollTop((rowIndex - rowCount + 1) * h);
        if ($(window).scrollTop() > 0 && rowIndex === 1)
            $(window).scrollTop(0);
    },
    ResetImage: function (item) {
        var img = $(item).find("img[Album='Image']");
        if (img.attr("OriginWidth") != undefined) {
            setTimeout(function () {
                img.css("width", img.attr("OriginWidth") + "px")
                    .css("height", img.attr("OriginHeight") + "px")
                    .css("left", img.attr("OriginLeft") + "px")
                    .css("top", img.attr("OriginTop") + "px");
            }, 400);
        }
    },
    StartZoom: function (img) {
        this.IsZoom = true;
        if ($(img).attr("OriginWidth") == undefined) {
            $(img).attr("OriginWidth", $(img).width())
                .attr("OriginHeight", $(img).height())
                .attr("OriginLeft", $(img).position().left)
                .attr("OriginTop", $(img).position().top);

        }
    },//开始放大
    Zoom: function (img) {
        if ($(img).is(":animated"))
            return;
        var self = this;
        var originWidth = parseInt($(img).attr("OriginWidth"));
        var originHeight = parseInt($(img).attr("OriginHeight"));
        var onewidth = self.StartTouches[0].pageX - self.StartTouches[1].pageX;
        var oneheight = self.StartTouches[0].pageY - self.StartTouches[1].pageY;
        var towwidth = self.EndTouches[0].pageX - self.EndTouches[1].pageX;
        var towheight = self.EndTouches[0].pageY - self.EndTouches[1].pageY;
        var diswidth = Math.abs(towwidth) - Math.abs(onewidth);
        var disheight = Math.abs(towheight) - Math.abs(oneheight);
        //位置
        var centerX = (self.OriginTouches[0].pageX - self.OriginTouches[1].pageX) / 2 + self.OriginTouches[1].pageX;
        var centerY = (self.OriginTouches[0].pageY - self.OriginTouches[1].pageY) / 2 + self.OriginTouches[1].pageY;
        var startMinX = self.StartTouches[0].pageX < self.StartTouches[1].pageX ? self.StartTouches[0].pageX : self.StartTouches[1].pageX;
        var startMinY = self.StartTouches[0].pageY < self.StartTouches[1].pageY ? self.StartTouches[0].pageY : self.StartTouches[1].pageY;
        var endMinX = self.EndTouches[0].pageX < self.EndTouches[1].pageX ? self.EndTouches[0].pageX : self.EndTouches[1].pageX;
        var endMinY = self.EndTouches[0].pageY < self.EndTouches[1].pageY ? self.EndTouches[0].pageY : self.EndTouches[1].pageY;
        var disTop = endMinY - startMinY;
        var disLeft = endMinX - startMinX;
        var width = diswidth * 2.02 + $(img).width();
        var height = disheight * 2.02 + $(img).height();
        var left = $(img).position().left + disLeft * 2;
        var top = $(img).position().top + disTop * 2;
        if (Math.abs(diswidth / disheight) > originWidth / originHeight) {
            height = originHeight / originWidth * width;
            top = $(img).position().top + ($(img).height() - height) * centerY / originHeight;
        } else {
            width = originWidth / originHeight * height;
            left = $(img).position().left + ($(img).width() - width) * centerX / originWidth;
        }
        var info = { width: width, height: height, left: left, top: top };

        var rev = this.ResetZoom(img, info);


        $(img).animate(info, rev ? 30 : 0);
    },//放大
    EndZoom: function (img) {
        var self = this;
        if (!self.IsZoom)
            return;
        if ($(img).is(":animated")) {
            $(img).stop(true, true);
        }
        setTimeout(function () {
            self.IsZoom = false;
        }, 300);
        var info = { width: $(img).width(), height: $(img).height(), top: $(img).position().top, left: $(img).position().left };
        this.ResetZoom(img, info, true);
        $(img).animate(info, 300);
    },
    ResetZoom: function (img, info, isRestore) {
        var originWidth = parseInt($(img).attr("OriginWidth"));
        var originHeight = parseInt($(img).attr("OriginHeight"));
        var originLeft = parseInt($(img).attr("OriginLeft"));
        var originTop = parseInt($(img).attr("OriginTop"));
        var rev = false;
        if (originWidth * this.MinZoomMultiple > info.width) {
            info.width = isRestore ? originWidth * this.MinZoomMultiple : info.width - 2;
            info.height = info.width * originHeight / originWidth;
            info.top = originTop + parseInt((originHeight - info.height) / 2);
            info.left = originLeft + parseInt((originWidth - info.width) / 2);
            rev = true;
        }
        else if (originHeight * this.MinZoomMultiple > info.height) {
            info.height = isRestore ? originHeight * this.MinZoomMultiple : info.height - 2;
            info.width = info.height * originWidth / originHeight;
            info.top = originTop + parseInt((originHeight - info.height) / 2);
            info.left = originLeft + parseInt((originWidth - info.width) / 2);
            rev = true;
        }
        else if (info.width > originWidth * this.MaxZoomMultiple) {
            info.width = isRestore ? originWidth * this.MaxZoomMultiple : info.width + 2;
            info.height = info.width * originHeight / originWidth;
            info.top = $(img).position().top + parseInt(($(img).height() - info.height) / 2);
            info.left = $(img).position().left + parseInt(($(img).width() - info.width) / 2);
            rev = true;
        }
        else if (info.height > originHeight * this.MaxZoomMultiple) {
            info.height = isRestore ? originHeight * this.MaxZoomMultiple : info.height + 2;
            info.width = info.height * originWidth / originHeight;
            info.top = $(img).position().top + parseInt(($(img).height() - info.height) / 2);
            info.left = $(img).position().left + parseInt(($(img).width() - info.width) / 2);
            rev = true;
        }
        if (isRestore) {

            if (info.height > $(window).height() && info.height + info.top < $(window).height()) {//底部空白
                info.top = $(window).height() - info.height;
            } else if (info.height > $(window).height() && info.top > 0) {//头部空白
                info.top = 0;
            }
            else if (rev) {//超过
                info.top = info.height > originHeight ? $(img).position().top + ($(img).height() - info.height) / 2 : originTop;
            }
            else if (info.height < $(window).height()) {//不够
                info.top = ($(window).height() - info.height) / 2;
            }
            if (info.width > $(window).width() && info.width + info.left < $(window).width()) {//右边空白
                info.left = $(window).width() - info.width;
            }
            else if (info.width > $(window).width() && info.left > 0) {//左边空白
                info.left = 0;
            } else if (rev) {
                info.left = info.width > originWidth ? $(img).position().left + ($(img).width() - info.width) / 2 : originLeft;
            }
            else if (info.width < $(window).width()) {
                info.left = ($(window).width() - info.width) / 2;
            }

        }
        info.width = parseInt(info.width);
        info.height = parseInt(info.height);
        info.left = parseInt(info.left);
        info.top = parseInt(info.top);

        return rev;
    }

};