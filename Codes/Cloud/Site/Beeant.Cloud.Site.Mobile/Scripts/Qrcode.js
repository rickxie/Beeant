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
                 },
                  {
                      Sender: $("#btnSearch")[0],
                      Event: "click",
                      Function: function (info) {
                          $(".list").html("");
                          info.Paramters.page = 0;
                          info.Paramters.key = $("#txtSearch").val();
                          info.IsFullLoadComplate = false;
                          return true;
                      }
                  },
                  {
                      Sender: $("#selType")[0],
                      Event: "change",
                      Function: function (info) {
                          $(".list").html("");
                          $("#txtSearch").val("");
                          info.Paramters.page = 0;
                          info.IsFullLoadComplate = false;
                          info.Paramters.key = $("#txtSearch").val();
                          info.Paramters.type = $("#selType").val();
                          return true;
                      }
                  }
             ],
             Loading: { Content: "Content", Type: "Append" },
             Url: "/Qrcode/List/" + window.SiteId,
             Paramters: { page: 0 },
             Content: $(".list")[0],
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
                 var length = $("<div>" + data + "</div>").find(".element").length;
                 if (length < 24)
                     info.IsFullLoadComplate = true;
             }
         }
    ];
    var dataloader = new Winner.DataLoader(config);
    dataloader.Initialize();
});



