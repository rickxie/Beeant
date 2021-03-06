﻿$(document).ready(function () {

    var config = [
         {
             Triggers: [
                 {
                     Sender: "",
                     Event: ""
                 }
             ],
             Loading: { Content: "Content", Type: "Append" },
             Url: "/Certificate/List/" + window.SiteId,
             Paramters: { page: 0 },
             Content: $(".certificate")[0],
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
                 $(info.Content).find("img").each(function () {
                     $(this).css("height", $(this).width() + "px");

                 });
             }
         }
    ];
    var dataloader = new Winner.DataLoader(config);
    dataloader.Initialize();
});



