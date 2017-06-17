$(document).ready(function () {
    function setOpButton(sender, rev) {
        if (rev) {
            $(sender).find("div[class='remove']").show(100);
        } else {
            $(sender).find("div[class='remove']").hide();
        }
    }
    function bindOpEvent(sender) {
        sender.find(".element").bind("swipeleft", function () {
            setOpButton(this, true);
        }).bind("swiperight", function () {
            setOpButton(this, false);
        });
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
            Url: "/Message/List",
            Paramters: { page: 0 },
            Content: $(".list"),
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
                bindOpEvent($(info.Content));
                if ($(info.Content).find(".element").length < 10)
                    info.IsFullLoadComplate = true;
            }
        }
    ];
    var dataloader = new Winner.DataLoader(config);
    dataloader.Initialize();

});

function removeMessage(sender, id) {
    $.ajax({
        type: "Post",
        url: "/Message/Remove",
        data: { id: id },
        async: true,
        dataType: "json",
        success: function (data) {
            if (data.Status) {
                $(sender).parent().remove();
            } else {
                alert(data.Message);
            }
        },
        error: function () {
            alert("系统忙，请稍后再试");
        }
    });
}
