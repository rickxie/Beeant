$(document).ready(function() {
    var itemConfig = [{
        Triggers: [
            {
                Sender: window,
                Event: "scroll",
                Function: function (info) {
                    return $(document).scrollTop() >= $(document).height() - $(window).height();
                }
            }
        ],
        Loading: { Content: "Content", Type: "Append" },
        Url: "/Commodity/List/" + window.SiteId,
        Paramters: { page: 0 },
        Content: $(".list")[0],
        ShowType: "Append",
        DataType: "text",
        RequestType: "Repeat",
        IsExecute: true,
        IsLoadHideContent: false,
        Group: "Detail",
        BeginLoadFunction: function (info) {
            this.Paramters.key = $("input[name='key']").val();
            this.Paramters.tagId = window.TagId == "" ? "" : window.TagId;
            this.Paramters.catalogId = this.Paramters.catalogId != undefined ? this.Paramters.catalogId : window.CatalogId;
        },
        BeginShowFunction: function (sender, info, data) {
            info.Paramters.page = info.Paramters.page + 1;
        },
        EndShowFunction: function (sender, info, data) {
            window.lazyloadImage($(info.Content)[0]);
            var elemnts = $(info.Content).find(".element");
            var length = $("<div>" + data + "</div>").find(".element").length;
            if (length < 24) {
                info.IsFullLoadComplate = true;
            }
            for (var i = elemnts.length - length; i < elemnts.length; i++) {
                $(elemnts[i]).attr("Index", i);
                window.BindCommodity(elemnts[i]);
            }
        }
    }];

    function bindCatalogItemEvent(itemConfig, item) {
        var catalogId = $(item).attr("DataId");
        itemConfig[0].Triggers.push({
            Sender: item,
            Event: "click",
            Function: function (info) {
                info.IsFullLoadComplate = false;
                info.Paramters.page = 0;
                info.Paramters.catalogId = $(item).attr("DataId");
                window.TagId = "";
                window.CatalogId = "";
                $("input[name='key']").val("");
                $(".list").html("");
                $(".items").find(".item").removeClass("select");
                $(item).addClass("select");
                $("input[name='key']").val("");
                return true;
            }
        });
    }

    var config = [
       {
           Triggers: [
               {
                   Sender: window,
                   Event: ""
               }
           ],
           Loading: { Content: "Content", Type: "Append" },
           Url: "/Commodity/Catalog/" + window.SiteId,
           Paramters: { page: 0 },
           Content: $(".items")[0],
           ShowType: "Replace",
           DataType: "text",
           RequestType: "OneTime",
           IsExecute: true,
           IsLoadHideContent: false,
           BeginLoadFunction: function () {
           },
           BeginShowFunction: function (sender, info, data) {

           },
           EndShowFunction: function (sender, info, data) {
               window.lazyloadImage($(info.Content)[0]);
               var item = $(info.Content).find(".item");
               $(info.Content).css("width", (item.width()+10) * item.length+2 );
               item.each(function (index, sender) {
                   bindCatalogItemEvent(itemConfig,this);
               });
               var dl = new Winner.DataLoader(itemConfig);
               dl.Initialize();
               window.CommodityDataLoader = dl;

           }
       }
    ];
    var dataloader = new Winner.DataLoader(config);
    dataloader.Initialize();

    var slider = new Winner.Slider($(".view")[0], $(".items")[0]);
    slider.Initialize();



});



 