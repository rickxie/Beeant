$(document).ready(function () {

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
             Url: "/Catalog/List/" + window.SiteId,
             Paramters: { page: 0 },
             Content: $(".catalog")[0],
             ShowType: "Append",
             DataType: "text",
             RequestType: "Repeat",
             IsExecute: true,
             IsLoadHideContent: false,
             Group: "",
             BeginLoadFunction: function () {

             },
             BeginShowFunction: function (sender, info, data) {
                 info.Paramters.page = info.Paramters.page + 1;
             },
             EndShowFunction: function (sender, info, data) {
                 window.lazyloadImage($(info.Content)[0]);
                 var length = $("<div>" + data + "</div>").find(".sqr").length;
                 if (length < 24)
                     info.IsFullLoadComplate = true;
                 $(info.Content).find(".itm").css("margin-top", $(info.Content).find(".itm").css("margin-left"));
                 $(info.Content).find("img").each(function() {
                     $(this).css("height", $(this).width() + "px");

                 });
             }
         }
    ];
    var dataloader = new Winner.DataLoader(config);
    dataloader.Initialize();
});



