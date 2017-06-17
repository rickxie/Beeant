$(document).ready(function () {
    $(".sqr").css("height", $(".sqr").width()).css("line-height", $(".sqr").width() + "px")
    .css("margin-top", $(".sqr").css("marginLeft") );

    function setNaver() {
        $(".naver").find("li").css("height", $(".naver").find("li").width() + "px").css("line-height", $(".naver").find("li").width() + "px");
    }

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
             Url: "/WindowHome/Tag/" + window.SiteId,
             Paramters: { page: 0 },
             Content: $(".naver").find("ul")[0],
             ShowType: "Append",
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
                 setNaver();
             }
         }
    ];
    var dataloader = new Winner.DataLoader(config);
    dataloader.Initialize();
    $("#divBanner").css("height", $(window).height() + "px");
    $(".home").css("height", $(window).height() + "px");
  
});



