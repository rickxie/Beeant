var Platform = function () {
    this.Base = new Winner.ClassBase();
};
Platform.prototype = {
    Initialize: function () {
        this.BindLoad();
        this.Dialog = new Winner.Dialog("平台同步信息", "", { IsShowDialog: false });
        this.Dialog.Initialize();
        this.InitOperate();
    },
    InitOperate: function () {
        var self = this;
        $(document).find("a[product='true']").click(function () {
            var div = $(this).next();
            self.Dialog.Title.innerHTML = $(this).html() + "-产品选择";
            self.Dialog.Detail.innerHTML = "";
            if (div.attr("isload") != "true") {
                var infos = [];
                $.ajax({
                    type: 'GET',
                    url: '/Ajax/Product/ProductByGoods.aspx?goodsid=' + div.attr("goodsid"),
                    async: false,
                    data: "",
                    success: function (msg) { infos = eval(msg); }
                });
                if (infos != null) {
                    var value = "";
                    for (var i = 0; i < infos.length; i++) {
                        value += "<a href='@Href@Id' target='_blank'>@Text</a></br>".replace("@Href", div.attr("Href"))
                            .replace("@Id", infos[i].Value).replace("@Text", infos[i].Text);
                    }
                    div.html(value);
                }
                div.attr("isload","true");
            }
            self.Dialog.Detail.innerHTML = div.html();
            self.Dialog.ShowDialog();
        });
    },
    BindLoad: function () {
        var self = this;
        $(document).find("a[name='Platform']").click(function () {
            var goodsId = $(this).attr("GoodsId");
            $.ajax({
                type: "Post",
                url: "/Ajax/Product/Platform.aspx?op=Load&goodsId=" + goodsId,
                async: false,
                dataType: "text",
                success: function (infos) {
                    self.Load(goodsId, eval(infos));
                },
                error: function () {
                    alert("加载失败");
                }
            });
        });
    },
    Load: function (goodsId, infos) {
        if (infos == null)
            return;
        var self = this;
        this.Dialog.Title.innerHTML = "平台同步信息";
        this.Dialog.Detail.innerHTML = "";
        for (var i = 0; i < infos.length; i++) {
            self.AddInfo(goodsId, infos[i]);
        }
        this.Dialog.ShowDialog();
    },
    AddInfo: function (goodsId, info) {//绑定
        var div = document.createElement('div');
        div.innerHTML = info.Name + "平台:";
        var self = this;
        var hfSynch = document.createElement('a');
        hfSynch.href = "javascript:void(0);";
        hfSynch.innerHTML = "同步";
        $(hfSynch).click(function () {
            self.Synch(goodsId, info.Type);
        });
        $(div).append(hfSynch);
        if (info.Id != 0) {
            var hfSynchRemove = document.createElement('a');
            hfSynchRemove.href = "javascript:void(0);";
            hfSynchRemove.innerHTML = "同步删除";
            $(hfSynchRemove).click(function () {
                self.SynchRemove(info.Id, info.Type);
            });
            $(div).append(hfSynchRemove);

            var hfRemove = document.createElement('a');
            hfRemove.href = "javascript:void(0);";
            hfRemove.innerHTML = "删除";
            $(hfRemove).click(function () {
                self.Remove(info.Id);
            });
            $(div).append(hfRemove);
        }
        $(this.Dialog.Detail).append(div);
    },
    Synch: function (goodsId, type) {//同步
        $.ajax({
            type: "Post",
            url: "/Ajax/Product/Platform.aspx?op=Synch&type=" + type + "&goodsId=" + goodsId,
            async: false,
            dataType: "text",
            success: function (mess) {
                alert(mess);
            },
            error: function () {
                alert("操作异常");
            }
        });
    },
    SynchRemove: function (id, type) {//同步删除
        $.ajax({
            type: "Post",
            url: "/Ajax/Product/Platform.aspx?op=SynchRemove&type=" + type + "&id=" + id,
            async: false,
            dataType: "text",
            success: function (mess) {
                alert(mess);
            },
            error: function () {
                alert("操作异常");
            }
        });
    },
    Remove: function (id) {//删除
        $.ajax({
            type: "Post",
            url: "/Ajax/Product/Platform.aspx?op=Remove&id=" + id,
            async: false,
            dataType: "text",
            success: function (mess) {
                alert(mess);
            },
            error: function () {
                alert("操作异常");
            }
        });
    }
};

 

