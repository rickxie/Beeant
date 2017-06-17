$(document).ready(function () {
    window.lazyloadImage($(".detail")[0]);
    var url = "/Commodity/DetailInfo/" + window.SiteId;
    if (window.ps != undefined) {
        for (var i = 0; i < window.ps.length; i++) {
            url +=(i==0?"?":"&")+"ps[" + i + "].id=" + window.ps[i].Id + "&ps[" + i + "].password=" + window.ps[i].Password;
        }
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
              Url: url,
              Paramters: {},
              Content: $(".detail")[0],
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
    $.ajax({
        type: "Post",
        url: "/commodity/WeixinQrCode/" + window.SiteId,
        data: {
        },
        async: true,
        dataType: "text",
        success: function (data) {
            if (data=="") {
                $(".detailqr").append('<img src="/Home/GetQrCode/' + window.SiteId + '" /> ');
            } else {
                $(".detailqr").append('<img src="' + data + '"  /> ');
            }
          
        },
        error: function () {
        
        }
    });
});



