$(document).ready(function () {
 
    var config = [
 
       {
           Triggers: [
               {
                   Sender:"",
                   Event: ""
               }
           ],
           Loading: { Content: "Content", Type: "Append" },
           Url: "/Company/Detail/" + window.SiteId,
           Paramters: {},
           Content: $("#divCompany")[0],
           ShowType: "Append",
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
      

           }
       }
    ];
    var dataloader = new Winner.DataLoader(config);
    dataloader.Initialize();
});



