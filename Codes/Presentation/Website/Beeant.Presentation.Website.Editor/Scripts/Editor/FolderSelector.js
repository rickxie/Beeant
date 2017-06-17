var Folder = function (swictherId, containerId, url) {
    this.Container = $("#" + containerId);
    this.Swicther = $("#" + swictherId);
    this.Url = url;
};
Folder.prototype = {
    Initialize: function () { //加载css样式文件
        this.BindEvent();
    },
    BindEvent: function () {//绑定事件
        var self = this;
        this.Swicther.bind("mouseover", function () {
            self.Container.show();
        });
        this.Swicther.bind("mouseout", function () {
            self.Container.hide();
        });
        this.Container.bind("mouseover", function () {
            self.Container.show();
        });
        this.Container.bind("mouseout", function () {
            self.Container.hide();
        });
        this.Container.find("a").click(function () {
            if (window.Finder.SelectFile == null)
                return;
            self.Move($(window.Finder.SelectFile).prev().val(), $(this).attr("value"));
        });
    },
    //移动目录
    Move: function(id, folderId) {
        var self = this;
        $.ajax({
            type: "Post",
            url: self.Url,
            data: { id: id, folderId: folderId },
            async: false,
            dataType: "text",
            success: function(data) {
                if (data == "") {
                    alert("移动成功");
                    window.location.reload(true);
                } else {
                    alert("移动失败");
                }
            },
            error: function() {
                alert("移动失败");
            }
        });
    }
};
 