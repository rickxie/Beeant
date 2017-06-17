$(document).ready(function () {
    var buyer = new Winner.Buyer();
    buyer.Reset = function (info) {
        if (info.Type == "")
            return false;
        if (info.OriginCount == info.Count)
            return false;
        $.getJSON("/Home/UpdateCount?callback=?&id=" + info.CountInput.attr("ShopcartId") + "&count=" + info.Count, function (data) {
        });
        return true;
    }
    function setOpButton(sender, rev) {
        if (rev) {
            $(sender).find("div[class='ck']").css("margin-left",0- $(sender).find("input[class='remove']").width()+"px");
            $(sender).find("span[class='op']").show();
            $(sender).find("input[class='remove']").show(100);
            $(sender).find("span[class='price']").hide();
            $(sender).find("span[class='til']").hide();
        } else {
            $(sender).find("div[class='ck']").css("margin-left", "0");
            $(sender).find("span[class='op']").hide();
            $(sender).find("input[class='remove']").hide();
            $(sender).find("span[class='price']").show();
            $(sender).find("span[class='til']").show();
        }
    }
    function bindOpEvent(sender) {
        sender.find(".element").bind("swipeleft", function () {
            setOpButton(this,true);
        }).bind("swiperight", function() {
            setOpButton(this, false);
        });
    }
    $("#hfEdit").click(function () {
        var rev = $("#hfEdit").attr("IsShow") == "true" || $("#hfEdit").attr("IsShow")==undefined;
        if (rev) {
            $("#hfEdit").attr("IsShow", "false");
        } else {
            $("#hfEdit").attr("IsShow", "true");
        }
        $(".list").find("div[class='element']").each(function (index, sender) {
            setOpButton(sender, rev);
        });

    });
    var clculator = function () {
        $(document).find("span[Buyer='Price']").html("0");
        $(document).find("span[Buyer='Count']").html("0");
        $(document).find("input[buyer='Count']").each(function (index, sender) {
            if ($(this).parent().parent().parent().find("input[type='checkbox']")[0].checked) {
                $(this).attr("OriginCount", "0");
                $(this).attr("IsCalculate", "true");
            } else {
                $(this).attr("OriginCount", $(this).val());
                $(this).attr("IsCalculate", "false");
            }
            buyer.SetCalculator(this, '');
        });
        if ($(document).find("span[Buyer='Price']").html() != "0") {
            $("#btnSubmit").removeAttr("disabled").css("background", "#ff0000") ;

        } else {
            $("#btnSubmit").attr("disabled", "disabled").css("background", "#EDEDED");
        }
    }
    var config = [
        {
            Triggers: [
                {
                    Sender: window,
                    Event: "scroll",
                    Function: function() {
                        return $(document).scrollTop() >= $(document).height() - $(window).height();
                    }
                }
            ],
            Loading: { Content: "Content", Type: "Append" },
            Url: "/Home/List",
            Paramters: { page: 0 },
            Content: $(".list"),
            ShowType: "Append",
            DataType: "text",
            RequestType: "Repeat",
            IsExecute: true,
            IsLoadHideContent: false,
            BeginLoadFunction: function() {

            },
            BeginShowFunction: function(sender, info, data) {
                info.Paramters.page = info.Paramters.page + 1;

            },
            EndShowFunction: function (sender, info, data) {
                $(info.Content).find(".element").each(function (index, sender) {
                    window.lazyloadImage(sender);
                });
                window.checkbox = new Winner.CheckBox("divShopcart",{StyleFile:null});
                checkbox.Initialize();
                window.checkbox.AfterClick=function() {
                    clculator();
                }
                buyer.Initialize();
                bindOpEvent($(info.Content));
                if ($(info.Content).find(".element").length < 10)
                    info.IsFullLoadComplate = true;
            }
        }
    ];
    var dataloader = new Winner.DataLoader(config);
    dataloader.Initialize();
 
});

function removeCart(sender, id) {
    $.ajax({
        type: "Post",
        url: "/Home/Remove",
        data: { id: id },
        async: true,
        dataType: "json",
        success: function (data) {
            if (data.Status) {
                $(sender).parent().parent().remove();
            } else {
                alert(data.Message);
            }
        },
        error: function () {
            alert("系统忙，请稍后再试");
        }
    });
}
 