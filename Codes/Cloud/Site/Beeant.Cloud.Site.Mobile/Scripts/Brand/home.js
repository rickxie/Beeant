$(document).ready(function () {

    function setNaver() {
        $(".naver").find(".ul").css("width", $(".naver").find(".li").width() * $(".naver").find(".li").length + "px");
        var slider = new Winner.Slider($(".naver")[0], $(".naver").find(".ul")[0]);
        slider.Initialize();
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
             Url: "/BrandHome/Tag/" + window.SiteId,
             Paramters: { page: 0 },
             Content: $("#divTag")[0],
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



