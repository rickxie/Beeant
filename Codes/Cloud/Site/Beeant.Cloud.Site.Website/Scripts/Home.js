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
           Url: "/Company/Get/" + window.SiteId+"?isDetail=true",
           Paramters: {},
           Content: $("#divcompany")[0],
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
            Url: "/Commodity/Top/" + window.SiteId,
            Paramters: { },
            Content: $(".commodity")[0],
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
        }, 
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
                 $(info.Content).find("div[class='prev']").css("top", parseInt(($(info.Content).height() - $(info.Content).find("div[class='prev']").height()) / 2));
                 $(info.Content).find("div[class='next']").css("top", parseInt(($(info.Content).height() - $(info.Content).find("div[class='next']").height()) / 2));
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
                 $(info.Content).find("img").each(function () {
                     $(this).css("height", $(this).width() + "px");

                 });
             }
         }
    ];
    var dataloader = new Winner.DataLoader(config);
    dataloader.Initialize();

 
 
});



 