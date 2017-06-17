$(document).ready(function () {
    function bindOpEvent(sender) {
   
        $(sender).find("input").blur(function () {
            if (this.value == $(this).attr("OriValue"))
                return false;
            var self = this;
            if (!window.checkSaveTag(self)) {
                return false;
            }
            var remark = this.value;
            var openid = $(this).attr("openid");
            $.ajax({
                type: "Post",
                url: "/Wechat/UpdateRemark?remark=" + remark + "&openid=" + openid,
                data: {},
                async: true,
                dataType: "json",
                success: function (data) {
                    if (data.Status) {
                        $(self).attr("OriValue", remark);

                    } else {
                        alert("更新失败");
                    }
                    window.removeSaveTag(self);
                },
                error: function () {
                    window.removeSaveTag(self);
                    alert("系统忙，请稍候再试");
                }
            });
       
        });
 
    }

    var nextOpenId = "";
    window.setWeixinUserInfo = function (total, nextopenid) {
        if (nextopenid == "") {
            nextOpenId = undefined;
        } else {
            nextOpenId = nextopenid;
        }
        $(".totalcount").find("span").html(total);
    }
    function getUrl() {
        
    }
    //异步加载
    var config = [
       {
           Triggers: [
               {
                   Sender: window,
                   Event: "scroll",
                   Function: function () {
                       
                       return $(document).scrollTop() >= $(document).height() - $(window).height() && nextOpenId!=undefined;
                   }
               }
           ],
           Loading: { Content: "Content", Type: "Append" },
           Url: "/Wechat/Users" ,
           Paramters: { page: 0 },
           Content: $(".list")[0],
           ShowType: "Append",
           DataType: "text",
           RequestType: "Repeat",
           IsExecute: true,
           IsLoadHideContent: false,
           BeginLoadFunction: function () {

           },
           BeginShowFunction: function (sender, info, data) {
          
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
               info.Paramters.page = info.Paramters.page + 1;
               info.Paramters.nextOpenid = nextOpenId;

           }
       }
    ];
    var dataloader = new Winner.DataLoader(config);
    dataloader.Initialize();
   
});
