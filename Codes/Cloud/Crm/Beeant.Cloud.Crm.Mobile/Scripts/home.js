$(document).ready(function () {
    $(".sqr").css("height", $(".sqr").width()).css("line-height", $(".sqr").width() + "px")
    .css("margin-top", $(".sqr").css("marginLeft") );


    var config = [
 
       {
           Triggers: [
               {
                   Sender:"",
                   Event: ""
               }
           ],
           Loading: { Content: "Content", Type: "Append" },
           Url: "/Company/Index/" + window.CrmId,
           Paramters: {},
           Content: $(".cp")[0],
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



