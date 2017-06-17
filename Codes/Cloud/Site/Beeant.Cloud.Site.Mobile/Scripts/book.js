$(document).ready(function () {

    var config = [
         {
             Triggers: [
                 {
                     Sender: "",
                     Event: "",
                     Function: null
                 }
             ],
             Loading: { Content: "Content", Type: "Append" },
             Url: "/Book/List/" + window.SiteId,
             Paramters: { page: 0 },
             Content: $("#divBook")[0],
             ShowType: "Append",
             DataType: "text",
             RequestType: "OneTime",
             IsExecute: true,
             IsLoadHideContent: false,
             Group: "",
             BeginLoadFunction: function () {

             },
             BeginShowFunction: function (sender, info, data) {
                 
             },
             EndShowFunction: function (sender, info, data) {
                 window.lazyloadImage($(info.Content)[0]);
                 var updowner = new Winner.Updowner("divBook");
                 updowner.Initialize();
             }
         }
    ];
    var dataloader = new Winner.DataLoader(config);
    dataloader.Initialize();
});



