$(document).ready(function () {
    function check(id) {
        if (!window.checkSaveTag(this)) {
            return;
        }
        var rev = true;
        var self = this;
        $.ajax({
            type: "Post",
            url: "/Book/Check",
            data: { albumId: id },
            async: false,
            dataType: "text",
            traditional: true,
            success: function (data) {
                if (data != null && data != "") {
                    rev = confirm("检查到该图册模板有些类目产品数量不符合：" + data + ",您确定要生成吗？");
                } 
                window.removeSaveTag(self);
            },
            error: function () {
                alert("系统忙，请稍候再试");
                window.removeSaveTag(self);
            }
        });
        return rev;
    }
    function create(id) {
        if (!confirm("重新生成目录本会覆盖原来的目录本，您确定重新生成吗？"))
            return;
        if (!check(id)) {
            return;
        }
        if (!window.checkSaveTag(this)) {
            return;
        }
        var self = this;
        $.ajax({
            type: "Post",
            url: "/Book/Create",
            data: { albumId: id },
            async: true,
            dataType: "json",
            traditional: true,
            success: function (data) {
                if (data.Status) {
                    alert("已经提交申请，等待后台生成目录本");
                } else {
                    alert("提交申请失败");
                }
                window.removeSaveTag(self);
            },
            error: function () {
                alert("系统忙，请稍候再试");
                window.removeSaveTag(self);
            }
        });

    }
    $(document).find("*[name=create]").click(function () {
        create($(this).attr("dataid"));
    });
    function bindOpEvent(sender) {
        $(sender).find("*[name=create]").click(function() {
            create($(this).attr("dataid"));
        });
    }
    var config = [
       {
           Triggers: [
             {
                 Sender: window,
                 Event: "scroll",
                 Function: function () {
                     return $(document).scrollTop() >= $(document).height() - $(window).height();
                 }
             }
           ],
           Loading: { Content: "Content", Type: "Append" },
           Url: "/Book/List",
           Paramters: {page:0 },
           Content: $("#list")[0],
           ShowType: "Replace",
           DataType: "text",
           RequestType: "Repeat",
           IsExecute: true,
           IsLoadHideContent: false,
           BeginLoadFunction: function () {

           },
           BeginShowFunction: function (sender, info, data) {
               info.Paramters.page = info.Paramters.page + 1;

           },
           EndShowFunction: function (sender, info, data) {
               window.lazyloadImage($(info.Content)[0]);
               var length = $("<div>" + data + "</div>").find(".element").length;
               if (length < 24)
                   info.IsFullLoadComplate = true;
               var elemnts = $(info.Content).find(".element");
               for (var i = elemnts.length - length; i < elemnts.length; i++) {
                   bindOpEvent(elemnts[i]);
               }
           }
       }
    ];
    var dataloader = new Winner.DataLoader(config);
    dataloader.Initialize();
});




 