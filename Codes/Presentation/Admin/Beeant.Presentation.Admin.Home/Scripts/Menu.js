var Menu = function (menuId) {
    this.Container = $("#" + menuId);
};
Menu.prototype = {
    Initialize: function () {
        this.SetMenu();
        this.BindEvent();
    },
    BindEvent: function () {//绑定事件
        var self = this;
        this.Container.find(".parent").bind("click", function () {
            var isShow = $(this).next()[0].style.display == "none";
            self.Container.find(".parent").attr("className", "parent");
            self.Container.find(".parent").next().hide();
            if (isShow) {
                $(this).next().show();
                this.className = "parent open"; 
            }
        });
    },
    SetMenu: function () {
        var link = null;
        this.Container.find(".parent").next().find("a").each(function () {
            if (window.location.href == this.href) {
                link = this;
                return false;
            }
            var index = this.href.lastIndexOf("/");
            if (index != -1) {
                var url = this.href.substring(0, index);
                if (window.location.href.indexOf(url+"/") > -1) {
                    link = this;
                }
            }
            return true;
        });
        if (link != null) {
            $(link).parent().parent().show();
            $(link).parent().parent().prev().className = "parent open";
            link.style.color = "#F2963D";
        }
    }
};
