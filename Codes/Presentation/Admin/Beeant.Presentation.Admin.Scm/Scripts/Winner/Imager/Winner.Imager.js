Winner.Imager = function (id, config) {
    this.Base = new Winner.ClassBase();
    this.Container = document.getElementById(id);
    if (this.Container == null) {
        return;
    }
    this.PropertyName = "Imager";
    this.ThumbnailViewUrlName = "ViewUrl";
    this.ThumbnailAmplifyUrlName = "AmplifyUrl";
    this.ThumbnailLinkUrlName = "LinkUrl";
    this.ThumbnailImages = [];
    this.Scope = 2;
    this.ThumbnailView = { Count: 0, Width: 0 };
    if (config != undefined) {
        this.Base.LoadConfig(this, config);
    }
};
Winner.Imager.prototype = {
    Initialize: function () { //加载css样式文件
        this.Container.style.position = "relative";
        this.LoadControl(this.Container.childNodes);
        this.BeginLoadImages();
        this.CurrentIndex = 0;
        this.BindEvent();
        this.SetButtonClass();
    },
    BeginLoadImages: function () {//异步加载图片
        if (this.ThumbnailImages.length == 0) return;
        var self = this;
        if (this.ViewLoader == undefined) {
            this.ViewLoader = document.createElement('img');
            this.ViewLoader.style.width = "0px";
            this.Container.appendChild(this.ViewLoader);
            this.ViewLoaderIndex = 0;
            var funcView = function () {
                if (self.ViewLoaderIndex >= self.ThumbnailImages.length)
                    return;
                self.ViewLoader.src = self.Base.GetAttribute(self.ThumbnailImages[self.ViewLoaderIndex], self.ThumbnailViewUrlName);
                self.ViewLoaderIndex++;
            };
            self.Base.BindEvent(this.ViewLoader, "load", function () {
                funcView();
            });
            setTimeout(funcView, 1);
        }
        if (this.AmplifyLoader == undefined) {
            this.AmplifyLoader = document.createElement('img');
            this.AmplifyLoader.style.width = "0px";
            this.Container.appendChild(this.AmplifyLoader);
            this.AmplifyLoaderIndex = 0;
            var funcAmplify = function () {
                if (self.AmplifyLoaderIndex >= self.ThumbnailImages.length)
                    return;
                self.AmplifyLoader.src = self.Base.GetAttribute(self.ThumbnailImages[self.AmplifyLoaderIndex], self.ThumbnailAmplifyUrlName);
                self.AmplifyLoaderIndex++;
            };
            self.Base.BindEvent(this.AmplifyLoader, "load", function () {
                funcAmplify();
            });
            setTimeout(funcAmplify, 1);
        }
    },
    LoadControl: function (nodes) { //加载控件
        for (var i = 0; i < nodes.length; i++) {
            if (nodes[i].childNodes.length > 0)
                this.LoadControl(nodes[i].childNodes);
            switch (this.Base.GetAttribute(nodes[i], this.PropertyName)) {
                case "Preview":
                    this.PreviewButton = nodes[i];
                    break;
                case "Next":
                    this.NextButton = nodes[i];
                    break;
                case "AmplifyContainer":
                    this.AmplifyContainer = nodes[i];
                    this.AmplifyContainer.style.overflow = "hidden";
                    this.AmplifyContainer.style.position = "absolute";
                    break;
                case "AmplifyImager":
                    this.AmplifyImager = nodes[i];
                    break;
                case "ViewContainer":
                    this.ViewContainer = nodes[i];
                    this.ViewContainer.style.position = "relative";
                    break;
                case "ViewImager":
                    this.ViewImager = nodes[i];
                    this.ViewImagerPosition =
                    {
                        Left: this.Base.GetElementLeft(this.ViewImager),
                        Top: this.Base.GetElementTop(this.ViewImager)
                    };
                    break;
                case "Magnifier":
                    this.Magnifier = nodes[i];
                    this.Magnifier.style.position = "absolute";
                    break;
                case "ViewLinker":
                    this.ViewLinker = nodes[i];
                    break;
                case "ThumbnailContainer":
                    this.ThumbnailContainer = nodes[i];
                    this.ThumbnailContainer.style.overflow = "hidden";
                    break;
                case "ThumbnailContent":
                    this.LoadThumbnailContent(nodes[i]);
                    break;
            }
        }
    },
    LoadThumbnailContent: function (node) {
        this.ThumbnailContent = node;
        var self = this;
        var thumbNodes = node.childNodes;
        var func = function() {
            self.LoadThumbnailImages(thumbNodes);
            for (var i = 0; i < self.ThumbnailImages.length; i++) {
                self.BindThumbnailImagesEvent(self.ThumbnailImages[i]);
            }
        };
        setTimeout(func, 500);
    },
    LoadThumbnailImages: function (nodes) {//加载缩略图
        for (var i = 0; i < nodes.length; i++) {
            if (nodes[i].childNodes.length > 0)
                this.LoadThumbnailImages(nodes[i].childNodes);
            switch (this.Base.GetAttribute(nodes[i], this.PropertyName)) {
                case "ThumbnailImage":
                    this.ThumbnailImages.push(nodes[i]);
                    break;
            }
        }
    },
    BindEvent: function () { //绑定事件
        var self = this;
        if (this.PreviewButton != null) {
            this.Base.BindEvent(this.PreviewButton, "click", function () {
                self.Move(1);
            });
        }
        if (this.NextButton != null) {
            this.Base.BindEvent(this.NextButton, "click", function () {
                self.Move(-1);
            });
        }
        this.Base.BindEvent(this.ViewImager, "mouseover", function (e) {
            self.Show(e);
        });
        this.Base.BindEvent(this.Magnifier, "mousemove", function (e) {
            self.Amplify(true, e);
        });
        this.Base.BindEvent(this.Magnifier, "mouseout", function () {
            self.Hide();
        });
        this.Base.BindEvent(this.ViewImager, "mouseout", function () {
            self.Hide();
        });
    },
    BindThumbnailImagesEvent: function (image) {//绑定缩略图事件
        var self = this;
        this.Base.BindEvent(image, "mouseover", function () {
            self.SelectImage(image);
        });
    },
    Move: function (step) {//移动
        var index = this.CurrentIndex + step;
        if (index <= 0 && this.ThumbnailView.Count - index <= this.ThumbnailImages.length) {
            this.CurrentIndex = index;
            this.ThumbnailContent.style.marginLeft = this.CurrentIndex * this.ThumbnailView.Width + "px";
        }
        this.SetButtonClass();
    },
    SetButtonClass: function () {//设置按钮样式
        if (this.ThumbnailImages.length <= this.ThumbnailView.Count) {
            this.PreviewButton.className = this.PreviewButton.className + " unclick";
            this.NextButton.className = this.NextButton.className + " unclick";
            return;
        }
        if (this.CurrentIndex < 0 && this.ThumbnailView.Count - this.CurrentIndex >= this.ThumbnailImages.length) {
            this.PreviewButton.className = this.PreviewButton.className.replace(" unclick", "");
            this.NextButton.className = this.NextButton.className + " unclick";
            return;
        }
        if (this.CurrentIndex == 0) {
            this.PreviewButton.className = this.PreviewButton.className + " unclick";
            this.NextButton.className = this.NextButton.className.replace(" unclick", "");
            return;
        }
        this.PreviewButton.className = this.PreviewButton.className.replace(" unclick", "");
        this.NextButton.className = this.NextButton.className.replace(" unclick", "");
    },
    SelectImage: function (image) {//选中图片
        for (var i = 0; i < this.ThumbnailImages.length; i++) {
            this.ThumbnailImages[i].className = this.ThumbnailImages[i].className.replace(" select", "");
        }
        image.className = image.className + " select";
        this.ViewImager.src = this.Base.GetAttribute(image, this.ThumbnailViewUrlName);
        this.ViewLinker.href = this.Base.GetAttribute(image, this.ThumbnailLinkUrlName);
        this.AmplifyImager.src = this.Base.GetAttribute(image, this.ThumbnailAmplifyUrlName);
    },
    Show: function (event) { //显示
        this.AmplifyContainer.style.display = "";
        this.Magnifier.style.display = "";
        this.ViewImager.className = this.ViewImager.className + " over";
        this.SetControlSize();
        this.Amplify(false, event);
    },
    SetControlSize: function () {//设置控件大小
        this.AmplifyContainer.style.width = this.Magnifier.clientWidth * this.Scope + "px";
        this.AmplifyContainer.style.height = this.Magnifier.clientHeight * this.Scope + "px";
    },
    Amplify: function (isMagnifier, event) {//放大
        event = window.event ? window.event : event;
        this.AmplifyContainer.style.display = "";
        this.Magnifier.style.display = "";
        var vX, vY;
        if (window.event) {
            if (isMagnifier) {
                var top = document.documentElement.scrollTop || document.body.scrollTop;
                var left = document.documentElement.scrollLeft || document.body.scrollLeft;
                vX = event.clientX + left - document.documentElement.clientLeft - this.ViewImagerPosition.Left;
                vY = event.clientY + top - document.documentElement.clientTop - this.ViewImagerPosition.Top;
            } else {
                vX = event.offsetX;
                vY = event.offsetY;
            }
        } else {
            vX = event.pageX - this.ViewImagerPosition.Left;
            vY = event.pageY - this.ViewImagerPosition.Top;
        }
        vX = vX - this.Magnifier.clientWidth / 2;
        vY = vY - this.Magnifier.clientHeight / 2;
        if (vX < 0) vX = 0;
        else if (vX > this.ViewImager.clientWidth - this.Magnifier.clientWidth)
            vX = this.ViewImager.clientWidth - this.Magnifier.clientWidth;
        if (vY < 0) vY = 0;
        else if (vY > this.ViewImager.clientHeight - this.Magnifier.clientHeight)
            vY = this.ViewImager.clientHeight - this.Magnifier.clientHeight;
        var leftMargin = vX * this.Scope;
        var topMargin = vY * this.Scope;
        if (leftMargin + this.AmplifyContainer.clientWidth > this.AmplifyImager.clientWidth) {
            leftMargin = this.AmplifyImager.clientWidth - this.AmplifyContainer.clientWidth;
        }
        if (topMargin + this.AmplifyContainer.clientHeight > this.AmplifyImager.clientHeight) {
            topMargin = this.AmplifyImager.clientHeight - this.AmplifyContainer.clientHeight;
        }
        this.AmplifyImager.style.marginLeft = -leftMargin + "px";
        this.AmplifyImager.style.marginTop = -topMargin + "px";
        this.Magnifier.style.left = vX + "px";
        this.Magnifier.style.top = vY + "px";
    },
    Hide: function () {//隐藏
        this.AmplifyContainer.style.display = "none";
        this.Magnifier.style.display = "none";
        this.ViewImager.className = this.ViewImager.className.replace(" over", "");
    }
};
