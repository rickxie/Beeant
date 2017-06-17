$(document).ready(function() {
 
    var config = [
       {
           Triggers: [
               {
                   Sender: "",
                   Event: ""
               }
           ],
           Loading: { Content: "Content", Type: "Append" },
           Url: "/Company/Get/" + window.SiteId,
           Paramters: {},
           Content: $(".bottom")[0],
           ShowType: "Replace",
           DataType: "json",
           RequestType: "OneTime",
           IsExecute: true,
           IsLoadHideContent: false,
           BeginLoadFunction: function () {
              
           },
           BeginShowFunction: function (sender, info, data) {
        
           },
           EndShowFunction: function (sender, info, data) {
               if (data.WeixinQrCodeUrl != null && data.WeixinQrCodeUrl != "") {
                   $(".headertop").append('<div class="qr"><img src="' + data.WeixinQrCodeUrl + '" /> </div>');
               } else {
                   $(".headertop").append('<div class="qr"><img src="/home/GetQrCode/'+window.SiteId+'" /> </div>');
               }
           }
       }
    ];
    var dataloader = new Winner.DataLoader(config);
    dataloader.Initialize();
    window.BindCommodity = function (sender) {
        $(sender).find("input[name='pass']").blur(function() {
            var pass = $(this).val();
            $.ajax({
                type: "Post",
                url: "/Commodity/GetFileName/" + window.SiteId,
                data: { id: $(sender).attr("DataId"), password: pass },
                async: false,
                dataType: "json",
                success: function (data) {
                    if (data != null && data.FileName != undefined) {
                        $(sender).find("img").attr("src", data.FileName + "?v=1");
                        $(sender).find("input[name='pass']").parent().remove();
                        $(sender).attr("Password", pass);
                    } else {
                        alert(window.Language.PasswordErrorMessage);
                    }
                },
                error: function () {
                    alert(window.Language.LoadErrorMessage);
                }
            });
        });
        $(sender).find("img").click(function() {
            $(".gallerycon").show();
            $(".mask").show();
            window.setGallery(parseInt($(sender).attr("Index")));
        });

    }

    window.setGallery = function (pageNumber) {
        pageNumber = pageNumber == undefined ? 0 : pageNumber;
        $(".ad-thumb-list").html("");
        $(".ad-controls").html("");
        $(".ad-image-wrapper").html("");
        $(".commodity").find(".element").each(function (index, sender) {
            var src = $(sender).find("img").attr("src");
            var osrc = src;
            var i = src.indexOf(".i.");
            if (i != -1) {
                osrc = src.substring(0, i);
            }
            var html = $('<li><a href="' + osrc + '"><img src="' + src + '" class="image0"> </a></li>');
            $(".ad-thumb-list").append(html);
            var tip = $(sender).attr("DataName") + "<span class='red'>" + $(sender).attr("DataPrice") + "</span>";
            if (window.IsOpenImages) {
                tip += '<a href="/Commodity/Detail/' + window.SiteId + "?id=" + $(sender).attr("DataId") +  ($(sender).attr("password") == undefined ? "" :"&password="+ $(sender).attr("password")) + '" target="_blank">' + window.Language.DetailTip + "</a>";
            }
            html.find("img").data("ad-title", tip);
            html.find("img").data("ad-desc", $(sender).attr("DataDescription"));
        });
        var galleries = $('#gallery').adGallery({ start_at_index: pageNumber });
        galleries[0].settings.effect = "slide-hori";
    }
    $(".nav").find("a").each(function(sender, index) {
        if (window.location.href.indexOf(this.href) > -1) {
            $(this).attr("class", "select");
            return false;
        }
    });
    $(".gallerycon").css("left", parseInt(($(document).width() - $(".ad-gallery").width()) / 2))
    $(".mask").click(function () {
        $(".gallerycon").hide();
        $(".mask").hide();
        });

});



 