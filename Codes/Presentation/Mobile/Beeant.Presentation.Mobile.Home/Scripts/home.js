$(document).ready(function() {
    var func = function() {
        var swidth = $(".home").width() * 0.485;
        var bwidth = $(".home").width() * 0.98;
        $(".home").find("div[shape='ssquare']").css("height", swidth + "px");
        $(".home").find("div[shape='ssquare']").find("img").css("height", swidth + "px");
        $(".home").find("div[shape='bsquare']").css("height", bwidth + "px");
        $(".home").find("div[shape='bsquare']").find("img").css("height", bwidth + "px");
        $(".home").find("div[shape='rectangle']").css("height", swidth + "px");
        $(".home").find("div[shape='rectangle']").find("img").css("height", swidth + "px");
        var obj = $(".home").find("div[shape]");
    }
    $(window).bind("orientationchange", function() {
        func();
    });
    $(window).bind("resize", function() {
        func();
    });
    func();
    var config = [
        {
            Triggers: [
                {
                    Sender: window,
                    Event: "scroll",
                    Function: function() {
                        return $(document).scrollTop() >= $(document).height() - $(window).height();
                    }
                }
            ],
            Loading: { Content: "Content", Type: "Append" },
            Url: "/Home/Goods",
            Paramters: { page: 0 },
            Content: $(".product")[0],
            ShowType: "Append",
            DataType: "text",
            RequestType: "Repeat",
            IsExecute: true,
            IsLoadHideContent: false,
            BeginLoadFunction: function() {

            },
            BeginShowFunction: function(sender, info, data) {
                info.Paramters.page = info.Paramters.page + 1;

            },
            EndShowFunction: function(sender, info, data) {
                func();
                window.lazyloadImage($(info.Content)[0]);
                if ($(info.Content).find(".element").last().find("img").length < 5)
                    info.IsFullLoadComplate = true;
            }
        },
        {
            Triggers: [
                {
                    Sender: window,
                    Event: ""
                }
            ],
            Loading: { Content: "Content", Type: "Replace" },
            Url: "/Home/Banner",
            Paramters: {},
            Content: $("#banner")[0],
            ShowType: "Append",
            DataType: "text",
            RequestType: "OneTime",
            IsExecute: true,
            IsLoadHideContent: false,
            BeginLoadFunction: function() {

            },
            BeginShowFunction: function(sender, info, data) {


            },
            EndShowFunction: function(sender, info, data) {
                window.lazyloadImage($("#banner")[0]);
                var banner = new Winner.Banner("");
                banner.Container = $("#banner");
                banner.Initialize();
            }
        }
    ];
    var dataloader = new Winner.DataLoader(config);
    dataloader.Initialize();

});



 