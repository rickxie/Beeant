$(document).ready(function() {
    $(".ori").css("height", $(window).height());
    //产品放大
    $(".detailimages").click(function() {
        $(this).hide();
        $(".commodity").show();
    });
    var album = new Winner.Album($("#list")[0], $("#biggroup" )[0], { ViewHeight: $(window).height() - $(".top").height() - $(".catalog").height() });
    album.GetHtml = function (sender) {
        var descHtml = "";
        if ($(sender).attr("DataDescription") != "") {
            descHtml = "<div class='desc'>" + $(sender).attr("DataDescription") + "</div>";
        }
        var priceHtml = "";
        if ($(sender).attr("DataPrice") != "") {
            priceHtml = "|" + window.Language.PriceTip + ":<span class='red'>" + $(sender).attr("DataPrice") + "</span>";
        }
        var authorHtml = "";
        if (window.ShowAuthorUrl != "") {
            authorHtml = "<div class='author'><a href='" + window.ShowAuthorUrl + "'>" + window.Language.AuthorTip + "</a></div>";
        }
        var passHtml = "";
        if ($(sender).attr("IsPassword") == "true") {
            passHtml = '<div class="pass"><input type="input" placeholder="' + window.Language.PasswordTip + '"></div>';
        }
        var openDetailHtml = "";
        if (window.IsOpenImages) {
            openDetailHtml = '<a href="javacript:void(0);" name="opendetail">' + window.Language.OpenDetailTip + '</a>';
        }
        return '<div id="' + album.GetId(sender) + '" Album="true" CatalogId="'
            + $(sender).attr("CatalogId")
            + '" class="big" Index="' + $(sender).attr("Index") 
            + '"><div class="con">' + '<div class="name">'
            + $(sender).attr("DataName") + priceHtml + '</div>'
            + '<div class="op"><a href="javacript:void(0);" onclick=\'$(".contact")[0].click();\'>' + window.Language.LinkTip + '</a>' 
            +openDetailHtml+
            '</div>' +
            descHtml + authorHtml + passHtml
            + '<div class="imgloading" Album="Loading" style="display:none;"><img src="/Images/loading.gif"/></div>'
            + '<img class="img" Album="Image" src="' + $(sender).find("img").attr("src")  + '"/>' +
             
            '</div></div>';
    }
    album.GetId = function (sender, index) {
        if (index == undefined)
            index = $(sender).attr("Index");
        return "itemc" + $(sender).attr("CatalogId") + "i" + index;
    }
    album.BeginShowFunction = function (sender) {
        $(".biggroup").hide();
        $("#biggroup" + $(sender).attr("catalogid")).show();
        return $("#divSelectItem").attr("IsSelect") != "true";
    }
    album.ShowContent = function () {
        album.Content.show();
        $(".ori").show();
    }
    album.HideContent = function () {
        album.Content.hide();
        $(".ori").hide();
        window.AlbumShowSender = null;
        window.SetWxEvent(null);
    }
    album.CreateFunction = function (sender, item) {
        item.find("input").bind("blur", function () {
            var pass = $(this).val();
            $.ajax({
                type: "Post",
                url: "/Commodity/GetFileName/" + window.SiteId,
                data: { id: $(sender).attr("DataId"), password: pass },
                async: false,
                dataType: "json",
                success: function (data) {
                    if (data != null && data.FileName != undefined) {
                        item.find("div[class='pass']").remove();
                        item.find("img[Album='Image']").attr("src", data.FileName + "?v=1");
                        $(sender).find("img").attr("src", data.FileName + "?v=1");
                        $(sender).find("img").attr("data-original", data.FileName + "?v=1");
                        $(sender).find("div[class='name']").hide();
                        $(sender).removeAttr("IsPassword");
                        $(sender).attr("Password", pass);
                        window.SetWxEvent(window.AlbumShowSender);
                    } else {
                        alert(window.Language.PasswordErrorMessage);
                    }
                },
                error: function () {
                    alert(window.Language.LoadErrorMessage);
                }
            });
        });
        item.find("a[name='opendetail']").bind("click", function () {
            if ($(sender).attr("IsPassword") == "true") {
                alert(window.Language.ImagesPasswordLockTip);
                item.find("input").focus();
                return;
            }
            $.ajax({
                type: "Post",
                url: "/Commodity/Images/" + window.SiteId,
                data: { id: $(sender).attr("DataId"), password: $(sender).attr("Password") == undefined ? "" : $(sender).attr("Password") },
                async: false,
                dataType: "json",
                success: function (data) {
                    var html = '<img src="' + window.PreRenderPicture + '" data-original="' + $(sender).attr("albumoriginalurl") + '" />';
                    if (data != null && data.FileNames != undefined) {
                        for (var i = 0; i < data.FileNames.length; i++) {
                            html += '<img src="' + window.PreRenderPicture + '" data-original="' + data.FileNames[i] + '" />';
                        }
                    }
                    $(".detailimages").html(html).show();
                    $(".commodity").hide();
                    window.lazyloadImage($(".detailimages")[0]);
                },
                error: function () {
                    alert(window.Language.LoadErrorMessage);
                }
            });
        });
    }
    album.EndShowFunction = function (sender, item) {
        window.AlbumShowSender = album.CurrentShowSender;
        if (window.SetWxEvent != undefined) {
            window.SetWxEvent(window.AlbumShowSender);
        }
    }
    album.ChangeFunction = function (item) {
        window.AlbumShowSender = album.CurrentShowSender;
        if (window.SetWxEvent != undefined) {

            window.SetWxEvent(window.AlbumShowSender);
        }
    }
   
    album.Initialize();
    function bindCommodityItemShowEvent(sender) {
        $(sender).find(".select").css("height", $(sender).height() + "px");

        function setWxEvent() {
            if (window.SetWxEvent != undefined) {
                var main = null;
                var others = [];
                var count = 0;
                $(".list").find(".selectok:visible").each(function () {
                    count++;
                    if (main == null) {
                        main = $(this).parent()[0];
                    } else {
                        others.push($(this).parent()[0]);
                    }
                    if (count >= 6) {
                        return false;
                    }
                });
                window.SetWxEvent(main, others);
            }
        }
        $(sender).click(function () {
            if ($("#divSelectItem").attr("IsSelect") == "true") {
                if ($(sender).find(".select")[0].style.display == "none") {
                    if ($(".list").find(".selectok:visible").length >= 6) {
                        alert(window.Language.SelectCountOver);
                        return;
                    }
                    $(sender).find(".select").show();
                    $(sender).find(".selectok").show();
                } else {
                    $(sender).find(".select").hide();
                    $(sender).find(".selectok").hide();
                }
                setWxEvent();

            }
   
        });
        album.Append(sender);
    }
 

 
    //异步加载类目
    $(".ori").find(".biggroup").css("height", $(".ori").height() + "px").css("width", $(".ori").width() + "px");
    var config = [
        {
            Triggers: [
                {
                    Sender: $(".searchinput")[0],
                    Event: "blur",
                    Function: function(info) {
                        var rev = $(".searchinput").attr("BeforValue") != $(".searchinput").val() && $(".searchinput").val() != "";
                        if (rev) {
                            info.Paramters.page = 0;
                            info.IsFullLoadComplate = false;
                            $("#list").html("");

                        } else if ($(".searchinput").val() == "") {
                            $("#list").show();
                        }
                        return rev;
                    }
                },
                {
                    Sender: window,
                    Event: "scroll",
                    Function: function(info) {
                        if ($(document).scrollTop() >= $(document).height() - $(window).height())
                            return info.Content.style.display != "none";
                        return false;
                    }
                }
            ],
            Loading: { Content: "Content", Type: "Append" },
            Url: "/Commodity/List/" + window.SiteId,
            Paramters: { page: 0 },
            Content: $("#list")[0],
            ShowType: "Append",
            DataType: "text",
            RequestType: "Repeat",
            IsExecute: true,
            IsLoadHideContent: false,
            Group: "Detail",
            BeginLoadFunction: function() {
                this.Paramters.catalogId = window.CatalogId;
                this.Paramters.tagId = window.TagId;
                this.Paramters.key = $(".searchinput").val();
            },
            BeginShowFunction: function(sender, info, data) {
                info.Paramters.page = info.Paramters.page + 1;
            },
            EndShowFunction: function(sender, info, data) {
                window.lazyloadImage($(info.Content)[0]);
                var catalogId = window.CatalogId;
                var elemnts = $(info.Content).find(".element");
                $(info.Content).find("img").css("height", $(info.Content).find("img").width() + "px");
                elemnts.css("margin-top", elemnts.css("marginLeft"));
                var length = $("<div>" + data + "</div>").find(".element").length;
                if (length < 24) {
                    info.IsFullLoadComplate = true;
                }
                for (var i = elemnts.length - length; i < elemnts.length; i++) {
                    var src = $(elemnts[i]).find("img").attr("data-original");
                    var index = src.indexOf(".i.");
                    if (index != -1) {
                        src = src.substring(0, index);
                    }
                    $(elemnts[i]).attr("CatalogId", catalogId);
                    $(elemnts[i]).attr("AlbumOriginalUrl", src);
                    bindCommodityItemShowEvent(elemnts[i]);
                }
            }
        },
       {
           Triggers: [
               {
                   Sender: $(".contact")[0],
                   Event: "click"
               }
           ],
           Loading: { Content: "Content", Type: "Append" },
           Url: "/Company/Detail/" + window.SiteId,
           Paramters: {  },
           Content: $(".lianxi")[0],
           ShowType: "Append",
           DataType: "text",
           RequestType: "OneTime",
           IsExecute: false,
           IsLoadHideContent: false,
           BeginLoadFunction: function () {
           },
           BeginShowFunction: function (sender, info, data) {

           },
           EndShowFunction: function (sender, info, data) {
               window.lazyloadImage($(info.Content)[0]);
               $(".lianxi").css("height", $(window).height());
               var btn = $('<div class="cancel">×</div>');
               btn.click(function () {
                   $(".lianxi").hide();
                   $(".commodity").show();
               });
               $(info.Content).find(".ctop").append(btn);

           }
       }
    ];
    var dataloader = new Winner.DataLoader(config);
    dataloader.Initialize();

    //联系信息


    $(".contact").click(function () {
        $(".lianxi").show();
        $(".commodity").hide();
    });
    if (window.IsOpenMultiShare) {
        $("#divSelectItem").insertBefore($(".searchico"));
        $(".toper").find(".name").css("width", $(window).width() - $(".contact").width() - $(".searchico").width() - $("#divSelectItem").width() - 10 + "px");
        $("#divSelectItem").html(window.Language.SelectItemTip);
        $("#divSelectItem").click(function () {
            if ($(this).attr("IsSelect") == "true") {
                $(this).removeAttr("IsSelect");
                $("#divSelectItem").html(window.Language.SelectItemTip);
                $(".list").find(".selectok:visible").hide();
                $(".list").find(".select:visible").hide();
            } else {
                $(this).attr("IsSelect", "true");
                $("#divSelectItem").html(window.Language.CancelItemTip);
            }
        });
    } else {
        $("#divSelectItem").remove();
    }
});



 