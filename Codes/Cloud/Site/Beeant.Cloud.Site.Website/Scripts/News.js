$(document).ready(function() {
    function setView(sender) {
        var obj = $(sender).find(".del");
        if (obj.html().length > 200) {
            obj.attr("relval", obj.html());
            obj.html(obj.html().substring(0, 200) + "...  ");
            var op = $("<span class='op'>" + window.Language.MoreTip + "</span>");
            op.click(function() {
                obj.html(obj.attr("relval"));
            });
            obj.append(op);
        }
    }
    var config = [
       {
           Triggers: [
               {
                   Sender: window,
                   Event: "scroll",
                   Function: function () {
                       return $(document).scrollTop() >= $(document).height() - $(window).height();
                   }
               }
           ],
           Loading: { Content: "Content", Type: "Append" },
           Url: "/News/List/" + window.SiteId,
           Paramters: { page: 0 },
           Content: $(".list")[0],
           ShowType: "Append",
           DataType: "text",
           RequestType: "Repeat",
           IsExecute: true,
           IsLoadHideContent: false,
           BeginLoadFunction: function () {
              
           },
           BeginShowFunction: function (sender, info, data) {
               info.Paramters.page = info.Paramters.page + 1;
           },
           EndShowFunction: function (sender, info, data) {
               window.lazyloadImage($(info.Content)[0]);
               var length = $("<div>" + data + "</div>").find(".element").length;
               if (length < 12)
                   info.IsFullLoadComplate = true;
               var elemnts = $(info.Content).find(".element");
               for (var i =  elemnts.length-length; i < elemnts.length; i++) {
                   setView(elemnts[i]);
               }
           }
       }
    ];
    var dataloader = new Winner.DataLoader(config);
    dataloader.Initialize();

 
 
});



 