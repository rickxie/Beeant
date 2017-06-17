$(document).ready(function () {
    $(".sqr").css("height", $(".sqr").width()).css("line-height", $(".sqr").width() + "px")
    .css("margin-top", $(".sqr").css("marginLeft") );


    var config = [
 
         {
             Triggers: [
                 {
                     Sender: "",
                     Event: ""
                 }
             ],
             Loading: { Content: "Content", Type: "Append" },
             Url: "/Home/Banner/" + window.SiteId,
             Paramters: { page: 0 },
             Content: $(".banner")[0],
             ShowType: "Replace",
             DataType: "text",
             RequestType: "Repeat",
             IsExecute: true,
             IsLoadHideContent: false,
             Group: "",
             BeginLoadFunction: function () {
            
             },
             BeginShowFunction: function (sender, info, data) {
              
             },
             EndShowFunction: function (sender, info, data) {
                 window.lazyloadImage($(info.Content)[0]);
                 var banner = new Winner.Banner("divBanner");
                 $(info.Content).find("div[class='btc']").css("top", parseInt(($(info.Content).height() / 2 - $(info.Content).find("div[class='btc']").height() / 2)));
                 $(info.Content).find("div[class='btcn']").css("top", parseInt(($(info.Content).height() / 2 - $(info.Content).find("div[class='btcn']").height() / 2)));
                 banner.Initialize();
                 $(info.Content).find(".view").css("left", parseInt(($(info.Content).width() - $(info.Content).find(".view").find(".li").length * 10) / 2));
             }
         },
         {
             Triggers: [
                 {
                     Sender: "",
                     Event: ""
                 }
             ],
             Loading: { Content: "Content", Type: "Append" },
             Url: "/Home/Tag/" + window.SiteId,
             Paramters: { page: 0 },
             Content: $("#divTag")[0],
             ShowType: "Replace",
             DataType: "text",
             RequestType: "Repeat",
             IsExecute: true,
             IsLoadHideContent: false,
             Group: "",
             BeginLoadFunction: function () {

             },
             BeginShowFunction: function (sender, info, data) {

             },
             EndShowFunction: function (sender, info, data) {
                 window.lazyloadImage($(info.Content)[0]);
                 $(info.Content).find(".sqr").css("margin-top", $(info.Content).find(".sqr").css("margin-left"));
                 $(info.Content).find("img").each(function() {
                     $(this).css("height", $(this).width() + "px");

                 });
             }
         }
    ];
    var dataloader = new Winner.DataLoader(config);
    dataloader.Initialize();
});



